// simple-plotting

using ScottPlot.Plottable;

namespace simple_plotting.src;

public partial class PlotBuilderFluent {
	/// <summary>
	///  The type of plot that is being built.
	/// </summary>
	public Type? PlotType { get; private set; }

	/// <summary>
	///  Creates a new instance of PlotBuilderFluent and PlottableFactory using type defined in 'T'
	/// </summary>
	/// <typeparam name="T">Is a class that implements IPlottable (SignalPlot, ScatterPlot, etc.)</typeparam>
	/// <returns>Fluent builder</returns>
	public IPlotBuilderFluentConfiguration OfType<T>() where T : class, IPlottable {
		PlotType     = typeof(T);
		FactoryPrime = PlottableFactory.StartNew<T>();

		SetInitialState(new ChannelRecordProcessor(_plots, FactoryPrime).Process);

		return this;
	}
}