// simple-plotting

using System.Drawing;

namespace simple_plotting.src;

public partial class PlotBuilderFluent {
	/// <summary>
	/// Sets the size of the plot window.
	/// </summary>
	/// <param name="plotSize">Container that defines width & height</param>
	/// <returns>PlotBuilderFluent (this)</returns>
	public IPlotBuilderFluentConfiguration WithSize(PlotSize plotSize) {
		var container = PlotSizeMapper.Map(plotSize);

		foreach (var plot in _plots) {
			plot.Width  = container.Width;
			plot.Height = container.Height;
		}

		return this;
	}

	/// <summary>
	///  Sets the title of the plot.
	/// </summary>
	/// <param name="title">String title for plot</param>
	/// <param name="fontSize">Size of font</param>
	/// <returns>Fluent builder</returns>
	/// <exception cref="Exception">Thrown if string is null or whitespace</exception>
	public IPlotBuilderFluentConfiguration WithTitle(string title, int fontSize) {
		if (string.IsNullOrWhiteSpace(title))
			throw new Exception(Message.EXCEPTION_TITLE_INVALID);

		var plotTracker = 1;
		foreach (var plot in _plots) {
			plot.Title($"{title} - #{plotTracker}", size: fontSize);
			plotTracker++;
		}

		return this;
	}

	/// <summary>
	///  Sets the label for the X axis.
	/// </summary>
	/// <param name="labelTxt">String value for the X axis label</param>
	/// <param name="fontColor">Color of font</param>
	/// <param name="fontSize">Size of font</param>
	/// <returns>Fluent builder</returns>
	/// <exception cref="Exception">Thrown if xAxisLabel is null or whitespace</exception>
	public IPlotBuilderFluentConfiguration WithPrimaryXAxisLabel(string labelTxt, Color fontColor, int fontSize) {
		if (string.IsNullOrWhiteSpace(labelTxt))
			throw new Exception(Message.EXCEPTION_AXIS_LABEL_INVALID);

		foreach (var plot in _plots) {
			plot.XAxis.Label(labelTxt, color: fontColor, size: fontSize);
		}

		return this;
	}

	/// <summary>
	/// Sets the label for the X axis.
	/// </summary>
	/// <param name="labelTxt">String value for the X axis label</param>
	/// <param name="fontSize">Size of font</param>
	/// <returns>Fluent builder</returns>
	public IPlotBuilderFluentConfiguration WithPrimaryXAxisLabel(string labelTxt, int fontSize = 14) {
		if (string.IsNullOrWhiteSpace(labelTxt))
			throw new Exception(Message.EXCEPTION_AXIS_LABEL_INVALID);

		foreach (var plot in _plots) {
			plot.XAxis.Label(labelTxt, size: fontSize);
		}

		return this;
	}

	/// <summary>
	///  Sets the label for the Y axis.
	/// </summary>
	/// <param name="labelTxt">String value for the Y axis label</param>
	/// <param name="fontColor">Color of font</param>
	/// <param name="fontSize">Size of font</param>
	/// <returns>Fluent builder</returns>
	/// <exception cref="Exception">Thrown if yAxisLabel is null or whitespace</exception>
	public IPlotBuilderFluentConfiguration WithPrimaryYAxisLabel(string labelTxt, Color fontColor, int fontSize) {
		if (string.IsNullOrWhiteSpace(labelTxt))
			throw new Exception(Message.EXCEPTION_AXIS_LABEL_INVALID);

		foreach (var plot in _plots) {
			plot.YAxis.Label(labelTxt, color: fontColor, size: fontSize);
		}

		return this;
	}

	/// <summary>
	/// Sets the label for the Y axis.
	/// </summary>
	/// <param name="labelTxt">String value for the Y axis label</param>
	/// <param name="fontSize">Size of font</param>
	/// <returns>Fluent builder</returns>
	public IPlotBuilderFluentConfiguration WithPrimaryYAxisLabel(string labelTxt, int fontSize = 14) {
		if (string.IsNullOrWhiteSpace(labelTxt))
			throw new Exception(Message.EXCEPTION_AXIS_LABEL_INVALID);

		foreach (var plot in _plots) {
			plot.YAxis.Label(labelTxt, size: fontSize);
		}

		return this;
	}

	/// <summary>
	///  Sets the rotation of the primary Y axis ticks.
	/// </summary>
	/// <param name="rotation">Rotation of axis</param>
	/// <returns>Fluent builder</returns>
	public IPlotBuilderFluentConfiguration RotatePrimaryYAxisTicks(PlotAxisRotation rotation) {
		foreach (var plot in _plots) {
			plot.YAxis.TickLabelStyle(rotation: PlotAxisRotationMapper.Map(rotation));
		}

		return this;
	}

