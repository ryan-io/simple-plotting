using ScottPlot;

namespace simple_plotting {
	/// <summary>
	/// Represents a fluent builder for creating and manipulating instances of the <see cref="Plot"/> class.
	/// </summary>
	public interface IPlotBuilderFluentCanvas : IPlotBuilderFluentCanvasConfiguration {
		/// <summary>
		/// Sets the source path for the object.
		/// </summary>
		/// <param name="path">A string representing the source path.</param>
		/// <exception cref="NullReferenceException">Thrown if the path is null, empty, or consists only of whitespace.</exception>
		IPlotBuilderFluentCanvas SetSourcePath(string path);
	}
}