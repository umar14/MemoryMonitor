# MemoryMonitor

A lightweight Windows 10 system tray tool that displays real-time RAM usage as a number in the taskbar. No installation required.

## Features

- Displays RAM usage % live in the system tray
- Color coded: **Green** < 60% · **Yellow** < 85% · **Red** > 85%
- Updates every 2 seconds
- Hover for full detail: `RAM: 5.2 / 16.0 GB (33%)`
- Double-click for a detailed popup
- Zero install — single `.exe`, runs silently in the background

## Requirements

- Windows 10
- .NET Framework 4 (pre-installed on virtually all Windows 10 machines)

## Build

1. Clone or download the repo
2. Double-click `build.bat`
3. Run the produced `MemoryMonitor.exe`

```bash
git clone https://github.com/umar14/MemoryMonitor.git
cd MemoryMonitor
build.bat
```

The build script uses the C# compiler (`csc.exe`) that ships with .NET Framework 4 — no Visual Studio or extra tools needed.

## Run without compiling

If you just want to try it without building:

1. Right-click `run_monitor.ps1`
2. Select **Run with PowerShell**

## Usage

- The tray icon shows your current RAM % at a glance
- **Hover** — shows exact GB used / total
- **Double-click** — opens details popup
- **Right-click → Exit** — closes the app

## Files

| File | Description |
|------|-------------|
| `MemoryMonitor.cs` | Main source code (C# 5, .NET Framework 4) |
| `build.bat` | One-click compiler script |
| `run_monitor.ps1` | Run directly via PowerShell without compiling |

## License

MIT
