# run_monitor.ps1
# Run this in PowerShell (no compile step needed) — right-click > Run with PowerShell
# Requires: Windows 10, .NET Framework 4

Add-Type -AssemblyName System.Windows.Forms
Add-Type -AssemblyName System.Drawing

$source = Get-Content "$PSScriptRoot\MemoryMonitor.cs" -Raw

Add-Type -TypeDefinition $source `
         -ReferencedAssemblies System.Windows.Forms, System.Drawing `
         -Language CSharp

[System.Windows.Forms.Application]::EnableVisualStyles()
[System.Windows.Forms.Application]::Run((New-Object MemoryMonitorApp))
