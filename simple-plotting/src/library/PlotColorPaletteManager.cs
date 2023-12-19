using ScottPlot;

namespace simple_plotting {
	/// <summary>
	/// Manages a cyclic iterator for colors in a palette. Visual plot colors of each
	/// channel are determined by this class.
	/// </summary>
	public class PlotColorPaletteManager : Singleton<PlotColorPaletteManager> {
		/// <summary>
		/// The color palette used for plotting.
		/// </summary>
		private IPalette Palette { get; set; }

		/// <summary>
		/// The current index in the color palette.
		/// </summary>
		private int CurrentIndex { get; set; } = 0;

		/// <summary>
		/// Resets the current index to the start of the palette.
		/// </summary>
		public void Reset() => CurrentIndex = 0;

		/// <summary>
		/// Returns the next color in the palette and increments the index.
		/// If the end of the palette is reached, it wraps around to the start.
		/// </summary>
		/// <returns>The next System.Drawing.Color in the palette.</returns>
		public System.Drawing.Color GetNext() {
			var color = Palette.Colors[CurrentIndex];
			CurrentIndex++;

			if (CurrentIndex >= Palette.Count()) {
				CurrentIndex = 0;
			}

			return color;
		}

		/// <summary>
		/// Changes the palette to a new palette and resets the index.
		/// </summary>
		/// <param name="newPalette">The new palette to use.</param>
		public void ChangePalette(IPalette newPalette) {
			Palette      = newPalette;
			CurrentIndex = 0;
		}

		/// <summary>
		/// Default constructor that sets the palette to ScottPlot.Palette.Category10.
		/// </summary>
		public PlotColorPaletteManager() {
			Palette = ScottPlot.Palette.Category10;
		}

		/// <summary>
		/// Constructor that accepts a specific palette to use.
		/// </summary>
		/// <param name="palette">The palette to use.</param>
		public PlotColorPaletteManager(IPalette palette) {
			Palette = palette;
		}
	}
}