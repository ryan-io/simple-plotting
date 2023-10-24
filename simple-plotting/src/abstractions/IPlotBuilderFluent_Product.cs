// simple-plotting

using ScottPlot;
using ScottPlot.Plottable;

namespace simple_plotting.src;

/// <summary>
///  The generated plots. Can call { get; } after Produce() has been invoked and will return as an enumerable.
/// </summary>
public interface IPlotBuilderFluentProduct : IPlotBuilderFluent {
	/// <summary>
	///  The generated plots. Can call { get; } after Produce() has been invoked and will return as an enumerable.
	/// </summary>
	IEnumerable<Plot> GetPlots();

	/// <summary>
	///  Extracts actual channel names from the plots. 
	/// </summary>
	/// <returns>Enumerable of strings containing names extracted from plots</returns>
	IEnumerable<string> GetScatterPlotLabels(int plotIndex);

    /// <summary>
    ///  Helper method that returns an enumerable of an enumerable of type T
    ///  This is used to extract the plottables from the plots.
    ///  This method invokes OfType with the generic type T.
    /// </summary>
    /// <typeparam name="T">Class that implements IPlottable</typeparam>
    /// <returns>Enumerable of enumerables containing the plottables as type T</returns>
    IEnumerable<T> GetPlottablesAs<T> (int plotIndex) where T : class, IPlottable;

    /// <summary>
    ///  Attempts to save the plot to the specified path. This will throw an <see cref="Exception"/> if the save fails.
    /// </summary>
    /// <param name="savePath">Directory to save plots</param>
    /// <param name="name">Name of each plot</param>
    /// <returns>True if could write (save) to directory, otherwise false</returns>
    /// <exception cref="Exception">Thrown if savePath is null or whitespace</exception>
    bool TrySave(string savePath, string name);

	/// <summary>
	///  Attempts to save the plot to the defined source path. This will throw an <see cref="Exception"/> if the save fails.
	///  This method requires you to call DefineSource.
	/// </summary>
	/// <param name="name">Name of each plot</param>
	/// <returns>True if could write (save) to directory, otherwise false</returns>
	/// <exception cref="Exception">Thrown if savePath is null or whitespace</exception>
	bool TrySaveAtSource(string name);

	/// <summary>
	///  Exposes post processing API.
	/// </summary>
	/// <returns>Fluent builder as IPlotBuilderFluent_PostProcess</returns>
	IPlotBuilderFluentPostProcess PostProcess();

	/// <summary>
	///  Exposes configuration API.
	/// </summary>
	/// <returns>Fluent builder as IPlotBuilderFluent_Configuration</returns>
	IPlotBuilderFluentConfiguration Configure();

	/// <summary>
	///  Resets the builder to an initial state. This is useful if you want to reuse the builder with new data.
	/// </summary>
	/// <param name="data">New data to populate the builder with</param>
	/// <returns>Fluent builder in a reset state</returns>
	IPlotBuilderFluentOfType Reset(IReadOnlyList<PlotChannel> data);
}