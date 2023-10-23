// simple-plotting

namespace simple_plotting.src;

/// <summary>
///  Core abstraction for fluent API. All configuration methods should be defined here.
/// </summary>
public interface IPlotBuilderFluentConfiguration : IPlotBuilderFluent {
	/// <summary>
	///  Register an action to be invoked when the plot is produced.
	/// </summary>
	/// <param name="action">Callback</param>
	/// <returns>Fluent builder</returns>
	IPlotBuilderFluentConfiguration WithObservable(Action action);

	/// <summary>
	///  Sets the size of the plot window.
	/// </summary>
	/// <param name="plotSize">Container that defines width & height</param>
	/// <returns>Fluent builder</returns>
	IPlotBuilderFluentConfiguration WithSize(PlotSize plotSize);

	/// <summary>
	///  Sets the title of the plot.
	/// </summary>
	/// <param name="title">String title for plot</param>
	/// <returns>Fluent builder</returns>
	/// <exception cref="Exception">Thrown if string is null or whitespace</exception>
	IPlotBuilderFluentConfiguration WithTitle(string title);

	/// <summary>
	///  Sets the label for the X axis.
	/// </summary>
	/// <param name="xAxisLabel">String value for the X axis label</param>
	/// <returns>Fluent builder</returns>
	/// <exception cref="Exception">Thrown if xAxisLabel is null or whitespace</exception>
	IPlotBuilderFluentConfiguration WithPrimaryXAxisLabel(string xAxisLabel);

	/// <summary>
	///  Sets the label for the Y axis.
	/// </summary>
	/// <param name="yAxisLabel">String value for the Y axis label</param>
	/// <returns>Fluent builder</returns>
	/// <exception cref="Exception">Thrown if yAxisLabel is null or whitespace</exception>
	IPlotBuilderFluentConfiguration WithPrimaryYAxisLabel(string yAxisLabel);

	/// <summary>
	///  Sets the rotation of the primary Y axis ticks.
	/// </summary>
	/// <param name="rotation">Rotation of axis</param>
	/// <returns>Fluent builder</returns>
	IPlotBuilderFluentConfiguration RotatePrimaryYAxisTicks(PlotAxisRotation rotation);

	/// <summary>
	///  Sets the rotation of the primary X axis ticks.
	/// </summary>
	/// <param name="rotation">Rotation of axis</param>
	/// <returns>Fluent builder</returns>
	IPlotBuilderFluentConfiguration RotatePrimaryXAxisTicks(PlotAxisRotation rotation);

	/// <summary>
	///  Sets the label for the secondary X axis.
	/// </summary>
	/// <param name="xAxisLabel">Label for the secondary x-axis (top side of plot)</param>
	/// <returns>Fluent builder</returns>
	/// <exception cref="Exception">Thrown if the label is null or whitespace</exception>
	IPlotBuilderFluentConfiguration WithSecondaryXAxisLabel(string xAxisLabel);

	/// <summary>
	///  Sets the label for the secondary Y axis.
	/// </summary>
	/// <param name="yAxisLabel">Label for the secondary x-axis (right side of plot)</param>
	/// <returns>Fluent builder</returns>
	/// <exception cref="Exception">Thrown if the label is null or whitespace</exception>
	IPlotBuilderFluentConfiguration WithSecondaryYAxisLabel(string yAxisLabel);

	/// <summary>
	///  Sets the rotation of the secondary Y axis ticks.
	/// </summary>
	/// <param name="container">Container for rotation of axis</param>
	/// <returns>Fluent builder</returns>
	IPlotBuilderFluentConfiguration RotateSecondaryYAxisTicks(PlotAxisRotationContainer container);

	/// <summary>
	///  Sets the rotation of the secondary X axis ticks.
	/// </summary>
	/// <param name="container">Container for rotation of axis</param>
	/// <returns>Fluent builder</returns>
	IPlotBuilderFluentConfiguration RotateSecondaryXAxisTicks(PlotAxisRotationContainer container);

	/// <summary>
	///  Sets whether or not the legend should be shown and the alignment of the legend.
	/// </summary>
	/// <param name="alignment">Where on the plot to display the legend</param>
	/// <returns>Fluent builder</returns>
	IPlotBuilderFluentConfiguration ShowLegend(PlotAlignment alignment);

	/// <summary>
	///  Hides the legend.
	/// </summary>
	/// <returns>Fluent builder</returns>
	IPlotBuilderFluentConfiguration HideLegend();

	/// <summary>
	///  Sets the margins of the plot. This affects the actual data area of the plot.
	/// </summary>
	/// <param name="percentX">Double value for x-axis</param>
	/// <param name="percentY">Double value for y-axis</param>
	/// <returns>Fluent builder</returns>
	IPlotBuilderFluentConfiguration SetDataPadding(double percentX = 0.05, double percentY = 0.05);

	/// <summary>
	///  Sets the SourcePath property to Source.Path. This is used to save the plot(s) to the same directory as the source.
	/// </summary>
	/// <param name="source">IPlotChannelProviderSource, typically derivation of CsvParser.Path</param>
	/// <returns>Fluent builder</returns>
	IPlotBuilderFluentConfiguration DefineSource(IPlotChannelProvider source);

	/// <summary>
	///  Sets the SourcePath property to the specified path. This is used to save the plot(s) to a specific directory.
	/// </summary>
	/// <param name="path">Standalone file path to try-save plots at</param>
	/// <returns>Fluent builder</returns>
	IPlotBuilderFluentConfiguration DefineSource(string path);

    /// <summary>
    ///  Determines the uppwer and lower y-axis limits
    ///  </summary>	
	///  <returns>Fluent builder</returns>
    IPlotBuilderFluentConfiguration SetPlotLimits ();

	/// <summary>
	///  Determines the uppwer and lower y-axis limits
	///  This overload takes into account default upper and lower values
	///  </summary>
	///  <param name="lower">Nullable lower limit; calculator will not go above this value (if not null)</param>
	///  <param name="upper">Nullable upper limit; calculator will not go below this value (if not null)</param>
	IPlotBuilderFluentConfiguration SetPlotLimits (double? upper, double? lower);

	/// <summary>
	///  Sets the margins of the plot. This affects the actual data area of the plot.
	///  This overload takes four parameters for each side of the plot
	/// </summary>
	/// <param name="right">Float-right padding</param>
	/// <param name="top">Float-top padding</param>
	/// <param name="bottom">Float-bottom padding</param>
	/// <param name="left">Float-left padding</param>
	/// <returns>Fluent builder</returns>
	IPlotBuilderFluentConfiguration SetDataPadding(float right, float top, float bottom, float left);

    /// <summary>
    ///  Finalizes the configuration of the plot. This should be the last call in the fluent API.
    ///  This allows you to call Produce().
    /// </summary>
    /// <returns>Fluent interface allowing consumer to Produce()</returns>
    IPlotBuilderFluentReadyToProduce FinalizeConfiguration();
}