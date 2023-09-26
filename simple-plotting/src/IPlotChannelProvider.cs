namespace SimplePlot;

public interface IPlotChannelProvider {
	string                           Path { get; }
	Task<IReadOnlyList<PlotChannel>> ExtractAsync();
	bool                             SetSource(string path);
}