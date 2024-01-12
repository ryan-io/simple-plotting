using System.Drawing;

namespace simple_plotting;

/// <summary>
/// The ImageGrabber class retrieves and manages image files from a specified directory.
/// </summary>
public class ImageGrabber : IDisposable {
	/// <summary>
	/// Creates an instance of ImageGrabber asynchronously.
	/// </summary>
	/// <param name="flags">The ImageExtension flags.</param>
	/// <param name="rootDirectoryPath">The root directory path.</param>
	/// <param name="searchOption">The search option. Default is TopDirectoryOnly.</param>
	/// <returns>An asynchronous task that represents the operation. The task result contains the created ImageGrabber instance.</returns>
	public static async Task<ImageGrabber> CreateAsync(
		ImageGrabberFlags flags,
		string rootDirectoryPath,
		SearchOption searchOption = SearchOption.TopDirectoryOnly) {
		var grabber = new ImageGrabber(flags);
		var files   = grabber.PrimeFiles(rootDirectoryPath, searchOption);

		await Task.Run((Action)(() => grabber.Parser = new BitmapParser(ref files)));

		return grabber;
	}

	/// <summary>
	/// Gets or sets the BitmapParser for the property.
	/// </summary>
	/// <value>
	/// The BitmapParser for the property
	/// </value>
	public BitmapParser? Parser { get; private set; }

	/// <summary>
	/// Retrieves all bitmaps from the assigned image parser.
	/// </summary>
	/// <returns>An array of bitmaps.</returns>
	/// <exception cref="NoImageParserAssignedException">Thrown when no image parser is assigned to the class.</exception>
	/// <exception cref="ImageGrabberNotPrimedException">Thrown when the image grabber is not yet primed.</exception>
	public ref Bitmap[] GetAllBitmaps() {
		if (Parser == null)
			throw new NoImageParserAssignedException();

		if (!IsPrimed)
			throw new ImageGrabberNotPrimedException();

		return ref Parser.GetAllBitmaps();
	}

	/// <summary>
	/// Returns the extensions associated with the type.
	/// </summary>
	/// <returns>An array of strings representing the extensions associated with the type.</returns>
	public ref readonly string[] GetTypeExtensions() => ref _extensions;

	/// <summary>
	/// Performs application-defined tasks associated with freeing, releasing, or resetting resources.
	/// </summary>
	public void Dispose() => Parser?.Dispose();

	/// <summary>
	/// Retrieves an array of prime files from the specified root directory.
	/// </summary>
	/// <param name="rootDirectoryPath">The root directory path.</param>
	/// <param name="searchOption">Specifies whether to search only in the top directory or in all subdirectories as well (default is TopDirectoryOnly).</param>
	/// <returns>An array of prime files found in the root directory.</returns>
	string[] PrimeFiles(string rootDirectoryPath, SearchOption searchOption = SearchOption.TopDirectoryOnly) {
		if (string.IsNullOrWhiteSpace(rootDirectoryPath))
			throw new ArgumentNullException(nameof(rootDirectoryPath));

		var enumeratedFiles = Directory.EnumerateFiles(
			rootDirectoryPath, "*.*", searchOption).ToArray();
		
		IsPrimed = true;

		return enumeratedFiles;
	}

	/// <summary>
	/// Gets or sets a value indicating whether the number is primed.
	/// </summary>
	/// <value>true if the number is primed; otherwise, false.</value>
	bool IsPrimed { get; set; }

	/// <summary>
	/// Constructor of the ImageGrabber class that initializes a new instance. </summary> <param name="flags">The image extensions to be used for grabbing images.</param>
	/// /
	ImageGrabber(ImageGrabberFlags flags) {
		var exts = new List<string>();

		var imageGrabberFlags =
			Enum.GetValues(typeof(ImageGrabberFlags)).Cast<ImageGrabberFlags>();

		if (flags.HasFlag(ImageGrabberFlags.ALL)) {
			var type = typeof(ImageGrabberStrings);
			_extensions = type.GetAllPublicConstantValues<string>().ToArray();
		}

		else {
			foreach (var flag in imageGrabberFlags) {
				if (flags.HasFlag(flag) && flag != ImageGrabberFlags.ALL)
					exts.Add(flag.ToString().ToLower());
			}
		}

		_extensions = exts.ToArray();
	}

	readonly string[] _extensions;
}

/// <summary>
/// Enum representing various flags for the ImageGrabber class.
/// </summary>
[Serializable, Flags]
public enum ImageGrabberFlags {
	PNG  = 1 << 0,
	JPG  = 1 << 1,
	JPEG = 1 << 2,
	BMP  = 1 << 3,
	ALL  = 1 << 4
}

/// <summary>
/// Contains constants for common image format strings.
/// </summary>
public static class ImageGrabberStrings {
	public const string PNG  = "png";
	public const string JPG  = "jpg";
	public const string JPEG = "jpeg";
	public const string BMP  = "bmp";
}