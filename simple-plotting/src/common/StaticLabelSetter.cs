using ScottPlot.Plottable;

namespace simple_plotting.src {
	/// <summary>
	///  Helper class for setting labels on plottables.
	/// </summary>
	internal static class StaticLabelSetter {
		/// <summary>
		///  Set all labels for all plottables in enumerable of type ScatterPlot
		/// </summary>
		/// <param name="newLabel">New label</param>
		/// <param name="plottables">Enumerable of ScatterPlots</param>
		/// <param name="indices">Index array</param>
		public static void SetScatterPlotLabels(
			ref string newLabel,
			ref IEnumerable<List<ScatterPlot>> plottables,
			ref int[] indices) {
			foreach (var plotty in plottables) {
				if (plotty.IsNullOrEmpty())
					continue;

				foreach (var index in indices) {
					if (index > plotty.Count)
						continue;

					var scatter = plotty[index];
					scatter.Label = newLabel;
				}
			}
		}

		/// <summary>
		///  Set all labels for all plottables in enumerable of type SignalPlotConstXY
		/// </summary>
		/// <param name="newLabel">New label</param>
		/// <param name="plottables">Enumerable of SignalPlotConstXY</param>
		/// <param name="indices">Index array</param>
		public static void SetSignalPlotLabels(ref string newLabel,
			ref IEnumerable<List<SignalPlotXYConst<double, double>>> plottables,
			ref int[] indices) {
			foreach (var plotty in plottables) {
				if (plotty.IsNullOrEmpty())
					continue;

				foreach (var index in indices) {
					if (index > plotty.Count)
						continue;

					var scatter = plotty[index];
					scatter.Label = newLabel;
				}
			}
		}
	}
}