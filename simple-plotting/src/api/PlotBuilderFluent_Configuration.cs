// simple-plotting

using System.Drawing;
using ScottPlot;

namespace simple_plotting;

public partial class PlotBuilderFluent {
	/// <inheritdoc />
	public IPlotBuilderFluentConfiguration WithSize(PlotSize plotSize) {
		var container = PlotSizeMapper.Map(plotSize);

		foreach (var plot in _plots) {
			plot.Width  = container.Width;
			plot.Height = container.Height;
		}

		return this;
	}

	/// <inheritdoc />
	public IPlotBuilderFluentConfiguration WithTitle(string title, int fontSize, bool isBold = false) {
		if (string.IsNullOrWhiteSpace(title))
			throw new Exception(Message.EXCEPTION_TITLE_INVALID);

		var plotTracker = 1;
		foreach (var plot in _plots) {
			plot.Title($"{title} - #{plotTracker}", size: fontSize, bold: isBold);
			plotTracker++;
		}

		return this;
	}

	/// <inheritdoc />
	public IPlotBuilderFluentConfiguration WithPrimaryXAxisLabel(string labelTxt, Color fontColor, int fontSize,
		bool isBold = false) {
		if (string.IsNullOrWhiteSpace(labelTxt))
			throw new Exception(Message.EXCEPTION_AXIS_LABEL_INVALID);

		foreach (var plot in _plots) {
			plot.XAxis.Label(labelTxt, color: fontColor, size: fontSize, bold: isBold);
		}

		return this;
	}

	/// <inheritdoc />
	public IPlotBuilderFluentConfiguration WithPrimaryXAxisLabel(string labelTxt, int fontSize = 14,
		bool isBold = false) {
		if (string.IsNullOrWhiteSpace(labelTxt))
			throw new Exception(Message.EXCEPTION_AXIS_LABEL_INVALID);

		foreach (var plot in _plots) {
			plot.XAxis.Label(labelTxt, size: fontSize, bold: isBold);
		}

		return this;
	}

	/// <inheritdoc />
	public IPlotBuilderFluentConfiguration WithPrimaryYAxisLabel(string labelTxt, Color fontColor, int fontSize,
		bool isBold = false) {
		if (string.IsNullOrWhiteSpace(labelTxt))
			throw new Exception(Message.EXCEPTION_AXIS_LABEL_INVALID);

		foreach (var plot in _plots) {
			plot.YAxis.Label(labelTxt, color: fontColor, size: fontSize, bold: isBold);
		}

		return this;
	}

	/// <inheritdoc />
	public IPlotBuilderFluentConfiguration WithPrimaryYAxisLabel(string labelTxt, int fontSize = 14,
		bool isBold = false) {
		if (string.IsNullOrWhiteSpace(labelTxt))
			throw new Exception(Message.EXCEPTION_AXIS_LABEL_INVALID);

		foreach (var plot in _plots) {
			plot.YAxis.Label(labelTxt, size: fontSize, bold: isBold);
		}

		return this;
	}

	/// <inheritdoc />
	public IPlotBuilderFluentConfiguration RotatePrimaryYAxisTicks(PlotAxisRotation rotation) {
		foreach (var plot in _plots) {
			plot.YAxis.TickLabelStyle(rotation: PlotAxisRotationMapper.Map(rotation));
		}

		return this;
	}

	/// <inheritdoc />
	public IPlotBuilderFluentConfiguration RotatePrimaryXAxisTicks(PlotAxisRotation rotation) {
		foreach (var plot in _plots) {
			plot.XAxis.TickLabelStyle(rotation: PlotAxisRotationMapper.Map(rotation));
		}

		return this;
	}

	/// <inheritdoc />
	public IPlotBuilderFluentConfiguration WithSecondaryXAxisLabel(string xAxisLabel, Color fontColor,
		int fontSize = 14, bool isBold = false) {
		if (string.IsNullOrWhiteSpace(xAxisLabel))
			throw new Exception(Message.EXCEPTION_AXIS_LABEL_INVALID);

		foreach (var plot in _plots) {
			plot.XAxis2.Label(xAxisLabel, color: fontColor, size: fontSize, bold: isBold);
		}

		return this;
	}

	/// <inheritdoc />
	public IPlotBuilderFluentConfiguration WithSecondaryYAxisLabel(string yAxisLabel, Color fontColor,
		int fontSize = 14, bool isBold = false) {
		if (string.IsNullOrWhiteSpace(yAxisLabel))
			throw new Exception(Message.EXCEPTION_AXIS_LABEL_INVALID);

		foreach (var plot in _plots) {
			plot.YAxis2.Label(yAxisLabel, color: fontColor, size: fontSize, bold: isBold);
		}

		return this;
	}

