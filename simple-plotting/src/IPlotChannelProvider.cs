namespace SimplePlot;

public interface IPlotChannelProvider {
	Task<IReadOnlyList<PlotChannel>> ExtractAsync();
}