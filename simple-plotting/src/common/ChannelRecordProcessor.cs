using NAMESPACE;
using ScottPlot;

namespace simple_plotting.src;

public class ChannelRecordProcessor {
	IReadOnlyList<Plot> Data         { get; }
	IPlottablePrime     FactoryPrime { get; }

	/// <summary>
	///  Helper method for SetInitialState. This method will add the data to the plot.
	/// </summary>
	/// <param name="batchedRecord">Current batched record</param>
	/// <param name="plotTracker">Iteration tracker</param>
	/// <param name="channel">Current channel being enumerated</param>
	public void Process(
		IEnumerable<PlotChannelRecord> batchedRecord,
		int plotTracker,
		PlotChannel channel) {
		var batchedArray = batchedRecord.ToArray();
		var dateTimes    = batchedArray.Select(x => x.DateTime.ToOADate()).ToArray();
		var values       = batchedArray.Select(v => v.Value).ToArray();

		var poco = new PlottableData {
			X          = dateTimes,
			Y          = values,
			SampleRate = channel.SampleRate.Value
		};

		var constructorFactory = new PlottableConstructorMapper(FactoryPrime.PlottableType);
		var constructor        = constructorFactory.Determine(ref poco);
		var workingPlot        = Data[plotTracker];
		var product            = FactoryPrime.PrimeProduct(workingPlot, ref constructor);

		var plotTypeMapper  = new PlottableFactoryTypeMapper(FactoryPrime.PlottableType);
		var factoryDelegate = plotTypeMapper.Determine(product, channel, poco);

		if (factoryDelegate == null)
			throw new Exception(Message.EXCEPTION_NO_PLOTTABLE_FAC_METHOD);

		var reset = product.AddViaFactoryMethod(factoryDelegate.Invoke);

		workingPlot.XAxis.DateTimeFormat(true);
		workingPlot.YAxis2.SetSizeLimit();

		reset.Reset();
	}

	public ChannelRecordProcessor(IReadOnlyList<Plot> plots, IPlottablePrime factoryPrime) {
		Data         = plots;
		FactoryPrime = factoryPrime;
	}
}