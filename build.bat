@echo off
:: ============================================================
::  build.bat  —  Compile MemoryMonitor.cs into MemoryMonitor.exe
::  Run this ONCE on your Windows 10 machine to produce the .exe
:: ============================================================

echo [1/3] Locating C# compiler...

:: Try .NET Framework 4.x compiler (ships with Windows)
set CSC=
for %%f in (
    "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
    "C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe"
) do (
    if exist %%f (
        set CSC=%%f
        goto :found
    )
)

echo ERROR: csc.exe not found. Make sure .NET Framework 4 is installed.
echo Download from: https://dotnet.microsoft.com/download/dotnet-framework
pause
exit /b 1

:found
echo    Found: %CSC%

echo [2/3] Compiling...
%CSC% /target:winexe ^
      /out:MemoryMonitor.exe ^
      /r:System.Windows.Forms.dll ^
      /r:System.Drawing.dll ^
      /optimize+ ^
      MemoryMonitor.cs

if errorlevel 1 (
    echo.
    echo ERROR: Compilation failed. See errors above.
    pause
    exit /b 1
)

echo [3/3] Done!
echo.
echo  MemoryMonitor.exe created successfully.
echo  Double-click it to start — look for the icon in your system tray (bottom-right).
echo.
pause
