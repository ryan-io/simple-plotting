using ScottPlot.Plottable;

namespace simple_plotting {
	// TODO: Add documentation
	public readonly struct ScatterPlotProduct {
		public IPlottableFactoryReset Factory     { get; }
		public ScatterPlot?           ScatterPlot { get; }

		public ScatterPlotProduct(IPlottableFactoryReset factory, ScatterPlot? scatterPlot) {
			Factory     = factory;
			ScatterPlot = scatterPlot;
		}
	}

	public readonly struct SignalPlotProduct {
		public IPlottableFactoryReset             Factory    { get; }
		public SignalPlotXYConst<double, double>? SignalPlot { get; }

		public SignalPlotProduct(IPlottableFactoryReset factory, SignalPlotXYConst<double, double>? signalPlot) {
			Factory    = factory;
			SignalPlot = signalPlot;
		}
	}
}