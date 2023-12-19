# simple-plotting
A library wrapper for ScottPlot &amp; CsvHelper. This library parses CSV files &amp; and generates plots and/or interactively plots them (WPF, Winforms, Avalonia, etc.).

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

    - Can be used with the following application environments: WPF, WinForms, Blazor, Console Apps
        ** Interactive plots are currently only support in WPF, WinForms and Console Apps

### Console Application Example

```cs
using ScottPlot.Plottable;
using simple_plotting;
using simple_plotting.runtime;

var file = "c71_hrl_therm_cycling_9222023_92144_AM.csv";
var path = @"C:\Development\visual-studio-projects\simple-plotting\simple-plotting-console\test-data";

try {
	const int numOfPlots = 4;

	// can use CsvParser.StartNew(path) to skip SetSource(path)
	var initialStrategy = new DefaultCsvParseStrategy(-65d, 165d);
	var source          = CsvParserFactory.StartNew(initialStrategy, path);

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
	                               .OfType<SignalPlot>()
	                               .WithTitle("Test title", 24)
	                               .WithSize(PlotSize.S1280X800)
	                               .WithPrimaryXAxisLabel(Constants.X_AXIS_LABEL_DATE_TIME, 20)
	                               .WithPrimaryYAxisLabel(Constants.Y_AXIS_LABEL_TEMP, 20)
	                               .WithSecondaryYAxisLabel(Constants.Y_AXIS_LABEL_RH, 20)
	                               .ShowLegend(PlotAlignment.UpperRight)
	                               .SetDataPadding(percentY: .2, percentX: 0.0)
	                               .DefineSource(source)
	                               .RotatePrimaryXAxisTicks(PlotAxisRotation.FortyFive)
	                               .RotatePrimaryYAxisTicks(PlotAxisRotation.Zero)
	                               .SetLayout(right: 123, top:123)
	                               .FinalizeConfiguration()
	                               .Produce();

	product.GotoPlottables().AddDraggableLine(0, 50, 40000, out _);
	product.GotoPostProcess().TrySetLabel("Test label", 0, 1);
	product.GotoPlottables().WithAnnotationAt("test annotation", 1, 100, 100, out _);

	var status = product.TrySaveAtSource("output");

	Console.WriteLine(status.State ? "Successfully saved plot!" : "Could not save plot...");
}
catch (Exception e) {
	Console.WriteLine(e);
	throw;
}

return 0;
```

### WPF Application Example

```cs
Taking a look at the included WPF example. This will showcase how to build a plot and interact with a WpfPlot
control and how to interact with it.
```
