// simple-plotting

namespace simple_plotting.src;

public interface IPlotChannelExtractor {
	Task<IReadOnlyList<PlotChannel>?> ExtractAsync(string fileName);
}