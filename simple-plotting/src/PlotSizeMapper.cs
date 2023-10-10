namespace simple_plotting.src {
	/// <summary>
	///  Maps a <see cref="PlotSize"/> to a <see cref="PlotSizeContainer"/>.
	/// </summary>
	internal static class PlotSizeMapper {
		/// <summary>
		///  Maps a <see cref="PlotSize"/> to a <see cref="PlotSizeContainer"/>.
		/// </summary>
		/// <param name="plotSize">Enum defining a standard size to use for the plot</param>
		/// <returns>A container with the appropriate width & height</returns>
		internal static PlotSizeContainer Map(PlotSize plotSize) {
			int width  = 1920;
			int height = 1080;
			
			switch (plotSize) {
				case PlotSize.S800X600:
					width = 800; height = 600;
					break;
				case PlotSize.S1920X1080:
					width = 1920; height = 1080;
					break;
				case PlotSize.S1440X900:
					width = 1440; height = 900;
					break;
				case PlotSize.S1600X900:
					width = 1600; height = 900;
					break;
				case PlotSize.S1536X864:
					width = 1536; height = 864;
					break;
				case PlotSize.S1366X768:
					 width = 1366; height = 768;
					break;
				case PlotSize.S1280X1024:
					 width = 1280; height = 1024;
					break;
				case PlotSize.S1280X800:
					width = 1280; height = 800;
					break;
				case PlotSize.S1280X720:
					width = 1280; height = 720;
					break;
				case PlotSize.S1024X768:
					width = 1024; height = 768;
					break;
				case PlotSize.S800X480:
					width = 800; height = 480;
					break;
				case PlotSize.S640X480:
					width = 640; height = 480;
					break;
				case PlotSize.S480X320:
					width = 480; height = 320;
					break;
				case PlotSize.S320X240:
					width = 320; height = 240;
					break;
				case PlotSize.S240X160:
					width = 240; height = 160;
					break;
				case PlotSize.S160X120:
					width = 160; height = 120;
					break;
				case PlotSize.S80X60:
					width = 80; height = 60;
					break;
			}

			return new PlotSizeContainer(width, height);
		}
	}
}