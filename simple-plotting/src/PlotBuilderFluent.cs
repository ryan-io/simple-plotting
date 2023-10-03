using ScottPlot;
using SimplePlot.Runtime;

namespace SimplePlot {
	/// <summary>
	///  This class is used to build a plot using a fluent API. It is recommended to use this class instead of ScottPlot.Plot
	///  or CsvPlotter.Plot wrapper directly.
	/// This is the view model in MVVM.
	/// </summary>
	public class PlotBuilderFluent : IPlotBuilderFluent_Configuration,
	                                 IPlotBuilderFluent_ReadyToProduce,
	                                 IPlotBuilderFluent_Tools,
	                                 IPlotBuilderFluent_Product {
		/// <summary>
		///  This ensures a the Produce() method has been invoked before allowing you to save a plot.
		/// </summary>
		public bool CanSave => _data.Any() && _plotWasProduced;

		/// <summary>
		///  The generated plots. Can call { get; } after Produce() has been invoked and will return as an enumerable.
		/// </summary>
		public IEnumerable<Plot> GetPlots() => _plots;

		/// <summary>
		/// Create a new PlotBuilderFluent instance. This is the entry point for the fluent API. Requires parsed CSV data
		/// from <see cref="CsvParser"/>. This static method also requires a pre-allocated collection of plots.
		/// </summary>
		/// <param name="data">Parsed data</param>
		/// <param name="plotInitializer">Collection of ScottPlot.Plot</param>
		/// <returns>New instance of PlotBuilderFluent (this)</returns>
		public static IPlotBuilderFluent_Configuration StartNew(
			IReadOnlyList<PlotChannel> data, IReadOnlyCollection<Plot> plotInitializer)
			=> new PlotBuilderFluent(data, plotInitializer);

		/// <summary>
		/// Create a new PlotBuilderFluent instance. This is the entry point for the fluent API. Requires parsed CSV data
		/// from <see cref="CsvParser"/>.
		/// </summary>
		/// <param name="data">Parsed data</param>
		/// <param name="numOfPlots">Number of plots to generate</param>
		/// <returns>New instance of PlotBuilderFluent (this)</returns>
		public static IPlotBuilderFluent_Configuration StartNew(IReadOnlyList<PlotChannel> data, int numOfPlots = 1)
			=> new PlotBuilderFluent(data, numOfPlots);

		/// <summary>
		/// Sets the size of the plot window.
		/// </summary>
		/// <param name="container">Container that defines width & height</param>
		/// <returns>PlotBuilderFluent (this)</returns>
		public IPlotBuilderFluent_Configuration WithSize(PlotSizeContainer container) {
			foreach (var plot in _plots) {
				plot.Width  = container.Width;
				plot.Height = container.Height;
			}

			return this;
		}

		/// <summary>
		///  Sets the title of the plot.
		/// </summary>
		/// <param name="title">String title for plot</param>
		/// <returns>Fluent builder</returns>
		/// <exception cref="Exception">Thrown if string is null or whitespace</exception>
		public IPlotBuilderFluent_Configuration WithTitle(string title) {
			if (string.IsNullOrWhiteSpace(title))
				throw new Exception(Message.EXCEPTION_TITLE_INVALID);

			foreach (var plot in _plots) {
				plot.Title(title);
			}

			return this;
		}

		/// <summary>
		///  Sets the label for the X axis.
		/// </summary>
		/// <param name="xAxisLabel">String value for the X axis label</param>
		/// <returns>Fluent builder</returns>
		/// <exception cref="Exception">Thrown if xAxisLabel is null or whitespace</exception>
		public IPlotBuilderFluent_Configuration WithPrimaryXAxisLabel(string xAxisLabel) {
			if (string.IsNullOrWhiteSpace(xAxisLabel))
				throw new Exception(Message.EXCEPTION_AXIS_LABEL_INVALID);

			foreach (var plot in _plots) {
				plot.XAxis.Label(xAxisLabel);
			}

			return this;
		}

		/// <summary>
		///  Sets the label for the Y axis.
		/// </summary>
		/// <param name="yAxisLabel">String value for the Y axis label</param>
		/// <returns>Fluent builder</returns>
		/// <exception cref="Exception">Thrown if yAxisLabel is null or whitespace</exception>
		public IPlotBuilderFluent_Configuration WithPrimaryYAxisLabel(string yAxisLabel) {
			if (string.IsNullOrWhiteSpace(yAxisLabel))
				throw new Exception(Message.EXCEPTION_AXIS_LABEL_INVALID);

			foreach (var plot in _plots) {
				plot.YAxis.Label(yAxisLabel);
			}

			return this;
		}

		/// <summary>
		///  Sets the rotation of the primary Y axis ticks.
		/// </summary>
		/// <param name="container">Container for rotation of axis</param>
		/// <returns>Fluent builder</returns>
		public IPlotBuilderFluent_Configuration RotatePrimaryYAxisTicks(PlotAxisRotationContainer container) {
			foreach (var plot in _plots) {
				plot.YAxis.TickLabelStyle(rotation: container.Rotation);
			}

			return this;
		}

		/// <summary>
		///  Sets the rotation of the primary X axis ticks.
		/// </summary>
		/// <param name="container">Container for rotation of axis</param>
		/// <returns>Fluent builder</returns>
		public IPlotBuilderFluent_Configuration RotatePrimaryXAxisTicks(PlotAxisRotationContainer container) {
			foreach (var plot in _plots) {
				plot.XAxis.TickLabelStyle(rotation: container.Rotation);
			}

			return this;
		}

		/// <summary>
		///  Sets the label for the secondary X axis.
		/// </summary>
		/// <param name="xAxisLabel">Label for the secondary x-axis (top side of plot)</param>
		/// <returns>Fluent builder</returns>
		/// <exception cref="Exception">Thrown if the label is null or whitespace</exception>
		public IPlotBuilderFluent_Configuration WithSecondaryXAxisLabel(string xAxisLabel) {
			if (string.IsNullOrWhiteSpace(xAxisLabel))
				throw new Exception(Message.EXCEPTION_AXIS_LABEL_INVALID);

			foreach (var plot in _plots) {
				plot.XAxis2.Label(xAxisLabel);
			}

			return this;
		}

		/// <summary>
		///  Sets the label for the secondary Y axis.
		/// </summary>
		/// <param name="yAxisLabel">Label for the secondary x-axis (right side of plot)</param>
		/// <returns>Fluent builder</returns>
		/// <exception cref="Exception">Thrown if the label is null or whitespace</exception>
		public IPlotBuilderFluent_Configuration WithSecondaryYAxisLabel(string yAxisLabel) {
			if (string.IsNullOrWhiteSpace(yAxisLabel))
				throw new Exception(Message.EXCEPTION_AXIS_LABEL_INVALID);

			foreach (var plot in _plots) {
				plot.YAxis2.Label(yAxisLabel);
			}

			return this;
		}

		/// <summary>
		///  Sets the rotation of the secondary Y axis ticks.
		/// </summary>
		/// <param name="container">Container for rotation of axis</param>
		/// <returns>Fluent builder</returns>
		public IPlotBuilderFluent_Configuration RotateSecondaryYAxisTicks(PlotAxisRotationContainer container) {
			foreach (var plot in _plots) {
				plot.YAxis2.TickLabelStyle(rotation: container.Rotation);
			}

			return this;
		}

		/// <summary>
		///  Sets the rotation of the secondary X axis ticks.
		/// </summary>
		/// <param name="container">Container for rotation of axis</param>
		/// <returns>Fluent builder</returns>
		public IPlotBuilderFluent_Configuration RotateSecondaryXAxisTicks(PlotAxisRotationContainer container) {
			foreach (var plot in _plots) {
				plot.XAxis2.TickLabelStyle(rotation: container.Rotation);
			}

			return this;
		}

		/// <summary>
		///  Sets whether or not the legend should be shown and the alignment of the legend.
		/// </summary>
		/// <param name="alignment">Where on the plot to display the legend</param>
		/// <returns>Fluent builder</returns>
		public IPlotBuilderFluent_Configuration ShowLegend(Alignment alignment) {
			foreach (var plot in _plots) {
				plot.Legend(true, alignment);
			}

			return this;
		}

		/// <summary>
		///  Hides the legend.
		/// </summary>
		/// <returns>Fluent builder</returns>
		public IPlotBuilderFluent_Configuration HideLegend() {
			foreach (var plot in _plots) {
				plot.Legend(false);
			}

			return this;
		}

		/// <summary>
		///  Sets the margins of the plot. This affects the actual data area of the plot.
		/// </summary>
		/// <param name="percentX">Double value for x-axis</param>
		/// <param name="percentY">Double value for y-axis</param>
		/// <returns>Fluent builder</returns>
		public IPlotBuilderFluent_Configuration SetDataPadding(double percentX = 1.0, double percentY = 1.0) {
			foreach (var plot in _plots) {
				plot.Margins(percentX, percentY);
			}

			return this;
		}

		/// <summary>
		///  Finalizes the configuration of the plot. This should be the last call in the fluent API.
		///  This allows you to call Produce().
		/// </summary>
		/// <returns>Fluent interface allowing consumer to Produce()</returns>
		public IPlotBuilderFluent_ReadyToProduce FinalizeConfiguration() => this;

		/// <summary>
		///  Attempts to save the plot to the specified path. This will throw an <see cref="Exception"/> if the save fails.
		/// </summary>
		/// <param name="savePath"></param>
		/// <param name="name">Name to give the graph. If there are multiple graphs, an integer will be appended.</param>
		/// <returns></returns>
		/// <exception cref="Exception">Thrown if savePath is null or whitespace</exception>
		public bool TrySave(string savePath, string name) {
			if (string.IsNullOrWhiteSpace(savePath) || string.IsNullOrWhiteSpace(name))
				throw new Exception(Message.EXCEPTION_SAVE_PATH_INVALID);

			try {
				var plotTracker = 1;
				foreach (var plot in _plots) {
					plot.SaveFig($@"{savePath}\{name}{plotTracker}{Constants.PNG_EXTENSION}");
					plotTracker++;
				}

				return true;
			}
			catch (Exception) {
				return false;
			}
		}

		/// <summary>
		///  Helper method to return this instance of the fluent builder. 
		/// </summary>
		/// <returns></returns>
		public IPlotBuilderFluent_Product GoToProduct() => this;

		/// <summary>
		///  Adds an annotation to the plot as specified coordinates (typically fed by the mouse position).
		/// </summary>
		/// <param name="annotation">Text to annotate</param>
		/// <param name="plot">Plot the annotation goes on</param>
		/// <param name="xOff">x-offset ('x' mouse position)</param>
		/// <param name="yOff">y-offset ('y' mouse position)</param>
		public IPlotBuilderFluent_Tools WithAnnotationAt(string annotation, Plot plot, float xOff, float yOff) {
			var a =plot.AddAnnotation(annotation, Alignment.LowerLeft);
			a.Border  = true;
			a.BorderWidth = 1;
			a.MarginX = xOff;
			a.MarginY = yOff;

			return this;
		}

		/// <summary>
		///  Returns the generated Plot instance. This should be the last call in the fluent API.
		/// </summary>
		/// <returns>ScottPlot.Plot instance (private)</returns>
		public IPlotBuilderFluent_Product Produce() {
			_plotWasProduced = true;

			if (!Observables.Any())
				return this;

			foreach (var o in Observables)
				o.Invoke();

			return this;
		}

		/// <summary>
		///  Resets the builder to an initial state. This is useful if you want to reuse the builder with new data.
		/// </summary>
		/// <param name="data">New data to populate the builder with</param>
		/// <returns>Fluent builder in a reset state</returns>
		public IPlotBuilderFluent_Configuration Reset(IReadOnlyList<PlotChannel> data) {
			return StartNew(data);
		}

		/// <summary>
		///  Register an action to be invoked when the plot is produced.
		/// </summary>
		/// <param name="action">Callback</param>
		/// <returns>Fluent builder</returns>
		public IPlotBuilderFluent_Configuration WithObservable(Action action) {
			Observables.Add(action);
			return this;
		}

		/// <summary>
		///  Hashset containing the actions to be observed by the view model. This is invoked when Produce() is called.
		/// </summary>
		HashSet<Action> Observables { get; } = new();

		/// <summary>
		///  Helper method to set the initial state of the plot. This is called in the constructor.
		///  This method will divvy up the data into separate plots based on on the number of plots specified in the constructor.
		/// </summary>
		void SetInitialState(int numOfPlots) {
			foreach (var channel in _data) {
				var batchSize = channel.Records.Count / numOfPlots;
				var batch     = channel.Records.Batch(batchSize).ToArray();

				if (batch[^1] != null && batch.Length > numOfPlots) {
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

					ProcessChannelRecord(batchedRecord, ref plotTracker, channel);
				}
			}
		}

		/// <summary>
		///  Helper method for SetInitialState. This method will add the data to the plot.
		/// </summary>
		/// <param name="batchedRecord">Current batched record</param>
		/// <param name="plotTracker">Iteration tracker</param>
		/// <param name="channel">Current channel being enumerated</param>
		void ProcessChannelRecord(IEnumerable<PlotChannelRecord> batchedRecord, ref int plotTracker, PlotChannel
			channel) {
			var batchedArray = batchedRecord.ToArray();
			var dateTimes    = batchedArray.Select(x => x.DateTime.ToOADate()).ToArray();
			var values       = batchedArray.Select(v => v.Value).ToArray();

			_plots[plotTracker].AddScatterLines(dateTimes, values, label: channel.ChannelIdentifier);
			_plots[plotTracker].XAxis.DateTimeFormat(true);
			_plots[plotTracker].YAxis2.SetSizeLimit(min: 40);

			plotTracker++;
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

			SetInitialState(numOfPlots);
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

			SetInitialState(plotInitializer.Count);
		}

		bool                                _plotWasProduced;
		readonly List<Plot>                 _plots;
		readonly IReadOnlyList<PlotChannel> _data;
	}
}