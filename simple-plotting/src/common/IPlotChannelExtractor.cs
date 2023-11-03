// simple-plotting

namespace simple_plotting.src;

/// <summary>
///  Abstraction for extracting data from a data source.
/// </summary>
public interface IPlotChannelExtractor {
	/// <summary>
	///  Parses, extracts, and returns the data from the CSV file.  
	/// </summary>
	/// <param name="fileName">Name of file to parse</param>
	/// <param name="cancellationToken">Optional cancellation token to stop the task</param>
	/// <returns>Readonly list of PlotChannel</returns>
	Task<IReadOnlyList<PlotChannel>?> ExtractAsync(string fileName, CancellationToken? cancellationToken = default);
}