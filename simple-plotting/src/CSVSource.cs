using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;

namespace simple_plotting {
	public class CsvSource : ICsvSourceExtract {
		/// <summary>
		///  Creates a new <see cref="CsvSource" /> instance.
		/// </summary>
		/// <param name="path">Path containing the Csv file</param>
		/// <returns>Fluent instance (CsvSource)</returns>
		public static ICsvSourceExtract StartNew(string path) => new CsvSource(path);

		/// <summary>
		///  Parses, extracts, and returns the data from the CSV file.  
		/// </summary>
		/// <exception cref="NotImplementedException"></exception>
		public async Task<IReadOnlyList<PlotChannel>> ExtractAsync() {
			using var sr   = new StreamReader(Path);
			using var csvr = new CsvReader(sr, _configuration);

			var output = new List<PlotChannel>();

			SkipRows(csvr, Constants.HEADER_START_ROW);
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
				sb.Append(Constants.SPACE);
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
				
					output[i].Records.Add( new PlotChannelRecord(date, value));
				}

				counter++;
			}

			return output;
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

		/// <summary>
		///   The path to the CSV file.
		/// </summary>
		string Path { get; }

		CsvSource(string path) {
			if (string.IsNullOrWhiteSpace(path))
				throw new Exception(Message.EXCEPTION_NO_PATH);

			Path = path;
		}

		/// <summary>
		///  Default configuration for CSV file parsing.
		/// </summary>
		readonly CsvConfiguration _configuration = new(CultureInfo.InvariantCulture) {
			Delimiter = ","
		};
	}

	public interface ICsvSourceExtract {
		Task<IReadOnlyList<PlotChannel>> ExtractAsync();
	}
}