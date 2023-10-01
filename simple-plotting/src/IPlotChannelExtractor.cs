// simple-plotting

namespace SimplePlot;

public interface IPlotChannelExtractor {
	Task<IReadOnlyList<PlotChannel>?> ExtractAsync();
}