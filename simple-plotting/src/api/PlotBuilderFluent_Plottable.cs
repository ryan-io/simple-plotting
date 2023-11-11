// simple-plotting

using System.Drawing;
using ScottPlot;
using ScottPlot.Plottable;

namespace simple_plotting;

public partial class PlotBuilderFluent {
    /// <summary>
    ///  Removes a plottable instance of type 'T' from a plot specified plotIndex
    /// </summary>
    /// <typeparam name="T">Type of plottables to remove</typeparam>
    /// <param name="plot">Plot remove the plottables of type 'T' from</param>
    /// <param name="plottable">Plottable instance to remove</param>
    /// <returns>Fluent builder as IPlotBuilderFluentPlottables</returns>
    /// <exception cref="Exception">Thrown if plot does not contain plottable instance</exception>
    public IPlotBuilderFluentPlottables Remove<T> (Plot? plot, IPlottable? plottable)
        where T : class, IPlottable {
        if (plot == null || plottable == null)
            return this;

        var plottables = plot.GetPlottables();

        if (plottables.Contains(plottable))
            plot.Remove(plottable);

        return this;
    }

    /// <summary>
    ///  Adds a draggable line to the plot at posX, posY
    /// </summary>
    /// <param name="plotIndex">Plot index to add line to</param>
    /// <param name="posX">Plot coordinate x</param>
    /// <param name="posY">Plot coordinate y</param>
    /// <param name="marker">Out DraggableMarkerPlot</param>
    /// <param name="arrowHeadLength">The length of the line</param>
    /// <param name="isSnap">If true, the draggable line will snap to data points</param>
    /// <param name="arrowHeadWidth">The width of th eline</param>
    /// <param name="arrowTailSize">Size of the circle tail end of the arrow</param>
    /// <returns>Fluent builder</returns>
    public IPlotBuilderFluentPlottables AddDraggableLine (int plotIndex, double posX, double posY,
        out DraggableArrow marker, float arrowTailSize = 3.0f, float arrowHeadWidth = 1.0f, float arrowHeadLength = 5.0f, bool isSnap = false,
        double? arrowTipXLocation = null, double? arrowTipYLocation = null) {
        plotIndex.ValidateInRange(_plots);

        var plot = _plots[plotIndex];

        var tipLocX = arrowTipXLocation == null ? posX + 0.2d : arrowTipXLocation;
        var tipLocY = arrowTipXLocation == null ? posY + 0.2d : arrowTipYLocation;

        marker = new DraggableArrow(posX, posY, tipLocX.Value, tipLocY.Value) {
            //marker = plot.AddMarkerDraggable(posX, posY, MarkerShape.verticalBar);
            Color = Color.Black,
            MarkerSize = arrowTailSize,
            DragEnabled = true,
            ArrowheadWidth = arrowHeadWidth,
            ArrowheadLength = arrowHeadLength
        };

        //marker.MarkerLineWidth = width;

        // TODO - data is ALWAYS pulled from channel with index 0. this is not necessarily the correct logic
        if (isSnap) {
            var x = _data[0].Records.Select(x => x.DateTime.ToOADate()).ToArray();
            var y = _data[0].Records.Select(y => y.Value).ToArray();

            marker.DragSnap = new ScottPlot.SnapLogic.Nearest2D(x, y);
        }

        plot.Add(marker);

        return this;
    }

    /// <summary>
    ///  Removes a plottable instance of type 'T' from a plot specified plotIndex
    /// </summary>
    /// <typeparam name="T">Type of plottables to remove</typeparam>
    /// <param name="plotIndex">Plot index to remove the plottables of type 'T' from</param>
    /// <param name="plottable">Plottable instance to remove</param>
    /// <returns>Fluent builder as IPlotBuilderFluentPlottables</returns>
    /// <exception cref="Exception">Thrown if plot does not contain plottable instance</exception>
    public IPlotBuilderFluentPlottables Remove<T> (int plotIndex, IPlottable plottable) where T : class, IPlottable {
        var plottables = GetPlottablesAs<T>(plotIndex);
        var contains = plottables.Contains(plottable);

        if (!contains)
            throw new Exception(Message.EXCEPTION_PLOT_DOES_NOT_CONTAIN_PLOTTABLE);

        _plots[plotIndex].Remove(plottable);

        return this;
    }

