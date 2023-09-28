using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using static SimplePlot.Constants;

namespace SimplePlot.Runtime {
	/// <summary>
	///  Parses a CSV file and extracts the data into a collection of <see cref="PlotChannel" /> instances.
	///  This is the data provider in MVVM.
	/// </summary>
	public class CsvParser : IPlotChannelProvider {
		/// <summary>
		///  Creates a new <see cref="CsvParser" /> instance.
		/// </summary>
		/// <param name="path">Path containing the Csv file</param>
		/// <returns>Fluent instance (CsvSource)</returns>
		public static IPlotChannelProvider StartNew(string path) => new CsvParser(path);
		
		/// <summary>
		///  Creates a new <see cref="CsvParser" /> instance. Use this is a data source is not known at compile time.
		/// </summary>
		/// <returns>Fluent instance (CsvSource)</returns>
		public static IPlotChannelProvider StartNew() => new CsvParser();

		/// <summary>
		///   The path to the CSV file.
		/// </summary>
		public string Path { get; private set; }
		
		/// <summary>
		///  Parses, extracts, and returns the data from the CSV file.  
		/// </summary>
		/// <exception cref="NotImplementedException"></exception>
		public async Task<IReadOnlyList<PlotChannel>> ExtractAsync() {
			using var sr   = new StreamReader(Path);
			using var csvr = new CsvReader(sr, _configuration);

			var output = new List<PlotChannel>();

			SkipRows(csvr, HEADER_START_ROW);
			csvr.ReadHeader();

			if (csvr.HeaderRecord == null)
				throw new Exception(Message.EXCEPTION_NO_HEADER);

			var channelsToParse = csvr.HeaderRecord.Length - 3;

			for (var i = 0; i < channelsToParse; i++) {
				output.Add(new PlotChannel(csvr.HeaderRecord[3 + i], PlotChannelType.Temperature));
			}

			StringBuilder sb = new StringBuilder();

			var counter = 0;

			while (await csvr.ReadAsync()) {
				sb.Clear();
				sb.Append(csvr[0]); // csvr[0] = date
				sb.Append(SPACE_CHAR);
				sb.Append(csvr[1]); // csvr[1] = time

				// csvr[2] = IGNORE (mSec)

				var date = ParseDate(sb);

				// csvr[...] = channel values

				for (var i = 0; i < channelsToParse; i++) {
					if (string.IsNullOrWhiteSpace(csvr[i]))
						continue;

					var hasValue = double.TryParse(csvr[3 + i]?.Trim(), out var value);

					if (!hasValue) {
						Console.WriteLine("Could not parse value: " + csvr[3 + i] + " at index: " + i);
						Console.WriteLine("Counter total is "       + counter);
						continue;
					}

					output[i].TryAddRecord(new PlotChannelRecord(date, value));
				}

				counter++;
			}

			return output;
		}

		/// <summary>
		///  Sets the source directory containing the CSV file.
		/// </summary>
		/// <param name="path">String to directory containing data</param>
		/// <returns>True if directory exists, false otherwise</returns>
		public bool SetSource(string path) {
			if (string.IsNullOrWhiteSpace(path))
				throw new Exception(Message.EXCEPTION_NO_PATH);

			if (!Directory.Exists(path))
				return false;

			Path = path;
			return true;
		}

		/// <summary>
		///  Helper method to parse a date from a string.
		/// </summary>
		/// <param name="sb">Pre-allocated StringBuilder</param>
		/// <returns>ParsedDate</returns>
		static DateTime ParseDate(StringBuilder sb)
			=> DateTime.ParseExact(sb.ToString(), "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);

		/// <summary>
		///  Helper method to skip rows in a CSV file.
		/// </summary>
		/// <param name="csvr">Pre-allocated Csv reader instance</param>
		/// <param name="numRows">The number of rows to skip (up to you to manage where this is invoked</param>
		static void SkipRows(IReader csvr, int numRows) {
			for (var i = 0; i < numRows; i++) {
				csvr.Read();
			}
		}

		public CsvParser(string path) {
			if (string.IsNullOrWhiteSpace(path))
				throw new Exception(Message.EXCEPTION_NO_PATH);

			Path = path;
		}

		/// <summary>
		///  Default constructor when a path is not provided (must be set later via 'SetSource').
		/// </summary>
		public CsvParser() {
			Path = string.Empty;
		}

		/// <summary>
		///  Default configuration for CSV file parsing.
		/// </summary>
		readonly CsvConfiguration _configuration = new(CultureInfo.InvariantCulture) {
			Delimiter = ","
		};
	}
}