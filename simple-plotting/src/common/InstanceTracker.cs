using System.Collections.Concurrent;

namespace simple_plotting {
	/// <summary>
	///  Tracks instances of PlotBuilderFluent. This is used to dispose of all instances when the application exits.
	/// </summary>
	internal class InstanceTracker : Singleton<InstanceTracker> {
		/// <summary>
		///  Internal ConcurrenBag that tracks instances of PlotBuilderFluent.
		/// </summary>
		ConcurrentBag<PlotBuilderFluent> BuilderInstances { get; set; } = new();

		/// <summary>
		///  Adds an instance of PlotBuilderFluent to the internal ConcurrentBag.
		/// </summary>
		/// <param name="builder">PlotBuilderFluent instance to add to the internal ConcurrentBag</param>
		internal void RegisterInstance(PlotBuilderFluent builder) {
			if (BuilderInstances.Contains(builder))
				return;

			BuilderInstances.Add(builder);
		}

		/// <summary>
		///  Adds an instance of PlotBuilderFluent to the internal ConcurrentBag.
		///  This should be invoked when your application exits
		/// </summary>
		void ApplicationExit() {
			if (!BuilderInstances.Any())
				return;

			foreach (var i in BuilderInstances) {
				i.Dispose();
			}
		}

		/// <summary>
		///  Default constructor. Adds an event handler to PlotManager.Global.OnApplicationExit.
		/// </summary>
		public InstanceTracker() {
			PlotManager.Global.OnApplicationExit += ApplicationExit;
		}
	}
}