    /// <summary>
    ///  Removes a plottable of type 'T' with plottableIndex from a specified plot via plotIndex
    /// </summary>
    /// <typeparam name="T">Type of plottables to remove</typeparam>
    /// <param name="plotIndex">Plot index to remove the plottables of type 'T' from</param>
    /// <param name="plottableIndex">Plottable index to remove from plot</param>
    /// <returns>Fluent builder as IPlotBuilderFluentPlottables</returns>
    public IPlotBuilderFluentPlottables Remove<T> (int plotIndex, int plottableIndex) where T : class, IPlottable {
        _plots[plotIndex].RemoveAt(plottableIndex);

        return this;
    }

    /// <summary>
    ///  Removes all plottables of type 'T' from the plot specified by plotIndex
    /// </summary>
    /// <typeparam name="T">Type of plottables to remove</typeparam>
    /// <param name="plotIndex">Plot index to remove the plottables of type 'T' from</param>
    /// <returns>Fluent builder as IPlotBuilderFluentPlottables</returns>
    public IPlotBuilderFluentPlottables RemoveAll<T> (int plotIndex) where T : class, IPlottable {
        var plot = _plots[plotIndex];
        var plottables = GetPlottablesAs<T>(plotIndex);

        foreach (var plottable in plottables) {
            plot.Remove(plottable);
        }

        return this;
    }

    /// <summary>
    ///  Adds an annotation to a plot at xOff, yOff.
    /// </summary>
    /// <param name="text">String text to display in annotation</param>
    /// <param name="index">Index of plot to annotate</param>
    /// <param name="xOff">x-offset (from lower-left of plot)</param>
    /// <param name="yOff">y-offset (from the lower-left of the plot)</param>
    /// <param name="annotation">Instance of Annotation plottable</param>
    /// <returns>Fluent builder as IPlotBuilderFluentPlottables</returns>
    public IPlotBuilderFluentPlottables WithAnnotationAt (string text, int index, float xOff, float yOff, out
        Annotation annotation) {
        index.ValidateInRange(_plots);

        var plot = _plots[index];
        annotation = plot.AddAnnotation(text, Alignment.LowerLeft);

        annotation.Border = true;
        annotation.BorderWidth = 1;
        annotation.MarginX = xOff;
        annotation.MarginY = yOff;

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
    public IPlotBuilderFluentPlottables WithCrosshair (int plotIndex, out Crosshair crosshair, bool isXDataDate = false,
        double positionX = 0, double positionY = 0) {
        plotIndex.ValidateInRange(_plots);

        var plot = _plots[plotIndex];

        crosshair = plot.AddCrosshair(0, 0);

        if (isXDataDate)
            crosshair.VerticalLine.PositionFormatter = p => DateTime.FromOADate(p).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");

        return this;
    }

    /// <summary>
    ///  Adds a marker plottable to the plot.
    /// </summary>
    /// <param name="plotIndex">Plot index to add marker to</param>
    /// <param name="marker">Instance of MarkerPlot plottable</param>
    /// <returns></returns>
    public IPlotBuilderFluentPlottables WithMarker (int plotIndex, out MarkerPlot marker) {
        plotIndex.ValidateInRange(_plots);

        var plot = _plots[plotIndex];

        marker = plot.AddPoint(0, 0);

        marker.Color = Color.Fuchsia;
        marker.MarkerSize = 10;
        marker.MarkerShape = MarkerShape.openCircle;

        return this;
    }

    public IPlotBuilderFluentPlottables WithText (int plotIndex, string text, double xCoord, double yCoord,
        out Text textPlottable) {
        plotIndex.ValidateInRange(_plots);

        var plot = _plots[plotIndex];
        textPlottable = plot.AddText(text, xCoord, yCoord);

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
    public static Annotation WithAnnotationAt (Plot plot, string text, float xOff, float yOff) {
        var annotation = plot.AddAnnotation(text, Alignment.LowerLeft);

        annotation.Border = true;
        annotation.BorderWidth = 1;
        annotation.MarginX = xOff;
        annotation.MarginY = yOff;

        return annotation;
    }

    /// <summary>
    ///  Static method for adding a crosshair plottable to the plot.
    /// </summary>
    /// <param name="plot">Plot to add the crosshair to</param>
    /// <param name="isXDataDate">OPTIONAL; default = false; formats x-axis data for date time if true</param>
    /// <returns></returns>
    public static Crosshair WithCrosshair (Plot plot, bool isXDataDate = false) {
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
    public static MarkerPlot WithMark (Plot plot) {
        var marker = plot.AddPoint(0, 0);

        marker.Color = Color.Fuchsia;
        marker.MarkerSize = 10;
        marker.MarkerShape = MarkerShape.openCircle;

        return marker;
    }
}