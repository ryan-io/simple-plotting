using ScottPlot;

namespace simple_plotting.src {
	/// <summary>
	///  Static class containing helper methods for plots.
	/// </summary>
	public static class PlotHelper {
		/// <summary>
		///  Sets the size of all plots within the enumerable via mapping of size
		/// </summary>
		/// <param name="plots">Enumerable of plots</param>
		/// <param name="size">Size to set each plot to</param>
		public static void SetSizeOfAll(IEnumerable<Plot> plots, PlotSize size) {
			var sizeContainer = PlotSizeMapper.Map(size);

			foreach (var plot in plots) {
				plot.Width  = sizeContainer.Width;
				plot.Height = sizeContainer.Height;
			}
		}

		/// <summary>
		/// Takes a PlotSize and maps it to a PlotSizeContainer.
		/// </summary>
		/// <param name="size">PlotSize to map</param>
		/// <returns>PlotSizeContainer containing width & height</returns>
		public static PlotSizeContainer GetPlotSizeContainer(PlotSize size) => PlotSizeMapper.Map(size);
	}
}