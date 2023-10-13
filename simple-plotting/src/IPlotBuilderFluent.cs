using ScottPlot;
using ScottPlot.Plottable;

namespace simple_plotting.src {
	/// <summary>
	///  Core abstraction for fluent API. All configuration methods should be defined here.
	/// </summary>
	public interface IPlotBuilderFluent_Configuration : IPlotBuilderFluent {
		/// <summary>
		///  Register an action to be invoked when the plot is produced.
		/// </summary>
		/// <param name="action">Callback</param>
		/// <returns>Fluent builder</returns>
		IPlotBuilderFluent_Configuration WithObservable(Action action);
		
		/// <summary>
		///  Sets the size of the plot window.
		/// </summary>
		/// <param name="plotSize">Container that defines width & height</param>
		/// <returns>Fluent builder</returns>
		IPlotBuilderFluent_Configuration WithSize(PlotSize plotSize);

		/// <summary>
		///  Sets the title of the plot.
		/// </summary>
		/// <param name="title">String title for plot</param>
		/// <returns>Fluent builder</returns>
		/// <exception cref="Exception">Thrown if string is null or whitespace</exception>
		IPlotBuilderFluent_Configuration WithTitle(string title);

		/// <summary>
		///  Sets the label for the X axis.
		/// </summary>
		/// <param name="xAxisLabel">String value for the X axis label</param>
		/// <returns>Fluent builder</returns>
		/// <exception cref="Exception">Thrown if xAxisLabel is null or whitespace</exception>
		IPlotBuilderFluent_Configuration WithPrimaryXAxisLabel(string xAxisLabel);

		/// <summary>
		///  Sets the label for the Y axis.
		/// </summary>
		/// <param name="yAxisLabel">String value for the Y axis label</param>
		/// <returns>Fluent builder</returns>
		/// <exception cref="Exception">Thrown if yAxisLabel is null or whitespace</exception>
		IPlotBuilderFluent_Configuration WithPrimaryYAxisLabel(string yAxisLabel);

		/// <summary>
		///  Sets the rotation of the primary Y axis ticks.
		/// </summary>
		/// <param name="rotation">Rotation of axis</param>
		/// <returns>Fluent builder</returns>
		IPlotBuilderFluent_Configuration RotatePrimaryYAxisTicks(PlotAxisRotation rotation);

		/// <summary>
		///  Sets the rotation of the primary X axis ticks.
		/// </summary>
		/// <param name="rotation">Rotation of axis</param>
		/// <returns>Fluent builder</returns>
		IPlotBuilderFluent_Configuration RotatePrimaryXAxisTicks(PlotAxisRotation rotation);

		/// <summary>
		///  Sets the label for the secondary X axis.
		/// </summary>
		/// <param name="xAxisLabel">Label for the secondary x-axis (top side of plot)</param>
		/// <returns>Fluent builder</returns>
		/// <exception cref="Exception">Thrown if the label is null or whitespace</exception>
		IPlotBuilderFluent_Configuration WithSecondaryXAxisLabel(string xAxisLabel);

		/// <summary>
		///  Sets the label for the secondary Y axis.
		/// </summary>
		/// <param name="yAxisLabel">Label for the secondary x-axis (right side of plot)</param>
		/// <returns>Fluent builder</returns>
		/// <exception cref="Exception">Thrown if the label is null or whitespace</exception>
		IPlotBuilderFluent_Configuration WithSecondaryYAxisLabel(string yAxisLabel);

		/// <summary>
		///  Sets the rotation of the secondary Y axis ticks.
		/// </summary>
		/// <param name="container">Container for rotation of axis</param>
		/// <returns>Fluent builder</returns>
		IPlotBuilderFluent_Configuration RotateSecondaryYAxisTicks(PlotAxisRotationContainer container);

		/// <summary>
		///  Sets the rotation of the secondary X axis ticks.
		/// </summary>
		/// <param name="container">Container for rotation of axis</param>
		/// <returns>Fluent builder</returns>
		IPlotBuilderFluent_Configuration RotateSecondaryXAxisTicks(PlotAxisRotationContainer container);

		/// <summary>
		///  Sets whether or not the legend should be shown and the alignment of the legend.
		/// </summary>
		/// <param name="alignment">Where on the plot to display the legend</param>
		/// <returns>Fluent builder</returns>
		IPlotBuilderFluent_Configuration ShowLegend(PlotAlignment alignment);

		/// <summary>
		///  Hides the legend.
		/// </summary>
		/// <returns>Fluent builder</returns>
		IPlotBuilderFluent_Configuration HideLegend();

		/// <summary>
		///  Sets the margins of the plot. This affects the actual data area of the plot.
		/// </summary>
		/// <param name="percentX">Double value for x-axis</param>
		/// <param name="percentY">Double value for y-axis</param>
		/// <returns>Fluent builder</returns>
		IPlotBuilderFluent_Configuration SetDataPadding(double percentX = 0.05, double percentY = 0.05);

		/// <summary>
		///  Sets the SourcePath property to Source.Path. This is used to save the plot(s) to the same directory as the source.
		/// </summary>
		/// <param name="source">IPlotChannelProviderSource, typically derivation of CsvParser.Path</param>
		/// <returns>Fluent builder</returns>
		IPlotBuilderFluent_Configuration DefineSource(IPlotChannelProvider source);
		
