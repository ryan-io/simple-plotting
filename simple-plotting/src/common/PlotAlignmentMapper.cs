// simple-plotting

using ScottPlot;

namespace simple_plotting.src;

internal static class PlotAlignmentMapper {
	internal static Alignment Map(PlotAlignment alignment) {
		switch (alignment) {
			case PlotAlignment.LowerCenter:
				return Alignment.LowerCenter;
			case PlotAlignment.LowerLeft:
				return Alignment.LowerLeft;
			case PlotAlignment.LowerRight:
				return Alignment.LowerRight;
			case PlotAlignment.MiddleCenter:
				return Alignment.MiddleCenter;
			case PlotAlignment.MiddleLeft:
				return Alignment.MiddleLeft;
			case PlotAlignment.MiddleRight:
				return Alignment.MiddleRight;
			case PlotAlignment.UpperCenter:
				return Alignment.UpperCenter;
			case PlotAlignment.UpperLeft:
				return Alignment.UpperLeft;
			case PlotAlignment.UpperRight:
				return Alignment.UpperRight;
			default:
				throw new ArgumentOutOfRangeException(nameof(alignment), alignment, null);
		}
	}
}