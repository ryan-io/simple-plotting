using System.Reflection.Metadata;

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

		public const string EXCEPTION_DEFINE_SOURCE_NOT_INVOKED =
			"Cannot save via TrySaveAtSource without defining a source path. Call DefineSource first.";

		public const string EXCEPTION_INVALID_SOURCE = "Source cannot be null, empty or whitespace.";

		public const string EXCEPTION_PARSE_FAILED = "Could not extract data from CSV file.";

		public const string EXCEPTION_STRATEGY_FAILED = "Failed to complete parsing strategy.";

		public const string EXCEPTION_FILE_NOT_FOUND = "Could not find the file name ";

		public const string EXCEPTION_NO_PLOTTABLE_ON_PLOT = "Could not find a plottable at index ";

		public const string EXCEPTION_CANNOT_CAST_PLOTTABLE = "Could not cast as plottable at index ";

		public const string EXCEPTION_CANNOT_FIND_PLOTS = "No plots were found for the specified index(es).";
		
		public const string EXCEPTION_INDEX_OUT_OF_RANGE = "Index is out of range.";
		
		public const string EXCEPTION_NO_PLOT_LABEL_SPECIFIED = "No plot label was specified.";

		public const string EXCEPTION_NO_PLOT_ENUMERABLES_LIMIT_CAL = "'Channels' parameter was empty. Cannot determine limits";

		public const string EXCEPTION_CANNOT_CREATE_GENERIC_PLOTTABLE = "Could not create a new plottable through reflection. Check " +
			"the provided 'constructorArgs' and ensure the type you are trying to instantiate contains a constructor with identical " +
			"provided objects in 'constructorArgs'.";

        public const string EXCEPTION_CAST_PLOTTABLE_ACTIVATOR_INSTANCES = "Cannot cast the object created via Activator.CreateInstance(). Ensure an appropriate type & object constructor has been passed.";

		public const string EXCEPTION_SUPPLIED_TOKEN_NULL = "Supplied token is null.";

		public const string EXCEPTION_NO_PLOTTABLE_FAC_METHOD = "Could not determine an appropriate factory delegate for the provided plottable type";

		public const string EXCEPTION_PLOTTABLE_TYPE_CONSTRUCTOR_NOT_DEFINED =
			"There is no plottable factory method constructor defined for the type of IPlottable you provided";
	}
}