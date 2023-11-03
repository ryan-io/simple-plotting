// simple-plotting

using ScottPlot;
using ScottPlot.Plottable;

namespace simple_plotting.src;

/// <summary>
///  Abstraction for adding plottables to a plot (annotations, crosshairs, text, etc.)
/// </summary>
public interface IPlotBuilderFluentPlottables : IPlotBuilderFluent {
	/// <summary>
	///  Helper method to return back to the product.
	/// </summary>
	/// <returns>Instance product</returns>
	IPlotBuilderFluentProduct GoToProduct();

	/// <summary>
	///  Helper method that returns an enumerable of an enumerable of type T
	///  This is used to extract the plottables from the plots.
	///  This method invokes OfType with the generic type T.
	/// </summary>
	/// <typeparam name="T">Class that implements IPlottable</typeparam>
	/// <returns>Enumerable of enumerables containing the plottables as type T</returns>
	IEnumerable<T> GetPlottablesAs<T>(int plotIndex) where T : class, IPlottable;

	/// <summary>
	///  Adds an annotation to a plot at xOff, yOff.
	/// </summary>
	/// <param name="text">String text to display in annotation</param>
	/// <param name="index">Index of plot to annotate</param>
	/// <param name="xOff">x-offset (from lower-left of plot)</param>
	/// <param name="yOff">y-offset (from the lower-left of the plot)</param>
	/// <param name="annotation">Instance of Annotation plottable</param>
	/// <returns>Fluent builder as IPlotBuilderFluent_PostProcess</returns>
	IPlotBuilderFluentPlottables WithAnnotationAt(string text, int index, float xOff, float yOff, out Annotation
		annotation);

	/// <summary>
	///  Adds a text plottable to the plot at xCoord, yCoord.
	/// </summary>
	/// <param name="plotIndex">Index of your plot</param>
	/// <param name="text">"String to display</param>
	/// <param name="xCoord">X coordinate (relative to data)</param>
	/// <param name="yCoord">Y coordinate (relative to data)</param>
	/// <param name="textPlottable">Instance of Text plottable</param>
	/// <returns>Fluent builder as IPlotBuilderFluent_Plottable</returns>
	IPlotBuilderFluentPlottables WithText(int plotIndex, string text, double xCoord, double yCoord,
		out Text textPlottable);

	/// <summary>
	///  Adds a crosshair plottable to the plot.
	/// </summary>
	/// <param name="plotIndex">Index of your plot</param>
	/// <param name="crosshair">Instance of Crosshair plottable</param>
	/// <param name="isXDataDate">OPTIONAL; default = false; formats x-axis data for date time if true</param>
	/// <param name="positionX">OPTIONAL; default = 0.0; x-offset from bottom-left of plot</param>
	/// <param name="positionY">OPTIONAL; default = 0.0; y-offset from bottom-left of plot</param>
	/// <returns>Fluent builder as IPlotBuilderFluent_Plottable</returns>s
	IPlotBuilderFluentPlottables WithCrosshair(int plotIndex, out Crosshair crosshair, bool isXDataDate = false,
		double positionX = 0.0, double positionY = 0.0);

	/// <summary>
	///  Adds a marker plottable to the plot.
	/// </summary>
	/// <param name="plotIndex">Plot index to add marker to</param>
	/// <param name="marker">Instance of MarkerPlot plottable</param>
	/// <returns>Fluent builder as IPlotBuilderFluent_Plottable</returns>
	IPlotBuilderFluentPlottables WithMarker(int plotIndex, out MarkerPlot marker);

    /// <summary>
    ///  Removes a plottable of type 'T' with plottableIndex from a specified plot via plotIndex
    /// </summary>
    /// <typeparam name="T">Type of plottables to remove</typeparam>
    /// <param name="plotIndex">Plot index to remove the plottables of type 'T' from</param>
    /// <param name="plottableIndex">Plottale index to remove from plot</param>
    /// <returns>Fluent builder as IPlotBuilderFluentPlottables</returns>
    IPlotBuilderFluentPlottables Remove<T> (int plotIndex, int plottableIndex) where T : class, IPlottable;

	/// <summary>
	///  Removes all plottables of type 'T' from the plot specifiged by plotIndex
	/// </summary>
	/// <typeparam name="T">Type of plottables to remove</typeparam>
	/// <param name="plotIndex">Plot index to remove the plottables of type 'T' from</param>
	/// <returns>Fluent builder as IPlotBuilderFluentPlottables</returns>
	IPlotBuilderFluentPlottables RemoveAll<T> (int plotIndex) where T : class, IPlottable;

	/// <summary>
	///  Removes a plottable instance of type 'T' from a plot specified plotIndex
	/// </summary>
	/// <typeparam name="T">Type of plottables to remove</typeparam>
	/// <param name="plotIndex">Plot index to remove the plottables of type 'T' from</param>
	/// <param name="plottable">Plottable instance to remove</param>
	/// <returns>Fluent builder as IPlotBuilderFluentPlottables</returns>
	/// <exception cref="Exception">Thrown if plot does not contain plottle instance</exception>
	IPlotBuilderFluentPlottables Remove<T> (int plotIndex, IPlottable plottable) where T : class, IPlottable;

    /// <summary>
    ///  Removes a plottable instance of type 'T' from a plot specified plotIndex
    /// </summary>
    /// <typeparam name="T">Type of plottables to remove</typeparam>
    /// <param name="plot">Plotto remove the plottables of type 'T' from</param>
    /// <param name="plottable">Plottable instance to remove</param>
    /// <returns>Fluent builder as IPlotBuilderFluentPlottables</returns>
    /// <exception cref="Exception">Thrown if plot does not contain plottle instance</exception>
    IPlotBuilderFluentPlottables Remove<T> (Plot? plot, IPlottable? plottable) where T : class, IPlottable;
    
    /// <summary>
    ///  Adds a draggable line to the plot at posX, posY
    /// </summary>
    /// <param name="plotIndex">Plot index to add line to</param>
    /// <param name="posX">Plot coordinate x</param>
    /// <param name="posY">Plot coordinate y</param>
    /// <param name="marker">Out DraggableMarkerPlot</param>
    /// <returns>Fluent builder</returns>
    IPlotBuilderFluentPlottables AddDraggableLine(int plotIndex, double posX, double posY,
	    out DraggableMarkerPlot marker);
}