using ScottPlot.Plottable;

namespace simple_plotting.src {
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
		public SignalPlot?            SignalPlot { get; }

		public SignalPlotProduct(IPlottableFactoryReset factory, SignalPlot? scatterPlot) {
			Factory    = factory;
			SignalPlot = scatterPlot;
		}
	}
}