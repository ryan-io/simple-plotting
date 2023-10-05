// simple-plotting

using simple_plotting.src;
using SimplePlot;

namespace simple_plotting.runtime;

/// <summary>
///  Simple default implementation of <see cref="IPlotChannelProviderSource"/> that returns an empty string.
/// </summary>
public class EmptyPlotChannelProviderSource : IPlotChannelProviderSource {
	public string Path => string.Empty;
}