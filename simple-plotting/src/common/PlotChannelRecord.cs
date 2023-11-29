using System.Collections;

namespace simple_plotting {
	/// <summary>
	/// Represents a record of a plot channel including its DateTime and value,
	/// and provides custom comparison behavior.
	/// </summary>
	/// <remarks>
	/// Implements <see cref="IComparer"/> to compare the value of two PlotChannelRecord instances.
	/// </remarks>
	public record PlotChannelRecord(DateTime DateTime, double Value) : IComparer {
		/// <summary>
		/// Compares the values of two PlotChannelRecord instances.
		/// </summary>
		/// <param name="r1">The first object to compare, which must be an instance of PlotChannelRecord.</param>
		/// <param name="r2">The second object to compare, which must be an instance of PlotChannelRecord.</param>
		/// <returns>
		/// A value that indicates the relative order of the objects being compared.
		/// Returns less than zero if record1 is less than record2, 
		/// zero if record1 equals record2, 
		/// greater than zero if record1 is greater than record2.
		/// </returns>
		/// <exception cref="Exception">
		/// Throws an exception if either <paramref name="r1"/> or <paramref name="r2"/> is not a PlotChannelRecord instance.
		/// </exception>
		public int Compare(object? r1, object? r2) {
			if (r1 is not PlotChannelRecord record1 || r2 is not PlotChannelRecord record2)
				throw new Exception(Message.EXCEPTION_INTERFACE_COMPARER_NOT_SAME);

			return record1.Value.CompareTo(record2.Value);
		}
	}
}