using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using simple_plotting.src;
using SimplePlot;

namespace simple_plotting.runtime {
	/// <summary>
	///  Parses a CSV file and extracts the data into a collection of <see cref="PlotChannel" /> instances.
	///  This is the data provider in MVVM.
	///  https://github.com/CarlVerret/csFastFloat is used for parsing double values.
	/// </summary>
	public class CsvParser : IPlotChannelProvider, IPlotChannelExtractor {
		StringBuilder Sb { get; } = new StringBuilder();

		/// <summary>
		///  Creates a new <see cref="CsvParser" /> instance.
		/// </summary>
		/// <param name="sourceProvider">Path wrapper containing the Csv file</param>
		/// <param name="strategy">Logic for parsing through Csv files</param>
		/// <returns>Fluent instance (CsvSource)</returns>
		public static IPlotChannelProvider StartNew(IPlotChannelProviderSource sourceProvider,
			ICsvParseStrategy strategy)
			=> new CsvParser(sourceProvider, strategy);

		/// <summary>
		///  Creates a new <see cref="CsvParser" /> instance. Use this is a data source is not known at compile time.
		/// </summary>
		/// <returns>Fluent instance (CsvSource)</returns>
		public static IPlotChannelProvider StartNew(ICsvParseStrategy strategy)
			=> new CsvParser(new EmptyPlotChannelProviderSource(), strategy);

		/// <summary>
		///  Creates a new <see cref="CsvParser" /> instance. Use this is a data source is not known at compile time.
		///  This constructor uses a default implementation for CsvParseStrategy (DefaultCsvParseStrategy).
		///  This method requires you to pass two values: lowerValueLimit and upperValueLimit.
		///  These two values will be used to ignore any values that are outside of these limits.
		/// </summary>
		/// <returns>Fluent instance (CsvSource)</returns>
		public static IPlotChannelProvider StartNewDefaultStrategy(
			IPlotChannelProviderSource sourceProvider, 
			double lowerValueLimit, 
			double upperValueLimit)
			=> new CsvParser(sourceProvider, new DefaultCsvParseStrategy(lowerValueLimit, upperValueLimit));

		/// <summary>
		///   The path to the CSV file.
		/// </summary>
		public string? Path { get; private set; }

		/// <summary>
		///  Parses, extracts, and returns the data from the CSV file.  
		/// </summary>
		public async Task<IReadOnlyList<PlotChannel>?> ExtractAsync(string fileName) {
			if (string.IsNullOrWhiteSpace(fileName))
				return default;

			var output = new List<PlotChannel>();

			using var sr   = new StreamReader(fileName);
			using var csvr = new CsvReader(sr, _configuration);

			await _strategy.Strategy(output, csvr);

			return output;
		}

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

		public CsvParser(IPlotChannelProviderSource sourceProvider, ICsvParseStrategy strategy) {
			Path      = sourceProvider.Path;
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
		/// <returns>An awaitable task</returns>
		Task Strategy(List<PlotChannel> output, CsvReader csvr);
	}
}