	/// <summary>
	///  Sets the rotation of the primary X axis ticks.
	/// </summary>
	/// <param name="rotation">Rotation of axis</param>
	/// <returns>Fluent builder</returns>
	public IPlotBuilderFluentConfiguration RotatePrimaryXAxisTicks(PlotAxisRotation rotation) {
		foreach (var plot in _plots) {
			plot.XAxis.TickLabelStyle(rotation: PlotAxisRotationMapper.Map(rotation));
		}

		return this;
	}
	
	/// <summary>
	///  Sets the label for the secondary X axis.
	/// </summary>
	/// <param name="xAxisLabel">Label for the secondary x-axis (top side of plot)</param>
	/// <param name="fontSize">Size of font</param>
	/// <param name="fontColor">Color of font</param>
	/// <returns>Fluent builder</returns>
	/// <exception cref="Exception">Thrown if the label is null or whitespace</exception>
	public IPlotBuilderFluentConfiguration WithSecondaryXAxisLabel(string xAxisLabel, Color fontColor, int fontSize = 14) {
		if (string.IsNullOrWhiteSpace(xAxisLabel))
			throw new Exception(Message.EXCEPTION_AXIS_LABEL_INVALID);

		foreach (var plot in _plots) {
			plot.XAxis2.Label(xAxisLabel, color: fontColor, size: fontSize);
		}

		return this;
	}
	
	/// <summary>
	///  Sets the label for the secondary Y axis.
	/// </summary>
	/// <param name="yAxisLabel">Label for the secondary x-axis (right side of plot)</param>
	/// <param name="fontColor">Color of font</param>
	/// <param name="fontSize">Size of font</param>
	/// <returns>Fluent builder</returns>
	/// <exception cref="Exception">Thrown if the label is null or whitespace</exception>
	public IPlotBuilderFluentConfiguration WithSecondaryYAxisLabel(string yAxisLabel, Color fontColor, int fontSize = 14) {
		if (string.IsNullOrWhiteSpace(yAxisLabel))
			throw new Exception(Message.EXCEPTION_AXIS_LABEL_INVALID);

		foreach (var plot in _plots) {
			plot.YAxis2.Label(yAxisLabel, color: fontColor, size: fontSize);
		}

		return this;
	}

	/// <summary>
	///  Sets the label for the secondary X axis.
	/// </summary>
	/// <param name="xAxisLabel">Label for the secondary x-axis (top side of plot)</param>
	/// <param name="fontSize">Size of font</param>
	/// <returns>Fluent builder</returns>
	/// <exception cref="Exception">Thrown if the label is null or whitespace</exception>
	public IPlotBuilderFluentConfiguration WithSecondaryXAxisLabel(string xAxisLabel, int fontSize = 14) {
		if (string.IsNullOrWhiteSpace(xAxisLabel))
			throw new Exception(Message.EXCEPTION_AXIS_LABEL_INVALID);

		foreach (var plot in _plots) {
			plot.XAxis2.Label(xAxisLabel, size: fontSize);
		}

		return this;
	}

	/// <summary>
	/// Disables the second x-axis of each plot
	/// </summary>
	/// <returns>Fluent builder</returns>
	public IPlotBuilderFluentConfiguration DisableSecondXAxis() {
		foreach (var plot in _plots) {
			plot.XAxis2.IsVisible = false;
		}

		return this;
	}

	/// <summary>
	///  Enables the second x-axis of each plot
	/// </summary>
	/// <returns>Fluent builder</returns>
	public IPlotBuilderFluentConfiguration EnableSecondXAxis() {
		foreach (var plot in _plots) {
			plot.XAxis2.IsVisible = true;
		}

		return this;
	}

	/// <summary>
	/// Disables the second y-axis of each plot
	/// </summary>
	/// <returns>Fluent builder</returns>
	public IPlotBuilderFluentConfiguration DisableSecondYAxis() {
		foreach (var plot in _plots) {
			plot.YAxis2.IsVisible = false;
		}

		return this;
	}

