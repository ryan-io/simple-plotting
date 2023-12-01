// simple-plotting

using ScottPlot.Plottable;

namespace simple_plotting;

public partial class PlotBuilderFluent {
	/// <inheritdoc />
	public IPlotBuilderFluentConfiguration WithType<T>() where T : class, IPlottable {
		PlotType     = typeof(T);
		FactoryPrime = PlottableFactory.StartNew<T>();

		SetInitialState(new ChannelRecordProcessor(_plots, FactoryPrime).Process);

		return this;
	}
}