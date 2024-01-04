// simple-plotting

using ScottPlot;
using ScottPlot.Plottable;

namespace simple_plotting;

/// <summary>
/// Represents a fluent canvas product for building plots.
/// </summary>
public interface IPlotBuilderFluentCanvasProduct : IDisposable {
	/// <summary>
	/// Navigates to the configuration of the plot builder fluent canvas.
	/// </summary>
	/// <returns>
	/// An instance of IPlotBuilderFluentCanvasConfigurationMinimal that represents the configuration of the plot builder fluent canvas.
	/// </returns>
	IPlotBuilderFluentCanvasConfigurationMinimal GotoConfiguration();

	/// <summary>
	///  Attempts to save the plot to the specified path. This will throw an <see cref="Exception"/> if the save fails.
	/// </summary>
	/// <param name="savePath">Directory to save plots</param>
	/// <param name="name">Name of each plot</param>
	/// <param name="disposeOnSuccess">Will dispose of all bitmap instances. No further modifications can be made.</param>
	/// <returns>Data structure with state (pass/fail) and list of strings containing full paths to each plot saved</returns>
	/// <exception cref="Exception">Thrown if savePath is null or whitespace</exception>
	Task<CanvasSaveStatus> TrySaveAsync(string savePath, string name, bool disposeOnSuccess);

	/// <summary>
	///  Attempts to save the plot to the specified source path path.
	/// *** NOTE: SetSource MUST be invoked for this method invocation to be successful. ***
	///  This will throw an <see cref="Exception"/> if the save fails.
	/// </summary>
	/// <param name="name">Name of each plot</param>
	/// <param name="disposeOnSuccess">Will dispose of all bitmap instances. No further modifications can be made.</param>
	/// <returns>Data structure with state (pass/fail) and list of strings containing full paths to each plot saved</returns>
	/// <exception cref="Exception">Thrown if savePath is null or whitespace</exception>
	CanvasSaveStatus TrySaveAtSource(string name, bool disposeOnSuccess);

	/// <summary>
	///  Attempts to save the plot to the defined source path. This will throw an <see cref="Exception"/> if the save fails.
	///  This method requires you to call DefineSource.
	/// </summary>
	/// <param name="name">Name of each plot</param>
	/// <param name="disposeOnSuccess">Will dispose of all bitmap instances. No further modifications can be made.</param>
	/// <returns>Data structure with state (pass/fail) and list of strings containing full paths to each plot saved</returns>
	/// <exception cref="Exception">Thrown if savePath is null or whitespace</exception>
	Task<CanvasSaveStatus> TrySaveAtBmpParserAsync(string name, bool disposeOnSuccess);

	/// <summary>
	///  The generated plots. Can call { get; } after Produce() has been invoked and will return as an enumerable.
	/// </summary>
	IEnumerable<Plot> GetPlots();

	/// <summary>
	/// Retrieves a reference to the <see cref="Plot"/> object located at the specified index in the internal plots array.
	/// </summary>
	/// <param name="index">The zero-based index in the array at which the desired <see cref="Plot"/> object is located.</param>
	/// <exception cref="IndexOutOfRangeException">Thrown when the <paramref name="index"/> is not a valid index in the plots array.</exception>
	/// <returns>A reference to the <see cref="Plot"/> object found at the specified index in the plots array.</returns>
	ref Plot GetPlotRef(int index);

	/// <summary>
	/// Gets a readonly collection of type PlotChannels
	/// A call to this method should only be made once plots have been generated
	/// </summary>
	/// <returns></returns>
	IReadOnlyList<PlotChannel> GetPlotChannels();

	/// <summary>
	///  The generated plots. Queries the plot collection and returns the plot at the specified index.
	/// </summary>
	Plot GetPlot(int plotIndex);

	/// <summary>
	///  Extracts actual channel names from the plots. 
	/// </summary>
	/// <returns>Enumerable of strings containing names extracted from plots</returns>
	IEnumerable<string> GetScatterPlottableLabels(int plotIndex);

	/// <summary>
	///  Extracts actual channel names from the plots. 
	/// </summary>
	/// <returns>Enumerable of strings containing names extracted from plots</returns>
	IEnumerable<string> GetSignalPlottableLabels(int plotIndex);

	/// <summary>
	///  Helper method that returns an enumerable of an enumerable of type T
	///  This is used to extract the plottables from the plots.
	///  This method invokes OfType with the generic type T.
	/// </summary>
	/// <typeparam name="T">Class that implements IPlottable</typeparam>
	/// <returns>HashSet of enumerables containing the plottables as type T</returns>
	HashSet<IPlottable> GetPlottablesAs<T>(int plotIndex) where T : class, IPlottable;

	/// <summary>
	/// Resizes the canvas image at the specified plot index with the given scale and resize criteria.
	/// </summary>
	/// <param name="plotIndex">The index of the canvas plot.</param>
	/// <param name="scale">The scale to resize the image by.</param>
	/// <param name="criteria">The criteria for resizing the image.</param>
	/// <exception cref="Exception">Thrown when the canvas plot at the specified index does not exist or when the bitmap parser is null.</exception>
	/// <returns>The <see cref="IPlotBuilderFluentCanvasProduct"/> instance for chaining.</returns>
	IPlotBuilderFluentCanvasProduct ResizeCanvasImage(int plotIndex, float scale, BitmapResizeCriteria criteria);
}