using System.Globalization;
using System.Text;
using csFastFloat;
using CsvHelper;
using simple_plotting.src;
using static simple_plotting.src.Constants;

namespace simple_plotting.runtime {
	public class DefaultCsvParseStrategy : ICsvParseStrategy {
		StringBuilder Sb { get; } = new();

		public async Task Strategy(List<PlotChannel> output, CsvReader csvr) {
			SkipRows(csvr, HEADER_START_ROW);
			csvr.ReadHeader();

			if (csvr.HeaderRecord == null)
				throw new Exception(Message.EXCEPTION_NO_HEADER);

			var channelsToParse = csvr.HeaderRecord.Length - 3;

			for (var i = 0; i < channelsToParse; i++) {
				output.Add(new PlotChannel(csvr.HeaderRecord[3 + i], PlotChannelType.Temperature));
			}

			DateTime date     = DateTime.Now;
			bool     hasValue = false;
			double   value    = 0.0d;

			while (await csvr.ReadAsync()) {
				Sb.Clear();
				Sb.Append(csvr[0]); // csvr[0] = date
				Sb.Append(SPACE_CHAR);
				Sb.Append(csvr[1]); // csvr[1] = time

				// csvr[2] = IGNORE (mSec)

				date = ParseDate();

				// csvr[...] = channel values

				for (var i = 0; i < channelsToParse; i++) {
					if (string.IsNullOrWhiteSpace(csvr[i]))
						continue;

					hasValue = FastDoubleParser.TryParseDouble(csvr[3 + i], out value);
					bool isOutside = value < _lowerValueLimit || _upperValueLimit > 200d;
					
					if (!hasValue || isOutside) {
						continue;
					}

					var record = new PlotChannelRecord(date, value);
					PlotEvent.OnRecordEnumerated(record);
					
					output[i].AddRecord(record);
				}
			}
		}

		/// <summary>
		///  Helper method to parse a date from a string.
		/// </summary>
		/// <returns>ParsedDate</returns>
		DateTime ParseDate()
			=> DateTime.ParseExact(Sb.ToString(), "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);

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

		public DefaultCsvParseStrategy(double lowerValueLimit, double upperValueLimit) {
			_lowerValueLimit = lowerValueLimit;
			_upperValueLimit = upperValueLimit;
		}
		
		readonly double _lowerValueLimit;
		readonly double _upperValueLimit;
	}
}