	/// <summary>
	///  Rotates the tick labels on the X axis
	/// </summary>
	/// <param name="rotation">Rotation amount (counter-clockwise</param>
	/// <param name="isBold">Flag to bold tick labels</param>
	/// <returns>Fluent builder</returns>
	public IPlotBuilderFluentConfiguration SetXAxisTicks(PlotAxisRotation rotation, bool isBold = false) {
		var rotationInt = PlotAxisRotationMapper.Map(rotation);
		
		foreach (var plot in _plots) {
			plot.XAxis.TickLabelStyle(rotation: rotationInt, fontBold: isBold);
		}

		return this;
	}

	/// <summary>
	///  Rotates the tick labels on the Y axis
	/// </summary>
	/// <param name="rotation">Rotation amount (counter-clockwise</param>
	/// <param name="isBold">Flag to bold tick labels</param>
	/// <returns>Fluent builder</returns>
	public IPlotBuilderFluentConfiguration SetYAxisTicks(PlotAxisRotation rotation, bool isBold = false) {
		var rotationInt = PlotAxisRotationMapper.Map(rotation);
		
		foreach (var plot in _plots) {
			plot.YAxis.TickLabelStyle(rotation: rotationInt, fontBold: isBold);
		}

		return this;
	}
	
	/// <summary>
	///  Rotates the tick labels on the secondary Y axis
	/// </summary>
	/// <param name="rotation">Rotation amount (counter-clockwise</param>
	/// <param name="isBold">Flag to bold tick labels</param>
	/// <returns>Fluent builder</returns>
	public IPlotBuilderFluentConfiguration SetSecondaryYAxisTicks(PlotAxisRotation rotation, bool isBold = false) {
		var rotationInt = PlotAxisRotationMapper.Map(rotation);
		
		foreach (var plot in _plots) {
			plot.YAxis2.TickLabelStyle(rotation: rotationInt, fontBold: isBold);
		}

		return this;
	}
	
	/// <summary>
	///  Rotates the tick labels on the secondary X axis
	/// </summary>
	/// <param name="rotation">Rotation amount (counter-clockwise</param>
	/// <param name="isBold">Flag to bold tick labels</param>
	/// <returns>Fluent builder</returns>
	public IPlotBuilderFluentConfiguration SetSecondaryXAxisTicks(PlotAxisRotation rotation, bool isBold = false) {
		var rotationInt = PlotAxisRotationMapper.Map(rotation);
		
		foreach (var plot in _plots) {
			plot.XAxis2.TickLabelStyle(rotation: rotationInt, fontBold: isBold);
		}

		return this;
	}

	/// <summary>
	///  Enables the second y-axis of each plot
	/// </summary>
	/// <returns>Fluent builder</returns>
	public IPlotBuilderFluentConfiguration EnableSecondYAxis() {
		foreach (var plot in _plots) {
			plot.YAxis2.IsVisible = true;
		}

		return this;
	}

	/// <summary>
	///  Sets the label for the secondary Y axis.
	/// </summary>
	/// <param name="yAxisLabel">Label for the secondary x-axis (right side of plot)</param>
	/// <param name="fontSize">Size of font</param>
	/// <returns>Fluent builder</returns>
	/// <exception cref="Exception">Thrown if the label is null or whitespace</exception>
	public IPlotBuilderFluentConfiguration WithSecondaryYAxisLabel(string yAxisLabel, int fontSize =14) {
		if (string.IsNullOrWhiteSpace(yAxisLabel))
			throw new Exception(Message.EXCEPTION_AXIS_LABEL_INVALID);

		foreach (var plot in _plots) {
			plot.YAxis2.Label(yAxisLabel, size: fontSize);
		}

		return this;
	}

	/// <summary>
	///  Sets the rotation of the secondary Y axis ticks.
	/// </summary>
	/// <param name="container">Container for rotation of axis</param>
	/// <returns>Fluent builder</returns>
	public IPlotBuilderFluentConfiguration RotateSecondaryYAxisTicks(PlotAxisRotationContainer container) {
		foreach (var plot in _plots) {
			plot.YAxis2.TickLabelStyle(rotation: container.Rotation);
		}

		return this;
	}

	/// <summary>
	///  Sets the rotation of the secondary X axis ticks.
	/// </summary>
	/// <param name="container">Container for rotation of axis</param>
	/// <returns>Fluent builder</returns>
	public IPlotBuilderFluentConfiguration RotateSecondaryXAxisTicks(PlotAxisRotationContainer container) {
		foreach (var plot in _plots) {
			plot.XAxis2.TickLabelStyle(rotation: container.Rotation);
		}

		return this;
	}

