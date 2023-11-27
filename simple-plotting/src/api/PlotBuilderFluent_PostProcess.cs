// simple-plotting

using ScottPlot.Plottable;

namespace simple_plotting;

public partial class PlotBuilderFluent {
	/// <inheritdoc cref="IPlotBuilderFluentPlottables.GoToProduct" />
	public IPlotBuilderFluentProduct GoToProduct() => this;

	/// <inheritdoc />
	public IPlotBuilderFluentPostProcess SetSizeOfAll(PlotSize size) {
		PlotHelper.SetSizeOfAll(_plots, size);
		return this;
	}

	/// <inheritdoc />
	public IPlotBuilderFluentPostProcess SetScatterLabel(IPlottable? plot, string newLabel) {
		if (plot is null)
			return this;

		var scatterPlot = (ScatterPlot)plot;
		scatterPlot.Label = newLabel;
		RefreshRenderers();

		return this;
	}

	/// <inheritdoc />
	public IPlotBuilderFluentPostProcess ChangeTitle(string newTitle, int fontSize = 14, bool isBold = false) {
		if (string.IsNullOrWhiteSpace(newTitle))
			throw new Exception(Message.EXCEPTION_TITLE_INVALID);

		var plotTracker = 1;
		foreach (var plot in _plots) {
			plot.Title($"{newTitle} - #{plotTracker}", size: fontSize, bold: isBold);
			plot.Render();
			plotTracker++;
		}

		return this;
	}

	/// <inheritdoc />
	public IPlotBuilderFluentPostProcess TrySetScatterLabel(string newLabel, params int[] plottableIndex) {
		if (string.IsNullOrWhiteSpace(newLabel))
			throw new NullReferenceException(Message.EXCEPTION_NO_PLOT_LABEL_SPECIFIED);

		var plottables = GetPlottablesAs<ScatterPlot>();

		foreach (var plotty in plottables) {
			if (plotty.IsNullOrEmpty())
				continue;

			foreach (var index in plottableIndex) {
				if (index > plotty.Count)
					continue;

				var scatter = plotty[index];
				scatter.Label = newLabel;
			}
		}

		RefreshRenderers();

		return this;
	}

	/// <inheritdoc />
    public IPlotBuilderFluentPostProcess TrySetLabel(string newLabel, params int[] plottableIndices) {
		if (string.IsNullOrWhiteSpace(newLabel))
			throw new NullReferenceException(Message.EXCEPTION_NO_PLOT_LABEL_SPECIFIED);

		if (PlotType == typeof(ScatterPlot)) {
			var plottables = GetPlottablesAs<ScatterPlot>();
			StaticLabelSetter.SetScatterPlotLabels(ref newLabel, ref plottables, ref plottableIndices);
		}
		else if (PlotType == typeof(SignalPlot)) {
			var plottables = GetPlottablesAs<SignalPlotXYConst<double, double>>();
			StaticLabelSetter.SetSignalPlotLabels(ref newLabel, ref plottables, ref plottableIndices);
		}

		RefreshRenderers();

		return this;
	}

	/// <inheritdoc />
    public IPlotBuilderFluentPostProcess TrySetLabelAll (string newLabel) {
        if (string.IsNullOrWhiteSpace(newLabel))
            throw new NullReferenceException(Message.EXCEPTION_NO_PLOT_LABEL_SPECIFIED);
		
		var indices = GetIndices();
		TrySetLabel(newLabel, indices.ToArray());

        return this;
    }

	/// <inheritdoc />
    public IPlotBuilderFluentPostProcess TrySetScatterLabelAll(string newLabel) {
		var indices = GetIndices();

		return TrySetScatterLabel(newLabel, indices);
	}

	/// <inheritdoc />	
	public IPlotBuilderFluentPostProcess RefreshRenderers() {
		foreach (var plot in _plots) {
			plot.Render();
		}

		return this;
	}

	/// <summary>
	///  Gets the indices of the plottables. A helper method for TrySetScatterLabelAll.
	/// </summary>
	/// <returns>Integer array containing indices of each plot, sequential, ascending</returns>
	int[] GetIndices() {
		var indices = new int[_data.Count];

		for (var i = 0; i < indices.Length; i++) {
			indices[i] = i;
		}

		return indices;
	}
}