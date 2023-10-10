using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using simple_plotting.src;

namespace simple_plotting.runtime {
	/// <summary>
	///  Parses a CSV file and extracts the data into a collection of <see cref="PlotChannel" /> instances.
	///  This is the data provider in MVVM.
	///  https://github.com/CarlVerret/csFastFloat is used for parsing double values.
	/// </summary>
	public class CsvParser : ICsvParser {
		StringBuilder Sb { get; } = new StringBuilder();

		/// <summary>
		///  Creates a new <see cref="CsvParser" /> instance.
		/// </summary>
		/// <param name="sourceProvider">Path wrapper containing the Csv file</param>
		/// <param name="strategy">Logic for parsing through Csv files</param>
		/// <returns>Fluent instance (CsvSource)</returns>
		public static ICsvParser StartNew(IPlotChannelProviderSource sourceProvider,
			ICsvParseStrategy strategy)
			=> new CsvParser(sourceProvider, strategy);

		/// <summary>
		///  Creates a new <see cref="CsvParser" /> instance.
		/// </summary>
		/// <param name="sourceProvider">Path wrapper containing the Csv file</param>
		/// <param name="strategy">Logic for parsing through Csv files</param>
		/// <param name="source">Directory to extract Csv files from</param>
		/// <returns>Fluent instance (CsvSource)</returns>
		public static ICsvParser StartNew(IPlotChannelProviderSource sourceProvider,
			ICsvParseStrategy strategy, string? source) {
			if (string.IsNullOrWhiteSpace(source))
				throw new Exception(Message.EXCEPTION_INVALID_SOURCE);

			var parser = new CsvParser(sourceProvider, strategy);
			parser.SetSource(source);

			return parser;
		}

		/// <summary>
		///  Creates a new <see cref="CsvParser" /> instance. Use this is a data source is not known at compile time.
		/// </summary>
		/// <returns>Fluent instance (CsvSource)</returns>
		public static ICsvParser StartNew(ICsvParseStrategy strategy)
			=> new CsvParser(new EmptyPlotChannelProviderSource(), strategy);

		/// <summary>
		///  Creates a new <see cref="CsvParser" /> instance. Use this is a data source is not known at compile time.
		/// </summary>
		/// <param name="strategy">Logic for parsing through Csv files</param>
		/// <param name="source">Directory to extract Csv files from</param>
		/// <returns>Fluent instance (CsvSource)</returns>
		/// <exception cref="Exception">Thrown if source is null or whitespace</exception>
		public static ICsvParser StartNew(ICsvParseStrategy strategy, string? source) {
			if (string.IsNullOrWhiteSpace(source))
				throw new Exception(Message.EXCEPTION_INVALID_SOURCE);

			var parser = new CsvParser(new EmptyPlotChannelProviderSource(), strategy);
			parser.SetSource(source);

			return parser;
		}

		/// <summary>
		///  Creates a new <see cref="CsvParser" /> instance. Use this is a data source is not known at compile time.
		///  This constructor uses a default implementation for CsvParseStrategy (DefaultCsvParseStrategy).
		///  This method requires you to pass two values: lowerValueLimit and upperValueLimit.
		///  These two values will be used to ignore any values that are outside of these limits.
		/// </summary>
		/// <returns>Fluent instance (CsvSource)</returns>
		public static ICsvParser StartNewDefaultStrategy(
			IPlotChannelProviderSource sourceProvider,
			double lowerValueLimit,
			double upperValueLimit)
			=> new CsvParser(sourceProvider, new DefaultCsvParseStrategy(lowerValueLimit, upperValueLimit));

		/// <summary>
		///  Creates a new <see cref="CsvParser" /> instance. Use this is a data source is not known at compile time.
		///  This constructor uses a default implementation for CsvParseStrategy (DefaultCsvParseStrategy).
		///  This method requires you to pass two values: lowerValueLimit and upperValueLimit.
		///  These two values will be used to ignore any values that are outside of these limits.
		/// </summary>
		/// <returns>Fluent instance (CsvSource)</returns>
		public static ICsvParser StartNewDefaultStrategy(
			IPlotChannelProviderSource sourceProvider,
			string? source,
			double lowerValueLimit,
			double upperValueLimit) {
			if (string.IsNullOrWhiteSpace(source))
				throw new Exception(Message.EXCEPTION_INVALID_SOURCE);
			
			var parser = new CsvParser(sourceProvider, new DefaultCsvParseStrategy(lowerValueLimit, upperValueLimit));
			parser.SetSource(source);
			
			return parser;
		}

		/// <summary>
		///   The path to the CSV file.
		/// </summary>
		public string? Path { get; private set; }

		/// <summary>
		///  Parses, extracts, and returns the data from the CSV file.  
		/// </summary>
		/// <param name="fileName">Name of file to parse</param>
		/// <returns>Readonly list of PlotChannel</returns>
		public async Task<IReadOnlyList<PlotChannel>?> ExtractAsync(string fileName) {
			if (string.IsNullOrWhiteSpace(fileName))
				return default;

			var output = new List<PlotChannel>();

			using var sr   = new StreamReader($@"{Path}\{fileName}");
			using var csvr = new CsvReader(sr, _configuration);

			await _strategy.Strategy(output, csvr);

			return output;
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

	/// <summary>
	///  Root interface for CsvParser.
	/// </summary>
	public interface ICsvParser : IPlotChannelProvider, IPlotChannelExtractor {
	}
}