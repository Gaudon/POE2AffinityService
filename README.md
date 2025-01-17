# Path of Exile 2 CPU Affinity Manager

This project aims to improve the stability of the game Path of Exile 2 on certain systems by monitoring the game process and restricting its access to specific CPU cores. This temporary solution is intended to help users experience fewer system crashes and performance issues until Grinding Gear Games releases an official patch to address the problem.

**NOTE:**
This will not entirely prevent POE2 from crashing. It should prevent POE2 from crashing your entire system. In my experience, in collaboration with this tool players should *ALWAYS* disable Game Engine Multi-Threading in POE2 Game Options prior to zoning into a map for the first time. This has drastically reduced my crashing but is quite annoying!

## Features

- **Process Monitoring**: Continuously monitors for the Path of Exile process.
- **CPU Affinity Management**: Automatically sets the CPU affinity for the Path of Exile process to exclude specific CPU cores.
- **Improved Stability**: Helps reduce crashes and performance issues on affected systems.

## Requirements

- [.NET 8.0 Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- Windows OS (64-bit)

## Installation

1. Download and extract the latest release from the [Releases](https://github.com/Gaudon/POE2AffinityService/releases) page.
2. Run the `POE2AffinityService.exe` file as administrator to start the service (Admin is required to modify processor affinity settings).
