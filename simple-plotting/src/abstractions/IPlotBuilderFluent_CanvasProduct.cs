// simple-plotting

using System.Drawing.Imaging;
using ScottPlot;
using ScottPlot.Plottable;

namespace simple_plotting;

/// <summary>
/// Represents a fluent canvas product for building plots.
/// </summary>
public interface IPlotBuilderFluentCanvasProduct : IDisposable {
	/// <summary>
	///  The path to the CSV file.
	/// </summary>
	string? SourcePath { get; }

	/// <summary>
	/// Returns true if a save process is currently in progress. This will prevent multiple save
	/// processes from being started.
	/// </summary>
	bool IsSaving { get; }
	
	/// <summary>
	/// Cancels all ongoing operations and disposes the cancellation token source.
	/// </summary>
	void CancelAllOperations();

	/// <summary>
	/// Navigates to the configuration of the plot builder fluent canvas.
	/// </summary>
	/// <returns>
	/// An instance of IPlotBuilderFluentCanvasConfigurationMinimal that represents the configuration of the plot builder fluent canvas.
	/// </returns>
	IPlotBuilderFluentCanvasConfiguration GotoConfiguration();

	/// <summary>
	///  Attempts to save the plot to the specified source path path.
	/// *** NOTE: SetSource MUST be invoked for this method invocation to be successful. ***
	///  This will throw an <see cref="Exception"/> if the save fails.
	/// </summary>
	/// <param name="name">Name of each plot</param>
	/// <param name="format">Extension flag to save each plot as.</param>
	/// <param name="disposeOnSuccess">Will dispose of all bitmap instances. No further modifications can be made.</param>
	/// <returns>Data structure with state (pass/fail) and list of strings containing full paths to each plot saved</returns>
	/// <exception cref="Exception">Thrown if savePath is null or whitespace</exception>
	SaveStatus TrySaveAtSource(string name, ImageFormat format, bool disposeOnSuccess = false);

	/// <summary>
	/// Tries to save the plots at the specified source asynchronously.
	/// </summary>
	/// <param name="name">The name of the save path.</param>
	/// <param name="format">Extension flag to save each plot as.</param>
	/// <param name="disposeOnSuccess">Determines whether to dispose the BitmapParser on success.</param>
	/// <param name="token">The cancellation token (optional).</param>
	/// <returns>The save status indicating whether the plots were saved successfully or not.</returns>
	/// <exception cref="Exception">Thrown when the save path is invalid or the source is not defined.</exception>
	Task<SaveStatus> TrySaveAtSourceAsync(string name, ImageFormat format, bool disposeOnSuccess, CancellationToken?
		token);

	/// <summary>
	///  Attempts to save the plot to the specified path. This will throw an <see cref="Exception"/> if the save fails.
	/// </summary>
	/// <param name="savePath">Directory to save plots</param>
	/// <param name="format">Extension flag to save each plot as.</param>
	/// <param name="token">Optional cancellation token to provide to async state machine. If this is not provided, an internal token will be used.</param>
	/// <param name="disposeOnSuccess">Will dispose of all bitmap instances. No further modifications can be made.</param>
	/// <returns>Data structure with state (pass/fail) and list of strings containing full paths to each plot saved</returns>
	/// <exception cref="Exception">Thrown if savePath is null or whitespace</exception>
	Task<SaveStatus> TrySaveAtBmpParserAsync(string savePath, ImageFormat format, bool disposeOnSuccess,
		CancellationToken? token);

	/// <summary>
	///  Attempts to save the plot to the defined source path. This will throw an <see cref="Exception"/> if the save fails.
	///  This method requires you to call DefineSource.
	/// </summary>
	/// <param name="format">Format to save each image as.</param>
	/// <param name="disposeOnSuccess">Will dispose of all bitmap instances. No further modifications can be made.</param>
	/// <param name="token">Optional cancellation token to provide to async state machine. If this is not provided, an internal token will be used.</param>
	/// <returns>Data structure with state (pass/fail) and list of strings containing full paths to each plot saved</returns>
	/// <exception cref="Exception">Thrown if savePath is null or whitespace</exception>
	Task<SaveStatus> TrySaveAtBmpParserAsync(ImageFormat format, bool disposeOnSuccess, CancellationToken? token);

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
	IPlotBuilderFluentCanvasProduct ResizeCanvasImage(int plotIndex, float scale,
		BitmapResizeCriteria criteria);

	/// <summary>
	/// Resizes all canvas images.
	/// </summary>
	/// <param name="scale">The scaling factor for the image.</param>
	/// <param name="criteria">The criteria used for resizing the image.</param>
	/// <returns>The updated plot builder fluent canvas product.</returns>
	IPlotBuilderFluentCanvasProduct ResizeAllCanvasImage(float scale, BitmapResizeCriteria criteria);

	/// <summary>
	/// Renders all plots.
	/// </summary>
	IPlotBuilderFluentCanvasProduct RenderAllCanvasPlots();
}