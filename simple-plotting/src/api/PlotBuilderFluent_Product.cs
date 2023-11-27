// simple-plotting

using ScottPlot;
using ScottPlot.Plottable;

namespace simple_plotting;

public partial class PlotBuilderFluent {
	/// <inheritdoc />
	public IEnumerable<Plot> GetPlots() => _plots;

	/// <inheritdoc />
	public IReadOnlyList<PlotChannel> GetPlotChannels () => _data;

	/// <inheritdoc />
	public Plot GetPlot(int plotIndex) {
		plotIndex.ValidateInRange(_plots);
		return _plots[plotIndex];
	}

	/// <inheritdoc />
	public IEnumerable<string> GetScatterPlottableLabels(int plotIndex) {
		List<string> output     = new();
		var          plottables = GetPlottablesAs<ScatterPlot>(plotIndex);

		foreach (var plottable in plottables) {
			output.Add(plottable.Label);
		}

		return output;
	}

	/// <inheritdoc />
	public IEnumerable<string> GetSignalPlottableLabels(int plotIndex) {
		List<string> output = new();

		var plottables =
			GetPlottablesAs<SignalPlotXYConst<double, double>>(plotIndex);

		foreach (var plottable in plottables) {
			output.Add(plottable.Label);
		}

		return output;
	}

	/// <inheritdoc cref="IPlotBuilderFluentProduct.GetPlottablesAs{T}" />
	public IEnumerable<T> GetPlottablesAs<T>(int plotIndex) where T : class, IPlottable {
		if (plotIndex > _plots.Count)
			throw new IndexOutOfRangeException(Message.EXCEPTION_INDEX_OUT_OF_RANGE);

		var plottables = new List<T>();
		var plot       = _plots[plotIndex];

		plottables.AddRange(plot.GetPlottables().OfType<T>().ToList());

		return plottables;
	}

	/// <inheritdoc />
	public async Task<SaveStatus> TrySaveAsync(string savePath, string name) {
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

			return new SaveStatus(true, CachedPlotPaths);
		}
		catch (Exception) {
			return new SaveStatus(false, Enumerable.Empty<string>());
		}
	}

	/// <inheritdoc />
	public async Task<SaveStatus?> TrySaveAsyncAtSource(string name) {
		if (string.IsNullOrWhiteSpace(name))
			throw new Exception(Message.EXCEPTION_SAVE_PATH_INVALID);

		if (string.IsNullOrWhiteSpace(SourcePath))
			throw new Exception(Message.EXCEPTION_DEFINE_SOURCE_NOT_INVOKED);

		return await Task.Run(() => TrySaveAsync(SourcePath, name));
	}

	/// <inheritdoc />
	public SaveStatus TrySave(string savePath, string name) {
		if (string.IsNullOrWhiteSpace(savePath) || string.IsNullOrWhiteSpace(name))
			throw new Exception(Message.EXCEPTION_SAVE_PATH_INVALID);

		try {
			List<string> paths       = new();
			var          plotTracker = 1;

			foreach (var plot in _plots) {
				var path = plot.SaveFig($@"{savePath}\{name}_{plotTracker}{Constants.PNG_EXTENSION}");

				if (!string.IsNullOrWhiteSpace(path))
					paths.Add(path);

				plotTracker++;
			}

			return new SaveStatus(true, paths);
		}
		catch (Exception) {
			return new SaveStatus(false, Enumerable.Empty<string>());
		}
	}

	/// <inheritdoc />
	public SaveStatus TrySaveAtSource(string name) {
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
		return StartNew(data);
	}

	/// <inheritdoc />
	public PlotSizeContainer GetPlotSizeContainer(PlotSize size) => PlotSizeMapper.Map(size);
}