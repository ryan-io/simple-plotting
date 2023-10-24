using ScottPlot.Plottable;

namespace simple_plotting.src {
	// TODO: Add documentation
	public readonly struct ScatterPlotProduct  {
		public IPlottableFactoryReset Factory     { get; }
		public ScatterPlot?           ScatterPlot { get; }

		public ScatterPlotProduct(IPlottableFactoryReset factory, ScatterPlot? scatterPlot) {
			Factory     = factory;
			ScatterPlot = scatterPlot;
		}
	}

	public readonly struct SignalPlotProduct {
		public IPlottableFactoryReset Factory    { get; }
		public SignalPlotXY?            SignalPlot { get; }

		public SignalPlotProduct(IPlottableFactoryReset factory, SignalPlotXY? signalPlot) {
			Factory    = factory;
			SignalPlot = signalPlot;
		}
	}
}