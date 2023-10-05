namespace simple_plotting.src {
	/// <summary>
	///  Constant messages used throughout the application. Typical to call when throwing exceptions.
	/// </summary>
	internal static class Message {
		public const string EXCEPTION_NO_HEADER =
			"Header record is null. Please provide a valid CSV file with parsable headers. Debugging may be required";

		public const string EXCEPTION_NO_PATH = "Path cannot be null, empty or whitespace.";

		public const string EXCEPTION_CANNOT_PARSE_FLOAT = "Cannot parse float value.";
		
		public const string EXCEPTION_SAVE_PATH_INVALID = "Save path cannot be null, empty or whitespace.";

		public const string EXCEPTION_TITLE_INVALID = "Title cannot be null, empty or whitespace.";
		
		public const string EXCEPTION_AXIS_LABEL_INVALID = "An axis label cannot be null, empty or whitespace.";

		public const string EXCEPTION_INTERFACE_COMPARER_NOT_SAME =
			"Interface comparer is not the same as the comparer used in the collection.";
	}
}