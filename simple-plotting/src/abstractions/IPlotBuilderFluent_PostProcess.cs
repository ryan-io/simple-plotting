// simple-plotting

using ScottPlot;
using ScottPlot.Plottable;

namespace simple_plotting.src;

/// <summary>
///  Provides the consumer with additional tools to manipulate the plots. Any interactive plot features should be defined here.
/// </summary>
public interface IPlotBuilderFluentPostProcess {
	/// <summary>
	///  Helper method to return back to the product.
	/// </summary>
	/// <returns>Instance product</returns>
	IPlotBuilderFluentProduct GoToProduct();

	/// <summary>
	///  Adds an annotation to a plot at xOff, yOff.
	/// </summary>
	/// <param name="annotation">String text to display in annotation</param>
	/// <param name="plot">Plot to annotate</param>
	/// <param name="xOff">x-offset (from lower-left of plot)</param>
	/// <param name="yOff">y-offset (from the lower-left of the plot)</param>
	/// <returns>Fluent builder as IPlotBuilderFluent_PostProcess</returns>
	IPlotBuilderFluentPostProcess WithAnnotationAt(string annotation, Plot plot, float xOff, float yOff);

	/// <summary>
	///  Sets the size of all plots.
	/// </summary>
	/// <param name="size">New plot size</param>
	/// <returns>Fluent builder as IPlotBuilderFluent_PostProcess</returns>
	IPlotBuilderFluentPostProcess SetSizeOfAll(PlotSize size);

	/// <summary>
	///  Takes an IPlottable, casts it to a ScatterPlot and sets the label.
	/// </summary>
	/// <param name="plot">IPlottable to change the label fro</param>
	/// <param name="newLabel">New label</param>
	/// <returns>Fluent builder as IPlotBuilderFluent_PostProcess</returns>
	IPlotBuilderFluentPostProcess SetScatterLabel(IPlottable? plot, string newLabel);

	/// <summary>
	///  Takes an IPlottable, casts it to a ScatterPlot and sets the label.
	///  This method will invoke Render() on the plot.
	/// </summary>
	/// <param name="newLabel">New label</param>
	/// <param name="plottableIndex">Plottable index to adjust label for</param>
	/// <returns>Fluent builder as IPlotBuilderFluent_PostProcess</returns>
	/// <exception cref="NullReferenceException">Thrown if plottable cast fails</exception>
	IPlotBuilderFluentPostProcess TrySetScatterLabel(string newLabel, params int[] plottableIndex);
}