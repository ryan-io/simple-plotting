﻿using System.Globalization;
using System.Text;
using CsvHelper;
using SimplePlot;
using SimplePlot.Runtime;
using static SimplePlot.Constants;

namespace simple_plotting.runtime {
    public class DefaultCsvParseStrategy : ICsvParseStrategy {
        StringBuilder Sb { get; } = new StringBuilder();

        public async Task Strategy(List<PlotChannel> output, CsvReader csvr) {
            SkipRows(csvr, HEADER_START_ROW);
            csvr.ReadHeader();

            if (csvr.HeaderRecord == null)
                throw new Exception(Message.EXCEPTION_NO_HEADER);

            var channelsToParse = csvr.HeaderRecord.Length - 3;

            for (var i = 0; i < channelsToParse; i++) {
                output.Add(new PlotChannel(csvr.HeaderRecord[3 + i], PlotChannelType.Temperature));
            }

            while (await csvr.ReadAsync()) {
                Sb.Clear();
                Sb.Append(csvr[0]); // csvr[0] = date
                Sb.Append(SPACE_CHAR);
                Sb.Append(csvr[1]); // csvr[1] = time


                DateTime date = DateTime.Now;
                // csvr[2] = IGNORE (mSec)

                date = ParseDate();

                // csvr[...] = channel values

                for (var i = 0; i < channelsToParse; i++) {
                    if (string.IsNullOrWhiteSpace(csvr[i]))
                        continue;

                    bool hasValue = double.TryParse(csvr[3 + i]?.Trim(), out var value);
                    if (!hasValue) {
                        continue;
                    }

                    output[i].AddRecord(new PlotChannelRecord(date, value));
                }
            }
        }

        /// <summary>
        ///  Helper method to parse a date from a string.
        /// </summary>
        /// <param name="sb">Pre-allocated StringBuilder</param>
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
    }
}
