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

	/// <summary>
	///  Takes an IPlottable, casts it to a ScatterPlot and sets the label.
	///  This method will invoke Render() on the plot.
	///  This version will set the label for all plots.
	/// </summary>
	/// <param name="newLabel">New label</param>
	/// <returns>Fluent builder as IPlotBuilderFluent_PostProcess</returns>
	/// <exception cref="NullReferenceException">Thrown if plottable cast fails</exception>
	IPlotBuilderFluentPostProcess TrySetScatterLabelAll(string newLabel);

	/// <summary>
	/// Changes the title for all plots
	/// </summary>
	/// <param name="newTitle">The new plot title</param>
	/// <param name="fontSize">Title font size</param>
	/// <param name="isBold">Whether the title is bold or not</param>
	/// <returns>Fluent builder</returns>
	IPlotBuilderFluentPostProcess ChangeTitle(string newTitle, int fontSize = 14, bool isBold = false);
	
	/// <summary>
	/// Invokes Render on all plots.
	/// </summary>
	IPlotBuilderFluentPostProcess RefreshRenderers();

	/// <summary>
	///  Generically sets labels defined in plottableIndices for ALL plots.
	/// </summary>
	/// <param name="newLabel">New label</param>
	/// <param name="plottableIndices">Array of label indices to change</param>
	/// <returns></returns>
	/// <exception cref="NullReferenceException">Thrown if newLabel is null or whitespace</exception>
	 IPlotBuilderFluentPostProcess TrySetLabel(string newLabel, params int[] plottableIndices);
}