using System.Runtime.CompilerServices;

namespace simple_plotting {
	/// <summary>
	///  A helper class to determine the maxima & minima y-axes limits
	/// </summary>
	public class PlotLimitCalculator {
		double? DefaultUpper { get; } = null;
		double? DefaultLower { get; } = null;
		double  DeltaPercent { get; }

		/// <summary>
		///  Returns a readonly struct containing the upper and lower values provided by 'channels'
		/// </summary>
		/// <param name="channels">Enumerable of plot channels</param>
		/// <param name="caller">Calculate()</param>
		/// <returns>Readonly struct with upper & lower limits</returns>
		/// <exception cref="Exception">Thrown if 'channels' is empty</exception>
		public PlotLimit Calculate(IEnumerable<PlotChannel> channels, [CallerMemberName] string caller = "") {
			if (!channels.Any())
				throw new Exception($"{caller}: {Message.EXCEPTION_NO_PLOT_ENUMERABLES_LIMIT_CAL}");

			double upper = double.NegativeInfinity, lower = double.PositiveInfinity;

			foreach (PlotChannel channel in channels) {
				foreach (var record in channel.Records) {
					if (record.Value.CompareTo(lower) < 0) {
						lower = record.Value;
					}

					if (record.Value.CompareTo(upper) > 0) {
						upper = record.Value;
					}
				}
			}

			if (DefaultUpper != null && upper < DefaultUpper)
				upper = (double)DefaultUpper;

			if (DefaultLower != null && lower > DefaultLower)
				lower = (double)DefaultLower;

			double sanitizedPercent;

			if (DeltaPercent < 0d)
				sanitizedPercent = DEFAULT_DELTA_PERCENT;
			else if (DeltaPercent > 100d)
				sanitizedPercent = 100d;
			else
				sanitizedPercent =
					DeltaPercent;

			var delta = upper - lower;
			delta *= sanitizedPercent / 100d;

			upper += delta;
			lower -= delta;

			return new PlotLimit(upper, lower);
		}

		/// <summary>
		///  Default constructor
		///  Plot axis limits will be set with respect to the Calculate method
		///  Takes a percent (deltaPercent) of the total delta (UpperLimit-LowerLimit) and adds to UpperLimit & subtracts from
		///  LowerLimit to ensure the entire plot is contained within a plot-window
		/// </summary>
		///  <param name="deltaPercent">Percent of total delta upper and lower limits</param>
		public PlotLimitCalculator(double deltaPercent = DEFAULT_DELTA_PERCENT) {
			DeltaPercent = deltaPercent;
		}

		/// <summary>
		///  Use this constructor if you want the plot to have a minimum/maximum default 
		///  Calculate will verify its calculated upper/lower limits are within the bounds of DefaultUpper & DefaultLower
		///  Takes a percent (deltaPercent) of the total delta (UpperLimit-LowerLimit) and adds to UpperLimit & subtracts from
		///  LowerLimit to ensure the entire plot is contained within a plot-window
		/// </summary>
		/// <param name="defaultUpper">Absolute minimum upper limit</param>
		/// <param name="defaultLower">Absolute maximum lower limit</param>
		/// /// <param name="deltaPercent">Percent of total delta upper and lower limits</param>
		public PlotLimitCalculator(double? defaultUpper, double? defaultLower,
			double deltaPercent = DEFAULT_DELTA_PERCENT) {
			DefaultUpper = defaultUpper;
			DefaultLower = defaultLower;
			DeltaPercent = deltaPercent;
		}

		const double DEFAULT_DELTA_PERCENT = 10d;
	}

	/// <summary>
	///  Readonly struct containing upper & lower limits for a collection of plots
	/// </summary>
	public readonly struct PlotLimit {
		public double Upper { get; }
		public double Lower { get; }

		public PlotLimit(double upper, double lower) {
			Upper = upper;
			Lower = lower;
		}
	}
}