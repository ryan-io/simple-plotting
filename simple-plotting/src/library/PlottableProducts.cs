using ScottPlot.Plottable;

namespace simple_plotting {
	/// <summary>
	/// Represents a struct containing a ScatterPlot and the factory used to create it.
	/// </summary>
	public readonly struct ScatterPlotProduct {
		/// <summary>
		/// The plottable factory that was used to create the ScatterPlot.
		/// </summary>
		public IPlottableFactoryReset Factory { get; }

		/// <summary>
		/// The ScatterPlot product created by the factory.
		/// </summary>
		public ScatterPlot? ScatterPlot { get; }

		/// <summary>
		/// Constructor that accepts a factory and its ScatterPlot product.
		/// </summary>
		/// <param name="factory">The ScatterPlot factory.</param>
		/// <param name="scatterPlot">The ScatterPlot created by the factory.</param>
		public ScatterPlotProduct(IPlottableFactoryReset factory, ScatterPlot? scatterPlot) {
			Factory     = factory;
			ScatterPlot = scatterPlot;
		}
	}

	/// <summary>
	/// Represents a struct containing a SignalPlot and the factory used to create it.
	/// </summary>
	public readonly struct SignalPlotProduct {
		/// <summary>
		/// The plottable factory that was used to create the SignalPlot.
		/// </summary>
		public IPlottableFactoryReset Factory { get; }

		/// <summary>
		/// The SignalPlot product created by the factory.
		/// </summary>
		public SignalPlotXYConst<double, double>? SignalPlot { get; }

		/// <summary>
		/// Constructor that accepts a factory and its SignalPlot product.
		/// </summary>
		/// <param name="factory">The SignalPlot factory.</param>
		/// <param name="signalPlot">The SignalPlot created by the factory.</param>
		public SignalPlotProduct(IPlottableFactoryReset factory, SignalPlotXYConst<double, double>? signalPlot) {
			Factory    = factory;
			SignalPlot = signalPlot;
		}
	}
}