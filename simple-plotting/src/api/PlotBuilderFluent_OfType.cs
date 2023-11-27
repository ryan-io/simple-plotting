// simple-plotting

using ScottPlot.Plottable;

namespace simple_plotting;

public partial class PlotBuilderFluent {
	/// <inheritdoc />
	public Type? PlotType { get; private set; }

	/// <inheritdoc />
	public IPlotBuilderFluentConfiguration OfType<T>() where T : class, IPlottable {
		PlotType     = typeof(T);
		FactoryPrime = PlottableFactory.StartNew<T>();

		SetInitialState(new ChannelRecordProcessor(_plots, FactoryPrime).Process);

		return this;
	}
}