using ScottPlot.Plottable;

//TODO: add documentation
namespace simple_plotting;

public class PlottableFactoryTypeMapper {
	public Action? Determine(IPlottableProduct factory, PlotChannel channel, PlottableData data) {
		if (_plottableType == typeof(ScatterPlot))
			return () => factory.AddScatterPlot(channel.Color, channel.ChannelIdentifier, data);

		if (_plottableType == typeof(SignalPlot))
			return () => factory.AddSignalPlot(channel.Color, channel.ChannelIdentifier, data);

		return default;
	}

	public PlottableFactoryTypeMapper(Type? plottableType) {
		_plottableType = plottableType;
	}

	readonly Type? _plottableType;
}