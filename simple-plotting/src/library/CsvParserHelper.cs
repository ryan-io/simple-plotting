namespace simple_plotting {
	/// <summary>
	/// Contains helpers for CSV parsing.
	/// </summary>
	public static class CsvParserHelper {
		/// <summary>
		/// Extracts the sample rate from a string formatted as 'hh:mm:ss'. 
		/// The string represents a duration, and this method converts that duration into total seconds.
		/// If the input is null, empty, or not following the pattern, the method returns null.
		/// </summary>
		/// <param name="parsed">The string to be parsed, must be in 'hh:mm:ss' format.</param>
		/// <returns>
		/// The sample rate represented as total seconds of the input 'hh:mm:ss' duration string.
		/// Returns null if the input is null, empty, or not formatted correctly.
		/// </returns>
		public static double? ExtractSampleRate(string? parsed) {
			if (string.IsNullOrEmpty(parsed))
				return default;

			const char   DELIMITER         = ':';
			const double SECONDS_IN_HOUR   = 3600d;
			const double SECONDS_IN_MINUTE = 60d;

			var splitValues = parsed.Split(DELIMITER, StringSplitOptions.RemoveEmptyEntries);

			if (splitValues.Length < 1)
				return default;

			double interval = 0.0d;

			// assumption here is we get a string in a date-time format
			interval += Convert.ToDouble(splitValues[0]) * SECONDS_IN_HOUR;   // {hh:mm:ss}; 'hh'
			interval += Convert.ToDouble(splitValues[1]) * SECONDS_IN_MINUTE; // {hh:mm:ss}; 'mm'
			interval += Convert.ToDouble(splitValues[2]);                     // {hh:mm:ss}; 'ss'

			return Convert.ToDouble(interval);
		}
	}
}