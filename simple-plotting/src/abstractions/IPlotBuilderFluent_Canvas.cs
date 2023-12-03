using System.Drawing;
using ScottPlot;

namespace simple_plotting {
	/// <summary>
	/// Represents a fluent builder for creating and manipulating instances of the <see cref="Plot"/> class.
	/// </summary>
	public interface IPlotBuilderFluentCanvas {
		/// <summary>
		/// Adds text to the plot at the specified x and y coordinates.
		/// </summary>
		/// <param name="text">The text to add.</param>
		/// <param name="plotIndex">Index of plot to add text to</param>
		/// <param name="xPosition">The x-coordinate at which to place the text.</param>
		/// <param name="yPosition">The y-coordinate at which to place the text.</param>
		/// <returns>Returns the current <see cref="IPlotBuilderFluentCanvas"/> instance to allow for method chaining.</returns>
		IPlotBuilderFluentCanvas         AddText(string text, int plotIndex, double xPosition, double yPosition);
		
		/// <summary>
		/// Adds an image to the plot at the specified x and y coordinates.
		/// </summary>
		/// <param name="img">The image to add.</param>
		/// <param name="plotIndex">Index of plot to add text to</param>
		/// <param name="xPosition">The x-coordinate at which to place the image.</param>
		/// <param name="yPosition">The y-coordinate at which to place the image.</param>
		/// <returns>Returns the current <see cref="IPlotBuilderFluentCanvas"/> instance to allow for method chaining.</returns>
		IPlotBuilderFluentCanvas         AddImage(Bitmap img, int plotIndex, double xPosition, double yPosition);
		IPlotBuilderFluentReadyToProduce FinalizeConfiguration();
	}
}