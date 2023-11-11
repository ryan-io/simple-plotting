// simple-plotting

namespace simple_plotting;

/// <summary>
///  This abstraction finalizes the configuration of the plot. This should be the last call in the fluent API.
/// </summary>
public interface IPlotBuilderFluentReadyToProduce : IPlotBuilderFluent {
	/// <summary>
	///  Returns the generated Plot instance. This should be the last call in the fluent API.
	/// </summary>
	/// <returns>ScottPlot.Plot instance (private)</returns>
	IPlotBuilderFluentProduct Produce();
}