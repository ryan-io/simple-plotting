using System.Collections;

namespace simple_plotting {
	public record PlotChannelRecord(DateTime DateTime, double Value) : IComparer {
		public int Compare(object? r1, object? r2) {
			if (r1 is not PlotChannelRecord record1 || r2 is not PlotChannelRecord record2)
				throw new Exception(Message.EXCEPTION_INTERFACE_COMPARER_NOT_SAME);

			return record1.Value.CompareTo(record2.Value);
		}
	}
}