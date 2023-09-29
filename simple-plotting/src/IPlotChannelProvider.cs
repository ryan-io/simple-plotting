namespace SimplePlot;

public interface IPlotChannelProvider {
	string? Path { get; }
	void    ForceSetSource(string? path);
	Task<IReadOnlyList<PlotChannel>?>  ExtractAsync();
	bool                             SetSource(string path);
}