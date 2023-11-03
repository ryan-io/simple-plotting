using System.Drawing;
using ScottPlot;
using ScottPlot.Plottable;

namespace simple_plotting.src {
	/// <summary>
	///  Base abstraction for PlottableFactory
	/// </summary>
	public interface IPlottableFactory {
		/// <summary>
		///  The type of plottable to graph
		/// </summary>
		Type? PlottableType { get; }
	}

	/// <summary>
	///  Fluent API for resetting the PlottableFactory for creating a plottable
	/// </summary>
	public interface IPlottableFactoryReset : IPlottableFactory {
		/// <summary>
		///  Resets the factory to an initial state
		/// </summary>
		/// <returns>Fluent factory as IPlottablePrime</returns>
		IPlottablePrime Reset();
	}

	/// <summary>
	///  Fluent API for prepping the PlottableFactory for creating a plottable
	/// </summary>
	public interface IPlottablePrime : IPlottableFactory {
		/// <summary>
		///  Populates the Plot property and constructorArgs in preparation for creating a plottable
		/// </summary>
		/// <param name="plot">Plot to add the graph to</param>
		/// <param name="constructorArgs">object[] with parameters matching your choice of plottable graph construction</param>
		/// <returns>Fluent factory</returns>
		IPlottableProduct PrimeProduct(Plot? plot, ref object[] constructorArgs);
	}

	/// <summary>
	///  Fluent API for adding plottable products to a graph
	/// </summary>
	public interface IPlottableProduct : IPlottableFactory {
		/// <summary>
		///  Use when you want to let the API determine what kind of plottable to graph when OfType is used
		///  This method will perform type-checking and determine what factory method from below to call
		/// </summary>
		/// <param name="action">Factory method delegate to invoke</param>
		/// <returns>Fluent factory</returns>
		IPlottableFactoryReset AddViaFactoryMethod(Action action);

		/// <summary>
		///  Adds a ScatterPlot to plot Plot.
		/// </summary>
		/// <param name="color">Color of the line</param>
		/// <param name="channelName">Legend label for this channel</param>
		/// <param name="data">POCO containing plot data</param>
		/// <returns>Product containing the scatter plot & factory reference</returns>
		ScatterPlotProduct? AddScatterPlot(Color color, string channelName, PlottableData data);

		/// <summary>
		///  Adds a SignalPlot to plot Plot.
		/// </summary>
		/// <param name="color">Color of the line</param>
		/// <param name="channelName">Legend label for this channel</param>
		/// <param name="data">POCO containing plot data</param>
		/// <returns>Product containing the signal plot & factory reference</returns>
		SignalPlotProduct? AddSignalPlot(Color color, string channelName, PlottableData data);
	}

	public class PlottableFactory : IPlottableFactoryReset, IPlottablePrime, IPlottableProduct {
		public Type? PlottableType { get; }

		Plot? Plot { get; set; }

		bool WasRun { get; set; }

		object[] ConstructorArgs { get; set; } = Array.Empty<object>();

		/// <summary>
		///  Entry point for the plottable factory.
		/// </summary>
		/// <returns>Fluent PlottableFactory as IPlottableFactory</returns>
		public static IPlottablePrime StartNew<T>() where T : class, IPlottable
			=> new PlottableFactory(typeof(T));

		/// <summary>
		///  Brings back to the beginning; allows chain calling PrimeProduct
		/// </summary>
		/// <returns>Fluent factory</returns>
		public IPlottablePrime Reset() {
			WasRun = false;
			return this;
		}

		/// <summary>
		///  Populates the Plot property and constructorArgs in preparation for creating a plottable
		/// </summary>
		/// <param name="plot">Plot to add the graph to</param>
		/// <param name="constructorArgs">object[] with parameters matching your choice of plottable graph construction</param>
		/// <returns>Fluent factory</returns>
		public IPlottableProduct PrimeProduct(Plot? plot, ref object[] constructorArgs) {
			Plot            = plot;
			ConstructorArgs = constructorArgs;
			return this;
		}

