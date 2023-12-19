namespace simple_plotting {
	/// <summary>
	///  Helper class to scale a value by a percentage.
	/// </summary>
	public static class ValueScaler {
		/// <summary>
		///  Scales a value by a percentage.
		/// </summary>
		/// <param name="value">The value to scale</param>
		/// <param name="percentScale">Value between -100 & +100</param>
		public static void ScaleByRef(ref double? value, double percentScale) {
			if (value == null)
				return;

			if (percentScale < -100d || percentScale > 100d)
				return;

			var scalar = percentScale / 100d;
			value *= scalar;
		}
	}
}