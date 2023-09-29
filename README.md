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

```cs
var path =
@"C:\Development\rider-projects\POC_plotting\generated-plots\c71_hrl_therm_cycling_9222023_92144_AM.csv";

try {
	const int numOfPlots = 5;
	var source = CsvSource.StartNew(path);
	var output = await source.ExtractAsync();
	
	var product = PlotBuilderFluent.StartNew(output, numOfPlots)
	                               .WithTitle("Test title")
	                               .WithSize(PlotSizeMapper.Map(PlotSize.s1024x768))
	                               .WithPrimaryXAxisLabel(Constants.X_AXIS_LABEL_DATE_TIME)
	                               .WithSecondaryYAxisLabel(Constants.Y_AXIS_LABEL_RH)
	                               .ShowLegend(Alignment.UpperRight)
	                               .SetDataPadding(percentY: 0.2)
	                               .RotatePrimaryXAxisTicks(PlotAxisRotationMapper.Map(PlotAxisRotation.Zero))
	                               .RotatePrimaryYAxisTicks(PlotAxisRotationMapper.Map(PlotAxisRotation.Zero))
	                               .WithPrimaryYAxisLabel(Constants.Y_AXIS_LABEL_TEMP).FinalizeConfiguration().Produce();

	var plot    = product.GetPlots().ToArray()[0];
	
	
	var canSave = product.TrySave(@"C:\Development\rider-projects\POC_plotting\generated-plots\output");
	if (canSave)
		Console.WriteLine("Successfully saved plot!");
	else {
		Console.WriteLine("Could not save plot...");
	}
}
catch (Exception e) {
	Console.WriteLine(e);
	throw;
}
```