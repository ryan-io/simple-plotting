// simple-plotting

using System.Drawing;
using ScottPlot;

namespace simple_plotting;

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
	/// <param name="fontSize">Size of font</param>
	/// <param name="isBold">True bolds, otherwise leaves as-is</param>
	/// <returns>Fluent builder</returns>
	/// <exception cref="Exception">Thrown if string is null or whitespace</exception>
	IPlotBuilderFluentConfiguration WithTitle(string title, int fontSize = 14, bool isBold = false);

	/// <summary>
	///  Sets the label for the X axis.
	/// </summary>
	/// <param name="labelTxt">String value for the X axis label</param>
	/// <param name="fontColor">Color of font</param>
	/// <param name="fontSize">Size of font</param>
	/// <param name="isBold">True bolds, otherwise leaves as-is</param>
	/// <returns>Fluent builder</returns>
	/// <exception cref="Exception">Thrown if xAxisLabel is null or whitespace</exception>
	IPlotBuilderFluentConfiguration WithPrimaryXAxisLabel(string labelTxt, Color fontColor, int fontSize,
		bool isBold = false);

	/// <summary>
	///  Sets the label for the Y axis.
	/// </summary>
	/// <param name="labelTxt">String value for the Y axis label</param>
	/// <param name="fontColor">Color of font</param>
	/// <param name="fontSize">Size of font</param>
	/// <param name="isBold">True bolds, otherwise leaves as-is</param>
	/// <returns>Fluent builder</returns>
	/// <exception cref="Exception">Thrown if yAxisLabel is null or whitespace</exception>
	IPlotBuilderFluentConfiguration WithPrimaryYAxisLabel(string labelTxt, Color fontColor, int fontSize,
		bool isBold = false);

	/// <summary>
	/// Sets the label for the X axis.
	/// </summary>
	/// <param name="labelTxt">String value for the X axis label</param>
	/// <param name="fontSize">Size of font</param>
	/// <param name="isBold">True bolds, otherwise leaves as-is</param>
	/// <returns>Fluent builder</returns>
	IPlotBuilderFluentConfiguration WithPrimaryXAxisLabel(string labelTxt, int fontSize = 14, bool isBold = false);

	/// <summary>
	/// Sets the label for the Y axis.
	/// </summary>
	/// <param name="labelTxt">String value for the Y axis label</param>
	/// <param name="fontSize">Size of font</param>
	/// <param name="isBold">True bolds, otherwise leaves as-is</param>
	/// <returns>Fluent builder</returns>
	IPlotBuilderFluentConfiguration WithPrimaryYAxisLabel(string labelTxt, int fontSize = 14, bool isBold = false);

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
	/// <param name="fontSize">Size of font</param>
	/// <param name="isBold">True bolds, otherwise leaves as-is</param>
	/// <returns>Fluent builder</returns>
	/// <exception cref="Exception">Thrown if the label is null or whitespace</exception>
	IPlotBuilderFluentConfiguration WithSecondaryXAxisLabel(string xAxisLabel, int fontSize = 14, bool isBold = false);

	/// <summary>
	///  Sets the label for the secondary X axis.
	/// </summary>
	/// <param name="xAxisLabel">Label for the secondary x-axis (top side of plot)</param>
	/// <param name="fontSize">Size of font</param>
	/// <param name="fontColor">Color of font</param>
	/// <param name="isBold">True bolds, otherwise leaves as-is</param>
	/// <returns>Fluent builder</returns>
	/// <exception cref="Exception">Thrown if the label is null or whitespace</exception>
	IPlotBuilderFluentConfiguration WithSecondaryXAxisLabel(string xAxisLabel, Color fontColor, int fontSize = 14,
		bool isBold = false);

	/// <summary>
	///  Sets the label for the secondary Y axis.
	/// </summary>
	/// <param name="yAxisLabel">Label for the secondary x-axis (right side of plot)</param>
	/// <param name="fontSize">Size of font</param>
	/// <param name="isBold">True bolds, otherwise leaves as-is</param>
	/// <returns>Fluent builder</returns>
	/// <exception cref="Exception">Thrown if the label is null or whitespace</exception>
	IPlotBuilderFluentConfiguration WithSecondaryYAxisLabel(string yAxisLabel, int fontSize = 14, bool isBold = false);

	/// <summary>
	///  Sets the label for the secondary Y axis.
	/// </summary>
	/// <param name="yAxisLabel">Label for the secondary x-axis (right side of plot)</param>
	/// <param name="fontColor">Color of font</param>
	/// <param name="fontSize">Size of font</param>
	/// <param name="isBold">True bolds, otherwise leaves as-is</param>
	/// <returns>Fluent builder</returns>
	/// <exception cref="Exception">Thrown if the label is null or whitespace</exception>
	IPlotBuilderFluentConfiguration WithSecondaryYAxisLabel(string yAxisLabel, Color fontColor, int fontSize = 14,
		bool isBold = false);

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
	/// Enables to show the plot's legend in the specified alignment position and orientation.
	/// </summary>
	/// <param name="alignment">The alignment position of the legend relative to the plot area.</param>
	/// <param name="orientation">The orientation of the legend elements. Could be horizontal or vertical.</param>
	/// <returns>IPlotBuilderFluentConfiguration: A reference to this instance after the operation completed.</returns>
	IPlotBuilderFluentConfiguration ShowLegend(PlotAlignment alignment, Orientation orientation);

	/// <summary>
	/// Configures to show the plot legend outside at the top right corner of the plot.
	/// </summary>
	/// <returns>A fluent configuration object for continuing the plot building process.</returns>
	IPlotBuilderFluentConfiguration ShowLegendOutSideTopRight();

	/// <summary>
	///  Hides the legend.
	/// </summary>
	/// <returns>Fluent builder</returns>
	IPlotBuilderFluentConfiguration HideLegend();

	/// <summary>
	/// Sets the data padding for all plots in the configuration.
	/// </summary>
	/// <param name="valueX">The padding value in X-direction, default to 0.05. This value must lie between 0 and 1 (Inclusive). Otherwise an Exception is thrown.</param>
	/// <param name="valueY">The padding value in Y-direction, default to 0.05. This value must lie between 0 and 1 (Inclusive). Otherwise an Exception is thrown.</param>
	/// <returns>The same IPlotBuilderFluentConfiguration with the updated padding information.</returns>
	/// <exception cref="Exception">Throws an exception if the given valueX or valueY is not between 0 and 1 (Inclusive).</exception>
	IPlotBuilderFluentConfiguration SetDataPadding(double valueX = 0.05, double valueY = 0.05);

	/// <summary>
	/// Sets the padding to add around the drawn data on a plot.
	/// </summary>
	/// <param name="chCnt">Number of channels to consider when calculating padding. Must be greater than 0.</param>
	/// <param name="valueX">Padding value to add on the x-axis for each channel. It should be a value between 0 and 1, defaults to 0.05.</param>
	/// <param name="valueY">Padding value to add on the y-axis for each channel. It should be a value between 0 and 1, defaults to 0.05.</param>
	/// <returns>Returns the updated configuration object.</returns>
	/// <exception cref="Exception">Throws an exception if chCnt is zero or negative.</exception>
	/// <exception cref="Exception">Throws an exception if either valueX or valueY is not between 0 and 1.</exception>
	/// <remarks>
	/// The padding is added incrementally for each channel specified by chCnt. 
	/// The total padding for the x-axis and y-axis is capped at 1.
	/// If the total calculated padding is negative or exceeds 1, it is clamped to the range 0-1.
	/// </remarks>
	IPlotBuilderFluentConfiguration SetDataPadding(int chCnt, double valueX = 0.05, double valueY = 0.05);

	/// <summary>
	/// Set data padding with respect to the number of channels (_data.Count) in both the X and Y axes.
	/// </summary>
	/// <param name="valueX">The padding value for the X-axis. Default is 0.05. It should be between 0 and 1.</param>
	/// <param name="valueY">The padding value for the Y-axis. Default is 0.05. It should be between 0 and 1.</param>
	/// <returns>Returns an instance of IPlotBuilderFluentConfiguration to chain the configuration.</returns>
	/// <exception cref="Exception">Thrown when the calculated margin is not between 0 and 1 for both axes.</exception>
	IPlotBuilderFluentConfiguration SetDataPaddingWrtChannels(double valueX = 0.05, double valueY = 0.05);

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
	///  Determines the upper and lower y-axis limits
	///  </summary>	
	///  <returns>Fluent builder</returns>
	IPlotBuilderFluentConfiguration SetPlotLimits();

	/// <summary>
	///  Determines the upper and lower y-axis limits
	///  This overload takes into account default upper and lower values
	///  </summary>
	///  <param name="lower">Nullable lower limit; calculator will not go above this value (if not null)</param>
	///  <param name="upper">Nullable upper limit; calculator will not go below this value (if not null)</param>
	IPlotBuilderFluentConfiguration SetPlotLimits(double? upper, double? lower);

	/// <summary>
	///  Sets the margins of the plot. This affects the actual data area of the plot.
	///  This overload takes four parameters for each side of the plot
	/// </summary>
	/// <param name="right">Float-right padding</param>
	/// <param name="top">Float-top padding</param>
	/// <param name="bottom">Float-bottom padding</param>
	/// <param name="left">Float-left padding</param>
	/// <returns>Fluent builder</returns>
	IPlotBuilderFluentConfiguration SetDataLayout(float right, float top, float bottom, float left);

	/// <summary>
	///  Finalizes the configuration of the plot. This should be the last call in the fluent API.
	///  This allows you to call Produce().
	/// </summary>
	/// <returns>Fluent interface allowing consumer to Produce()</returns>
	IPlotBuilderFluentReadyToProduce FinalizeConfiguration();

	/// <summary>
	///  Enables the second x-axis of each plot
	/// </summary>
	/// <returns>Fluent builder</returns>
	IPlotBuilderFluentConfiguration EnableSecondXAxis();

	/// <summary>
	/// Disables the second x-axis of each plot
	/// </summary>
	/// <returns>Fluent builder</returns>
	IPlotBuilderFluentConfiguration DisableSecondXAxis();

	/// <summary>
	///  Enables the second y-axis of each plot
	/// </summary>
	/// <returns>Fluent builder</returns>
	IPlotBuilderFluentConfiguration EnableSecondYAxis();

	/// <summary>
	/// Disables the second y-axis of each plot
	/// </summary>
	/// <param name="keepVisible">If true, axis will still be rendered</param>
	/// <returns>Fluent builder</returns>
	IPlotBuilderFluentConfiguration DisableSecondYAxis(bool keepVisible = true);

	/// <summary>
	///  Rotates the tick labels on the X axis
	/// </summary>
	/// <param name="rotation">Rotation amount (counter-clockwise</param>
	/// <param name="isBold">Flag to bold tick labels</param>
	/// <param name="fontSize">Size of tick font</param>
	/// <returns>Fluent builder</returns>
	IPlotBuilderFluentConfiguration SetXAxisTicks(PlotAxisRotation rotation, bool isBold = false, int fontSize = 14);

	/// <summary>
	///  Rotates the tick labels on the Y axis
	/// </summary>
	/// <param name="rotation">Rotation amount (counter-clockwise</param>
	/// <param name="isBold">Flag to bold tick labels</param>
	/// <param name="fontSize">Size of tick font</param>
	/// <returns>Fluent builder</returns>
	IPlotBuilderFluentConfiguration SetYAxisTicks(PlotAxisRotation rotation, bool isBold = false, int fontSize = 14);

	/// <summary>
	///  Rotates the tick labels on the secondary Y axis
	/// </summary>
	/// <param name="rotation">Rotation amount (counter-clockwise</param>
	/// <param name="isBold">Flag to bold tick labels</param>
	/// <param name="fontSize">Size of tick font</param>
	/// <returns>Fluent builder</returns>
	IPlotBuilderFluentConfiguration SetSecondaryYAxisTicks(PlotAxisRotation rotation, bool isBold = false, int
		fontSize = 14);

	/// <summary>
	///  Rotates the tick labels on the secondary X axis
	/// </summary>
	/// <param name="rotation">Rotation amount (counter-clockwise</param>
	/// <param name="isBold">Flag to bold tick labels</param>
	/// <param name="fontSize">Size of tick font</param>
	/// <returns>Fluent builder</returns>
	IPlotBuilderFluentConfiguration SetSecondaryXAxisTicks(PlotAxisRotation rotation, bool isBold = false, int
		fontSize = 14);

	/// <summary>
	///  Sets the layout of the plot; this is the padding around the plot
	/// </summary>
	/// <param name="left">Left padding</param>
	/// <param name="right">Right padding</param>
	/// <param name="top">Top padding</param>
	/// <param name="bottom">Bottom padding</param>
	/// <param name="padding">Additional padding</param>
	/// <returns>Fluent builder</returns>
	/// <returns>Fluent builder</returns>
	IPlotBuilderFluentConfiguration SetLayout(
		float? left = 50f
		, float? right = 50f
		, float? top = 50f
		, float? bottom = 50f
		, float? padding = 0f);

	/// <summary>
	///  Sets the font for the legend. Currently can set font size and boldness.
	/// </summary>
	/// <param name="fontSize">Font size</param>
	/// <param name="isBold">If true, font will be bold</param>
	/// <returns>Fluent builder</returns>
	IPlotBuilderFluentConfiguration SetLegendFont(float fontSize = 14f, bool isBold = false);
}