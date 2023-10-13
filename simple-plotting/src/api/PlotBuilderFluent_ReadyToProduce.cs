// simple-plotting

namespace simple_plotting.src;

public partial class PlotBuilderFluent {
	/// <summary>
	///  Returns the generated Plot instance. This should be the last call in the fluent API.
	/// </summary>
	/// <returns>ScottPlot.Plot instance (private)</returns>
	public IPlotBuilderFluentProduct Produce() {
		_plotWasProduced = true;

		if (!Observables.Any())
			return this;

		foreach (var o in Observables)
			o.Invoke();

		return this;
	}
}