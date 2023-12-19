namespace simple_plotting {
	/// <summary>
	///   A singleton class. Provides a single static instance of class type T.
	///  <para>Usage: public class MyClass : Singleton&lt;MyClass&gt; { }</para>
	///  Can be accessed from anywhere using MyClass.Instance
	///  Can also inject a logger using InjectLogger(ILogging logger); this implementation of logger is internal
	///  to riot-bcl.
	/// </summary>
	/// <typeparam name="T">Type instance create a Singleton for</typeparam>
	public class Singleton<T> where T : class, new() {
		public static T Global {
			get {
				lock (_mutex) 
					return _instance.Value;
			}
		}

		static readonly object _mutex = new();

		static readonly Lazy<T> _instance = new (() => new T());
		
		public Singleton() {
			
		}
	}
}