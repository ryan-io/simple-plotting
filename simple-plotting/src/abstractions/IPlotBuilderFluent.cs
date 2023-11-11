namespace simple_plotting {
	/// <summary>
	///  The base abstraction for the fluent API.
	/// </summary>
	public interface IPlotBuilderFluent {
		/// <summary>
		///  This ensures a the Produce() method has been invoked before allowing you to save a plot.
		/// </summary>
		bool CanSave { get; }
	}
}