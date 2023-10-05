namespace simple_plotting.src;

/// <summary>
///  Interface for a source of plot channels. This could be a file, a database, a web service, etc.
///  The Path property is used to identify the source.
/// </summary>
public interface IPlotChannelProviderSource {
	string? Path { get; }
}