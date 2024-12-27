using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class Program
{
    public static void Main(string[] args)
    {
        Console.Title = "POE2 Affinity Service";
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            services.AddHostedService<POE2AffinityService>();
        })
        .ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
        })
        .UseWindowsService();


    private static void InstallService()
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = "sc.exe",
            Arguments = $"create POE2AffinityService binPath= \"{System.Reflection.Assembly.GetExecutingAssembly().Location}\"",
            Verb = "runas",
            UseShellExecute = true
        })?.WaitForExit();

        Process.Start(new ProcessStartInfo
        {
            FileName = "sc.exe",
            Arguments = "start POE2AffinityService",
            Verb = "runas",
            UseShellExecute = true
        })?.WaitForExit();
    }
}
