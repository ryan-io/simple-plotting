// simple-plotting

using ScottPlot;
using ScottPlot.Plottable;

namespace simple_plotting;

public partial class PlotBuilderFluent {
	/// <inheritdoc />
	public IEnumerable<Plot> GetPlots() {
		return _plots ?? throw new InvalidOperationException(Message.EXCEPTION_INTERNAL_PLOT_COL_NULL);
	}

	/// <inheritdoc />
	public ref Plot GetPlotRef(int index) {
		if (_plots == null) 
			throw new InvalidOperationException(Message.EXCEPTION_INTERNAL_PLOT_COL_NULL);
		
		if (index > _plots.Length - 1)
			throw new IndexOutOfRangeException(Message.EXCEPTION_INDEX_OUT_OF_RANGE);

		return ref _plots[index];
	}

	/// <inheritdoc />
	public IReadOnlyList<PlotChannel> GetPlotChannels() 
		=> _data ?? throw new InvalidOperationException(Message.EXCEPTION_INTERNAL_PLOT_COL_NULL);

	/// <inheritdoc />
	public Plot GetPlot(int plotIndex) {
		if (_plots == null)
			throw new InvalidOperationException(Message.EXCEPTION_INTERNAL_PLOT_COL_NULL);
		
		plotIndex.ValidateInRange(_plots);
		return _plots[plotIndex];
	}

	/// <inheritdoc />
	public IEnumerable<string> GetScatterPlottableLabels(int plotIndex) {
		List<string> output = new();
		_cachedSignalPlottables.Clear();

		GetPlottablesAs<ScatterPlot>(plotIndex).CastTo(ref _cachedSignalPlottables);

		foreach (var plottable in _cachedSignalPlottables) 
			output.Add(plottable.Label);

		return output;
	}

	/// <inheritdoc />
	public IEnumerable<string> GetSignalPlottableLabels(int plotIndex) {
		var output = new List<string>();
		_cachedSignalPlottables.Clear();

		GetPlottablesAs<SignalPlotXYConst<double, double>>(plotIndex).CastTo(ref _cachedSignalPlottables);

		foreach (var plottable in _cachedSignalPlottables)
			output.Add(plottable.Label);

		return output;
	}

	/// <inheritdoc cref="IPlotBuilderFluentProduct.GetPlottablesAs{T}" />
	public HashSet<IPlottable> GetPlottablesAs<T>(int plotIndex) where T : class, IPlottable {
		if (_plots == null)
			throw new ArgumentException(Message.EXCEPTION_PLOT_IS_NULL);

		if (plotIndex > _plots.Length - 1)
			throw new IndexOutOfRangeException(Message.EXCEPTION_INDEX_OUT_OF_RANGE);

		var plot = _plots[plotIndex];

		_returnPlottables.Clear();
		_returnPlottables.AddRange(plot.GetPlottables().OfType<T>());

		return _returnPlottables;
	}

	/// <inheritdoc />
	public async Task<PlotSaveStatus> TrySaveAsync(string savePath, string name) {
		if (_plots == null)
			throw new InvalidOperationException(Message.EXCEPTION_INTERNAL_PLOT_COL_NULL);
		
		ValidateCancellationTokenSource(true);
		CachedPlotPaths.Clear();

		try {
			await Task.Run(
				() => {
					var plotTracker = new IntSafe(1);

					foreach (var plot in _plots) {
						var result = plot.SaveFig($@"{savePath}\{name}_{plotTracker}{Constants.PNG_EXTENSION}");
						CachedPlotPaths.Add(result);
						plotTracker++;
					}
				});

			return new PlotSaveStatus(true, CachedPlotPaths);
		}
		catch (Exception) {
			return new PlotSaveStatus(false, Enumerable.Empty<string>());
		}
	}

	/// <inheritdoc />
	public async Task<PlotSaveStatus?> TrySaveAsyncAtSource(string name) {
		if (string.IsNullOrWhiteSpace(name))
			throw new Exception(Message.EXCEPTION_SAVE_PATH_INVALID);

		if (string.IsNullOrWhiteSpace(SourcePath))
			throw new Exception(Message.EXCEPTION_DEFINE_SOURCE_NOT_INVOKED);

		return await Task.Run(() => TrySaveAsync(SourcePath, name));
	}

	/// <inheritdoc />
	public PlotSaveStatus TrySave(string savePath, string name) {
		if (string.IsNullOrWhiteSpace(savePath) || string.IsNullOrWhiteSpace(name))
			throw new Exception(Message.EXCEPTION_SAVE_PATH_INVALID);
		
		if (_plots == null)
			throw new InvalidOperationException(Message.EXCEPTION_INTERNAL_PLOT_COL_NULL);

		try {
			List<string> paths       = new();
			var          plotTracker = 1;

			foreach (var plot in _plots) {
				var path = plot.SaveFig($@"{savePath}\{name}_{plotTracker}{Constants.PNG_EXTENSION}");

				if (!string.IsNullOrWhiteSpace(path))
					paths.Add(path);

				plotTracker++;
			}

			return new PlotSaveStatus(true, paths);
		}
		catch (Exception) {
			return new PlotSaveStatus(false, Enumerable.Empty<string>());
		}
	}

	/// <inheritdoc />
	public PlotSaveStatus TrySaveAtSource(string name) {
		if (string.IsNullOrWhiteSpace(name))
			throw new Exception(Message.EXCEPTION_SAVE_PATH_INVALID);

		if (string.IsNullOrWhiteSpace(SourcePath))
			throw new Exception(Message.EXCEPTION_DEFINE_SOURCE_NOT_INVOKED);

		try {
			return TrySave(SourcePath, name);
		}
		catch (Exception e) {
			Console.WriteLine(e);
			throw;
		}
	}

	/// <inheritdoc />
	public IPlotBuilderFluentPostProcess GotoPostProcess() => this;

	/// <inheritdoc />
	public IPlotBuilderFluentPlottables GotoPlottables() => this;

	/// <inheritdoc />
	public IPlotBuilderFluentConfiguration GotoConfiguration() => this;

	/// <inheritdoc />
	public IPlotBuilderFluentOfType Reset(IReadOnlyList<PlotChannel> data) {
		return StartNewPlot(data);
	}

	/// <inheritdoc />
	public PlotSizeContainer GetPlotSizeContainer(PlotSize size) => PlotSizeMapper.Map(size);
}