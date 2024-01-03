// simple-plotting

using System.Drawing.Drawing2D;

namespace simple_plotting;

/// <summary>
/// Represents the criteria used to resize a bitmap image.
/// </summary>
public readonly struct BitmapResizeCriteria {
	/// <summary>
	/// Gets the compositing mode for this property.
	/// </summary>
	/// <value>
	/// The compositing mode for this property.
	/// </value>
	public CompositingMode    CompositingMode    { get; }

	/// <summary>
	/// Gets or sets the rendering quality during compositing.
	/// </summary>
	/// <value>
	/// A CompositingQuality object that represents the rendering quality during compositing.
	/// </value>
	public CompositingQuality CompositingQuality { get; }

	/// <summary>
	/// Gets the interpolation mode used for scaling images.
	/// </summary>
	/// <remarks>
	/// The interpolation mode determines how the pixels are calculated when scaling an image.
	/// </remarks>
	/// <returns>
	/// The InterpolationMode used for scaling images.
	/// </returns>
	public InterpolationMode  InterpolationMode  { get; }

	/// <summary>
	/// Gets the smoothing mode of the graphics object.
	/// </summary>
	/// <value>
	/// The smoothing mode of the graphics object.
	/// </value>
	public SmoothingMode      SmoothingMode      { get; }

	/// <summary>
	/// Gets the pixel offset mode used for drawing.
	/// </summary>
	/// <value>
	/// The pixel offset mode used for drawing.
	/// </value>
	public PixelOffsetMode    PixelOffsetMode    { get; }

	/// <summary>
	/// Represents the criteria for resizing a Bitmap.
	/// </summary>
	public BitmapResizeCriteria(CompositingMode compositingMode, CompositingQuality compositingQuality, InterpolationMode interpolationMode, SmoothingMode smoothingMode, PixelOffsetMode pixelOffsetMode) {
		CompositingMode    = compositingMode;
		CompositingQuality = compositingQuality;
		InterpolationMode  = interpolationMode;
		SmoothingMode      = smoothingMode;
		PixelOffsetMode    = pixelOffsetMode;
	}
}