// simple-plotting

namespace simple_plotting;

/// <summary>
/// Represents the result of a save operation, including the state of the operation and the paths involved.
/// This is a read-only struct.
/// </summary>
public readonly struct PlotSaveStatus {
	/// <summary>
	/// Represents the status of the save operation. True represents a successful save, while false represents a failed save.
	/// </summary>
	public bool State { get; }

	/// <summary>
	/// Represents the paths involved in the save operation.
	/// </summary>
	public IEnumerable<string> Paths { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="PlotSaveStatus"/> struct with the specified state and paths.
	/// </summary>
	/// <param name="state">The status of the save operation.</param>
	/// <param name="paths">The paths involved in the save operation.</param>
	public PlotSaveStatus(bool state, IEnumerable<string> paths) {
		State = state;
		Paths = paths;
	}
}

public readonly struct CanvasSaveStatus {
	/// <summary>
	/// Represents the status of the save operation. True represents a successful save, while false represents a failed save.
	/// </summary>
	public bool State { get; }

	/// <summary>
	/// Represents the paths involved in the save operation.
	/// </summary>
	public IEnumerable<string> Paths { get; }

	/// <summary>
	/// Represents the save status of a canvas.
	/// </summary>
	/// <param name="state">The state of the save status.</param>
	/// <param name="paths">The paths of the saved canvases.</param>
	public CanvasSaveStatus(bool state, IEnumerable<string> paths) {
		State = state;
		Paths = paths;
	}
}