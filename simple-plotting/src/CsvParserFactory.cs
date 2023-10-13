using simple_plotting.runtime;

namespace simple_plotting.src {
	public static class CsvParserFactory {
		/// <summary>
		///  Creates a new <see cref="CsvParser" /> instance.
		/// </summary>
		/// <param name="source">Directory to extract Csv files from</param>
		/// <param name="strategy">Logic for parsing through Csv files</param>
		/// <returns>Fluent instance (CsvSource)</returns>
		public static ICsvParser StartNew(string? source,
			ICsvParseStrategy strategy)
			=> new CsvParser(source, strategy);

		/// <summary>
		///  Creates a new <see cref="CsvParser" /> instance.
		/// </summary>
		/// <param name="strategy">Logic for parsing through Csv files</param>
		/// <param name="source">Directory to extract Csv files from</param>
		/// <returns>Fluent instance (CsvSource)</returns>
		public static ICsvParser StartNew(ICsvParseStrategy strategy, string? source) {
			if (string.IsNullOrWhiteSpace(source))
				throw new Exception(Message.EXCEPTION_INVALID_SOURCE);

			var parser = new CsvParser(source, strategy);
			parser.SetSource(source);

			return parser;
		}

		/// <summary>
		///  Creates a new <see cref="CsvParser" /> instance. Use this is a data source is not known at compile time.
		/// </summary>
		/// <returns>Fluent instance (CsvSource)</returns>
		public static ICsvParser StartNew(ICsvParseStrategy strategy)
			=> new CsvParser(string.Empty, strategy);

		/// <summary>
		///  Creates a new <see cref="CsvParser" /> instance. Use this is a data source is not known at compile time.
		///  This constructor uses a default implementation for CsvParseStrategy (DefaultCsvParseStrategy).
		///  This method requires you to pass two values: lowerValueLimit and upperValueLimit.
		///  These two values will be used to ignore any values that are outside of these limits.
		/// </summary>
		/// <returns>Fluent instance (CsvSource)</returns>
		public static ICsvParser StartNewDefaultStrategy(
			string? source,
			double lowerValueLimit,
			double upperValueLimit)
			=> new CsvParser(source, new DefaultCsvParseStrategy(lowerValueLimit, upperValueLimit));
	}
}