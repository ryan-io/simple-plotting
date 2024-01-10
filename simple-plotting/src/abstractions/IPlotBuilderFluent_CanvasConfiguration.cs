// simple-plotting

using System.Drawing;

namespace simple_plotting;

/// <summary>
/// Represents the configuration for a fluent canvas plot builder.
/// </summary>
public interface IPlotBuilderFluentCanvasConfiguration {
	/// <summary>
	/// Sets the source path for the object.
	/// </summary>
	/// <param name="path">A string representing the source path.</param>
	/// <exception cref="NullReferenceException">Thrown if the path is null, empty, or consists only of whitespace.</exception>
	IPlotBuilderFluentCanvasConfiguration SetSourcePath(string path);
	
	/// <summary>
	/// Adds text to the plot at the specified x and y coordinates.
	/// </summary>
	/// <param name="text">The text to add.</param>
	/// <param name="plotIndex">Index of plot to add text to</param>
	/// <param name="xPosition">The x-coordinate at which to place the text.</param>
	/// <param name="yPosition">The y-coordinate at which to place the text.</param>
	/// <param name="color">Color of the annotation</param>
	/// <param name="fontSize">Size of the annotation text</param>
	/// <returns>Returns the current <see cref="IPlotBuilderFluentCanvas"/> instance to allow for method chaining.</returns>
	IPlotBuilderFluentCanvasConfiguration AddText(string text, int plotIndex, double xPosition, double yPosition,
		Color? color = null, float fontSize = 12f);

	/// <summary>
	/// Adds an image to the plot at the specified x and y coordinates.
	/// </summary>
	/// <param name="img">The image to add.</param>
	/// <param name="plotIndex">Index of plot to add text to</param>
	/// <param name="xPosition">The x-coordinate at which to place the image.</param>
	/// <param name="yPosition">The y-coordinate at which to place the image.</param>
	/// <returns>Returns the current <see cref="IPlotBuilderFluentCanvas"/> instance to allow for method chaining.</returns>
	IPlotBuilderFluentCanvasConfiguration AddImage(Bitmap img, int plotIndex, double xPosition, double yPosition);

	/// <summary>
	/// Sets the BitmapParser used by the PlotBuilderFluentCanvas.
	/// </summary>
	/// <param name="bitmapParser">The BitmapParser to be set.</param>
	/// <returns>The PlotBuilderFluentCanvas with the specified BitmapParser set.</returns>
	IPlotBuilderFluentCanvasConfiguration SetBitmapParser(BitmapParser? bitmapParser);
	
	/// <summary>
	/// Finalizes the configuration of the canvas for plotting and returns the plot builder object.
	/// </summary>
	/// <returns>
	/// The plot builder object that is ready to produce the plot.
	/// </returns>
	IPlotBuilderFluentCanvasReadyToProduce FinalizeCanvasConfiguration();
}