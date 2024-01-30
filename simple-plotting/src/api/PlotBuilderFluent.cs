using System.Collections.Concurrent;
using System.Drawing;
using System.Drawing.Imaging;
using ScottPlot;
using ScottPlot.Plottable;

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
	                                         IPlotBuilderFluentProduct,
	                                         IPlotBuilderFluentCanvasReadyToProduce,
	                                         IPlotBuilderFluentCanvasProduct {
		/// <inheritdoc />
		public bool CanSave => _data != null && _data.Any() && _plotWasProduced;

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
		ConcurrentBag<string> CachedPlotPaths { get; set; } = new();

		/// <summary>
		///  Factory for adding graphs to plots
		/// </summary>
		IPlottablePrime? FactoryPrime { get; set; }

		/// <summary>
		///  Hashset containing the actions to be observed by the view model. This is invoked when Produce() is called.
		/// </summary>
		HashSet<Action> Observables { get; } = new();

		/// <inheritdoc cref="IPlotBuilderFluentProduct.SourcePath" />
		public string? SourcePath { get; private set; } = string.Empty;

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
		/// Starts a new canvas for plotting with the given image paths. The method instantiates a new PlotBuilderFluent object and returns it.
		/// </summary>
		/// <param name="imagePaths">The paths of images to load into the canvas. This parameter is variadic, so you can input multiple string values without wrapping them into an array.</param>
		/// <param name="lockAxis">If true, cannot pan canvas</param>
		/// <returns>An instance of IPlotBuilderFluentCanvas which provides a fluent interface for plotting.</returns>
		/// <example>
		/// Here is an example of how to use the StartNewCanvas method.
		/// <code>
		/// var plotBuilderFluentCanvas = StartNewCanvas("path_to_image1", "path_to_image2");
		/// </code>
		/// </example>
		public static IPlotBuilderFluentCanvas StartNewCanvas(ref Bitmap[] imagePaths, bool lockAxis = false)
			=> new PlotBuilderFluent(ref imagePaths, lockAxis);

		/// <summary>
		/// Initializes a new PlotBuilderFluent canvas object with the provided Bitmap images and a collection of initial Plots.
		/// </summary>
		/// <param name="images">
		/// An array of Bitmap images to be manipulated or plotted against. This parameter is passed by reference.
		/// </param>
		/// <param name="plotInitializer">
		/// A collection of Plot objects that serve as the basis for the new PlotBuilderFluent object. This collection could contain initial configurations for plot designs. This parameter is passed by reference.
		/// </param>
		/// <param name="lockAxis">If true, cannot pan canvas</param>
		/// <returns>
		/// A new instance of the PlotBuilderFluent class, initialized with the provided Bitmap images and initial Plots collection.
		/// </returns>
		public static IPlotBuilderFluentCanvas StartNewCanvas(
			ref Bitmap[] images,
			ref HashSet<Plot> plotInitializer,
			bool lockAxis = false)
			=> new PlotBuilderFluent(ref images, ref plotInitializer, lockAxis);

		/// <summary>
		/// Starts a new canvas for plotting.
		/// </summary>
		/// <param name="plotInitializer">A reference to a <see cref="HashSet{Plot}"/> object for storing initialized plots.</param>
		/// <param name="lockAxis">A boolean value indicating whether to lock the axis. Default is false.</param>
		/// <param name="images">An array of <see cref="Bitmap"/> objects to be used as background images.</param>
		/// <returns>A new instance of <see cref="IPlotBuilderFluentCanvas"/>.</returns>
		public static IPlotBuilderFluentCanvas StartNewCanvas(
			ref HashSet<Plot> plotInitializer,
			bool lockAxis = false,
			params Bitmap[] images)
			=> new PlotBuilderFluent(ref images, ref plotInitializer, lockAxis);

		///	<inheritdoc />
		public Type? PlotType { get; private set; }

		/// <summary>
		/// Disposes resources used by the object.
		/// </summary>
		public void Dispose() {
			ValidateCancellationTokenSource();

			foreach (var bmp in _imageMap.Values)
				bmp.Bitmap.Dispose();

			BitmapParser?.Dispose();
		}

		/// <inheritdoc cref="IPlotBuilderFluentProduct.CancelAllOperations" />
		public void CancelAllOperations() {
			CancellationTokenSource.Cancel();
			CancellationTokenSource.Dispose();
		}

		/// <summary>
		/// Saves the plots to the specified directory with the given name.
		/// </summary>
		/// <param name="name">The name of the plots.</param>
		/// <param name="format">Extension flag to save plots as.</param>
		/// <param name="token">The cancellation token (optional).</param>
		/// <returns>A task that represents the asynchronous saving of plots.</returns>
		/// <exception cref="InvalidOperationException">Thrown when _plots is null.</exception>
		async Task InternalSavePlots(string name, ImageFormat format, CancellationToken? token) {
			if (_plots == null)
				throw new InvalidOperationException(Message.EXCEPTION_INTERNAL_PLOT_COL_NULL);

			if (token == null) {
				ValidateCancellationTokenSource(true);

				token = CancellationTokenSource.Token;
			}

			CachedPlotPaths.Clear();

			await Task.Run(() => {
				               var plotTracker = new IntSafe(1);

				               foreach (var plot in _plots) {
					               var expectedPath = InternalGetSavePath(name, format.ToString(), plotTracker);
					               var actualPath   = plot.SaveFig(expectedPath);

					               if (!string.IsNullOrWhiteSpace(actualPath))
						               CachedPlotPaths.Add(actualPath);

					               plotTracker++;
				               }
			               }, token.Value);
		}

		/// <summary>
		/// Constructs a save path for a given name and plot tracker.
		/// </summary>
		/// <param name="name">The name of the file.</param>
		/// <param name="format">Extension to save each image as.</param>
		/// <param name="plotTracker">The plot tracker.</param>
		/// <returns>The constructed save path.</returns>
		/// <remarks>
		/// The save path is constructed by appending the file name and plot tracker to the source path,
		/// and then checking if a file with the same name already exists. If it does, a unique identifier
		/// is appended to the save path to avoid overwriting existing files. Finally, the ".png" format
		/// is added to the save path.
		/// </remarks>
		string InternalGetSavePath(string name, string format, IntSafe plotTracker) {
			var targetPath = $@"{SourcePath}\{name}_{plotTracker}";

			if (File.Exists($"{targetPath}.{format}")) {
				var guid = Guid.NewGuid();
				targetPath += $"_{guid.ToString()}";
			}

			targetPath += $".{format}";
			return targetPath;
		}

		/// <summary>
		///  Helper method to set the initial state of the plot. This is called in the constructor.
		///  This method will divvy up the data into separate plots based on on the number of plots specified in the constructor.
		/// </summary>
		void SetInitialState(Action<IEnumerable<PlotChannelRecord>, int, PlotChannel> actionDelegate) {
			// ReSharper disable once ForCanBeConvertedToForeach
			if (_data == null)
				throw new ArgumentException(Message.EXCEPTION_DATA_NULL_EMPTY);

			for (var index = 0; index < _data.Count; index++) {
				var channel     = _data[index];
				var batch       = channel.Records.Batch(PlotCount).ToArray();
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
			if (_plots == null)
				throw new ArgumentException(Message.EXCEPTION_PLOT_IS_NULL);

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
			_plots = new Plot[numOfPlots];

			for (var i = 0; i < numOfPlots; i++) {
				_plots[i] = new Plot(Constants.DEFAULT_WIDTH, Constants.DEFAULT_HEIGHT);
			}

			PlotCount = numOfPlots;
			//_type     = InternalType.PLOT;
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
				throw new ArgumentException(Message.EXCEPTION_DATA_NULL_EMPTY);

			_data  = data;
			_plots = new Plot[plotInitializer.Count];

			var i = 0;
			foreach (var p in plotInitializer) {
				_plots[i]        = p;
				_plots[i].Width  = Constants.DEFAULT_WIDTH;
				_plots[i].Height = Constants.DEFAULT_HEIGHT;
				i++;
			}

			PlotCount = plotInitializer.Count;
			//_type     = InternalType.PLOT;
		}

		/// <summary>
		/// Instantiates a new instance of `PlotBuilderFluent` for an array of Bitmap objects by creating
		/// plot for each item.
		/// </summary>
		/// <param name="images">An array of Bitmap images to be assigned to the plots. Cannot be null or empty.</param>
		/// <param name="lockAxis">If true, cannot pan canvas</param>
		/// <exception cref="ArgumentException">Thrown when the images array is null or empty.</exception>
		/// <remarks>
		/// This constructor initializes a plot for each Bitmap in the "images" array.
		/// It sets the width and height of each plot to match the associated Bitmap.
		/// The frames of all the plots are set to Frameless.
		/// </remarks>
		PlotBuilderFluent(ref Bitmap[] images, bool lockAxis = false) {
			if (images.IsNullOrEmpty())
				throw new ArgumentException(Message.EXCEPTION_NO_IMG_PATHS);

			_plots = new Plot[images.Length];

			for (var i = 0; i < images.Length; i++) {
				var img = images[i];

				_plots[i] = new Plot(img.Width, img.Height);
				AddImgToCanvas(lockAxis, i, img);

				i++;
			}

			//_type = InternalType.CANVAS;
		}

		/// <summary>
		/// Constructor for the PlotBuilderFluent class which initializes internal plots with given images and plot initializers.
		/// </summary>
		/// <param name="images">A reference to an array of Bitmap objects.</param>
		/// <param name="plotInitializer">A reference to a collection of Plot objects serving as initializers.</param>
		/// <param name="lockAxis">If true, cannot pan canvas</param>
		/// <exception cref="ArgumentException">Thrown when images array is null or empty.</exception>
		/// <exception cref="ArgumentException">Thrown when plotInitializer is null or empty.</exception>
		/// <exception cref="Exception">Thrown when the sizes of images array and plotInitializer collection are not equal.</exception>
		PlotBuilderFluent(ref Bitmap[] images, ref HashSet<Plot> plotInitializer, bool lockAxis = false) {
			if (images.IsNullOrEmpty())
				throw new ArgumentException(Message.EXCEPTION_NO_IMG_PATHS);

			if (plotInitializer == null || !plotInitializer.Any())
				throw new ArgumentException(Message.EXCEPTION_DATA_NULL_EMPTY);

			if (images.Length != plotInitializer.Count)
				throw new Exception(Message.EXCEPTION_DISSIMILAR_COLLECTION_SIZE);

			_plots = new Plot[plotInitializer.Count];

			var i = 0;

			foreach (var p in plotInitializer) {
				var img = images[i];

				_plots[i] = p;
				AddImgToCanvas(lockAxis, i, img);

				i++;
			}

			PlotCount = plotInitializer.Count;
			//_type     = InternalType.CANVAS;
		}

		void AddImgToCanvas(bool lockAxis, int i, Bitmap img) {
			if (_plots == null || !_plots.Any())
				throw new Exception(Message.EXCEPTION_PLOT_IS_NULL);

			_plots[i].Frameless();
			var plotImg = _plots[i].AddImage(img, 0, 0, anchor: Alignment.MiddleCenter);
			_plots[i].Width  = img.Width;
			_plots[i].Height = img.Height;
			_plots[i].Frameless();
			_plots[i].Grid(false);
			_plots[i].AxisAuto();

			if (lockAxis) {
				_plots[i].XAxis.LockLimits();
				_plots[i].YAxis.LockLimits();
			}

			_imageMap[i] = plotImg;
		}

		HashSet<SignalPlotXYConst<double, double>> _cachedSignalPlottables = new();
		bool                                       _plotWasProduced;

		readonly Plot[]                                _plots;
		readonly IReadOnlyList<PlotChannel>?           _data;
		readonly HashSet<IPlottable>                   _returnPlottables = new();
	}

	// internal static class InternalType {
	// 	public const byte PLOT   = 1 << 0;
	// 	public const byte CANVAS = 1 << 1;
	// }
}