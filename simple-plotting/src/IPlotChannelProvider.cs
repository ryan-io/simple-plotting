namespace SimplePlot;

public interface IPlotChannelProvider {
	string? Path { get; }
	void    ForceSetSource(string? path);
	
	bool                             SetSource(string path);
}