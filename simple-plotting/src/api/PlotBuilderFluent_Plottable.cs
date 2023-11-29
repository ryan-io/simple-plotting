// simple-plotting

using System.Drawing;
using ScottPlot;
using ScottPlot.Plottable;

namespace simple_plotting;

public partial class PlotBuilderFluent {
    /// <inheritdoc />
    public IPlotBuilderFluentPlottables Remove<T> (Plot? plot, IPlottable? plottable)
        where T : class, IPlottable {
        if (plot == null || plottable == null)
            return this;

        var plottables = plot.GetPlottables();

        if (plottables.Contains(plottable))
            plot.Remove(plottable);

        return this;
    }

    /// <inheritdoc />
    public IPlotBuilderFluentPlottables AddDraggableLine (
        int plotIndex, 
        double posX, double posY,
        out DraggableArrow marker, 
        float arrowTailSize = 3.0f, 
        float arrowHeadWidth = 1.0f, 
        float arrowHeadLength = 5.0f, 
        bool isSnap = false,
        double? arrowTipXLocation = null, double? arrowTipYLocation = null) {
        plotIndex.ValidateInRange(_plots);

        var plot = _plots[plotIndex];

        var tipLocX = arrowTipXLocation == null ? posX + 0.2d : arrowTipXLocation;
        var tipLocY = arrowTipXLocation == null ? posY + 0.2d : arrowTipYLocation;

        marker = new DraggableArrow(posX, posY, tipLocX.Value, tipLocY.Value) {
            Color = Color.Black,
            MarkerSize = arrowTailSize,
            DragEnabled = true,
            ArrowheadWidth = arrowHeadWidth,
            ArrowheadLength = arrowHeadLength
        };

        // TODO - data is ALWAYS pulled from channel with index 0. this is not necessarily the correct logic
        if (isSnap) {
            var x = _data[0].Records.Select(x => x.DateTime.ToOADate()).ToArray();
            var y = _data[0].Records.Select(y => y.Value).ToArray();

            marker.DragSnap = new ScottPlot.SnapLogic.Nearest2D(x, y);
        }

        plot.Add(marker);

        return this;
    }

    /// <inheritdoc />
    public IPlotBuilderFluentPlottables Remove<T> (int plotIndex, IPlottable plottable) where T : class, IPlottable {
        var plottables = GetPlottablesAs<T>(plotIndex);
        var contains = plottables.Contains(plottable);

        if (!contains)
            throw new Exception(Message.EXCEPTION_PLOT_DOES_NOT_CONTAIN_PLOTTABLE);

        _plots[plotIndex].Remove(plottable);

        return this;
    }

    /// <inheritdoc />
    public IPlotBuilderFluentPlottables Remove<T> (int plotIndex, int plottableIndex) where T : class, IPlottable {
        _plots[plotIndex].RemoveAt(plottableIndex);

        return this;
    }

    /// <inheritdoc />
    public IPlotBuilderFluentPlottables RemoveAll<T> (int plotIndex) where T : class, IPlottable {
        var plot = _plots[plotIndex];
        var plottables = GetPlottablesAs<T>(plotIndex);

        foreach (var plottable in plottables) {
            plot.Remove(plottable);
        }

        return this;
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public IPlotBuilderFluentPlottables WithCrosshair (int plotIndex, out Crosshair crosshair, bool isXDataDate = false,
        double positionX = 0, double positionY = 0) {
        plotIndex.ValidateInRange(_plots);

        var plot = _plots[plotIndex];

        crosshair = plot.AddCrosshair(0, 0);

        if (isXDataDate)
            crosshair.VerticalLine.PositionFormatter = p => DateTime.FromOADate(p).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");

        return this;
    }

    /// <inheritdoc />
    public IPlotBuilderFluentPlottables WithMarker (int plotIndex, out MarkerPlot marker) {
        plotIndex.ValidateInRange(_plots);

        var plot = _plots[plotIndex];

        marker = plot.AddPoint(0, 0);

        marker.Color = Color.Fuchsia;
        marker.MarkerSize = 10;
        marker.MarkerShape = MarkerShape.openCircle;

        return this;
    }

    /// <inheritdoc />
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
    /// <returns>Annotation instance</returns>
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
    /// <returns>Crosshair instance</returns>
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
    /// <returns>MarkerPlot instance</returns>
    public static MarkerPlot WithMark (Plot plot) {
        var marker = plot.AddPoint(0, 0);

        marker.Color = Color.Fuchsia;
        marker.MarkerSize = 10;
        marker.MarkerShape = MarkerShape.openCircle;

        return marker;
    }
}