	/// <inheritdoc />
	public IPlotBuilderFluentConfiguration WithSecondaryXAxisLabel(string xAxisLabel, int fontSize = 14,
		bool isBold = false) {
		if (string.IsNullOrWhiteSpace(xAxisLabel))
			throw new Exception(Message.EXCEPTION_AXIS_LABEL_INVALID);

		foreach (var plot in _plots) {
			plot.XAxis2.Label(xAxisLabel, size: fontSize, bold: isBold);
		}

		return this;
	}

	/// <inheritdoc />
	public IPlotBuilderFluentConfiguration DisableSecondXAxis() {
		foreach (var plot in _plots) {
			plot.XAxis2.IsVisible = false;
		}

		return this;
	}

	/// <inheritdoc />
	public IPlotBuilderFluentConfiguration EnableSecondXAxis() {
		foreach (var plot in _plots) {
			plot.XAxis2.IsVisible = true;
		}

		return this;
	}

	/// <inheritdoc />
	public IPlotBuilderFluentConfiguration DisableSecondYAxis(bool keepVisible = true) {
		foreach (var plot in _plots) {
			plot.YAxis2.Label(" ");
			plot.YAxis2.IsVisible = keepVisible;
		}

		return this;
	}

	/// <inheritdoc />
	public IPlotBuilderFluentConfiguration SetXAxisTicks(PlotAxisRotation rotation, bool isBold = false, int fontSize
		= 14) {
		var rotationInt = PlotAxisRotationMapper.Map(rotation);

		foreach (var plot in _plots) {
			plot.XAxis.TickLabelStyle(rotation: rotationInt, fontBold: isBold, fontSize: fontSize);
		}

		return this;
	}

	/// <inheritdoc />
	public IPlotBuilderFluentConfiguration SetYAxisTicks(PlotAxisRotation rotation, bool isBold = false,
		int fontSize = 14) {
		var rotationInt = PlotAxisRotationMapper.Map(rotation);

		foreach (var plot in _plots) {
			plot.YAxis.TickLabelStyle(rotation: rotationInt, fontBold: isBold, fontSize: fontSize);
		}

		return this;
	}

	/// <inheritdoc />
	public IPlotBuilderFluentConfiguration SetSecondaryYAxisTicks(PlotAxisRotation rotation, bool isBold = false, int
		fontSize = 14) {
		var rotationInt = PlotAxisRotationMapper.Map(rotation);

		foreach (var plot in _plots) {
			plot.YAxis2.TickLabelStyle(rotation: rotationInt, fontBold: isBold, fontSize: fontSize);
		}

		return this;
	}

	/// <inheritdoc />
	public IPlotBuilderFluentConfiguration SetSecondaryXAxisTicks(PlotAxisRotation rotation, bool isBold = false, int
		fontSize = 14) {
		var rotationInt = PlotAxisRotationMapper.Map(rotation);

		foreach (var plot in _plots) {
			plot.XAxis2.TickLabelStyle(rotation: rotationInt, fontBold: isBold, fontSize: fontSize);
		}

		return this;
	}

	/// <inheritdoc />
	public IPlotBuilderFluentConfiguration SetLayout(
		float? left = 50f
		, float? right = 50f
		, float? top = 50f
		, float? bottom = 50f
		, float? padding = 0f) {
		foreach (var p in _plots) {
			p.Layout(left, right, bottom, top, padding);
		}

		return this;
	}

	/// <inheritdoc />
	public IPlotBuilderFluentConfiguration EnableSecondYAxis() {
		foreach (var plot in _plots) {
			plot.YAxis2.IsVisible = true;
		}

		return this;
	}

	/// <inheritdoc />
	public IPlotBuilderFluentConfiguration WithSecondaryYAxisLabel(string yAxisLabel, int fontSize = 14,
		bool isBold = false) {
		if (string.IsNullOrWhiteSpace(yAxisLabel))
			throw new Exception(Message.EXCEPTION_AXIS_LABEL_INVALID);

		foreach (var plot in _plots) {
			plot.YAxis2.Label(yAxisLabel, size: fontSize, bold: isBold);
		}

		return this;
	}

	/// <inheritdoc />
	public IPlotBuilderFluentConfiguration RotateSecondaryYAxisTicks(PlotAxisRotationContainer container) {
		foreach (var plot in _plots) {
			plot.YAxis2.TickLabelStyle(rotation: container.Rotation);
		}

		return this;
	}

	/// <inheritdoc />
	public IPlotBuilderFluentConfiguration RotateSecondaryXAxisTicks(PlotAxisRotationContainer container) {
		foreach (var plot in _plots) {
			plot.XAxis2.TickLabelStyle(rotation: container.Rotation);
		}

		return this;
	}

	/// <inheritdoc />
	public IPlotBuilderFluentConfiguration ShowLegend(PlotAlignment alignment) {
		foreach (var plot in _plots) {
			plot.Legend(true, PlotAlignmentMapper.Map(alignment));
		}

		return this;
	}

