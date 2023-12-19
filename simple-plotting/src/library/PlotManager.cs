namespace simple_plotting {
	/// <summary>
	/// A Singleton class to interact with your application (WPF, WinForms, etc.) and the PlotBuilderFluent instances.
	/// </summary>
	public class PlotManager : Singleton<PlotManager> {
		/// <summary>
		///  Internal event handler for when your application exits.
		/// </summary>
		internal event Action OnApplicationExit = delegate { };

		/// <summary>
		///  Invokes the OnApplicationExit event. This should be invoked when your application exits.
		/// </summary>
		public void HandleOnApplicationExit() {
			OnApplicationExit.Invoke();
		}
	}
}