namespace simple_plotting.src {
	internal static class PlotAxisRotationMapper {
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