	/// <inheritdoc />
	public IPlotBuilderFluentConfiguration ShowLegend(PlotAlignment alignment, Orientation orientation) {
		foreach (var plot in _plots) {
			var l = plot.Legend(true, PlotAlignmentMapper.Map(alignment));
			l.Orientation = orientation;
		}

		return this;
	}

	/// <inheritdoc />
	public IPlotBuilderFluentConfiguration ShowLegendOutSideTopRight() {
		foreach (var p in _plots) {
			Bitmap bmpPlot   = p.GetBitmap();
			Bitmap bmpLegend = p.RenderLegend();

			Bitmap         bmp = new Bitmap(bmpPlot.Width + bmpLegend.Width, bmpPlot.Height);
			using Graphics gfx = Graphics.FromImage(bmp);
			gfx.Clear(Color.White);
			gfx.DrawImage(bmpPlot,   0,             0);
			gfx.DrawImage(bmpLegend, bmpPlot.Width, (bmp.Height - bmpLegend.Height) / 2);

			p.Render();
		}

		return this;
	}

	/// <inheritdoc />
	public IPlotBuilderFluentConfiguration HideLegend() {
		foreach (var plot in _plots) {
			plot.Legend(false);
		}

		return this;
	}

	/// <inheritdoc />
	public IPlotBuilderFluentConfiguration SetDataPadding(double valueX = 0.05, double valueY = 0.05) {
		if (valueX < 0 || valueY < 0 || valueX > 1 || valueY > 1)
			throw new Exception(Message.EXCEPTION_MARGIN_NOT_BETWEEN_ZERO_AND_ONE);

		foreach (var plot in _plots) {
			plot.Margins(valueX, valueY);
		}

		return this;
	}


	public IPlotBuilderFluentConfiguration SetDataPadding(int chCnt, double valueX = 0.05, double valueY = 0.05) {
		if (chCnt <= 0)
			throw new Exception(Message.EXCEPTION_CHANNEL_COUNT_ZERO_OR_NEG);

		var tracker = 1;
		var yMargin = 0.0d;
		var xMargin = 0.0d;

		do {
			yMargin += valueY;
			xMargin += valueX;
			tracker++;
		} while (tracker <= chCnt);

		xMargin = Math.Clamp(xMargin, MIN_MARGIN, MAX_MARGIN);
		yMargin = Math.Clamp(yMargin, MIN_MARGIN, MAX_MARGIN);

		SetDataPadding(xMargin, yMargin);

		return this;
	}

	/// <inheritdoc />
	public IPlotBuilderFluentConfiguration SetDataPaddingWrtChannels(double valueX = 0.05, double valueY = 0.05) {
		SetDataPadding(_data.Count, valueX, valueY);

		return this;
	}

	/// <inheritdoc />
	public IPlotBuilderFluentConfiguration SetDataLayout(float right, float top, float bottom, float left) {
		foreach (var plot in _plots) {
			plot.Layout(left, right, bottom, top);
		}

		return this;
	}

	/// <inheritdoc />
	public IPlotBuilderFluentConfiguration DefineSource(IPlotChannelProvider source) {
		if (string.IsNullOrWhiteSpace(source.Path))
			throw new Exception(Message.EXCEPTION_SAVE_PATH_INVALID);

		SourcePath = source.Path;
		return this;
	}

	/// <inheritdoc />
	public IPlotBuilderFluentConfiguration DefineSource(string path) {
		if (string.IsNullOrWhiteSpace(path))
			throw new Exception(Message.EXCEPTION_SAVE_PATH_INVALID);

		SourcePath = path;
		return this;
	}

	/// <inheritdoc />
	public IPlotBuilderFluentConfiguration SetPlotLimits() {
		var calc   = new PlotLimitCalculator();
		var limits = calc.Calculate(_data);

		foreach (var plot in _plots) {
			plot.SetAxisLimitsY(limits.Lower, limits.Upper);
		}

		return this;
	}

	/// <inheritdoc />
	public IPlotBuilderFluentConfiguration SetPlotLimits(double? upper, double? lower) {
		var calc   = new PlotLimitCalculator(upper, lower);
		var limits = calc.Calculate(_data);

		foreach (var plot in _plots) {
			plot.SetAxisLimitsY(limits.Lower, limits.Upper);
		}

		return this;
	}

	/// <inheritdoc cref="IPlotBuilderFluentConfiguration.FinalizeConfiguration" />
	public IPlotBuilderFluentReadyToProduce FinalizeConfiguration() => this;
	
	/// <inheritdoc />
	public IPlotBuilderFluentConfiguration WithObservable(Action action) {
		Observables.Add(action);
		return this;
	}

	/// <inheritdoc />
	public IPlotBuilderFluentConfiguration SetLegendFont(float fontSize = 14f, bool isBold = false) {
		foreach (var plot in _plots) {
			plot.Legend().Font.Bold = isBold;
			plot.Legend().Font.Size = fontSize;
		}

		return this;
	}

	const  double                          MAX_MARGIN = 0.99;
	const  double                          MIN_MARGIN = 0.01;
}