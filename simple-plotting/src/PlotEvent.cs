namespace simple_plotting.src {
	/// <summary>
	///  Static class for event handling
	/// </summary>
	public static class PlotEvent  {
		/// <summary>
		///  Event for when a record is enumerated. This will only be called if the record is valid during parsing.
		///  It is up to the consumer to keep track of RecordEnumerated subscriptions.
		/// </summary>
		public static event Action<PlotChannelRecord?> RecordEnumerated;
		
		/// <summary>
		///  Invoke the RecordEnumerated event.
		/// </summary>
		/// <param name="record">Record to broadcast</param>
		internal static void OnRecordEnumerated(PlotChannelRecord record) {
			RecordEnumerated.Invoke(record);
		}
	}
}