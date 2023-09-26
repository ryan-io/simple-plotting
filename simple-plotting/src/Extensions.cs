namespace SimplePlot
{
    /// <summary>
    ///  Plotting library extension methods.
    /// </summary>
    internal static class Extensions
    {
        /// <summary>
        ///  https://briancaos.wordpress.com/2022/07/04/c-list-batch-braking-an-ienumerable-into-batches-with-net/
        ///  This method will break an IEnumerable into batches of a given size. Any overflow will be returned as a final batch.
        /// </summary>
        /// <param name="enumerator">Collection to batch</param>
        /// <param name="size">Batch size</param>
        /// <typeparam name="T">Generic type</typeparam>
        /// <returns>Enumerable of an enumerable with appropriate batch sizes. The final element may not have equal size
        ///  due to it being use as 'overflow'</returns>
        public static IEnumerable<IEnumerable<T>?> Batch<T>(this IEnumerable<T> enumerator, int size)
        {
            T[]? batch = null;
            var count = 0;

            foreach (var item in enumerator)
            {
                batch ??= new T[size];

                batch[count++] = item;
                if (count != size)
                    continue;

                yield return batch;

                batch = null;
                count = 0;
            }

            if (batch != null && count > 0)
                yield return batch.Take(count).ToArray();
        }
    }
}