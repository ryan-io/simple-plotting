using ScottPlot;
using ScottPlot.Plottable;
using simple_plotting.runtime;

namespace simple_plotting.src {
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
		/// <summary>
		///  This ensures a the Produce() method has been invoked before allowing you to save a plot.
		/// </summary>
		public bool CanSave => _data.Any() && _plotWasProduced;

		/// <summary>
		/// Total number of graphs to generate
		/// </summary>
		int PlotCount { get; }

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
		public static IPlotBuilderFluentOfType StartNew(
			IReadOnlyList<PlotChannel> data, IReadOnlyCollection<Plot> plotInitializer)
			=> new PlotBuilderFluent(data, plotInitializer);

		/// <summary>
		/// Create a new PlotBuilderFluent instance. This is the entry point for the fluent API. Requires parsed CSV data
		/// from <see cref="CsvParser"/>.
		/// </summary>
		/// <param name="data">Parsed data</param>
		/// <param name="numOfPlots">Number of plots to generate</param>
		/// <returns>New instance of PlotBuilderFluent (this)</returns>
		public static IPlotBuilderFluentOfType StartNew(IReadOnlyList<PlotChannel> data, int numOfPlots = 1)
			=> new PlotBuilderFluent(data, numOfPlots);

		/// <summary>
		///  Helper method to set the initial state of the plot. This is called in the constructor.
		///  This method will divvy up the data into separate plots based on on the number of plots specified in the constructor.
		/// </summary>
		void SetInitialState(Action<IEnumerable<PlotChannelRecord>, int, PlotChannel> actionDelegate) {
			// ReSharper disable once ForCanBeConvertedToForeach
			for (var index = 0; index < _data.Count; index++) {
				var channel   = _data[index];
				var batchSize = channel.Records.Count / PlotCount;
				var batch     = channel.Records.Batch(batchSize).ToArray();

				if (batch[^1] != null && batch.Length > PlotCount) {
					var lastBatch = batch.Last();

					if (lastBatch != null && batch[^2] != null) {
						batch[^2] = batch[^2]?.Concat(lastBatch);
						batch[^1] = null;
					}
				}

				var plotTracker = 0;

				foreach (var batchedRecord in batch) {
					if (batchedRecord == null)
						continue;

					actionDelegate.Invoke(batchedRecord, plotTracker, channel);
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