using System.Collections.Concurrent;
using ScottPlot;
using ScottPlot.Plottable;
using simple_plotting.runtime;

namespace simple_plotting {
	/// <summary>
	///  This class is used to build a plot using a fluent API. It is recommended to use this class instead of ScottPlot.Plot
	///  or CsvPlotter.Plot wrapper directly.
	/// This is the view model in MVVM.
	/// </summary>
	public partial class PlotBuilderFluent : IPlotBuilderFluentOfType,
	                                         IPlotBuilderFluentConfiguration,
	                                         IPlotBuilderFluentReadyToProduce,
	                                         IPlotBuilderFluentPlottables,
	                                         IPlotBuilderFluentPostProcess,
	                                         IPlotBuilderFluentProduct {
		/// <inheritdoc />
		public bool CanSave => _data.Any() && _plotWasProduced;

		/// <summary>
		/// Total number of graphs to generate
		/// </summary>
		int PlotCount { get; }

		/// <summary>
		/// Cancellation source used for saving plots asynchronously
		/// </summary>
		CancellationTokenSource CancellationTokenSource { get; set; } = new();
		
		/// <summary>
		///  Thread safe collection for plot paths
		/// </summary>
		ConcurrentBag<string> CachedPlotPaths { get; } = new();

		/// <summary>
		///  Factory for adding graphs to plots
		/// </summary>
		IPlottablePrime? FactoryPrime { get; set; }

		/// <summary>
		///  Hashset containing the actions to be observed by the view model. This is invoked when Produce() is called.
		/// </summary>
		HashSet<Action> Observables { get; } = new();

		/// <summary>
		///  The path to the CSV file.
		/// </summary>
		string SourcePath { get; set; } = string.Empty;

		/// <summary>
		/// Create a new PlotBuilderFluent instance. This is the entry point for the fluent API. Requires parsed CSV data
		/// from <see cref="data"/>. This static method also requires a pre-allocated collection of plots.
		/// </summary>
		/// <param name="data">Parsed data</param>
		/// <param name="plotInitializer">Collection of ScottPlot.Plot</param>
		/// <returns>New instance of PlotBuilderFluent (this)</returns>
		public static IPlotBuilderFluentOfType StartNewPlot(
			IReadOnlyList<PlotChannel> data, IReadOnlyCollection<Plot> plotInitializer)
			=> new PlotBuilderFluent(data, plotInitializer);

		/// <summary>
		/// Create a new PlotBuilderFluent instance. This is the entry point for the fluent API. Requires parsed CSV data
		/// from <see cref="CsvParser"/>.
		/// </summary>
		/// <param name="data">Parsed data</param>
		/// <param name="numOfPlots">Number of plots to generate</param>
		/// <returns>New instance of PlotBuilderFluent (this)</returns>
		public static IPlotBuilderFluentOfType StartNewPlot(IReadOnlyList<PlotChannel> data, int numOfPlots = 1)
			=> new PlotBuilderFluent(data, numOfPlots);

		/// <summary>
		/// Creates a new canvas with the specified width and height.
		/// </summary>
		/// <param name="width">The width of the canvas in pixels.</param>
		/// <param name="height">The height of the canvas in pixels.</param>
		/// <returns>Returns a new instance of the <see cref="PlotBuilderFluentPlotBuilderFluentCanvas"/> class with a plot within the specified width and height.</returns>
		public static IPlotBuilderFluentCanvas StartNewCanva
			(int width, int height) => new PlotBuilderFluentPlotBuilderFluentCanvas(new Plot(width, height));

		///	<inheritdoc />
		public Type? PlotType { get; private set; }
		
		/// <summary>
		///  Dispose of the cancellation token source. This should be invoked on application exit or stop.
		/// </summary>
		public void Dispose() {
			ValidateCancellationTokenSource();
		}

		/// <summary>
		///  Helper method to set the initial state of the plot. This is called in the constructor.
		///  This method will divvy up the data into separate plots based on on the number of plots specified in the constructor.
		/// </summary>
		void SetInitialState(Action<IEnumerable<PlotChannelRecord>, int, PlotChannel> actionDelegate) {
			// ReSharper disable once ForCanBeConvertedToForeach
			for (var index = 0; index < _data.Count; index++) {
				var channel   = _data[index];
				var batch     = channel.Records.Batch(PlotCount).ToArray();
				var plotTracker = 0;

				foreach (var batchedRecord in batch) {
					if (batchedRecord.Value.IsNullOrEmpty())
						continue;
				
					actionDelegate.Invoke(batchedRecord.Value, plotTracker, channel);
					plotTracker++;
				}
			}
		}

		/// <summary>
		///  Helper method that returns an enumerable of an enumerable of type T
		///  This is used to extract the plottables from the plots.
		///  This method invokes OfType with the generic type T.
		/// </summary>
		/// <typeparam name="T">Class that implements IPlottable</typeparam>
		/// <returns>Enumerable of enumerables containing the plottables as type T</returns>
		IEnumerable<List<T>> GetPlottablesAs<T>() where T : class, IPlottable {
			var plottables = new List<List<T>>();

			foreach (var p in _plots) {
				plottables.Add(p.GetPlottables().OfType<T>().ToList());
			}

			return plottables;
		}
		
		void ValidateCancellationTokenSource(bool createNewIfDisposed = false) {
			if (!CancellationTokenSource.IsCancellationRequested)
				CancellationTokenSource.Cancel();
			
			CancellationTokenSource.Dispose();
			
			if (createNewIfDisposed)
				CancellationTokenSource = new CancellationTokenSource();
		}

		/// <summary>
		///  Create a new PlotBuilderFluent instance. This is the entry point for the fluent API. Requires parsed CSV data
		///  This will throw an <see cref="ArgumentException"/> if the data is null or empty.
		/// </summary>
		/// <param name="data">Parsed data</param>
		/// <param name="numOfPlots">Number of plot files to generate</param>
		/// <exception cref="ArgumentException">Thrown if data is null or empty</exception>
		PlotBuilderFluent(IReadOnlyList<PlotChannel> data, int numOfPlots = 1) {
			if (data == null || !data.Any())
				throw new ArgumentException("Data cannot be null or empty.");

			_data  = data;
			_plots = new List<Plot>();

			for (var i = 0; i < numOfPlots; i++) {
				_plots.Add(new Plot(Constants.DEFAULT_WIDTH, Constants.DEFAULT_HEIGHT));
			}

			PlotCount = numOfPlots;
		}

		/// <summary>
		///  Create a new PlotBuilderFluent instance. This is the entry point for the fluent API. Requires parsed CSV data
		///  and a pre-allocated collection of plots. Use this constructor when working with Avalonia, WPF, or WinForms.
		///  This will throw an <see cref="ArgumentException"/> if the data is null or empty.
		/// </summary>
		/// <param name="data">Parsed data</param>
		/// <param name="plotInitializer">Pre-allocated collection of plots</param>
		/// <exception cref="ArgumentException">Thrown if data is null or empty</exception>
		PlotBuilderFluent(IReadOnlyList<PlotChannel> data, IReadOnlyCollection<Plot> plotInitializer) {
			if (data == null || !data.Any())
				throw new ArgumentException("Data cannot be null or empty.");

			_data  = data;
			_plots = new List<Plot>(plotInitializer);

			foreach (var p in _plots) {
				p.Width  = Constants.DEFAULT_WIDTH;
				p.Height = Constants.DEFAULT_HEIGHT;
			}

			PlotCount = plotInitializer.Count;
		}

		bool                                _plotWasProduced;
		readonly List<Plot>                 _plots;
		readonly IReadOnlyList<PlotChannel> _data;
	}
}