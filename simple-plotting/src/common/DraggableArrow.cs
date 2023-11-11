using System.Drawing;
using ScottPlot;
using ScottPlot.Plottable;
using ScottPlot.SnapLogic;

namespace simple_plotting {
    /// <summary>
    ///  Adds a draggable arrow to a plot. Implements IPlottable, IDraggable, IHasPixelOffset, IHasLine, IHasColor.
    /// </summary>
    public class DraggableArrow : IPlottable, IDraggable, IHasPixelOffset, IHasLine, IHasColor { 
        /// <summary>
        ///  Event handler for when the arrow is dragged
        /// </summary>
        public event EventHandler Dragged = delegate { };

        /// <summary>
        ///  Cursor to display while hovering over this marker if dragging is enabled.
        /// </summary>
        public Cursor DragCursor => Cursor.Hand;

        /// <summary>
        ///  Indicates whether this marker is draggable in user controls.
        /// </summary>
        public bool DragEnabled { get; set; } = true;

        /// <summary>
        ///  This is the snap logic for the arrow
        /// </summary>
        public ISnap2D DragSnap { get; set; } = new NoSnap2D();

        /// <summary>
        ///  If dragging is enabled the marker cannot be dragged more negative than this position
        /// </summary>
        public double DragXLimitMin { get; set; } = double.NegativeInfinity;

        /// <summary>
        ///  If dragging is enabled the marker cannot be dragged more positive than this position
        /// </summary>
        public double DragXLimitMax { get; set; } = double.PositiveInfinity;

        /// <summary>
        ///  If dragging is enabled the marker cannot be dragged more negative than this position
        /// </summary>
        public double DragYLimitMin { get; set; } = double.NegativeInfinity;

        /// <summary>
        ///  If dragging is enabled the marker cannot be dragged more positive than this position
        /// </summary>
        public double DragYLimitMax { get; set; } = double.PositiveInfinity;

        /// <summary>
        /// Coordinates of the base of the arrow
        /// </summary>
        public Coordinate Base = new(0, 0);

        /// <summary>
        /// Coordinates of the arrow tip
        /// </summary>
        public Coordinate Tip = new(0, 0);

        /// <summary>
        /// Color of the arrow and arrowhead
        /// </summary>
        public Color Color { get; set; } = Color.Black;

        /// <summary>
        /// Color of the arrow and arrowhead
        /// </summary>
        public Color LineColor {
            get => Color;
            set { Color = value; }
        }

        /// <summary>
        /// Thickness of the arrow line
        /// </summary>
        public double LineWidth { get; set; } = 2;

        /// <summary>
        /// Style of the arrow line
        /// </summary>
        public LineStyle LineStyle { get; set; } = LineStyle.Solid;

        /// <summary>
        /// Label to appear in the legend
        /// </summary>
        public string? Label { get; set; }

        /// <summary>
        /// Width of the arrowhead (pixels)
        /// </summary>
        public double ArrowheadWidth { get; set; } = 3;

        /// <summary>
        /// Height of the arrowhead (pixels)
        /// </summary>
        public double ArrowheadLength { get; set; } = 3;

        /// <summary>
        /// The arrow will be lengthened to ensure it is at least this size on the screen
        /// </summary>
        public float MinimumLengthPixels { get; set; } = 0;

        /// <summary>
        /// Marker to be drawn at the base (if MarkerSize > 0)
        /// </summary>
        public MarkerShape MarkerShape { get; set; } = MarkerShape.filledCircle;

        /// <summary>
        /// Size of marker (in pixels) to draw at the base
        /// </summary>
        public float MarkerSize { get; set; } = 0;

        /// <summary>
        ///  Indicates whether the arrow is visible
        /// </summary>
        public bool IsVisible { get; set; } = true;

        /// <summary>
        ///  X axis index
        /// </summary>
        public int XAxisIndex { get; set; } = 0;

        /// <summary>
        ///  Y axis index
        /// </summary>
        public int YAxisIndex { get; set; } = 0;

        /// <summary>
        ///  X pixel offset
        /// </summary>
        public float PixelOffsetX { get; set; } = 0;

        /// <summary>
        ///  Y pixel offset
        /// </summary>
        public float PixelOffsetY { get; set; } = 0;

        /// <summary>
        ///  True if mouse is hovering over the base of the arrow.
        /// </summary>
        public bool IsOverBase { get; set; } = false;

        /// <summary>
        ///  True if mouse is hovering over the head of the arrow
        /// </summary>
        public bool IsOverTip { get; set; } = false;

        /// <summary>
        ///  Move the marker to a new coordinate in plot space.
        /// </summary>
        /// <param name="coordinateX">X coordinate to move to</param>
        /// <param name="coordinateY">Y coordinate to move to</param>
        /// <param name="fixedSize">If true, will not resize the arrow</param>
        public void DragTo (double coordinateX, double coordinateY, bool fixedSize) {
            if (!DragEnabled)
                return;

            //Coordinate original = new(coordinateX, coordinateY);
            //  Coordinate snapped = DragSnap.Snap(original);

            // coordinateX = snapped.X;
            // coordinateY = snapped.Y;

            if (coordinateX < DragXLimitMin) coordinateX = DragXLimitMin;
            if (coordinateX > DragXLimitMax) coordinateX = DragXLimitMax;
            if (coordinateY < DragYLimitMin) coordinateY = DragYLimitMin;
            if (coordinateY > DragYLimitMax) coordinateY = DragYLimitMax;

            if (IsOverBase) {
                Base.X = coordinateX; Base.Y = coordinateY;
            }
            else if (IsOverTip) {
                Tip.X = coordinateX; Tip.Y = coordinateY;
            }

            Dragged(this, EventArgs.Empty);
        }

