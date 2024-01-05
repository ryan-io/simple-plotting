using System.Collections.Concurrent;
using System.Drawing;
using ScottPlot;
using Image = ScottPlot.Plottable.Image;

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
	public IPlotBuilderFluentCanvas AddText(string text, int plotIndex, double xPosition, double yPosition, Color? 
            color = null, float fontSize = 12f) {
		if (_plots == null || plotIndex >= _plots.Length)
			throw new IndexOutOfRangeException(Message.EXCEPTION_INDEX_OUT_OF_RANGE);
		
		_plots[plotIndex].AddText(text, xPosition, yPosition, color: color, size: fontSize);
		return this;
	}

	/// <inheritdoc />
	public IPlotBuilderFluentCanvas AddImage(Bitmap img, int plotIndex, double xPosition, double yPosition) {
		if (_plots == null || plotIndex >= _plots.Length)
			throw new IndexOutOfRangeException(Message.EXCEPTION_INDEX_OUT_OF_RANGE);
		
		var plotImg = _plots[plotIndex].AddImage(img, xPosition, yPosition);
		_imageMap[plotIndex] = plotImg;
		
		return this;
	}
	
	/// <inheritdoc />
	public IPlotBuilderFluentCanvas SetBitmapParser(BitmapParser bitmapParser) {
		BitmapParser = bitmapParser;
		return this;
	}

	/// <inheritdoc cref="IPlotBuilderFluentCanvasConfiguration"/>
	public IPlotBuilderFluentCanvasReadyToProduce FinalizeCanvasConfiguration() => this;

	/// <inheritdoc />
	public IPlotBuilderFluentCanvas SetSourcePath(string path) {
		if (string.IsNullOrWhiteSpace(path))
			throw new NullReferenceException(Message.EXCEPTION_INVALID_SOURCE);

		SourcePath = path;
		return this;
	}
	
	readonly ConcurrentDictionary<int, Image> _imageMap = new();
}