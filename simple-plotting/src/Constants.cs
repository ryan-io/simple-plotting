namespace simple_plotting {
	/// <summary>
	///  Contains constants used throughout the application.
	///  THESE CONSTANTS ARE UNIQUE TO QL LAKE ZURICH AND SHOULD BE CHANGED IF USED ELSEWHERE.
	/// </summary>
	public static class Constants {
		public const string SPACE_STRING           = " ";                         // space character (string)
		public const char   SPACE_CHAR             = ' ';                         // space character (char)
		public const string Y_AXIS_LABEL_TEMP      = "Temperature (°C)";          // X axis label for temperature plots
		public const string Y_AXIS_LABEL_RH        = "Relative Humidity (%)";     // X axis label for RH plots
		public const string Y_AXIS_LABEL_TEMP_RH   = "Temperature (°C) & RH (%)"; // X axis label for RH plots
		public const string PNG_EXTENSION          = ".png";                      // extension for PNG files
		public const string X_AXIS_LABEL_DATE_TIME = "Date & Time";               // X axis label for temperature plots

		public const int DEFAULT_WIDTH    = 1920; // default width of the plot
		public const int DEFAULT_HEIGHT   = 1080; // default height of the plot
		public const int HEADER_START_ROW = 11;   // index of the header row in the CSV file
		public const int DATA_START_ROW   = 12;   // index of the first data row in the CSV file

		public const double MAX_DATA_VALUE = 200; // maximum value for data
		public const double MIN_DATA_VALUE = -80; // minimum value for data
	}
}