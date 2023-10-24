// simple-plotting

using ScottPlot;
using ScottPlot.Plottable;

namespace simple_plotting.src;

/// <summary>
///  Static class for creating callbacks for plots. Use these as default implementations.
/// </summary>
public static class Callbacks  {
	/// <summary>
	///  Creates a callback for a scatter plot.
	/// </summary>
	/// <param name="lineWidth">Width of line</param>
	/// <param name="markerSize">Size of each point</param>
	/// <param name="lineStyle">Type of line; dashed, solid, dotted, etc.</param>
	/// <returns></returns>
	public static IPlotCallback ForScatterPlot(
		int lineWidth = 1, 
		int markerSize = 10, 
		LineStyle lineStyle = LineStyle.Solid) {
		return new ScatterPlotCallback {
			LineStyle  = lineStyle,
			LineWidth  = lineWidth,
			MarkerSize = markerSize
		};
	}
	
	/// <summary>
	///  Creates a callback for a signal plot.
	/// </summary>
	/// <param name="lineWidth">Width of line</param>
	/// <param name="markerSize">Size of each point</param>
	/// <param name="lineStyle">Type of line; dashed, solid, dotted, etc.</param>
	/// <returns></returns>
	public static IPlotCallback ForSignalPlot(
		int lineWidth = 1, 
		int markerSize = 10, 
		LineStyle lineStyle = LineStyle.Solid) {
		return new SignalPlotCallback {
			LineStyle  = lineStyle,
			LineWidth  = lineWidth,
			MarkerSize = markerSize
		};
	}
}

/// <summary>
///  Concrete implementation of IPlotCallback for a signal plot.
/// </summary>
public class SignalPlotCallback : IPlotCallback {
	public int       LineWidth  { get; set; } = 1;
	public int       MarkerSize { get; set; } = 10;
	public LineStyle LineStyle  { get; set; } = LineStyle.Solid;
	
	/// <summary>
	///  Sets LineWidth, MarkerSize, & LineStyle for a signal plot.
	/// </summary>
	/// <param name="plot">Plot to add signal plot to</param>
	/// <typeparam name="T">Signal plot</typeparam>
	/// <exception cref="InvalidCastException">Thrown if cannot cast plottable to SignalPlot</exception>
	public void Callback<T>(T plot) where T : class, IPlottable {
		if (typeof(T) != typeof(SignalPlot))
			return;

		if (plot is not SignalPlot sPlot)
			throw new InvalidCastException(Message.EXCEPTION_CAST_PLOTTABLE_ACTIVATOR_INSTANCES);

		sPlot.LineWidth  = 1;
		sPlot.MarkerSize = 0;
		sPlot.LineStyle  = LineStyle.Solid;
	}
}

public class ScatterPlotCallback : IPlotCallback {
	public int       LineWidth  { get; set; } = 1;
	public int       MarkerSize { get; set; } = 10;
	public LineStyle LineStyle  { get; set; } = LineStyle.Solid;
		
	/// <summary>
	///  Sets LineWidth, MarkerSize, & LineStyle for a scatter plot.
	/// </summary>
	/// <param name="plot">Plot to add scatter plot to</param>
	/// <typeparam name="T">Scatter plot</typeparam>
	/// <exception cref="InvalidCastException">Thrown if cannot cast plottable to ScatterPlot</exception>
	public void Callback<T>(T plot) where T : class, IPlottable {
		if (typeof(T) != typeof(ScatterPlot))
			return;

		if (plot is not ScatterPlot sPlot)
			throw new InvalidCastException(Message.EXCEPTION_CAST_PLOTTABLE_ACTIVATOR_INSTANCES);

		sPlot.LineWidth  = 1;
		sPlot.MarkerSize = 0;
		sPlot.LineStyle  = LineStyle.Solid;
	}
}