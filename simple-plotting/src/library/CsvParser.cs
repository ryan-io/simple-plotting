using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace simple_plotting {
	/// <summary>
	///  Parses a CSV file and extracts the data into a collection of <see cref="PlotChannel" /> instances.
	///  This is the data provider in MVVM.
	///  https://github.com/CarlVerret/csFastFloat is used for parsing double values.
	/// </summary>
	public class CsvParser : ICsvParser {
		/// <summary>
		///   The path to the CSV file.
		/// </summary>
		public string? Path { get; private set; }

		/// <summary>
		///  Parses, extracts, and returns the data from the CSV file.  
		/// </summary>
		/// <param name="fileName">Name of file to parse</param>
		/// <param name="cancellationToken">Optional cancellation token to stop the task</param>
		/// <returns>Readonly list of PlotChannel</returns>
		public async Task<IReadOnlyList<PlotChannel>?> ExtractAsync(string fileName,
			CancellationToken? cancellationToken = default) {
			try {
				if (string.IsNullOrWhiteSpace(fileName))
					return default;

				var output = new List<PlotChannel>();

				using var sr   = new StreamReader($@"{Path}\{fileName}");
				using var csvr = new CsvReader(sr, _configuration);

				await _strategy.StrategyAsync(output, csvr, cancellationToken);

				return output;
			}
			catch (FileNotFoundException e) {
				throw new Exception($"{Message.EXCEPTION_FILE_NOT_FOUND}{fileName}", e);
			}
			catch (Exception e) {
				throw new Exception(Message.EXCEPTION_PARSE_FAILED, e);
			}
		}

		/*	----> This method is not used, but is kept for reference. <----
			----> Should a consumer be allowed to define a file name or should ExtractAsync(string fileName) be used? <----
		/// <summary>
		///  Parses, extracts, and returns the data from the property Path.
		/// </summary>
		/// <returns>Readonly list of PlotChannel</returns>
		public async Task<IReadOnlyList<PlotChannel>?> ExtractAsync() {
			if (string.IsNullOrWhiteSpace(Path))
				return default;

			return await ExtractAsync(Path);
		}
		*/

		/// <summary>
		///  Sets the source directory containing the CSV file.
		/// </summary>
		/// <param name="path">String to directory containing data</param>
		/// <returns>True if directory exists, false otherwise</returns>
		public bool SetSource(string path) {
			if (!Directory.Exists(path)) {
				Path = string.Empty;
				return false;
			}

			Path = path;
			return true;
		}

		/// <summary>
		///  Sets the source directory containing the CSV file.
		/// </summary>
		/// <param name="path">string to set Path to</param>
		public void ForceSetSource(string? path) => Path = path;

		internal CsvParser(string? source, ICsvParseStrategy strategy) {
			Path      = source;
			_strategy = strategy;
		}

		readonly ICsvParseStrategy _strategy;

		/// <summary>
		///  Default configuration for CSV file parsing.
		/// </summary>
		readonly CsvConfiguration _configuration = new(CultureInfo.InvariantCulture) {
			Delimiter = ","
		};
	}

	/// <summary>
	///  Abstraction for implementing a very simple strategy pattern to parse CSV files.
	/// </summary>
	public interface ICsvParseStrategy {
		/// <summary>
		///  Parses the CSV file and extracts the data into a collection of <see cref="PlotChannel" /> instances.
		/// </summary>
		/// <param name="output">Pre-allocated collection of PlotChannel</param>
		/// <param name="csvr">Instance of Csv Reader</param>
		/// <param name="cancellationToken">Cancellation token for cancelling the awaitable task</param>
		/// <returns>An awaitable task</returns>
		Task 
			StrategyAsync(List<PlotChannel> output, CsvReader csvr, CancellationToken? cancellationToken = default);
	}

	/// <summary>
	///  Root interface for CsvParser.
	/// </summary>
	public interface ICsvParser : IPlotChannelProvider, IPlotChannelExtractor {
	}
}