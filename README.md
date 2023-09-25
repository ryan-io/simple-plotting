# simple-plotting
A library wrapper for ScottPlot &amp; CsvHelper. This library parses CSV files &amp; interactively plots them in WPF, WinForms, etc. This libray is intended to be used with the MVVM pattern.

### Dependencies
    - CsvHelper.30.0.1
        - https://joshclose.github.io/CsvHelper/
    - ScottPlot.4.1.67
        - https://github.com/ScottPlot/ScottPlot
    - Microsoft.NETCore.Platforms.3.1.4
    - Microsoft.Win32.SystemEvents.4.7.0
    - System.Drawing.Common.4.7.2
    - System.NUmerics.Vectors.4.5.0

### Simple Use Case
    - Can be used with the following application environments: WPF, WinForms, Blazor, Console Apps
        ** Interactive plots are currently only support in WPF, WinForms and Console Apps