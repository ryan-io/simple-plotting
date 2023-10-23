﻿using System.Drawing;
using ScottPlot;
using ScottPlot.Plottable;

namespace simple_plotting.src {
	/// <summary>
	///  Fluent API for resetting the PlottableFactory for creating a plottable
	/// </summary>
	public interface IPlottableFactoryReset {
		/// <summary>
		///  Resets the factory to an initial state
		/// </summary>
		/// <returns>Fluent factory as IPlottablePrime</returns>
		IPlottablePrime Reset();
	}

	/// <summary>
	///  Fluent API for prepping the PlottableFactory for creating a plottable
	/// </summary>
	public interface IPlottablePrime {
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
	public interface IPlottableProduct {
		/// <summary>
		///  Adds a ScatterPlot to plot Plot.
		/// </summary>
		/// <param name="color">Color of the line</param>
		/// <param name="channelName">Legend label for this channel</param>
		/// <returns>Product containing the scatter plot & factory reference</returns>
		ScatterPlotProduct? AddScatterPlot(Color color, string channelName);
		
		/// <summary>
		///  Adds a SignalPlot to plot Plot.
		/// </summary>
		/// <param name="color">Color of the line</param>
		/// <param name="channelName">Legend label for this channel</param>
		/// <returns>Product containing the signal plot & factory reference</returns>
		SignalPlotProduct?  AddSignalPlot(Color color, string channelName);
	}

	public class PlottableFactory : IPlottableFactoryReset, IPlottablePrime, IPlottableProduct {
		Plot? Plot { get; set; }

		bool WasRun { get; set; }

		object[] ConstructorArgs { get; set; } = Array.Empty<object>();

		/// <summary>
		///  Entry point for the plottable factory.
		/// </summary>
		/// <returns>Fluent PlottableFactory as IPlottableFactory</returns>
		public static IPlottablePrime StartNew() => new PlottableFactory();

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
		///  Adds a ScatterPlot to plot Plot.
		/// </summary>
		/// <param name="color">Color of the line</param>
		/// <param name="channelName">Legend label for this channel</param>
		/// <returns>Product containing the scatter plot & factory reference</returns>
		public ScatterPlotProduct? AddScatterPlot(Color color, string channelName) {
			if (WasRun)
				return new  ScatterPlotProduct(this, default);
			
			var scatterPlot = AddPlotOfType<ScatterPlot>(new ScatterPlotCallback());

			if (scatterPlot == null)
				return default;

			scatterPlot.Color = color;
			scatterPlot.Label = channelName;

			Finish();
			return new ScatterPlotProduct(this, scatterPlot);
		}

		/// <summary>
		///  Adds a SignalPlot to plot Plot.
		/// </summary>
		/// <param name="color">Color of the line</param>
		/// <param name="channelName">Legend label for this channel</param>
		/// <returns>Product containing the signal plot & factory reference</returns>
		public SignalPlotProduct? AddSignalPlot(Color color, string channelName) {
			if (WasRun)
				return new SignalPlotProduct(this, default);
				
			var signalPlot = AddPlotOfType<SignalPlot>(new SignalPlotCallback());

			if (signalPlot == null)
				return default;
			
			signalPlot.Color = color;
			signalPlot.Label = channelName;

			Finish();
			return new SignalPlotProduct(this, signalPlot);
		}

		/// <summary>
		///  Sets WasRun to true. This is to prevent the factory from being run more than once.
		/// </summary>
		void Finish() {
			WasRun = true;
		}

		/// <summary>
		///  Adds a generic plot of type T.
		///  object[] elements should match a constructor overload of type T for the plot you are trying to add.
		/// </summary>
		/// <param name="creationCallback">Callback logic specific to the type of graph being added to Plot</param>
		/// <typeparam name="T">Class & implements IPlottable</typeparam>
		/// <exception cref="Exception">Thrown if plottable T cannot be created</exception>
		/// <exception cref="InvalidCastException">Thrown if the cast to IPlottable fails</exception>
		public T? AddPlotOfType<T>(IPlotCallback? creationCallback = null)
			where T : class, IPlottable {
			if (ConstructorArgs.Length == 0 || Plot == null)
				return default;

			var plottable = Activator.CreateInstance(typeof(T), ConstructorArgs);

			if (plottable == null)
				throw new Exception(Message.EXCEPTION_CANNOT_CREATE_GENERIC_PLOTTABLE);

			var castedPlottable = (T)plottable;

			if (castedPlottable == null)
				throw new InvalidCastException(Message.EXCEPTION_CANNOT_CREATE_GENERIC_PLOTTABLE);

			creationCallback?.Callback(castedPlottable);
			Plot.Add(castedPlottable);
			Plot.Render();

			return castedPlottable;
		}
	}
}