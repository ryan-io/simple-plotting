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
    - System.Numerics.Vectors.4.5.0
	- System.Buffers.4.5.1
	- System.Memory.4.5.5
	- System.Runtime.CompilerServices.Unsafe.5.0.0
	- System.Threading.Tasks.Dataflow.4.5.24

### Simple Use Case
    - Can be used with the following application environments: WPF, WinForms, Blazor, Console Apps
        ** Interactive plots are currently only support in WPF, WinForms and Console Apps

```cs
using simple_plotting.src;
using simple_plotting.runtime;

var path = @"c:\your_directory";
var file = "your_file.csv";

// this API throws exceptions, so you must wrap it in a try-catch block
try {
	const int numOfPlots = 5;

	// can use CsvParser.StartNew(path) to skip SetSource(path)
	var initialStrategy = new DefaultCsvParseStrategy(-65d, 165d);
	var source          = CsvParser.StartNew(initialStrategy, path);
	
	// if you prefer to set the pat manually, you can do the following
	// var initialStrategy = new DefaultCsvParseStrategy(-65d, 165d);
	// var source          = CsvParser.StartNew(initialStrategy);
	// source.SetSource(path);
	
	// invoke source.ExtractAsync(file) to generate output
	var output = await source.ExtractAsync(file);

	// check if output is null
	if (output == null) {
		Console.WriteLine("Could not extract data from CSV file...");
		return -1;
	}

	// begin building the plot via fluent builder
	var product = PlotBuilderFluent.StartNew(output, numOfPlots)
	                               .WithTitle("Test title")
	                               .WithSize(PlotSize.S1280X800)
	                               .WithPrimaryXAxisLabel(Constants.X_AXIS_LABEL_DATE_TIME)
	                               .WithSecondaryYAxisLabel(Constants.Y_AXIS_LABEL_RH)
	                               .ShowLegend(PlotAlignment.UpperRight)
	                               .SetDataPadding(percentY: 0.2)
	                               .DefineSource(source)
	                               .RotatePrimaryXAxisTicks(PlotAxisRotation.Zero)
	                               .RotatePrimaryYAxisTicks(PlotAxisRotation.Zero)
	                               .WithPrimaryYAxisLabel(Constants.Y_AXIS_LABEL_TEMP).FinalizeConfiguration()
	                               .Produce();

	// can also define a path in by invoking product.TrySave()
	// var canSave = product.TrySave(@"c:\your-directory", "output");
	
	// you could invoke product.TrySave if you call PlotBuilderFluent.DefineSource
	var canSaveAtSource = product.TrySaveAtSource("output");
	
	Console.WriteLine(canSaveAtSource ? "Successfully saved plot!" : "Could not save plot...");
}
catch (Exception e) {
	Console.WriteLine(e);
	throw;
}

return 0;
```