		/// <summary>
		///  Use when you want to let the API determine what kind of plottable to graph when OfType is used
		///  This method will perform type-checking and determine what factory method from below to call
		/// </summary>
		/// <param name="action">Factory method delegate to invoke</param>
		/// <returns>Fluent factory</returns>
		public IPlottableFactoryReset AddViaFactoryMethod(Action action) {
			action.Invoke();
			return this;
		}

		/// <summary>
		///  Adds a ScatterPlot to plot Plot.
		/// </summary>
		/// <param name="color">Color of the line</param>
		/// <param name="channelName">Legend label for this channel</param>
		/// <param name="data">POCO containing plot data</param>
		/// <returns>Product containing the scatter plot & factory reference</returns>
		public ScatterPlotProduct? AddScatterPlot(Color color, string channelName, PlottableData data) {
			if (WasRun)
				return new ScatterPlotProduct(this, default);

			var scatterPlot = AddPlotOfType<ScatterPlot>(ref data, new ScatterPlotCallback());

			if (scatterPlot == null)
				return default;

			scatterPlot.Color = color;
			scatterPlot.Label = channelName;

			Finish();
			return new ScatterPlotProduct(this, scatterPlot);
		}

		/// <summary>
		///  Adds a SignalPlot to plot Plot. This method (suffix 'XY') will adjust the sample rate b
		/// </summary>
		/// <param name="color">Color of the line</param>
		/// <param name="channelName">Legend label for this channel</param>
		/// <param name="data">POCO containing plot data</param>
		/// <returns>Product containing the signal plot & factory reference</returns>
		public SignalPlotProduct? AddSignalPlot(Color color, string channelName, PlottableData data) {
			if (WasRun)
				return new SignalPlotProduct(this, default);

			if (Plot == null)
				throw new Exception(Message.EXCEPTION_PLOT_IS_NULL);

			// var signalPlot = AddPlotOfType<SignalPlot>(ref data, new SignalPlotCallback());

			if (!data.SampleRate.HasValue)
				throw new Exception(Message.EXCEPTION_CREATE_SIGNAL_NO_SAMPLE_RATE);

			var signalPlot = Plot.AddSignalXYConst(data.X, data.Y);

			if (signalPlot == null)
				return default;

			signalPlot.Color      = color;
			signalPlot.Label      = channelName;
			signalPlot.Smooth     = true;
			signalPlot.MarkerSize = 0;

			Finish();
			return new SignalPlotProduct(this, signalPlot);
		}

		/// <summary>
		///  Sets WasRun to true. This is to prevent the factory from being run more than once.
		/// </summary>
		void Finish() => WasRun = true;

		/// <summary>
		///  Adds a generic plot of type T.
		///  object[] elements should match a constructor overload of type T for the plot you are trying to add.
		/// </summary>
		/// <param name="data">POCO containing plot data</param>
		/// <param name="creationCallback">Callback logic specific to the type of graph being added to Plot</param>
		/// <typeparam name="T">Class & implements IPlottable</typeparam>
		/// <exception cref="Exception">Thrown if plottable T cannot be created</exception>
		/// <exception cref="InvalidCastException">Thrown if the cast to IPlottable fails</exception>
		T? AddPlotOfType<T>(ref PlottableData data, IPlotCallback? creationCallback = null)
			where T : class, IPlottable {
			if (Plot == null)
				return default;

			var plottable = Activator.CreateInstance(typeof(T), ConstructorArgs);

			if (plottable == null)
				throw new Exception(Message.EXCEPTION_CANNOT_CREATE_GENERIC_PLOTTABLE);

			var castedPlottable = (T)plottable;

			if (castedPlottable == null)
				throw new InvalidCastException(Message.EXCEPTION_CANNOT_CREATE_GENERIC_PLOTTABLE);

			creationCallback?.Callback(castedPlottable, ref data);

			Plot.Add(castedPlottable);
			Plot.Render();

			return castedPlottable;
		}

		/// <summary>
		///  Base constructor. Sets PlottableType
		/// </summary>
		/// <param name="plotType"></param>
		PlottableFactory(Type plotType) {
			PlottableType = plotType;
		}
	}
}