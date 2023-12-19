// simple-plotting

namespace simple_plotting;

/// <summary>
///  POCO for holding data for a plottable.
/// </summary>
public struct PlottableData {
	public double[] X;
	public double[] Y;
	public double?  SampleRate;

	public PlottableData() {
		X          = new double[0];
		Y          = new double[0];
		SampleRate = 0.0;
	}
}