		/// <summary>
		///  Sets the SourcePath property to the specified path. This is used to save the plot(s) to a specific directory.
		/// </summary>
		/// <param name="path">Standalone file path to try-save plots at</param>
		/// <returns>Fluent builder</returns>
		IPlotBuilderFluent_Configuration DefineSource(string path);
		
		/// <summary>
		///  Finalizes the configuration of the plot. This should be the last call in the fluent API.
		///  This allows you to call Produce().
		/// </summary>
		/// <returns>Fluent interface allowing consumer to Produce()</returns>
		IPlotBuilderFluent_ReadyToProduce FinalizeConfiguration();
	}

	/// <summary>
	///  This abstraction finalizes the configuration of the plot. This should be the last call in the fluent API.
	/// </summary>
	public interface IPlotBuilderFluent_ReadyToProduce : IPlotBuilderFluent {
		/// <summary>
		///  Returns the generated Plot instance. This should be the last call in the fluent API.
		/// </summary>
		/// <returns>ScottPlot.Plot instance (private)</returns>
		IPlotBuilderFluent_Product Produce();
	}

	/// <summary>
	///  The generated plots. Can call { get; } after Produce() has been invoked and will return as an enumerable.
	/// </summary>
	public interface IPlotBuilderFluent_Product : IPlotBuilderFluent {
		/// <summary>
		///  The generated plots. Can call { get; } after Produce() has been invoked and will return as an enumerable.
		/// </summary>
		IEnumerable<Plot> GetPlots();

		/// <summary>
		///  Attempts to save the plot to the specified path. This will throw an <see cref="Exception"/> if the save fails.
		/// </summary>
		/// <param name="savePath">Directory to save plots</param>
		/// <param name="name">Name of each plot</param>
		/// <returns>True if could write (save) to directory, otherwise false</returns>
		/// <exception cref="Exception">Thrown if savePath is null or whitespace</exception>
		bool TrySave(string savePath, string name);
		
		/// <summary>
		///  Attempts to save the plot to the defined source path. This will throw an <see cref="Exception"/> if the save fails.
		///  This method requires you to call DefineSource.
		/// </summary>
		/// <param name="name">Name of each plot</param>
		/// <returns>True if could write (save) to directory, otherwise false</returns>
		/// <exception cref="Exception">Thrown if savePath is null or whitespace</exception>
		bool TrySaveAtSource(string name);

		/// <summary>
		///  Exposes post processing API.
		/// </summary>
		/// <returns>Fluent builder as IPlotBuilderFluent_PostProcess</returns>
		IPlotBuilderFluent_PostProcess PostProcess();
		
		/// <summary>
		///  Resets the builder to an initial state. This is useful if you want to reuse the builder with new data.
		/// </summary>
		/// <param name="data">New data to populate the builder with</param>
		/// <returns>Fluent builder in a reset state</returns>
		IPlotBuilderFluent_Configuration Reset(IReadOnlyList<PlotChannel> data);
	}

	/// <summary>
	///  Provides the consumer with additional tools to manipulate the plots. Any interactive plot features should be defined here.
	/// </summary>
	public interface IPlotBuilderFluent_PostProcess {
		/// <summary>
		///  Helper method to return back to the product.
		/// </summary>
		/// <returns>Instance product</returns>
		IPlotBuilderFluent_Product GoToProduct();
		
		/// <summary>
		///  Adds an annotation to a plot at xOff, yOff.
		/// </summary>
		/// <param name="annotation">String text to display in annotation</param>
		/// <param name="plot">Plot to annotate</param>
		/// <param name="xOff">x-offset (from lower-left of plot)</param>
		/// <param name="yOff">y-offset (from the lower-left of the plot)</param>
		/// <returns>Fluent builder as IPlotBuilderFluent_PostProcess</returns>
		IPlotBuilderFluent_PostProcess   WithAnnotationAt(string annotation, Plot plot, float xOff, float yOff);

		/// <summary>
		///  Sets the size of all plots.
		/// </summary>
		/// <param name="size">New plot size</param>
		/// <returns>Fluent builder as IPlotBuilderFluent_PostProcess</returns>
		IPlotBuilderFluent_PostProcess SetSizeOfAll(PlotSize size);

		/// <summary>
		///  Takes an IPlottable, casts it to a ScatterPlot and sets the label.
		/// </summary>
		/// <param name="plot">IPlottable to change the label fro</param>
		/// <param name="newLabel">New label</param>
		/// <returns>Fluent builder as IPlotBuilderFluent_PostProcess</returns>
		IPlotBuilderFluent_PostProcess SetScatterLabel(IPlottable? plot, string newLabel);

		/// <summary>
		///  Takes an IPlottable, casts it to a ScatterPlot and sets the label.
		///  This method will invoke Render() on the plot.
		/// </summary>
		/// <param name="newLabel">New label</param>
		/// <param name="plottableIndex">Plottable index to adjust label for</param>
		/// <returns>Fluent builder as IPlotBuilderFluent_PostProcess</returns>
		/// <exception cref="NullReferenceException">Thrown if plottable cast fails</exception>
		IPlotBuilderFluent_PostProcess TrySetScatterLabel(string newLabel, params int[] plottableIndex);
	}

	/// <summary>
	///  The base abstraction for the fluent API.
	/// </summary>
	public interface IPlotBuilderFluent {
		/// <summary>
		///  This ensures a the Produce() method has been invoked before allowing you to save a plot.
		/// </summary>
		bool CanSave { get; }
	}
}