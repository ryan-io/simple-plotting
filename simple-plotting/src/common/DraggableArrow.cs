using System.Drawing;
using ScottPlot;
using ScottPlot.Plottable;
using ScottPlot.SnapLogic;

namespace simple_plotting.src {
	/// <summary>
	///  Adds a draggable arrow to a plot. Implements IPlottable.
	/// </summary>
	public class DraggableArrow : ArrowCoordinated, IDraggable {
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
		///  X coordinate of the arrow
		/// </summary>
		double XCoord { get; set; }

		/// <summary>
		///  Y coordinate of the arrow
		/// </summary>
		double YCoord { get; set; }

		public int DebugRenderCount { get; private set; }
		
		/// <summary>
		///  If dragging is enabled the marker cannot be dragged more negative than this position
		/// </summary>
		double DragXLimitMin { get; set; } = double.NegativeInfinity;

		/// <summary>
		///  If dragging is enabled the marker cannot be dragged more positive than this position
		/// </summary>
		double DragXLimitMax { get; set; } = double.PositiveInfinity;

		/// <summary>
		///  If dragging is enabled the marker cannot be dragged more negative than this position
		/// </summary>
		double DragYLimitMin { get; set; } = double.NegativeInfinity;

		/// <summary>
		///  If dragging is enabled the marker cannot be dragged more positive than this position
		/// </summary>
		double DragYLimitMax { get; set; } = double.PositiveInfinity;

		/// <summary>
		///  Move the marker to a new coordinate in plot space.
		/// </summary>
		/// <param name="coordinateX">X coordinate to move to</param>
		/// <param name="coordinateY">Y coordinate to move to</param>
		/// <param name="fixedSize">If true, will not resize the arrow</param>
		public void DragTo(double coordinateX, double coordinateY, bool fixedSize) {
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

			XCoord = coordinateX;
			YCoord = coordinateY;

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
		public bool IsUnderMouse(double coordinateX, double coordinateY, double snapX, double snapY)
			=> Math.Abs(YCoord - coordinateY) <= snapY && Math.Abs(XCoord - coordinateX) <= snapX;

		/// <summary>
		/// Renders the draggable arrow. This specific Render implementation is for GDI+ and overrides (new) the base
		/// class implementation.
		/// </summary>
		/// <param name="dims">Plot dimensions</param>
		/// <param name="bmp">Bitmap of the plot</param>
		/// <param name="lowQuality">If true, GDI API call will be rendered with low quality</param>
		public new void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false) {
			DebugRenderCount++;
			
			if (IsVisible == false)
				return;

			using var gfx     = ScottPlot.Drawing.GDI.Graphics(bmp, dims, lowQuality);
			using var penLine = ScottPlot.Drawing.GDI.Pen(Color, LineWidth, LineStyle, true);

			var basePixel = dims.GetPixel(Base);
			var tipPixel  = dims.GetPixel(Tip);

			basePixel.Translate(PixelOffsetX, -PixelOffsetY);
			tipPixel.Translate(PixelOffsetX, -PixelOffsetY);

			float lengthPixels = basePixel.Distance(tipPixel);
			
			if (lengthPixels < MinimumLengthPixels) {
				float expandBy = MinimumLengthPixels / lengthPixels;
				float dX       = tipPixel.X - basePixel.X;
				float dY       = tipPixel.Y - basePixel.Y;
				basePixel.X = tipPixel.X - dX * expandBy;
				basePixel.Y = tipPixel.Y - dY * expandBy;
			}

			gfx.RotateTransform(90);
			MarkerTools.DrawMarker(gfx, new(basePixel.X, basePixel.Y), MarkerShape, MarkerSize, Color);

			penLine.CustomEndCap =
				new System.Drawing.Drawing2D.AdjustableArrowCap((float)ArrowheadWidth, (float)ArrowheadLength, true);
			penLine.StartCap = System.Drawing.Drawing2D.LineCap.Flat;
			gfx.DrawLine(penLine, basePixel.X, basePixel.Y, tipPixel.X, tipPixel.Y);
		}

		/// <summary>
		///  Creates a new draggable arrow
		/// </summary>
		/// <param name="xBase">Starting X coordinate</param>
		/// <param name="yBase">Starting Y coordinate</param>
		/// <param name="xTip">X coordinate for the tip of the arrow head</param>
		/// <param name="yTip">Y coordinate for the tip of the arrow head</param>
		public DraggableArrow(double xBase, double yBase, double xTip, double yTip)
			: base(xBase, yBase, xTip, yTip) {
		}
	}
}