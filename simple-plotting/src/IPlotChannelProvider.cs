namespace SimplePlot;

public interface IPlotChannelProvider {
	Task<IReadOnlyList<PlotChannel>> ExtractAsync();
	bool                             SetSource(string path);
}