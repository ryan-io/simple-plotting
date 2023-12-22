// simple-plotting

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
	Task<CanvasSaveStatus> TrySaveAsync(string savePath, string name , bool disposeOnSuccess);

	/// <summary>
	///  Attempts to save the plot to the defined source path. This will throw an <see cref="Exception"/> if the save fails.
	///  This method requires you to call DefineSource.
	/// </summary>
	/// <param name="name">Name of each plot</param>
	/// <param name="disposeOnSuccess">Will dispose of all bitmap instances. No further modifications can be made.</param>
	/// <returns>Data structure with state (pass/fail) and list of strings containing full paths to each plot saved</returns>
	/// <exception cref="Exception">Thrown if savePath is null or whitespace</exception>
	Task<CanvasSaveStatus> TrySaveAtSourceAsync(string name, bool disposeOnSuccess);
}