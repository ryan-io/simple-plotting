// simple-plotting

namespace simple_plotting;

/// <summary>
///  This abstraction finalizes the configuration of the plot. This should be the last call in the fluent API.
/// </summary>
public interface IPlotBuilderFluentReadyToProduce : IPlotBuilderFluent {
	/// <summary>
	/// Produces the plot product based on the current configuration.
	/// </summary>
	/// <returns>A configured plot product.</returns>
	IPlotBuilderFluentProduct Produce();
}