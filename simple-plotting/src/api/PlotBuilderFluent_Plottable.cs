// simple-plotting

using System.Drawing;
using ScottPlot;
using ScottPlot.Plottable;

namespace simple_plotting.src;

public partial class PlotBuilderFluent {
	/// <summary>
	///  Adds an annotation to a plot at xOff, yOff.
	/// </summary>
	/// <param name="text">String text to display in annotation</param>
	/// <param name="index">Index of plot to annotate</param>
	/// <param name="xOff">x-offset (from lower-left of plot)</param>
	/// <param name="yOff">y-offset (from the lower-left of the plot)</param>
	/// <param name="annotation">Instance of Annotation plottable</param>
	/// <returns>Fluent builder as IPlotBuilderFluent_PostProcess</returns>
	public IPlotBuilderFluentPlottables WithAnnotationAt(string text, int index, float xOff, float yOff, out
		Annotation annotation) {
		index.ValidateInRange(_plots);

		var plot = _plots[index];
		annotation = plot.AddAnnotation(text, Alignment.LowerLeft);

		annotation.Border      = true;
		annotation.BorderWidth = 1;
		annotation.MarginX     = xOff;
		annotation.MarginY     = yOff;

		return this;
	}

	/// <summary>
	///  Adds a crosshair plottable to the plot.
	/// </summary>
	/// <param name="plotIndex">Index of your plot</param>
	/// <param name="crosshair">Instance of Crosshair plottable</param>
	/// <param name="isXDataDate">OPTIONAL; default = false; formats x-axis data for date time if true</param>
	/// <param name="positionX">OPTIONAL; default = 0.0; x-offset from bottom-left of plot</param>
	/// <param name="positionY">OPTIONAL; default = 0.0; y-offset from bottom-left of plot</param>
	/// <returns></returns>
	public IPlotBuilderFluentPlottables WithCrosshair(int plotIndex, out Crosshair crosshair, bool isXDataDate = false,
		double positionX = 0, double positionY = 0) {
		plotIndex.ValidateInRange(_plots);

		var plot = _plots[plotIndex];

		crosshair = plot.AddCrosshair(0, 0);

		if (isXDataDate)
			crosshair.VerticalLine.PositionFormatter = p => DateTime.FromOADate(p).ToString("d");

		return this;
	}

	/// <summary>
	///  Adds a marker plottable to the plot.
	/// </summary>
	/// <param name="plotIndex">Plot index to add marker to</param>
	/// <param name="marker">Instance of MarkerPlot plottable</param>
	/// <returns></returns>
	public IPlotBuilderFluentPlottables WithMarker(int plotIndex, out MarkerPlot marker) {
		plotIndex.ValidateInRange(_plots);

		var plot = _plots[plotIndex];

		marker = plot.AddPoint(0, 0);
		
		marker.Color       = Color.Fuchsia;
		marker.MarkerSize = 10;
		marker.MarkerShape = MarkerShape.openCircle;

		return this;
	}

	/// <summary>
	///  Static method for adding an annotation to a plot at xOff, yOff.
	/// </summary>
	/// <param name="plot">Plot to add the annotation to</param>
	/// <param name="text">String text to display in annotation</param>
	/// <param name="xOff">x-offset (from lower-left of plot)</param>
	/// <param name="yOff">y-offset (from the lower-left of the plot)</param>
	/// <returns></returns>
	public static Annotation WithAnnotationAt(Plot plot, string text, float xOff, float yOff) {
		var annotation = plot.AddAnnotation(text, Alignment.LowerLeft);

		annotation.Border      = true;
		annotation.BorderWidth = 1;
		annotation.MarginX     = xOff;
		annotation.MarginY     = yOff;

		return annotation;
	}

	/// <summary>
	///  Static method for adding a crosshair plottable to the plot.
	/// </summary>
	/// <param name="plot">Plot to add the crosshair to</param>
	/// <param name="isXDataDate">OPTIONAL; default = false; formats x-axis data for date time if true</param>
	/// <returns></returns>
	public static Crosshair WithCrosshair(Plot plot, bool isXDataDate = false) {
		var crosshair = plot.AddCrosshair(0, 0);

		if (isXDataDate)
			crosshair.VerticalLine.PositionFormatter = p => DateTime.FromOADate(p).ToString("d");

		return crosshair;
	}

	/// <summary>
	///  Static method for adding a marker plottable to the plot.
	/// </summary>
	/// <param name="plot">Plot to add a marker to</param>
	/// <returns></returns>
	public static MarkerPlot WithMark(Plot plot) {
		var marker = plot.AddPoint(0, 0);
		
		marker.Color       = Color.Fuchsia;
		marker.MarkerSize  = 10;
		marker.MarkerShape = MarkerShape.openCircle;

		return marker;
	}
}