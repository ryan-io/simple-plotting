using System.Drawing;
using ScottPlot;
using simple_plotting.runtime;

namespace simple_plotting;

/// <summary>
/// Represents a fluent builder for creating and manipulating instances of the <see cref="Plot"/> class.
/// </summary>
public partial class PlotBuilderFluent : IPlotBuilderFluentCanvas {
	/// <summary>
	/// Gets or sets the BitmapParser instance.
	/// </summary>
	/// <value>
	/// The BitmapParser.
	/// </value>
	BitmapParser? BitmapParser { get; set; }

	/// <inheritdoc />
	public IPlotBuilderFluentCanvas AddText(string text, int plotIndex, double xPosition, double yPosition) {
		if (_plots == null || plotIndex >= _plots.Length)
			throw new IndexOutOfRangeException(Message.EXCEPTION_INDEX_OUT_OF_RANGE);
		
		_plots[plotIndex].AddText(text, xPosition, yPosition);
		return this;
	}

	/// <inheritdoc />
	public IPlotBuilderFluentCanvas AddImage(Bitmap img, int plotIndex, double xPosition, double yPosition) {
		if (_plots == null || plotIndex >= _plots.Length)
			throw new IndexOutOfRangeException(Message.EXCEPTION_INDEX_OUT_OF_RANGE);
		
		_plots[plotIndex].AddImage(img, xPosition, yPosition);
		
		return this;
	}
	
	/// <inheritdoc />
	public IPlotBuilderFluentCanvas SetBitmapParser(BitmapParser bitmapParser) {
		BitmapParser = bitmapParser;
		return this;
	}

	/// <inheritdoc cref="IPlotBuilderFluentCanvasConfiguration"/>
	public IPlotBuilderFluentCanvasReadyToProduce FinalizeCanvasConfiguration() => this;
}