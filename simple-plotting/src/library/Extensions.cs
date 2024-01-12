using System.Reflection;

namespace simple_plotting {
	/// <summary>
	///  Plotting library extension methods.
	/// </summary>
	internal static class Extensions {
		/// <summary>
		///  Ensures a provided integer is within the range of a collection and greater than or equal to zero.
		/// </summary>
		/// <param name="integer">Integer to check</param>
		/// <param name="collection">Collection to compare against</param>
		/// <typeparam name="T">Generic type parameter 'T'</typeparam>
		public static void ValidateInRange<T>(this ref int integer, ICollection<T> collection) {
			if (collection.IsNullOrEmpty())
				return;

			if (integer < 0)
				integer = 0;

			else if (integer >= collection.Count)
				integer = collection.Count - 1;
		}

		/// <summary>
		///  Queries a nullable CancellationToken to see if it was cancelled or if it has a value.
		/// </summary>
		/// <param name="token">Instance of CancellationToken</param>
		/// <returns>True if null or IsCancellationRequested</returns>
		public static bool WasCancelled(this CancellationToken? token) {
			return token.HasValue && token.Value.IsCancellationRequested;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public static bool WasCancelled(this CancellationToken token) {
			return token != CancellationToken.None && token.IsCancellationRequested;
		}

		/// <summary>
		///  https://briancaos.wordpress.com/2022/07/04/c-list-batch-braking-an-ienumerable-into-batches-with-net/
		///  This method will break an IEnumerable into batches of a given size. Any overflow will be returned as a final batch.
		/// </summary>
		/// <param name="enumerator">Collection to batch</param>
		/// <param name="targetSize">Batch size</param>
		/// <typeparam name="T">Generic type</typeparam>
		/// <returns>Enumerable of an enumerable with appropriate batch sizes. The final element may not have equal size
		///  due to it being use as 'overflow'</returns>
		public static Dictionary<int, List<T>> Batch<T>(this IEnumerable<T> enumerator, int targetSize) {
			var output     = new Dictionary<int, List<T>>();
			var enumerable = enumerator as T[] ?? enumerator.ToArray();

			var step     = enumerable.Length / targetSize;
			var overFlow = enumerable.Length % targetSize;

			var tracker     = int.MaxValue;
			var plotTracker = -1;
			//var totalCounter = 0;

			for (var i = 0; i < enumerable.Length; i++) {
				if (tracker > step - 1) {
					if (plotTracker != targetSize - 1) {
						plotTracker++;
						output.Add(plotTracker, new List<T>());
						tracker = 0;
					}
				}

				output[plotTracker].Add(enumerable[i]);
				tracker++;
				//totalCounter++;
			}

			return output;
		}

		/// <summary>
		///  Helper method for checking if an enumerable is null or empty.
		/// </summary>
		/// <param name="enumerable">Enumerable to check</param>
		/// <typeparam name="T">Generic type T of enumerable</typeparam>
		/// <returns>True if collection is null or empty</returns>
		public static bool IsNullOrEmpty<T>(this IEnumerable<T>? enumerable)
			=> enumerable == null || !enumerable.Any();

		/// <summary>
		/// Adds the elements of the specified collection to the HashSet.
		/// </summary>
		/// <typeparam name="T">The type of elements in the HashSet.</typeparam>
		/// <param name="hashSet">The HashSet to add the elements to.</param>
		/// <param name="enumerable">The collection whose elements should be added to the HashSet.</param>
		public static void AddRange<T>(this HashSet<T> hashSet, IEnumerable<T> enumerable) {
			// this is OKAY; _returnPlottables is cached and will not result in an allocation
			foreach (var item in enumerable)
				hashSet.Add(item);
		}

		public static HashSet<TInput> CastTo<TInput, TOutput>
			(this HashSet<TInput> hashSet, ref HashSet<TOutput> output) 
			where TOutput : TInput {
			// foreach IS efficient here
			// Enumerating a HashSet will only invoke GetEnumerator.MoveNext() 
			foreach (var element in hashSet) 
				if (element!=null && element is TOutput outputElement)
					output.Add(outputElement);

			return hashSet;
		}
		
		public static List<T> GetAllPublicConstantValues<T>(this Type type) {
			return type
			      .GetFields(CONSTANT_FLAGS)
			      .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(T))
			      .Select(x => (T)x.GetRawConstantValue()!)
			      .ToList();
		}
		
		const BindingFlags CONSTANT_FLAGS = BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;

	}
}