        /// <summary>
        ///  Checks if the arrow is under the mouse
        /// </summary>
        /// <param name="coordinateX">X coordinate to move to</param>
        /// <param name="coordinateY">Y coordinate to move to</param>
        /// <param name="snapX">Closest X coordinate to snap to</param>
        /// <param name="snapY">Closest Y coordinate to snap to</param>
        /// <returns></returns>
        public bool IsUnderMouse (double coordinateX, double coordinateY, double snapX, double snapY) {
            snapX *= 1.15;
            snapY *= 1.15;

            if (Math.Abs(Base.Y - coordinateY) <= snapY && Math.Abs(Base.X - coordinateX) <= snapX) {
                IsOverBase = true;
                IsOverTip = false;
                return true;
            }

            if (Math.Abs(Tip.Y - coordinateY) <= snapY && Math.Abs(Tip.X - coordinateX) <= snapX) {
                IsOverBase = false;
                IsOverTip = true;
                return true;
            }

            IsOverBase = false;
            IsOverTip = false;
            return false;
        }

        /// <summary>
        /// Renders the draggable arrow. This specific Render implementation is for GDI+ and overrides (new) the base
        /// class implementation.
        /// </summary>
        /// <param name="dims">Plot dimensions</param>
        /// <param name="bmp">Bitmap of the plot</param>
        /// <param name="lowQuality">If true, GDI API call will be rendered with low quality</param>
        public void Render (PlotDimensions dims, Bitmap bmp, bool lowQuality = false) {
            if (IsVisible == false)
                return;

            using var gfx = ScottPlot.Drawing.GDI.Graphics(bmp, dims, lowQuality);
            using var penLine = ScottPlot.Drawing.GDI.Pen(Color, LineWidth, LineStyle, true);
            
            var coord = new Coordinate(Base.X, Base.Y);
            var basePixel = dims.GetPixel(coord);
            var tipPixel = dims.GetPixel(Tip);

            basePixel.Translate(PixelOffsetX, -PixelOffsetY);
            tipPixel.Translate(PixelOffsetX, -PixelOffsetY);

            float lengthPixels = basePixel.Distance(tipPixel);

            if (lengthPixels < MinimumLengthPixels) {
                float expandBy = MinimumLengthPixels / lengthPixels;
                float dX = tipPixel.X - basePixel.X;
                float dY = tipPixel.Y - basePixel.Y;
                basePixel.X = tipPixel.X - dX * expandBy;
                basePixel.Y = tipPixel.Y - dY * expandBy;
            }

            MarkerTools.DrawMarker(gfx, new(basePixel.X, basePixel.Y), MarkerShape, MarkerSize, Color);

            penLine.CustomEndCap =
                new System.Drawing.Drawing2D.AdjustableArrowCap((float)ArrowheadWidth, (float)ArrowheadLength, true);
            penLine.StartCap = System.Drawing.Drawing2D.LineCap.Flat;

            gfx.DrawLine(penLine, basePixel.X, basePixel.Y, tipPixel.X, tipPixel.Y);
        }

        /// <summary>
        ///  Returns the axis limits of the arrow
        /// </summary>
        /// <returns>Instance of AxisLimits</returns>
        public AxisLimits GetAxisLimits () {
            double xMin = Math.Min(Base.X, Tip.X);
            double xMax = Math.Max(Base.X, Tip.X);
            double yMin = Math.Min(Base.Y, Tip.Y);
            double yMax = Math.Max(Base.Y, Tip.Y);
            return new AxisLimits(xMin, xMax, yMin, yMax);
        }

        /// <summary>
        ///  Returns the legend items for the arrow
        /// </summary>
        /// <returns>Array containing all LegendItems</returns>
        public LegendItem[] GetLegendItems () {
            LegendItem item = new(this) {
                label = Label,
                lineWidth = LineWidth,
                color = Color,
            };

            return LegendItem.Single(item);
        }

        /// <summary>
        ///  Validates the data for the arrow
        /// </summary>
        /// <param name="deep">Not currently in use</param>
        /// <exception cref="InvalidOperationException">Thrown if Base or Tip is finite</exception>
        public void ValidateData (bool deep = false) {
            if (!Base.IsFinite() || !Tip.IsFinite())
                throw new InvalidOperationException("Base and Tip coordinates must be finite");
        }

        /// <summary>
        ///  Creates a new draggable arrow
        /// </summary>
        /// <param name="xBase">Starting X coordinate</param>
        /// <param name="yBase">Starting Y coordinate</param>
        /// <param name="xTip">X coordinate for the tip of the arrow head</param>
        /// <param name="yTip">Y coordinate for the tip of the arrow head</param>
        public DraggableArrow (double xBase, double yBase, double xTip, double yTip) {
            Base.X = xBase;
            Base.Y = yBase;
            Tip.X = xTip;
            Tip.Y = yTip;
        }

        /// <summary>
        ///  Creates a new draggable arrow
        /// </summary>
        /// <param name="arrowBase">Coordinates for the base of the arrow</param>
        /// <param name="arrowTip">Coordinates for the tip of the arrow</param>
        public DraggableArrow (Coordinate arrowBase, Coordinate arrowTip) {
            Base.X = arrowBase.X;
            Base.Y = arrowTip.Y;
            Tip.X = arrowTip.X;
            Tip.Y = arrowTip.Y;
        }
    }
}