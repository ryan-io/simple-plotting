namespace simple_plotting {
	/// <summary>
	///  Static class for event handling
	/// </summary>
	public static class PlotEvent {
		/// <summary>
		///  Event for when a record is enumerated. This will only be called if the record is valid during parsing.
		///  It is up to the consumer to keep track of RecordEnumerated subscriptions.
		/// </summary>
		public static event Action<PlotEventData?>? RecordEnumerated;

		/// <summary>
		///  Invoke the RecordEnumerated event.
		/// </summary>
		/// <param name="record">Record to broadcast</param>
		internal static void OnRecordEnumerated(PlotEventData record) {
			RecordEnumerated?.Invoke(record);
		}
	}

	public readonly struct PlotEventData {
		public string            Channel           { get; }
		public PlotChannelRecord PlotChannelRecord { get; }

		public PlotEventData(string channel, PlotChannelRecord plotChannelRecord) {
			Channel           = channel;
			PlotChannelRecord = plotChannelRecord;
		}
	}
}