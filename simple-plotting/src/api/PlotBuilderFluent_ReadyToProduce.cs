// simple-plotting

namespace simple_plotting;

public partial class PlotBuilderFluent {
	/// <inheritdoc />
	public IPlotBuilderFluentProduct Produce() {
		InstanceTracker.Global.RegisterInstance(this);
		_plotWasProduced = true;

		if (!Observables.Any())
			return this;
 
		foreach (var o in Observables)
			o.Invoke();
		
		return this;
	}
}