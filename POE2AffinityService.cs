using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Management;

public class POE2AffinityService : BackgroundService
{
    private readonly ILogger<POE2AffinityService> _logger;
    private ManagementEventWatcher _processStartWatcher;

    public POE2AffinityService(ILogger<POE2AffinityService> logger)
    {
        _logger = logger;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting process watcher...");

        _processStartWatcher = new ManagementEventWatcher(new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));
        _processStartWatcher.EventArrived += OnProcessStarted;
        _processStartWatcher.Start();

        return base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    private void OnProcessStarted(object sender, EventArrivedEventArgs e)
    {
        string fullProcessName = e.NewEvent.Properties["ProcessName"].Value.ToString();
        string processName = fullProcessName.Replace(".exe", "");
        
        if (processName != null && processName.Contains("PathOfExile"))
        {
            _logger.LogInformation($"POE Detected: (Name: {processName})");
            try
            {
                Process proc = Process.GetProcessesByName(processName).FirstOrDefault();
                if (proc != null)
                {
                    _logger.LogInformation($"Found process: {proc.ProcessName.Replace(".exe", "")} (PID: {proc.Id})");
                    proc.ProcessorAffinity = new IntPtr(0xFC); // Excluding cores 0 and 1 (binary 11111100)
                } else
                {
                    _logger.LogInformation($"Failed to get process: {proc.ProcessName} (PID: {proc.Id})");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Could not set affinity for process {processName}: {ex.Message}");
            }
        }
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping process watcher...");
        _processStartWatcher.Stop();
        _processStartWatcher.Dispose();
        return base.StopAsync(cancellationToken);
    }
}
