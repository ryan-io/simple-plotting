namespace simple_plotting;

/// <summary>
/// Represents a plot builder that uses a fluent interface to build a canvas ready to produce plots.
/// </summary>
public partial class PlotBuilderFluent {
	public IPlotBuilderFluentCanvasProduct ProduceCanvas() {
		InstanceTracker.Global.RegisterInstance(this);
		_plotWasProduced = true;

		if (!Observables.Any())
			return this;
 
		foreach (var o in Observables)
			o.Invoke();
		
		return this;
	}
}