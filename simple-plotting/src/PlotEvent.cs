namespace simple_plotting.src {
	/// <summary>
	///  Static class for event handling
	/// </summary>
	public static class PlotEvent  {
		/// <summary>
		///  Event for when a record is enumerated. This will only be called if the record is valid during parsing.
		///  It is up to the consumer to keep track of RecordEnumerated subscriptions.
		/// </summary>
		public static event Action<PlotEventData?>? RecordEnumerated;
		
		/// <summary>
		///  Invoke the RecordEnumerated event.
		/// </summary>
		/// <param name="data">Record to broadcast</param>
		internal static void OnRecordEnumerated(PlotEventData data) {
			RecordEnumerated?.Invoke(data);
		}
	}

	/// <summary>
	///  Data passed from event handler to subscribers
	/// </summary>
	public readonly struct PlotEventData {
		/// <summary>
		///  Channel name the record belongs to
		/// </summary>
		public string Channel { get; }
		
		/// <summary>
		///  Event record
		/// </summary>
		public PlotChannelRecord Record { get; }

		/// <summary>
		///  Constructor 
		/// </summary>
		/// <param name="channel">Channel name the record belongs to</param>
		/// <param name="record">Event record</param>
		public PlotEventData(string channel, PlotChannelRecord record) {
			Channel     = channel;
			Record = record;
		}
	}
}