	/// <summary>
	///  Sets whether or not the legend should be shown and the alignment of the legend.
	/// </summary>
	/// <param name="alignment">Where on the plot to display the legend</param>
	/// <returns>Fluent builder</returns>
	public IPlotBuilderFluentConfiguration ShowLegend(PlotAlignment alignment) {
		foreach (var plot in _plots) {
			plot.Legend(true, PlotAlignmentMapper.Map(alignment));
		}

		return this;
	}

	/// <summary>
	///  Hides the legend.
	/// </summary>
	/// <returns>Fluent builder</returns>
	public IPlotBuilderFluentConfiguration HideLegend() {
		foreach (var plot in _plots) {
			plot.Legend(false);
		}

		return this;
	}

	/// <summary>
	///  Sets the margins of the plot. This affects the actual data area of the plot.
	/// </summary>
	/// <param name="percentX">Double value for x-axis</param>
	/// <param name="percentY">Double value for y-axis</param>
	/// <returns>Fluent builder</returns>
	public IPlotBuilderFluentConfiguration SetDataPadding(double percentX = 1.0, double percentY = 1.0) {
		foreach (var plot in _plots) {
			plot.Margins(percentX, percentY);
		}

		return this;
	}

	/// <summary>
	///  Sets the margins of the plot. This affects the actual data area of the plot.
	///  This overload takes four parameters for each side of the plot
	/// </summary>
	/// <param name="right">Float-right padding</param>
	/// <param name="top">Float-top padding</param>
	/// <param name="bottom">Float-bottom padding</param>
	/// <param name="left">Float-left padding</param>
	/// <returns>Fluent builder</returns>
	public IPlotBuilderFluentConfiguration SetDataPadding(float right, float top, float bottom, float left) {
		foreach (var plot in _plots) {
			plot.Layout(left, right, bottom, top);
		}

		return this;
	}

	/// <summary>
	///  Sets the SourcePath property to Source.Path. This is used to save the plot(s) to the same directory as the source.
	/// </summary>
	/// <param name="source">IPlotChannelProviderSource, typically derivation of CsvParser.Path</param>
	/// <returns>Fluent builder</returns>
	public IPlotBuilderFluentConfiguration DefineSource(IPlotChannelProvider source) {
		if (string.IsNullOrWhiteSpace(source.Path))
			throw new Exception(Message.EXCEPTION_SAVE_PATH_INVALID);

		SourcePath = source.Path;
		return this;
	}

	/// <summary>
	///  Sets the SourcePath property to the specified path. This is used to save the plot(s) to a specific directory.
	/// </summary>
	/// <param name="path">Standalone file path to try-save plots at</param>
	/// <returns>Fluent builder</returns>
	public IPlotBuilderFluentConfiguration DefineSource(string path) {
		if (string.IsNullOrWhiteSpace(path))
			throw new Exception(Message.EXCEPTION_SAVE_PATH_INVALID);

		SourcePath = path;
		return this;
	}

	/// <summary>
	///  Determines the upper and lower y-axis limits
	///  </summary>	
	///  <returns>Fluent builder</returns>
	public IPlotBuilderFluentConfiguration SetPlotLimits() {
		var calc   = new PlotLimitCalculator();
		var limits = calc.Calculate(_data);

		foreach (var plot in _plots) {
			plot.SetAxisLimitsY(limits.Lower, limits.Upper);
		}

		return this;
	}

	/// <summary>
	///  Determines the upper and lower y-axis limits
	///  This overload takes into account default upper and lower values
	///  </summary>
	///  <param name="lower">Nullable lower limit; calculator will not go above this value (if not null)</param>
	///  <param name="upper">Nullable upper limit; calculator will not go below this value (if not null)</param>
	public IPlotBuilderFluentConfiguration SetPlotLimits(double? upper, double? lower) {
		var calc   = new PlotLimitCalculator(upper, lower);
		var limits = calc.Calculate(_data);

		foreach (var plot in _plots) {
			plot.SetAxisLimitsY(limits.Lower, limits.Upper);
		}

		return this;
	}

	/// <summary>
	///  Finalizes the configuration of the plot. This should be the last call in the fluent API.
	///  This allows you to call Produce().
	/// </summary>
	/// <returns>Fluent interface allowing consumer to Produce()</returns>
	public IPlotBuilderFluentReadyToProduce FinalizeConfiguration() => this;

	/// <summary>
	///  Register an action to be invoked when the plot is produced.
	/// </summary>
	/// <param name="action">Callback</param>
	/// <returns>Fluent builder</returns>
	public IPlotBuilderFluentConfiguration WithObservable(Action action) {
		Observables.Add(action);
		return this;
	}
}