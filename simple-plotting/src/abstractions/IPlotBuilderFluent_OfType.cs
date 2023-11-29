// simple-plotting

using ScottPlot.Plottable;

namespace simple_plotting;

/// <summary>
///  Base abstraction for defining the type of the plot.
/// </summary>
public interface IPlotBuilderFluentOfType : IPlotBuilderFluent {
	/// <summary>
	/// Gets the type of the plot being built.
	/// </summary>
	Type? PlotType { get; }

	/// <summary>
	/// Defines the type of the plot.
	/// </summary>
	/// <typeparam name="T">The type of the plot. Must be a class implementing IPlottable interface.</typeparam>
	/// <returns>An instance of IPlotBuilderFluentConfiguration for further configuration.</returns>
	IPlotBuilderFluentConfiguration OfType<T>() where T : class, IPlottable;
}