namespace simple_plotting;

public interface IPlotChannelProvider {
	string? Path { get; }
	void    ForceSetSource(string? path);

	bool SetSource(string path);
}