// simple-plotting

namespace simple_plotting;

/// <summary>
/// Represents a ready-to-produce canvas for building plots using a fluent interface.
/// </summary>
public interface IPlotBuilderFluentCanvasReadyToProduce : IPlotBuilderFluent {
	/// <summary>
	/// Finalizes the configuration of the canvas.
	/// </summary>
	/// <returns>An instance of the <see cref="IPlotBuilderFluentCanvasReadyToProduce"/> interface.</returns>
	IPlotBuilderFluentCanvasProduct ProduceCanvas();
}