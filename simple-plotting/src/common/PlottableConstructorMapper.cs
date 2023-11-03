// simple-plotting

using ScottPlot.Plottable;

namespace simple_plotting.src;

/// <summary>
///  Maps a plottable type to a constructor. Will return an object array with the parameters for the constructor.
///  The assumption is a constructor exists with a constructor signature that matches the parameters in the object array.
/// </summary>
public class PlottableConstructorMapper {
	/// <summary>
	///  Determines the constructor parameters for a plottable type.
	/// </summary>
	/// <param name="data">POCO containing x values, y values, and sample rate</param>
	/// <returns>object[] with elements matching a ScottPlot plottable constructor signature</returns>
	public object[] Determine(ref PlottableData data) {
		if (_plottableType == null)
			return Array.Empty<object>();

		if (_plottableType == typeof(ScatterPlot))
			return GetScatterPlotConstructor(ref data.X, ref data.Y);

		if (_plottableType == typeof(SignalPlot))
			return GetSignalPlotConstructor();

		return Array.Empty<object>();
	}

	static object[] GetScatterPlotConstructor(ref double[] x, ref double[] y) => new object[] { x, y, null, null };

	static object[] GetSignalPlotConstructor() => Array.Empty<object>();

	public PlottableConstructorMapper(Type? plottableType) {
		_plottableType = plottableType;
	}

	readonly Type? _plottableType;
}