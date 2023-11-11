// simple-plotting

namespace simple_plotting;

public readonly struct SaveStatus {
	public bool                State { get; }
	public IEnumerable<string> Paths { get; }

	public SaveStatus(bool state, IEnumerable<string> paths) {
		State = state;
		Paths = paths;
	}
}