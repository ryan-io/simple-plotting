// simple-plotting

using ScottPlot;
using ScottPlot.Plottable;

namespace simple_plotting.src;

public partial class PlotBuilderFluent {
	/// <summary>
	///  The generated plots. Can call { get; } after Produce() has been invoked and will return as an enumerable.
	/// </summary>
	public IEnumerable<Plot> GetPlots() => _plots;

	/// <summary>
	///  The generated plots. Queries the plot collection and returns the plot at the specified index.
	/// </summary>
	public Plot GetPlot(int plotIndex) {
		plotIndex.ValidateInRange(_plots);
		return _plots[plotIndex];
	}

	/// <summary>
    ///  Extracts actual channel names from the plots. 
    /// </summary>
    /// <returns>Enumerable of strings containing names extracted from plots</returns>
    public IEnumerable<string> GetScatterPlotLabels(int plotIndex) {
		List<string> output = new();
		var plottables = GetPlottablesAs<ScatterPlot>(plotIndex);
		
		foreach (var plottable in plottables) {
			output.Add(plottable.Label);			
		}

		return output;
	}

    /// <summary>
    ///  Helper method that returns an enumerable of type T that implements IPlottable.
    ///  This method invokes OfType with the generic type T.
    ///  It requires an index to the plot to extract the plottables from.
    /// </summary>
    /// <param name="plotIndex">Index of plot to get</param>
    /// <typeparam name="T">Class that implements IPlottable</typeparam>
    /// <returns>Enumerable containing the plottables as type T</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown if index > plot count</exception>
    public IEnumerable<T> GetPlottablesAs<T> (int plotIndex) where T : class, IPlottable {
        if (plotIndex > _plots.Count)
            throw new IndexOutOfRangeException(Message.EXCEPTION_INDEX_OUT_OF_RANGE);

        var plottables = new List<T>();
        var plot = _plots[plotIndex];

        plottables.AddRange(plot.GetPlottables().OfType<T>().ToList());

        return plottables;
    }

    /// <summary>
    ///  Attempts to save the plot to the specified path. This will throw an <see cref="Exception"/> if the save fails.
    /// </summary>
    /// <param name="savePath">Directory to save plots</param>
    /// <param name="name">Name of each plot</param>
    /// <returns>True if could write (save) to directory, otherwise false</returns>
    /// <exception cref="Exception">Thrown if savePath is null or whitespace</exception>
    public bool TrySave(string savePath, string name) {
		if (string.IsNullOrWhiteSpace(savePath) || string.IsNullOrWhiteSpace(name))
			throw new Exception(Message.EXCEPTION_SAVE_PATH_INVALID);

		try {
			var plotTracker = 1;
			foreach (var plot in _plots) {
				plot.SaveFig($@"{savePath}\{name}_{plotTracker}{Constants.PNG_EXTENSION}");
				plotTracker++;
			}

			return true;
		}
		catch (Exception) {
			return false;
		}
	}

	/// <summary>
	///  Attempts to save the plot to the defined source path. This will throw an <see cref="Exception"/> if the save fails.
	///  This method requires you to call DefineSource.
	/// </summary>
	/// <param name="name">Name of each plot</param>
	/// <returns>True if could write (save) to directory, otherwise false</returns>
	/// <exception cref="Exception">Thrown if savePath is null or whitespace</exception>
	public bool TrySaveAtSource(string name) {
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

	/// <summary>
	///  Exposes post processing API.
	/// </summary>
	/// <returns>Fluent builder as IPlotBuilderFluent_PostProcess</returns>
	public IPlotBuilderFluentPostProcess GotoPostProcess() => this;

    /// <summary>
    ///  Exposes plottables API
    /// </summary>
    /// <returns>Fluent builder as  IPlotBuilderFluent_Plottables</returns>
    public IPlotBuilderFluentPlottables GotoPlottables () => this;

    /// <summary>
    ///  Exposes configuration API.
    /// </summary>
    /// <returns>Fluent builder as IPlotBuilderFluent_Configuration</returns>
    public IPlotBuilderFluentConfiguration GotoConfiguration() => this;

	/// <summary>
	///  Resets the builder to an initial state. This is useful if you want to reuse the builder with new data.
	/// </summary>
	/// <param name="data">New data to populate the builder with</param>
	/// <returns>Fluent builder in a reset state</returns>
	public IPlotBuilderFluentOfType Reset(IReadOnlyList<PlotChannel> data) {
		return StartNew(data);
	}
}