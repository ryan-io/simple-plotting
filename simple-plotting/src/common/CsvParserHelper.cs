namespace simple_plotting.src {
	public static class CsvParserHelper {
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