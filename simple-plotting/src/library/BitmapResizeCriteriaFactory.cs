using System.Drawing.Drawing2D;

namespace simple_plotting {
	public static class BitmapResizeCriteriaFactory {
		public static BitmapResizeCriteria HighQualityBicubic => new (
			CompositingMode.SourceCopy,
			CompositingQuality.HighQuality,
			InterpolationMode.HighQualityBicubic,
			SmoothingMode.HighQuality,
			PixelOffsetMode.HighQuality);
	}
}