using ScottPlot.Plottable;

//TODO: add documentation
namespace simple_plotting.src; 

public class PlottleTypeMapper {
    public Action? Determine (IPlottableProduct factory, PlotChannel channel) {
        if (_plottableType == typeof(ScatterPlot))
            return () => factory.AddScatterPlot(channel.Color, channel.ChannelIdentifier);

        if (_plottableType == typeof(SignalPlotXY))
            return () => factory.AddSignalPlotXY(channel.Color, channel.ChannelIdentifier);

        return default;
    }

    public PlottleTypeMapper (Type? plottableType) {
        _plottableType = plottableType;

    }

    readonly Type? _plottableType;
}