using System.Globalization;
using System.Text;
using csFastFloat;
using CsvHelper;
using static simple_plotting.Constants;

namespace simple_plotting.runtime {
	/// <summary>
	///  Base class for implementing Csv parsing logic. This class implements ICsvParseStrategy
	/// </summary>
	public abstract class CsvParseStrategy : ICsvParseStrategy {
		/// <summary>
		///  The core method to override and provide implementation details for
		/// </summary>
		/// <param name="output">Awaitable task</param>
		/// <param name="csvr">CsvReader that is supplied by the CsvParser</param>
		/// <param name="cancellationToken">Async cancellation token</param>
		/// <returns>Awaitable task</returns>
		public abstract Task StrategyAsync(List<PlotChannel> output, CsvReader csvr,
			CancellationToken? cancellationToken = default);
	}

	public class DefaultCsvParseStrategy : CsvParseStrategy {
		StringBuilder Sb { get; } = new();

		double? SampleRate { get; set; }

		public override async Task StrategyAsync(
			List<PlotChannel> output,
			CsvReader csvr,
			CancellationToken? cancellationToken = default) {
			try {
				const int SKIP_FOUR_ROWS = 4;
				const int SKIP_SIX_ROWS  = 6;

				SkipRowsNumberOfRows(csvr, SKIP_SIX_ROWS);

				await csvr.ReadAsync();

				SampleRate = CsvParserHelper.ExtractSampleRate(csvr[1]); // we could ignore this entirely

				SkipRowsNumberOfRows(csvr, SKIP_FOUR_ROWS);

				csvr.ReadHeader();

				if (csvr.HeaderRecord == null)
					throw new Exception(Message.EXCEPTION_NO_HEADER);

				var channelsToParse = csvr.HeaderRecord.Length - 3;

				for (var i = 0; i < channelsToParse; i++) {
					if (cancellationToken.HasValue && cancellationToken.Value.IsCancellationRequested) 
						break;

					output.Add(new PlotChannel(csvr.HeaderRecord[3 + i], PlotChannelType.Temperature, SampleRate));
				}

				while (await csvr.ReadAsync()) {
					if (cancellationToken.WasCancelled()) 
						break;

					ParseCurrentReaderIndex(output, csvr, cancellationToken, channelsToParse);
				}
			}
			catch (Exception e) {
				throw new Exception(Message.EXCEPTION_STRATEGY_FAILED, e);
			}
		}

		void ParseCurrentReaderIndex(List<PlotChannel> output, CsvReader csvr, CancellationToken? cancellationToken, int channelsToParse) {
			const double EPSILON = 5E-6d;

			Sb.Clear();
			Sb.Append(csvr[0]); // csvr[0] = date
			Sb.Append(SPACE_CHAR);
			Sb.Append(csvr[1]); // csvr[1] = time

			// csvr[2] = IGNORE (mSec)

			var date = ParseDate();

			// csvr[...] = channel values

			for (var i = 0; i < channelsToParse; i++) {
				if (string.IsNullOrWhiteSpace(csvr[i]))
					continue;

				if (cancellationToken.WasCancelled()) {
					break;
				}

				var  hasValue          = FastDoubleParser.TryParseDouble(csvr[3 + i], out var value);
				bool isOutside         = value < _lowerValueLimit || value > _upperValueLimit;
				bool isEssentiallyZero = value >= 0.0d - EPSILON && value  <= 0.0d + EPSILON;

				if (isOutside || isEssentiallyZero || !hasValue) {
					continue;
				}

				var record    = new PlotChannelRecord(date, value);
				var eventData = new PlotEventData(output[i].ChannelIdentifier, record);

				PlotEvent.OnRecordEnumerated(eventData);

				output[i].AddRecord(record);
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
		static void SkipRowsNumberOfRows(IReader csvr, int numRows) {
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