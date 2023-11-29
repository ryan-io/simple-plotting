namespace simple_plotting {
	/// <summary>
	/// Provides mapping between <see cref="PlotAxisRotation"/> and its actual rotation value.
	/// </summary>
	internal static class PlotAxisRotationMapper {
		/// <summary>
		/// Maps a <see cref="PlotAxisRotation"/> enum value to its corresponding rotation magnitude in degrees.
		/// </summary>
		/// <param name="plotAxisRotation">The <see cref="PlotAxisRotation"/> enum that represents specific rotation.</param>
		/// <returns>The rotation magnitude in degrees corresponding to the <see cref="PlotAxisRotation"/> enum value.</returns>
		internal static int Map(PlotAxisRotation plotAxisRotation) {
			var rotation = 0;

			switch (plotAxisRotation) {
				case PlotAxisRotation.Zero:
					rotation = 0;
					break;
				case PlotAxisRotation.FortyFive:
					rotation = 45;
					break;
				case PlotAxisRotation.Sixty:
					rotation = 60;
					break;
				case PlotAxisRotation.Ninety:
					rotation = 90;
					break;
			}

			return rotation;
		}
	}
}