using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace simple_plotting {
	/// <summary>
	/// Parses and manipulates bitmap images.
	/// </summary>
	public class BitmapParser : IDisposable {
		/// <summary>
		/// A delegate type for manipulating the RGB values of a particular pixel in a bitmap.
		/// </summary>
		public delegate void BitmapRgbDelegate(ref int pxlIndex, ref int red, ref int green, ref int blue);

		/// <summary>
		/// Gets a value indicating whether the object has been disposed.
		/// </summary>
		/// <value>
		/// <c>true</c> if the object has been disposed; otherwise, <c>false</c>.
		/// </value>
		public bool IsDisposed { get; private set; }

		/// <summary>
		/// Returns a reference to the internal array of Bitmap objects.
		/// </summary>
		public ref Bitmap[] GetAllBitmaps() => ref _bitmaps;

		/// <summary>
		/// Returns a read-only reference to the array of the paths to all images.
		/// </summary>
		public ref readonly string[] GetAllPaths() => ref _paths;

		/// <summary>
		/// Retrieves a specific Bitmap object from the BitmapParser's array, using a provided index.
		/// </summary>
		/// <param name="bitmapIndex">The index of the Bitmap object to be retrieved.</param>
		/// <returns>A reference to the Bitmap object at the specified index.</returns>
		/// <exception cref="NullReferenceException">Thrown when the internal Bitmap array has not been initialized.</exception>
		/// <exception cref="IndexOutOfRangeException">Thrown when the provided index is outside the bounds of the Bitmap array.</exception>
		public ref Bitmap GetBitmap(int bitmapIndex) {
			if (IsDisposed)
				throw new BitMapParserDisposedException();

			if (_bitmaps == null)
				throw new NullReferenceException();

			if (bitmapIndex > _bitmaps.Length - 1)
				throw new IndexOutOfRangeException(Message.EXCEPTION_INDEX_OUT_OF_RANGE);

			return ref _bitmaps[bitmapIndex];
		}

		public void ScaleBitmapAndSetNew(int bitmapIndex, float scale, BitmapResizeCriteria criteria) {
			var newBmp = GetNewScaledBitmap(bitmapIndex, scale, criteria);
			
			_bitmaps[bitmapIndex].Dispose();
			_bitmaps[bitmapIndex] = newBmp;
		}
		
		public void SetNewBitmap(int bitmapIndex, Bitmap newBmp) {
			_bitmaps[bitmapIndex]?.Dispose();
			_bitmaps[bitmapIndex] = newBmp;
		}

		/// <summary>
		/// Returns a new scaled Bitmap based on the specified bitmap index, scale and criteria.
		/// </summary>
		/// <param name="bitmapIndex">The index of the bitmap.</param>
		/// <param name="scale">The scale factor.</param>
		/// <param name="criteria">The criteria for resizing.</param>
		/// <returns>A new scaled Bitmap.</returns>
		/// <exception cref="IndexOutOfRangeException">Thrown if the bitmap index is less than zero.</exception>
		/// Reference -> //https://stackoverflow.com/questions/1922040/how-to-resize-an-image-c-sharp
		public Bitmap GetNewScaledBitmap(int bitmapIndex, float scale, BitmapResizeCriteria criteria) {
			if (bitmapIndex < 0)
				throw new IndexOutOfRangeException(Message.EXCEPTION_INDEX_LESS_THAN_ZERO);

			ref var bmp = ref GetBitmap(bitmapIndex);

			return GetNewScaledBitmap(ref bmp, scale, criteria);
		}

		/// <summary>
		/// Scales the given bitmap to a new size based on the specified scale and resizing criteria.
		/// </summary>
		/// <param name="bmp">The original bitmap to be scaled.</param>
		/// <param name="scale">The scaling factor. Must be a positive value.</param>
		/// <param name="criteria">The criteria used for resizing. Determines the quality of the resized image.</param>
		/// <returns>
		/// The new scaled bitmap with the specified size and quality settings.
		/// </returns>
		/// Reference -> https://stackoverflow.com/questions/1922040/how-to-resize-an-image-c-sharp
		public static Bitmap GetNewScaledBitmap(ref Bitmap bmp, float scale, BitmapResizeCriteria criteria) {
			scale = MathF.Abs(scale);
			var scaledWidth  = (int)(bmp.Width  * scale);
			var scaledHeight = (int)(bmp.Height * scale);

			var scaledBmp  = new Bitmap(scaledWidth, scaledHeight);
			var scaledRect = GetNewRect(ref scaledBmp);
			scaledBmp.SetResolution(bmp.HorizontalResolution, bmp.VerticalResolution);

			using var graphics = Graphics.FromImage(scaledBmp);

			graphics.CompositingMode    = criteria.CompositingMode;
			graphics.CompositingQuality = criteria.CompositingQuality;
			graphics.InterpolationMode  = criteria.InterpolationMode;
			graphics.SmoothingMode      = criteria.SmoothingMode;
			graphics.PixelOffsetMode    = criteria.PixelOffsetMode;

			using var wrap = new ImageAttributes();

			wrap.SetWrapMode(WrapMode.TileFlipXY);
			graphics.DrawImage(bmp, scaledRect, 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, wrap);

			return scaledBmp;
		}

		/// <summary>
		/// Retrieves the path of a specific bitmap image.
		/// </summary>
		/// <param name="index">Index position of the image path in the list of paths.</param>
		/// <returns>Returns a reference to the path string of the image.</returns>
		/// <exception cref="NullReferenceException">Thrown when the list of paths is null.</exception>
		/// <exception cref="IndexOutOfRangeException">Thrown when index is out of the range of the list of paths.</exception>
		public ref string GetPath(int index) {
			if (_paths == null)
				throw new NullReferenceException();

			if (index > _paths.Length - 1)
				throw new IndexOutOfRangeException(Message.EXCEPTION_INDEX_OUT_OF_RANGE);

			return ref _paths[index];
		}

		/// <summary>
		/// Releases the resources used by the BitmapParser class.
		/// </summary>
		public void Dispose() {
			if (IsDisposed)
				return;

			//TODO: research the need for GC.SuppressFinalize(this);
			// GC.SuppressFinalize(this);

			foreach (var bitmap in _bitmaps)
				bitmap.Dispose();

			_bitmaps   = default!;
			IsDisposed = true;
		}

		/// <summary>
		/// Modifies the RGB values of a Bitmap at a specific index in an unsafe and concurrent manner. 
		/// This code is base on: https://csharpexamples.com/fast-image-processing-c/
		///		from author Turgay
		/// Modifies the RGB values of a Bitmap at a specific index in an unsafe and concurrent manner.
		/// </summary>
		/// <param name="bitmapIndex">The index of the Bitmap in the internal array.</param>
		/// <param name="functor">A delegate function that performs the desired modifications on the pixel data.</param>
		/// <returns>Returns reference to the internal array of Bitmap objects.</returns>
		public unsafe ref Bitmap[] ModifyRgbUnsafe(int bitmapIndex, BitmapRgbDelegate functor) {
			if (IsDisposed)
				throw new BitMapParserDisposedException();

			ref var bmp = ref GetBitmap(bitmapIndex);

			// Lock the bitmap bits. This will allow us to modify the bitmap data.
			// Data is modified by traversing bitmap data (created in this method) and invoking functor.

			var bmpData = bmp.LockBits(
				GetNewRect(ref bmp),
				ImageLockMode.ReadWrite,
				bmp.PixelFormat);

			// this gives us size in bits... divide by 8 to get size in bytes
			var   bytesPerPixel = Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
			var   imgHeight     = bmpData.Height;
			var   imgWidth      = bmpData.Width * bytesPerPixel;
			byte* headPtr       = (byte*)bmpData.Scan0;

			// note documentation for Parallel.For
			//		from INCLUSIVE ::: to EXCLUSIVE
			//		okay to pass 0, height
			Parallel.For(0, imgHeight, _parallelism,
				pxlIndex => {
					byte* row = headPtr + pxlIndex * bmpData.Stride;

					for (var i = 0; i < imgWidth; i += bytesPerPixel) {
						// the current pixel to work on; passes rgb values to a delegate
						int red   = row[i + 2];
						int green = row[i + 1];
						int blue  = row[i];

						functor.Invoke(ref pxlIndex, ref red, ref green, ref blue);
						GuardRgbValue(ref red);
						GuardRgbValue(ref green);
						GuardRgbValue(ref blue);

						row[i + 2] = (byte)red;
						row[i + 1] = (byte)green;
						row[i]     = (byte)blue;
					}
				});

			bmp.UnlockBits(bmpData);

			return ref _bitmaps;
		}

		/// <summary>
		/// Asynchronously saves Bitmap objects to the files at a specified path.
		/// </summary>
		/// <param name="path">The directory path where the Bitmaps should be saved.</param>
		/// <param name="disposeOnSuccess">Optional parameter determining whether to dispose the BitmapParser on successful save. Default is false.</param>
		/// <returns>Returns a Task that represents the asynchronous operation. The task result contains void.</returns>
		/// <exception cref="DirectoryNotFoundException">Thrown when the provided path is null, empty, or consists only of white-space characters.</exception>
		public async Task<List<string>> SaveBitmapsAsync(string path, bool disposeOnSuccess = false) {
			if (IsDisposed)
				throw new BitMapParserDisposedException();

			if (string.IsNullOrWhiteSpace(path))
				throw new DirectoryNotFoundException(Message.EXCEPTION_NULL_BITMAP_PATHS);

			if (!Directory.Exists(path)) {
				Directory.CreateDirectory(path);
			}

			var output = new List<string>();
			var count  = _bitmaps.Length;
			var tasks  = new Task[count];

			for (var i = 0; i < count; i++) {
				var sanitizedPath = GetSanitizedPath(ref path);
				output.Add(sanitizedPath);
				tasks[i] = SaveImageTask(_bitmaps[i], sanitizedPath);
			}

			await Task.WhenAll(tasks);

			if (disposeOnSuccess)
				Dispose();

			return output;
		}

		/// <summary>
		/// Gets a sanitized path by combining the given path with the filename without extension of the path at index 0, and appends "_bmpParsed.png" extension to it.
		/// </summary>
		/// <param name="path">The path to be sanitized. This parameter is passed by reference and will be modified to the sanitized path.</param>
		/// <returns>The sanitized path.</returns>
		string GetSanitizedPath(ref string path) {
			var newPath = Path.Combine(path, Path.GetFileNameWithoutExtension(GetPath(0)) + "_bmpParsed.png");
			return newPath;
		}

		/// <summary>
		/// Guards the RGB value to ensure it is within the valid range.
		/// </summary>
		/// <remarks>
		/// If the <paramref name="value"/> is less than 0, it will be set to 0.
		/// If the <paramref name="value"/> is greater than 255, it will be set to 255.
		/// </remarks>
		/// <param name="value">The RGB value to be guarded.</param>
		/// <returns>None.</returns>
		static void GuardRgbValue(ref int value) {
			if (value < 0)
				value = 0;

			if (value > 255)
				value = 255;
		}

		/// <summary>
		/// Saves an image to the specified path asynchronously.
		/// </summary>
		/// <param name="bmp">The image to be saved.</param>
		/// <param name="path">The path where the image will be saved.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		static Task SaveImageTask(Image bmp, string path) => Task.Run(() => bmp.Save(path, ImageFormat.Png));

		/// <summary>
		/// Creates a new <see cref="Rectangle"/> object with the specified dimensions based on the provided <see cref="Bitmap"/>.
		/// </summary>
		/// <param name="bmp">The <see cref="Bitmap"/> used to determine the width and height of the rectangle.</param>
		/// <returns>A new <see cref="Rectangle"/> object with the specified dimensions.</returns>
		static Rectangle GetNewRect(ref Bitmap bmp) => new(0, 0, bmp.Width, bmp.Height);

#region PLUMBING

		/// Represents a bitmap parser used to load and parse a collection of image paths.
		/// /
		public BitmapParser(ref string[] imgPaths) {
			_paths       = imgPaths;
			_bitmaps     = new Bitmap[imgPaths.Length];
			_parallelism = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };

			for (var i = 0; i < imgPaths.Length; i++) {
				if (!File.Exists(imgPaths[i]))
					throw new FileNotFoundException(Message.EXCEPTION_FILE_NOT_FOUND + " " + imgPaths[i]);

				var bmp = new Bitmap(imgPaths[i]);
				_bitmaps[i] = bmp;
			}
		}

		Bitmap[]                 _bitmaps;
		readonly ParallelOptions _parallelism;
		readonly string[]        _paths;

#endregion
	}
}