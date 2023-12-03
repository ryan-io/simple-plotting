using System.Drawing;
using System.Drawing.Imaging;

namespace simple_plotting.runtime {
	public class BitmapParser : IDisposable {
		/// <summary>
		/// A delegate type for manipulating the RGB values of a particular pixel in a bitmap.
		/// </summary>
		public delegate void BitmapProcessDelegate(ref int red, ref int green, ref int blue);
		
		/// <summary>
		/// Returns a reference to the internal array of Bitmap objects.
		/// </summary>
		public ref Bitmap[] GetAllBitmaps() => ref _bitmaps;
		
		/// <summary>
		/// Returns a read-only reference to the array of the paths to all images.
		/// </summary>
		public ref readonly string[] GetAllPaths() => ref _paths;

		/// <summary>
		/// Retrieves a specific Bitmap object based on an index.
		/// </summary>
		public ref Bitmap GetBitmap(int bitmapIndex) {
			if (_bitmaps == null)
				throw new NullReferenceException();

			if (bitmapIndex > _bitmaps.Length - 1)
				throw new IndexOutOfRangeException(Message.EXCEPTION_INDEX_OUT_OF_RANGE);

			return ref _bitmaps[bitmapIndex];
		}

		/// <summary>
		/// Retrieves the path for a specific bitmap based on an index.
		/// </summary>
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
			if (_isDisposed)
				return;

			//TODO: research the need for GC.SuppressFinalize(this);
			// GC.SuppressFinalize(this);

			foreach (var bitmap in _bitmaps)
				bitmap.Dispose();

			_bitmaps    = default!;
			_isDisposed = true;
		}

		/// <summary>
		/// Modifies the RGB values of the pixels in a bitmap unsafely and concurrently.
		/// </summary>
		public unsafe ref Bitmap[] ModifyRgbUnsafe(int bitmapIndex, BitmapProcessDelegate functor) {
			ref var bmp = ref GetBitmap(bitmapIndex);

			// Lock the bitmap bits. This will allow us to modify the bitmap data.
			// Data is modified by traversing bitmap data (created in this method) and invoking functor.

			var bitmapData = bmp.LockBits(
				GetNewRect(ref bmp),
				ImageLockMode.ReadWrite,
				bmp.PixelFormat);

			// this gives us size in bits... divide by 8 to get size in bytes
			var   bytesPerPixel = Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
			var   height        = bitmapData.Height;
			var   width         = bitmapData.Width * bytesPerPixel;
			byte* pxlPtr        = (byte*)bitmapData.Scan0;

			// note documentation for Parallel.For
			//		from INCLUSIVE ::: to EXCLUSIVE
			//		okay to pass 0, height
			Parallel.For(0, height,
				integer => {
					byte* row = pxlPtr + integer * bitmapData.Stride;

					for (var i = 0; i < width; i += bytesPerPixel) {
						// the current pixel to work on; passes rgb values to a delegate
						int red   = row[i + 2];
						int green = row[i + 1];
						int blue  = row[i];

						functor.Invoke(ref red, ref green, ref blue);
						GuardRgbValue(ref red); GuardRgbValue(ref green); GuardRgbValue(ref blue);
						
						row[i + 2] = (byte)red;
						row[i + 1] = (byte)green;
						row[i]     = (byte)blue;
					}
				});

			bmp.UnlockBits(bitmapData);
			
			return ref _bitmaps;
		}

		/// <summary>
		/// Asynchronously saves all the Bitmap objects in the _bitmap array to disk.
		/// </summary>
		public async Task SaveBitmapsAsync(string path) {
			if (string.IsNullOrWhiteSpace(path))
				throw new DirectoryNotFoundException(Message.EXCEPTION_NULL_BITMAP_PATHS);

			if (!Directory.Exists(path)) {
				Directory.CreateDirectory(path);
			}

			var count = _bitmaps.Length;
			var tasks = new Task[count];

			for (var i = 0; i < count; i++) {
				tasks[i] = SaveBitmapTask(_bitmaps[i], GetSanitizedPath(path));
			}

			await Task.WhenAll(tasks);
		}

		string GetSanitizedPath(string path) {
			var newPath = Path.Combine(path, Path.GetFileNameWithoutExtension(GetPath(0)) + "_bmpParsed.png");
			return newPath;
		}

		static void GuardRgbValue(ref int value) {
			if (value < 0)
				value = 0;

			if (value > 255)
				value = 255;
		}
		
		static Task SaveBitmapTask(Bitmap bmp, string path) => Task.Run(() => bmp.Save(path, ImageFormat.Png));

		static Rectangle GetNewRect(ref Bitmap bmp) => new(0, 0, bmp.Width, bmp.Height);

#region PLUMBING

		public BitmapParser(ref string[] imgPaths) {
			_paths   = imgPaths;
			_bitmaps = new Bitmap[imgPaths.Length];

			for (var i = 0; i < imgPaths.Length; i++) {
				if (!File.Exists(imgPaths[i]))
					throw new FileNotFoundException(Message.EXCEPTION_FILE_NOT_FOUND + " " + imgPaths[i]);

				var bmp = new Bitmap(imgPaths[i]);
				_bitmaps[i] = bmp;
			}
		}

		Bitmap[]          _bitmaps;
		bool              _isDisposed;
		readonly string[] _paths;

#endregion
	}
}