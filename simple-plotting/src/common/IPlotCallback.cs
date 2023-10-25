// simple-plotting

using ScottPlot.Plottable;

namespace simple_plotting.src;

/// <summary>
/// Abstraction of a callback for a plot object creation.
/// </summary>
public interface IPlotCallback {
	/// <summary>
	///  Callback for a plot object creation. This is passed to the fluent builder when creating a plot.
	/// </summary>
	/// <param name="plot">Plot to add a graph too</param>
	/// <param name="data">POCO containing plot data</param>
	/// <typeparam name="T">Type T, class & implements IPlottable</typeparam>
	void Callback<T>(T plot, ref PlottableData data) where T : class, IPlottable;
}