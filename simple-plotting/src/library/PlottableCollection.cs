using ScottPlot.Plottable;

namespace simple_plotting {
	/// <summary>
	///  A collection of IPlottable objects. This is serializable.
	/// </summary>
	[Serializable]
	public class PlottableCollection : Dictionary<int, IPlottable> {
	}
}