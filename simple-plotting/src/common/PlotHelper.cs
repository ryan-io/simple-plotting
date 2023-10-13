using ScottPlot;

namespace simple_plotting.src {
	public static class PlotHelper {
		public static void SetSizeOfAll(IEnumerable<Plot> plots, PlotSize size) {
			var sizeContainer = PlotSizeMapper.Map(size);

			foreach (var plot in plots) {
				plot.Width  = sizeContainer.Width;
				plot.Height = sizeContainer.Height;
			}
		}
	}
}