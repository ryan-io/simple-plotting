// CSV-Plotter

namespace SimplePlot {
	/// <summary>
	///  Simple container that defines the width and height of a plot.
	/// </summary>
	public readonly struct PlotSizeContainer {
		public int Width  { get; }
		public int Height { get; }

		public PlotSizeContainer(int width, int height) {
			Width  = width;
			Height = height;
		}
	}
}