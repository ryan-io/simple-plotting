namespace simple_plotting;

/// <summary>
///  Defines characteristics for a channel in a plot.
/// </summary>
public class PlotChannel {
	/// <summary>
	///  The identifier of the channel. Should be parsed from CsvHelper
	///  Usually in the form of darwin.yokogawa.chxxx
	/// </summary>
	public string ChannelIdentifier { get; }

	/// <summary>
	/// Typically temperature or humidity
	/// </summary>
	public PlotChannelType ChannelType { get; }

	/// <summary>
	///  The values of the channel. 
	/// </summary>
	public ICollection<PlotChannelRecord> Records => _records;
	
	/// <summary>
	///  Adds a record to the channel.
	/// </summary>
	/// <param name="record">Record to add to internal collection</param>
	public void AddRecord(PlotChannelRecord record) => _records.Add(record);
	
	/// <summary>
	///  Adds a record to the channel. Returns true if the record was added, false if it already exists.
	/// </summary>
	/// <param name="record">Record to add to internal collection</param>
	/// <returns>False if exists, true if not</returns>
	public bool TryAddRecord(PlotChannelRecord record) {
		if (_records.Contains(record))
			return false;
		
		_records.Add(record);
		return true;
	}

	public PlotChannel(string channelIdentifier, PlotChannelType channelType) {
		_records          = new List<PlotChannelRecord>();
		ChannelIdentifier = channelIdentifier;
		ChannelType       = channelType;
	}

	readonly List<PlotChannelRecord> _records;
}