// simple-plotting

using ScottPlot.Plottable;

namespace simple_plotting.src;

public interface IPlotBuilderFluentOfType : IPlotBuilderFluent {
    /// <summary>
    ///  Creates a new instance of PlotBuilderFluent and PlottableFactory using type defined in 'T'
    /// </summary>
    /// <typeparam name="T">Is a class that implements IPlottable (SignalPlot, ScatterPlot, etc.)</typeparam>
    /// <returns>Fluent builder</returns>
    IPlotBuilderFluentConfiguration OfType<T> () where T : class, IPlottable;
}
