// simple-plotting

namespace simple_plotting;

/// <summary>
/// Represents the result of a save operation, including the state of the operation and the paths involved.
/// This is a read-only struct.
/// </summary>
public readonly struct SaveStatus {
	/// <summary>
	/// Represents the status of the save operation. True represents a successful save, while false represents a failed save.
	/// </summary>
	public bool State { get; }

	/// <summary>
	/// Represents the paths involved in the save operation.
	/// </summary>
	public IEnumerable<string> Paths { get; }
	
	/// <summary>
	/// If an exception was thrown, this optional property can be set with the error message.
	/// </summary>
	public string? ErrorMessage { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="SaveStatus"/> struct with the specified state and paths.
	/// </summary>
	/// <param name="state">The status of the save operation.</param>
	/// <param name="paths">The paths involved in the save operation.</param>
 	/// <param name="errorMessage">Optional error message (if exception thrown)</param>
	public SaveStatus(bool state, IEnumerable<string> paths, string? errorMessage = default) {
		State        = state;
		Paths        = paths;
		ErrorMessage = errorMessage;
	}
}