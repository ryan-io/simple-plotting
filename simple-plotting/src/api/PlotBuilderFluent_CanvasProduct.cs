namespace simple_plotting;

/// <summary>
/// Represents a class for building plots on a canvas using a fluent interface.
/// </summary>
public partial class PlotBuilderFluent {
	/// <inheritdoc />
	IPlotBuilderFluentCanvasConfigurationMinimal IPlotBuilderFluentCanvasProduct.GotoConfiguration() => this;

	/// <inheritdoc />
	public IPlotBuilderFluentCanvasProduct ResizeCanvasImage(int plotIndex, float scale, BitmapResizeCriteria criteria) {
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
	async Task<CanvasSaveStatus> IPlotBuilderFluentCanvasProduct.TrySaveAsync(string savePath, string name,
		bool disposeOnSuccess) {
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
	public CanvasSaveStatus TrySaveAtSource(string name, bool disposeOnSuccess) {
		if (string.IsNullOrWhiteSpace(name))
			throw new Exception(Message.EXCEPTION_SAVE_PATH_INVALID);

		if (string.IsNullOrWhiteSpace(SourcePath))
			throw new Exception(Message.EXCEPTION_DEFINE_SOURCE_NOT_INVOKED);

		if (_plots == null)
			throw new InvalidOperationException(Message.EXCEPTION_INTERNAL_PLOT_COL_NULL);

		ValidateCancellationTokenSource(true);

		try {
			List<string> paths       = new();
			var          plotTracker = 1;

			foreach (var plot in _plots) {
				var path = plot.SaveFig($@"{SourcePath}\{name}_{plotTracker}{Constants.PNG_EXTENSION}");

				if (!string.IsNullOrWhiteSpace(path))
					paths.Add(path);

				plotTracker++;
			}

			if (disposeOnSuccess && BitmapParser != null)
				BitmapParser.Dispose();

			return new CanvasSaveStatus(true, paths);
		}
		catch (Exception e) {
			return new CanvasSaveStatus(false, Enumerable.Empty<string>());
		}
	}

	/// <inheritdoc />
	async Task<CanvasSaveStatus> IPlotBuilderFluentCanvasProduct.TrySaveAtBmpParserAsync(
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