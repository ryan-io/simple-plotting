namespace simple_plotting;

/// <summary>
/// Represents a container for the rotation value of a plot axis.
/// This is a read-only struct.
/// </summary>
public readonly struct PlotAxisRotationContainer {
	/// <summary>
	/// Gets the rotation value of the plot axis.
	/// </summary>
	public int Rotation { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="PlotAxisRotationContainer"/> struct with a specified rotation value.
	/// </summary>
	/// <param name="rotation">The rotation value of the plot axis.</param>
	public PlotAxisRotationContainer(int rotation) {
		Rotation = rotation;
	}
}