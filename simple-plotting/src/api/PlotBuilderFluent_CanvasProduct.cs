using ScottPlot.Plottable;

namespace simple_plotting;

/// <summary>
/// Represents a class for building plots on a canvas using a fluent interface.
/// </summary>
public partial class PlotBuilderFluent {
	/// <inheritdoc />
	IPlotBuilderFluentCanvasConfiguration IPlotBuilderFluentCanvasProduct.GotoConfiguration() => this;

	/// <inheritdoc />
	public IPlotBuilderFluentCanvasProduct
		ResizeCanvasImage(int plotIndex, float scale, BitmapResizeCriteria criteria) {
		if (!_imageMap.ContainsKey(plotIndex))
			throw new Exception(Message.EXCEPTION_DOES_NOT_CONTAIN_CANVAS_INDEX);

		if (BitmapParser == null)
			throw new Exception(Message.EXCEPTION_NO_BITMAP_PARSER);

		var resizedImg = BitmapParser.GetNewScaledBitmap(plotIndex, scale, criteria);

		var mappedImg = _imageMap[plotIndex];
		mappedImg.Bitmap.Dispose();

		_plots[plotIndex].Remove(mappedImg);
		AddImgToCanvas(true, plotIndex, resizedImg);
		BitmapParser.SetNewBitmap(plotIndex, resizedImg);
		RenderImagePlottablesBack(plotIndex);

		return this;
	}

	/// <inheritdoc />
	public IPlotBuilderFluentCanvasProduct ResizeAllCanvasImage(float scale, BitmapResizeCriteria criteria) {
		if (BitmapParser == null)
			throw new Exception(Message.EXCEPTION_NO_BITMAP_PARSER);

		for (var i = 0; i < _plots.Length; i++) {
			ResizeCanvasImage(i, scale, criteria);
		}

		return this;
	}

	/// <inheritdoc />
	public async Task<SaveStatus> TrySaveAtBmpParserAsync(string savePath, bool disposeOnSuccess,
		CancellationToken? token) {
		if (string.IsNullOrWhiteSpace(savePath))
			throw new Exception(Message.EXCEPTION_SAVE_PATH_INVALID);

		if (_plots == null)
			throw new InvalidOperationException(Message.EXCEPTION_INTERNAL_PLOT_COL_NULL);

		if (BitmapParser == null)
			throw new NoBitmapParserException();

		if (BitmapParser.IsDisposed)
			return new SaveStatus(false, Enumerable.Empty<string>());

		if (token == null) {
			ValidateCancellationTokenSource(true);
			token = CancellationTokenSource.Token;
		}

		try {
			var paths = await BitmapParser.SaveBitmapsAsync(savePath, token.Value, disposeOnSuccess);
			return new SaveStatus(true, paths);
		}
		catch (Exception e) {
			return new SaveStatus(false, Enumerable.Empty<string>(), e.Message);
		}
	}

	/// <inheritdoc />
	public async Task<SaveStatus> TrySaveAtBmpParserAsync(bool disposeOnSuccess,
		CancellationToken? token) {
		if (string.IsNullOrWhiteSpace(SourcePath))
			throw new Exception(Message.EXCEPTION_DEFINE_SOURCE_NOT_INVOKED);

		return await TrySaveAtBmpParserAsync(SourcePath, disposeOnSuccess, token);
	}

	/// <inheritdoc />
	public async Task<SaveStatus> TrySaveAtSourceAsync(string name, bool disposeOnSuccess, CancellationToken? token) {
		if (string.IsNullOrWhiteSpace(name))
			throw new Exception(Message.EXCEPTION_SAVE_PATH_INVALID);

		if (string.IsNullOrWhiteSpace(SourcePath))
			throw new Exception(Message.EXCEPTION_DEFINE_SOURCE_NOT_INVOKED);
		
		try {
			await InternalSavePlots(name, token);

			if (disposeOnSuccess && BitmapParser != null)
				BitmapParser.Dispose();

			return new SaveStatus(true, CachedPlotPaths);
		}
		catch (Exception e) {
			return new SaveStatus(false, Enumerable.Empty<string>(), e.Message);
		}
	}

	/// <inheritdoc />
	public SaveStatus TrySaveAtSource(string name, bool disposeOnSuccess) {
		if (string.IsNullOrWhiteSpace(name))
			throw new Exception(Message.EXCEPTION_SAVE_PATH_INVALID);

		if (string.IsNullOrWhiteSpace(SourcePath))
			throw new Exception(Message.EXCEPTION_DEFINE_SOURCE_NOT_INVOKED);

		if (_plots == null)
			throw new InvalidOperationException(Message.EXCEPTION_INTERNAL_PLOT_COL_NULL);

		try {
			List<string> paths       = new();
			var          plotTracker = 1;

			foreach (var plot in _plots) {
				var path = plot.SaveFig($@"{SourcePath}\{name}_{plotTracker}{Constants.PNG_EXTENSION }");

				if (!string.IsNullOrWhiteSpace(path))
					paths.Add(path);

				plotTracker++;
			}

			if (disposeOnSuccess && BitmapParser != null)
				BitmapParser.Dispose();

			return new SaveStatus(true, paths);
		}
		catch (Exception e) {
			return new SaveStatus(false, Enumerable.Empty<string>(), e.Message);
		}
	}

	/// <summary>
	/// Renders specified image to the back of the plot.
	/// </summary>
	/// <param name="plotIndex">The index of the plot in which the image should be rendered.</param>
	void RenderImagePlottablesBack(int plotIndex) {
		var plottables = GetPlottablesAs<Image>(plotIndex);

		if (!plottables.Any())
			return;

		foreach (var plottable in plottables)
			_plots[plotIndex].MoveFirst(plottable);
	}
}