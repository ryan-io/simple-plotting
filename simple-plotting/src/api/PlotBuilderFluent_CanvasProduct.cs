using simple_plotting.runtime;

namespace simple_plotting;

/// <summary>
/// Represents a class for building plots on a canvas using a fluent interface.
/// </summary>
public partial class PlotBuilderFluent {
	/// <inheritdoc />
	IPlotBuilderFluentCanvasConfigurationMinimal IPlotBuilderFluentCanvasProduct.GotoConfiguration() => this;
	
	/// <inheritdoc />
	async Task<CanvasSaveStatus> IPlotBuilderFluentCanvasProduct.TrySaveAsync(string savePath, string name, bool disposeOnSuccess) {
		if (string.IsNullOrWhiteSpace(savePath))
			throw new Exception(Message.EXCEPTION_SAVE_PATH_INVALID);
		
		if (string.IsNullOrWhiteSpace(name))
			throw new Exception(Message.EXCEPTION_SAVE_PATH_INVALID);
		
		if (_plots == null)
			throw new InvalidOperationException(Message.EXCEPTION_INTERNAL_PLOT_COL_NULL);
		
		if (BitmapParser == null)
			throw new NoBitmapParserException();

		
		if (BitmapParser.IsDisposed)
			return new CanvasSaveStatus(false, Enumerable.Empty<string>());
		
		ValidateCancellationTokenSource(true);
		
		try {
			var paths = await BitmapParser.SaveBitmapsAsync(savePath, disposeOnSuccess);
			return new CanvasSaveStatus(true, paths);
		}
		catch (Exception) {
			return new CanvasSaveStatus(false, Enumerable.Empty<string>());
		}
	}
	
	/// <inheritdoc />
	async Task<CanvasSaveStatus> IPlotBuilderFluentCanvasProduct.TrySaveAtSourceAsync(
		string name, bool disposeOnSuccess) {
		if (string.IsNullOrWhiteSpace(name))
			throw new Exception(Message.EXCEPTION_SAVE_PATH_INVALID);

		if (string.IsNullOrWhiteSpace(SourcePath))
			throw new Exception(Message.EXCEPTION_DEFINE_SOURCE_NOT_INVOKED);
		
		if (_plots == null)
			throw new InvalidOperationException(Message.EXCEPTION_INTERNAL_PLOT_COL_NULL);
		
		if (BitmapParser == null)
			throw new NoBitmapParserException();
		
		if (BitmapParser.IsDisposed)
			return new CanvasSaveStatus(false, Enumerable.Empty<string>());
		
		ValidateCancellationTokenSource(true);

		try {
			var paths = await BitmapParser.SaveBitmapsAsync(SourcePath, disposeOnSuccess);
			return new CanvasSaveStatus(true, paths);
		}
		catch (Exception) {
			return new CanvasSaveStatus(false, Enumerable.Empty<string>());
		}
	}
}