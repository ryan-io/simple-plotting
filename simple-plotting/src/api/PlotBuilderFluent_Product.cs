// simple-plotting

using System.Drawing.Imaging;
using ScottPlot;
using ScottPlot.Plottable;

namespace simple_plotting;

public partial class PlotBuilderFluent {
	/// <summary>
	/// Renders all plots.
	/// </summary>
	/// ///	<inheritdoc />
	public IPlotBuilderFluentProduct RenderAllPlots() {
		foreach (var plot in _plots) 
			plot.Render();

		return this;
	}
	
	/// <inheritdoc cref="IPlotBuilderFluentProduct.GetPlots" />
	public IEnumerable<Plot> GetPlots() {
		return _plots ?? throw new InvalidOperationException(Message.EXCEPTION_INTERNAL_PLOT_COL_NULL);
	}

	/// <inheritdoc cref="IPlotBuilderFluentProduct.GetPlots" />
	public ref Plot GetPlotRef(int index) {
		if (_plots == null)
			throw new InvalidOperationException(Message.EXCEPTION_INTERNAL_PLOT_COL_NULL);

		if (index > _plots.Length - 1)
			throw new IndexOutOfRangeException(Message.EXCEPTION_INDEX_OUT_OF_RANGE);

		return ref _plots[index];
	}

	/// <inheritdoc cref="IPlotBuilderFluentProduct.GetPlots" />
	public IReadOnlyList<PlotChannel> GetPlotChannels()
		=> _data ?? throw new InvalidOperationException(Message.EXCEPTION_INTERNAL_PLOT_COL_NULL);

	/// <inheritdoc cref="IPlotBuilderFluentProduct.GetPlots" />
	public Plot GetPlot(int plotIndex) {
		if (_plots == null)
			throw new InvalidOperationException(Message.EXCEPTION_INTERNAL_PLOT_COL_NULL);

		plotIndex.ValidateInRange(_plots);
		return _plots[plotIndex];
	}

	/// <inheritdoc cref="IPlotBuilderFluentProduct.GetPlots" />
	public IEnumerable<string> GetScatterPlottableLabels(int plotIndex) {
		List<string> output = new();
		_cachedSignalPlottables.Clear();

		GetPlottablesAs<ScatterPlot>(plotIndex).CastTo(ref _cachedSignalPlottables);

		foreach (var plottable in _cachedSignalPlottables)
			output.Add(plottable.Label);

		return output;
	}

	/// <inheritdoc cref="IPlotBuilderFluentProduct.GetPlots" />
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

	/// <inheritdoc cref="IPlotBuilderFluentProduct.GetPlottablesAsCache{T}" />
	public void GetPlottablesAsCache<T>(int plotIndex, ref HashSet<T> cache)
		where T : class, IPlottable {
		if (_plots == null)
			throw new ArgumentException(Message.EXCEPTION_PLOT_IS_NULL);

		if (plotIndex > _plots.Length - 1)
			throw new IndexOutOfRangeException(Message.EXCEPTION_INDEX_OUT_OF_RANGE);

		var plot = _plots[plotIndex];

		cache.Clear();
		cache.AddRange(plot.GetPlottables().OfType<T>());
	}

	/// <inheritdoc/>
	public async Task<SaveStatus> TrySaveAsync(string savePath, string name, ImageFormat format, CancellationToken? 
            token = 
            default) {
		if (string.IsNullOrWhiteSpace(name))
			throw new Exception(Message.EXCEPTION_SAVE_PATH_INVALID);

		if (string.IsNullOrWhiteSpace(savePath))
			throw new Exception(Message.EXCEPTION_DEFINE_SOURCE_NOT_INVOKED);

		if (!CanSave)
			throw new Exception(Message.EXCEPTION_SAVE_PROCESS_RUNNING);

		IsSaving = true;

		try {
			await InternalSavePlots(name, format, token);
			return new SaveStatus(true, CachedPlotPaths);
		}
		catch (Exception) {
			return new SaveStatus(false, Enumerable.Empty<string>());
		}
		finally {
			IsSaving = false;
		}
	}

	/// <inheritdoc />
	public async Task<SaveStatus?> TrySaveAsyncAtSource(string name, ImageFormat format, CancellationToken? token = 
            default) {
		if (string.IsNullOrWhiteSpace(name))
			throw new Exception(Message.EXCEPTION_SAVE_PATH_INVALID);

		if (string.IsNullOrWhiteSpace(SourcePath))
			throw new Exception(Message.EXCEPTION_DEFINE_SOURCE_NOT_INVOKED);
		
		if (token == null) {
			ValidateCancellationTokenSource(true);
			token = CancellationTokenSource.Token;
		}
		
		return await Task.Run(() => TrySaveAsync(SourcePath, name, format, token.Value), token.Value);
	}

	/// <inheritdoc />
	public SaveStatus TrySave(string savePath, string name, ImageFormat format) {
		if (string.IsNullOrWhiteSpace(savePath) || string.IsNullOrWhiteSpace(name))
			throw new Exception(Message.EXCEPTION_SAVE_PATH_INVALID);

		if (!CanSave)
			throw new Exception(Message.EXCEPTION_SAVE_PROCESS_RUNNING);
		
		if (_plots == null)
			throw new InvalidOperationException(Message.EXCEPTION_INTERNAL_PLOT_COL_NULL);

		IsSaving = true;

		try {
			List<string> paths       = new();
			var          plotTracker = 1;

			foreach (var plot in _plots) {
				var path = plot.SaveFig($@"{savePath}\{name}_{plotTracker}.{format}");

				if (!string.IsNullOrWhiteSpace(path))
					paths.Add(path);

				plotTracker++;
			}

			return new SaveStatus(true, paths);
		}
		catch (Exception) {
			return new SaveStatus(false, Enumerable.Empty<string>());
		}
		finally {
			IsSaving = false;
		}
	}

	/// <inheritdoc />
	public SaveStatus TrySaveAtSource(string name, ImageFormat format) {
		if (string.IsNullOrWhiteSpace(name))
			throw new Exception(Message.EXCEPTION_SAVE_PATH_INVALID);

		if (!CanSave)
			throw new Exception(Message.EXCEPTION_SAVE_PROCESS_RUNNING);
		
		if (string.IsNullOrWhiteSpace(SourcePath))
			throw new Exception(Message.EXCEPTION_DEFINE_SOURCE_NOT_INVOKED);

		IsSaving = true;

		try {
			return TrySave(SourcePath, name, format);
		}
		catch (Exception e) {
			Console.WriteLine(e);
			throw;
		}
		finally {
			IsSaving = false;
		}
	}

	/// <inheritdoc />
	public IPlotBuilderFluentPostProcess GotoPostProcess() => this;

	/// <inheritdoc />
	public IPlotBuilderFluentPlottables GotoPlottables() => this;

	/// <inheritdoc />
	public IPlotBuilderFluentConfiguration GotoConfiguration() => this;

	/// <inheritdoc />
	public IPlotBuilderFluentOfType Reset(IReadOnlyList<PlotChannel> data) => StartNewPlot(data);

	/// <inheritdoc />
	public PlotSizeContainer GetPlotSizeContainer(PlotSize size) => PlotSizeMapper.Map(size);
}