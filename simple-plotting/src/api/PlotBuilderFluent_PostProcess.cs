// simple-plotting

using ScottPlot;
using ScottPlot.Plottable;

namespace simple_plotting.src;

public partial class PlotBuilderFluent {
	/// <summary>
	///  Helper method to return back to the product.
	/// </summary>
	/// <returns>Instance product</returns>
	public IPlotBuilderFluentProduct GoToProduct() => this;

	/// <summary>
	///  Adds an annotation to a plot at xOff, yOff.
	/// </summary>
	/// <param name="annotation">String text to display in annotation</param>
	/// <param name="plot">Plot to annotate</param>
	/// <param name="xOff">x-offset (from lower-left of plot)</param>
	/// <param name="yOff">y-offset (from the lower-left of the plot)</param>
	/// <returns></returns>
	public IPlotBuilderFluentPostProcess WithAnnotationAt(string annotation, Plot plot, float xOff, float yOff) {
		var a = plot.AddAnnotation(annotation, Alignment.LowerLeft);
		a.Border      = true;
		a.BorderWidth = 1;
		a.MarginX     = xOff;
		a.MarginY     = yOff;

		return this;
	}

	/// <summary>
	///  Sets the size of all plots.
	/// </summary>
	/// <param name="size">New plot size</param>
	/// <returns>Fluent builder as IPlotBuilderFluent_PostProcess</returns>
	public IPlotBuilderFluentPostProcess SetSizeOfAll(PlotSize size) {
		PlotHelper.SetSizeOfAll(_plots, size);
		return this;
	}

	/// <summary>
	///  Takes an IPlottable, casts it to a ScatterPlot and sets the label.
	///  This version REQUIRES you call Render() on the appropriate plot.
	/// </summary>
	/// <param name="plot">IPlottable to change the label fro</param>
	/// <param name="newLabel">New label</param>
	/// <returns>Fluent builder as IPlotBuilderFluent_PostProcess</returns>
	public IPlotBuilderFluentPostProcess SetScatterLabel(IPlottable? plot, string newLabel) {
		if (plot is null)
			return this;

		var scatterPlot = (ScatterPlot)plot;
		scatterPlot.Label = newLabel;
		RefreshRender();
		
		return this;
	}

	/// <summary>
	///  Takes an IPlottable, casts it to a ScatterPlot and sets the label.
	///  This method will invoke Render() on the plot.
	/// </summary>
	/// <param name="newLabel">New label</param>
	/// <param name="plottableIndex">Plottable index to adjust label for</param>
	/// <returns>Fluent builder as IPlotBuilderFluent_PostProcess</returns>
	/// <exception cref="NullReferenceException">Thrown if plottable cast fails</exception>
	public IPlotBuilderFluentPostProcess TrySetScatterLabel(string newLabel, params int[] plottableIndex) {
		var plottables = GetPlottablesAs<ScatterPlot>();

		foreach (var plotty in plottables) {
			if (plotty.IsNullOrEmpty())
				continue;
			
			foreach (var index in plottableIndex) {
				if (index > plotty.Count)
					continue;

				var scatter = plotty[index];
				scatter.Label = newLabel;
			}
		}
		
		RefreshRender();
		
		return this;
	}
}