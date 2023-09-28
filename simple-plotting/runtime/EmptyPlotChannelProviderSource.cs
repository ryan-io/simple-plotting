// simple-plotting

namespace SimplePlot.Runtime;

/// <summary>
///  Simple default implementation of <see cref="IPlotChannelProviderSource"/> that returns an empty string.
/// </summary>
public class EmptyPlotChannelProviderSource : IPlotChannelProviderSource {
	public string Path => string.Empty;
}