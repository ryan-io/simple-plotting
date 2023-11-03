using ScottPlot;

namespace simple_plotting.src {
	public class PlotColorPaletteManager {
		IPalette Palette { get; set; }

		int CurrentIndex { get; set; } = 0;

		public System.Drawing.Color GetNext() {
			var color = Palette.Colors[CurrentIndex];
			CurrentIndex++;

			if (CurrentIndex >= Palette.Count()) {
				CurrentIndex = 0;
			}

			return color;
		}

		public void ChangePalette(IPalette newPalette) {
			Palette      = newPalette;
			CurrentIndex = 0;
		}

		public PlotColorPaletteManager() {
			Palette = ScottPlot.Palette.Category10;
		}

		public PlotColorPaletteManager(IPalette palette) {
			Palette = palette;
		}
	}
}