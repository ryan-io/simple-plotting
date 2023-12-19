using ScottPlot.Plottable;

namespace simple_plotting;

/// <summary>
/// Provides mapping service to determine appropriate plot creation method based on plottable type.
/// </summary>
public class PlottableFactoryTypeMapper {
	/// <summary>
	/// Generates a creation action for the specific tpye of plot specified in the factory type, using data and 
	/// channel information provided.
	/// </summary>
	/// <param name="factory">Factory used to create the plot.</param>
	/// <param name="channel">Channel information to be used in the plot creation.</param>
	/// <param name="data">Data to be plotted.</param>
	/// <returns>An action that creates a plot via the specified factory method, or null if factory type is unrecognized.</returns>
	public Action? Determine(IPlottableProduct factory, PlotChannel channel, PlottableData data) {
		if (_plottableType == typeof(ScatterPlot))
			return () => factory.AddScatterPlot(channel.Color, channel.ChannelIdentifier, data);

		if (_plottableType == typeof(SignalPlot))
			return () => factory.AddSignalPlot(channel.Color, channel.ChannelIdentifier, data);

		return default;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="PlottableFactoryTypeMapper"/> class with the plot type that the factory will create.
	/// </summary>
	/// <param name="plottableType">Type of the plot to be created by this factory.</param>
	public PlottableFactoryTypeMapper(Type? plottableType) {
		_plottableType = plottableType;
	}

	private readonly Type? _plottableType;
}