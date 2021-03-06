﻿// ================================================================================
//
// Atmo 2
// Copyright (C) 2011  BARANI DESIGN
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
// 
// Contact: Jan Barani mailto:jan@baranidesign.com
//
// ================================================================================

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Atmo.Data;
using Atmo.Stats;
using Atmo.Units;
using DevExpress.XtraCharts;

namespace Atmo.UI.DevEx.Controls {
	public partial class HistoricalGraphBreakdown : DevExpress.XtraEditors.XtraUserControl {
		public HistoricalGraphBreakdown() {
			InitializeComponent();
		}

		public event Action OnSelectedPropertyChanged;

		private List<ChartControl> _cumulativeCharts = new List<ChartControl>();

		public ReadingValuesConverterCache<ReadingValues> ConverterCacheReadingValues { get; set; }

		public TemperatureUnit TemperatureUnit { get; set; }

		public SpeedUnit SpeedUnit { get; set; }

		public PressureUnit PressureUnit { get; set; }

		public PersistentState State { get; set; }

		public void SetDataSource<T>(IEnumerable<T> items) where T : IReadingsSummary {
			SetDataSource(items is List<T> ? items as List<T> : new List<T>(items));
		}

		public void SetDataSource<T>(List<T> items) where T : IReadingsSummary {
			List<ReadingsSummary> readings;
			if (typeof(T) == typeof(ReadingsSummary)) {
				readings = items as List<ReadingsSummary>;
			}
			else {
				readings = new List<ReadingsSummary>(items.Count);
				for (int i = 0; i < items.Count; i++) {
					readings.Add(new ReadingsSummary(items[i]));
				}
			}
			SetDataSource(readings);
		}

		public class CumulativeTimeInfo : List<CumulativeWindow> {

			//public TimeSpan unitSpan = OneDay;
			public TimeUnit unit = TimeUnit.Day;

			public DateTime MinStamp {
				get {
					return this.Min(w => w.min);
				}
			}

			public DateTime MaxStamp {
				get {
					return this.Max(w => w.max);
				}
			}

		}

		public class CumulativeWindow {
			public DateTime min;
			public DateTime max;
			public string name;

			public CumulativeWindow(DateTime min, DateTime max) {
				this.min = min;
				this.max = max;
			}

			public string Name {
				get {
					return name ?? min.ToString();
				}
				set {
					name = value;
				}
			}

			public TimeSpan Span {
				get {
					return max.Subtract(min);
				}
			}
		}

		private static readonly TimeSpan FifteenMinutes = new TimeSpan(0, 15, 0);
		private static readonly TimeSpan TwoHour = new TimeSpan(2, 0, 0);
		private static readonly TimeSpan OneDay = new TimeSpan(1, 0, 0, 0);
		private static readonly TimeSpan OneWeek = new TimeSpan(7, 0, 0, 0);

		public static CumulativeTimeInfo GetCumulativeWindows(DateTime startValue, TimeSpan span, bool stepBack) {


			if (span == new TimeSpan(3, 0, 0)) {
				// 3 hours
				// 12 - 15 minute chunks
				// align to 15 minute period
				//if (!drillNowChk.Checked) {
				//    startValue = startValue.Subtract(FifteenMinutes).AddTicks(1);
				//}
				if (stepBack) {
					startValue = startValue.Subtract(FifteenMinutes);
				}
				int currentHourQuarter = Math.Min(startValue.Minute / 15, 3);
				DateTime currentBaseTime = startValue.Date.Add(new TimeSpan(startValue.Hour, currentHourQuarter * 15, 0));
				CumulativeTimeInfo result = new CumulativeTimeInfo();

				//result.unitSpan = new TimeSpan(0, 1, 0);
				result.unit = TimeUnit.Minute;
				DateTime now = DateTime.Now;
				for (int i = 0; i < 12; i++) {
					DateTime currentCap = currentBaseTime.Add(FifteenMinutes).AddTicks(-1);
					CumulativeWindow window = new CumulativeWindow(currentBaseTime, currentCap);
					if (now.Date.Equals(currentBaseTime.Date)) {
						window.Name = currentBaseTime.ToString("HH:mm");
					}
					else {
						window.Name = currentBaseTime.ToString("MM/dd/yyyy HH:mm");
					}
					result.Add(window);
					currentBaseTime = currentBaseTime.Subtract(FifteenMinutes);
				}
				return result;
			}
			else if (span == new TimeSpan(1, 0, 0, 0)) {
				// 1 day
				// 12 - 2 hour chunks
				// align to hour
				//if (!drillNowChk.Checked) {
				//    startValue = startValue.Subtract(TwoHour).AddTicks(1);
				//}
				if (stepBack) {
					startValue = startValue.Subtract(TwoHour);
				}
				DateTime currentBaseTime = startValue.Date.Add(new TimeSpan(startValue.Hour, 0, 0));
				CumulativeTimeInfo result = new CumulativeTimeInfo();
				//result.unitSpan = new TimeSpan(0, 10, 0);
				result.unit = TimeUnit.Minute;
				DateTime now = DateTime.Now;
				for (int i = 0; i < 12; i++) {
					DateTime currentCap = currentBaseTime.Add(TwoHour).AddTicks(-1);
					CumulativeWindow window = new CumulativeWindow(currentBaseTime, currentCap);
					if (now.Date.Equals(currentBaseTime.Date)) {
						window.Name = currentBaseTime.ToString("HH:mm");
					}
					else {
						window.Name = currentBaseTime.ToString("MM/dd/yyyy HH:mm");
					}
					result.Add(window);
					currentBaseTime = currentBaseTime.Subtract(TwoHour);
				}
				return result;
			}
			else if (span == new TimeSpan(12, 0, 0, 0)) {
				// 12 days
				// 12 - 1 day chunks
				// align to day
				//if (!drillNowChk.Checked) {
				//    startValue = startValue.Subtract(OneDay).AddTicks(1);
				//}
				if (stepBack) {
					startValue = startValue.Subtract(OneDay);
				}
				DateTime currentBaseTime = startValue.Date;
				CumulativeTimeInfo result = new CumulativeTimeInfo();
				//result.unitSpan = new TimeSpan(1, 0, 0);
				result.unit = TimeUnit.Hour;
				for (int i = 0; i < 12; i++) {
					DateTime currentCap = currentBaseTime.Add(OneDay).AddTicks(-1);
					CumulativeWindow window = new CumulativeWindow(currentBaseTime, currentCap);
					window.Name = currentBaseTime.ToShortDateString();
					result.Add(window);
					currentBaseTime = currentBaseTime.Subtract(OneDay);
				}
				return result;
			}
			else if (span == new TimeSpan(84, 0, 0, 0)) {
				// 12 weeks
				// 12 - 7 day chunks
				// align to week number
				//if (!drillNowChk.Checked) {
				//    startValue = startValue.Subtract(OneWeek).AddTicks(1);
				//}
				if (stepBack) {
					startValue = startValue.Subtract(OneWeek);
				}
				int startDayOfWeek = (int)startValue.DayOfWeek;
				DateTime currentBaseTime = startValue.Date.Subtract(new TimeSpan(startDayOfWeek, 0, 0, 0));
				CumulativeTimeInfo result = new CumulativeTimeInfo();
				//result.unitSpan = new TimeSpan(1, 0, 0, 0);
				result.unit = TimeUnit.Day;
				for (int i = 0; i < 12; i++) {
					DateTime currentCap = currentBaseTime.Add(OneWeek).AddTicks(-1);
					CumulativeWindow window = new CumulativeWindow(currentBaseTime, currentCap);
					window.Name = "Week of " + currentBaseTime.ToShortDateString();
					result.Add(window);
					currentBaseTime = currentBaseTime.Subtract(OneWeek);
				}
				return result;
			}
			else if (span >= new TimeSpan(365, 0, 0, 0)) {
				// 1 year
				// 12 - 1 month chunks
				// align to month
				//if (!drillNowChk.Checked) {
				//    startValue = startValue.AddMonths(-1).AddTicks(1);
				//}
				if (stepBack) {
					if (startValue.Month == 1) {
						startValue = new DateTime(startValue.Year - 1, 12, 1);
					}
					else {
						startValue = new DateTime(startValue.Year, startValue.Month - 1, 1);
					}
				}
				DateTime currentBaseTime = new DateTime(startValue.Year, startValue.Month, 1);
				CumulativeTimeInfo result = new CumulativeTimeInfo();
				//result.unitSpan = new TimeSpan(1, 0, 0, 0);
				result.unit = TimeUnit.Day;
				for (int i = 0; i < 12; i++) {
					DateTime currentCap = currentBaseTime.AddMonths(1).AddTicks(-1);
					CumulativeWindow window = new CumulativeWindow(currentBaseTime, currentCap);
					window.Name = currentBaseTime.ToString("MMMM yyyy");
					result.Add(window);
					if (1 == currentBaseTime.Month) {
						currentBaseTime = new DateTime(currentBaseTime.Year - 1, 12, 1);
					}
					else {
						currentBaseTime = new DateTime(currentBaseTime.Year, currentBaseTime.Month - 1, 1);
					}
				}
				return result;
			}
			throw new NotSupportedException();
		}

		public DateTime DrillStartDate { get; set; }

		public TimeSpan CumulativeTimeSpan { get; set; }

		public bool StepBack { get; set; }

		public ReadingAttributeType SelectedAttributeType {
			get {
				switch(comboBoxEditSelProp.SelectedItem.ToString()) {
				case "Wind Speed": return ReadingAttributeType.WindSpeed;
				case "Temperature": return ReadingAttributeType.Temperature;
				case "Humidity": return ReadingAttributeType.Humidity;
				case "Pressure": return ReadingAttributeType.Pressure;
				case "Wind Direction": return ReadingAttributeType.WindDirection;
				default:
					throw new NotSupportedException(
						String.Format("Selection {0} is not supported.", comboBoxEditSelProp.SelectedItem)
					);
				}
			}
		}

		private void RecreateCumulativeGraphs() {

			var cumulativeChartCoverage = CumulativeTimeSpan;

			int numberGraphsToMake = 12;
			if (cumulativeChartCoverage == new TimeSpan(0, 0, 30)) {
				numberGraphsToMake = 3;
			}
			else if (
				cumulativeChartCoverage == new TimeSpan(0, 1, 0)
				|| cumulativeChartCoverage == new TimeSpan(1, 0, 0)
			) {
				numberGraphsToMake = 4;
			}
			else {
				numberGraphsToMake = 12;
			}

			TimeSpan timeSpanPerGraph = new TimeSpan(cumulativeChartCoverage.Ticks / numberGraphsToMake);
			var forAdd = new List<ChartControl>();
			while(_cumulativeCharts.Count < numberGraphsToMake) {
				var chart = new ChartControl();
				chart.Show();
				//chart.DataSource = bindingSourceReadingSummary;
				chart.Size = new Size(200, 200);
				chart.Dock = DockStyle.Fill;
				chart.Legend.Visible = false;
				chart.Titles.Add(new ChartTitle());
				var dataSeries = new Series{DataSource = bindingSourceReadingSummary};
				var stdDevSeries = new Series{DataSource = bindingSourceReading};
				chart.Series.Add(stdDevSeries);
				chart.Series.Add(dataSeries);

				var stdDevAxis = new SecondaryAxisY("StdDev");
				(chart.Diagram as XYDiagram).SecondaryAxesY.Add(stdDevAxis);
				_cumulativeCharts.Insert(0,chart); // these need to be reversed
				tableLayout.Controls.Add(chart);
				forAdd.Add(chart);
			}

			for (int i = 0; i < numberGraphsToMake; i++) {
				
				var chart = _cumulativeCharts[i];


				var series = chart.Series[1];
				series.ArgumentDataMember = "TimeStamp";
				series.ArgumentScaleType = ScaleType.DateTime;
				series.ValueDataMembersSerializable = SelectedAttributeType.ToString().Replace(" ", "");
				series.ValueScaleType = ScaleType.Numerical;
				var seriesView = new LineSeriesView{
					AxisYName = series.ValueDataMembersSerializable + " AxisY",
					Color = Color.Black,
					//LineTensionPercent = 50,
					PaneName = "DataPane"
				};
				seriesView.LineStyle.Thickness = 1;
				seriesView.LineStyle.DashStyle = DashStyle.Solid;
				seriesView.LineMarkerOptions.Visible = false;
				series.View = seriesView;
				series.Label.Visible = false;

				var stdDevSeries = chart.Series[0];
				stdDevSeries.ArgumentDataMember = series.ArgumentDataMember;
				stdDevSeries.ArgumentScaleType = series.ArgumentScaleType;
				stdDevSeries.ValueDataMembersSerializable = series.ValueDataMembersSerializable + "Property";
				stdDevSeries.ValueScaleType = series.ValueScaleType;
				var stdDebSeriesView = new LineSeriesView{
					AxisXName = seriesView.AxisXName + " StdDev",
					Color = Color.PaleTurquoise,
					PaneName = seriesView.PaneName + "StdDev",
					AxisY = (chart.Diagram as XYDiagram).SecondaryAxesY[0]
				};
				stdDebSeriesView.LineStyle.Thickness = 1;
				stdDebSeriesView.LineStyle.DashStyle = DashStyle.Solid;
				stdDebSeriesView.LineMarkerOptions.Visible = false;
				stdDevSeries.View = stdDebSeriesView;
				stdDevSeries.Label.Visible = false;

				var chartTitle = chart.Titles[0];
				chartTitle.Text = "";
				chartTitle.Visible = true;
				chartTitle.Font = new Font(chart.Titles[0].Font.FontFamily, 10.0f);

				var axisX = (chart.Diagram as XYDiagram).AxisX;
				var axisY = (chart.Diagram as XYDiagram).AxisY;
				axisX.Reverse = false;
				axisX.GridLines.Visible = true;
				;
			}

			//tableLayout.Controls.Clear();
			//tableLayout.Controls.AddRange(_cumulativeCharts.ToArray());

		}

		public void SetDataSource(List<ReadingsSummary> items) {

			var cumTimeInfo = GetCumulativeWindows(
				DrillStartDate.Add(CumulativeTimeSpan),
				CumulativeTimeSpan,
				StepBack
			);

			// todo: can this copy be eliminated?

			Func<ReadingsSummary, double> summaryValueFunction;
			Func<ReadingValues, double> valueFunction;
			var selAttrType = SelectedAttributeType;
			switch (selAttrType) {
			case ReadingAttributeType.WindSpeed:
				summaryValueFunction = r => r.WindSpeed;
				valueFunction = r => r.WindSpeed;
				break;
			case ReadingAttributeType.Temperature:
				summaryValueFunction = r => r.Temperature;
				valueFunction = r => r.Temperature;
				break;
			case ReadingAttributeType.Humidity:
				summaryValueFunction = r => r.Humidity;
				valueFunction = r => r.Humidity;
				break;
			case ReadingAttributeType.Pressure:
				summaryValueFunction = r => r.Pressure;
				valueFunction = r => r.Pressure;
				break;
			case ReadingAttributeType.WindDirection:
				summaryValueFunction = r => r.WindDirection;
				valueFunction = r => r.WindDirection;
				break;
			default:
				throw new NotSupportedException(String.Format("SelectedAttributeType {0} is not supported", selAttrType));
			}

			var min = 0.0;
			var max = 0.0;
			var stdDevMin = 0.0;
			var stdDevMax = 0.0;
			if(items.Count > 0) {
				min = max = summaryValueFunction(items[0]);
				stdDevMin = stdDevMax = valueFunction(items[0].SampleStandardDeviation);
				for(int i = 1; i < items.Count; i++) {
					var value = summaryValueFunction(items[i]);
					if(value < min)
						min = value;
					else if(value > max)
						max = value;

					var stdDevValue = valueFunction(items[i].SampleStandardDeviation);
					if(stdDevValue < stdDevMin)
						stdDevMin = stdDevValue;
					else if(stdDevValue > stdDevMax)
						stdDevMax = stdDevValue;
				}
			}

			RecreateCumulativeGraphs(); // TODO: only when they need to change?

			for(int i = 0; i < cumTimeInfo.Count; i++) {
				var window = cumTimeInfo[i];
				var chartReadings = items.Where(r => r.TimeStamp >= window.min && r.TimeStamp <= window.max).ToList();
				//_cumulativeCharts[i].DataSource = chartReadings;
				var chart =_cumulativeCharts[i];
				var diagram = chart.Diagram as XYDiagram;
				var seriesData = chart.Series[1];
				var seriesStdDev = chart.Series[0];
				seriesData.DataSource = chartReadings;
				seriesStdDev.DataSource = chartReadings.Select(r => new Reading(r.TimeStamp, r.SampleStandardDeviation)).ToList();
				chart.Invalidate();
				chart.Titles[0].Text = window.Name;

				AxisX axisX = diagram.AxisX;
				AxisY axisY = diagram.AxisY;

				var timeSpanPerGraph = window.Span;
				SetAxisYRanges(min, max, axisY);
				SetAxisYRanges(stdDevMin, stdDevMax, diagram.SecondaryAxesY[0]);
				SetAxisXRanges(window.min, window.max, timeSpanPerGraph, axisX);
			}


		}

		private void SetAxisYRanges(double min, double max, AxisYBase axisY) {
			if (Double.MaxValue == min || Double.MinValue == max || min == max) {
				axisY.Range.Auto = true;
			}
			else {
				axisY.Range.Auto = false;
				axisY.Range.SetMinMaxValues(min, max);
			}
		}

		private void SetAxisXRanges(DateTime min, DateTime max, TimeSpan timeSpanPerGraph, AxisX axisX) {
			axisX.DateTimeScaleMode = DateTimeScaleMode.Manual;
			if (timeSpanPerGraph < new TimeSpan(0, 1, 0)) {
				axisX.DateTimeGridAlignment = DateTimeMeasurementUnit.Second;
				axisX.DateTimeMeasureUnit = DateTimeMeasurementUnit.Second;
				axisX.DateTimeOptions.Format = DateTimeFormat.ShortTime;
			}
			else if (timeSpanPerGraph <= new TimeSpan(12, 0, 0)) {
				axisX.DateTimeGridAlignment = DateTimeMeasurementUnit.Second;
				axisX.DateTimeMeasureUnit = DateTimeMeasurementUnit.Minute;
				axisX.DateTimeOptions.Format = DateTimeFormat.ShortTime;
			}
			else if (timeSpanPerGraph <= new TimeSpan(31, 0, 0, 1)) {
				axisX.DateTimeGridAlignment = DateTimeMeasurementUnit.Minute;
				axisX.DateTimeMeasureUnit = DateTimeMeasurementUnit.Minute;
				axisX.DateTimeOptions.Format = DateTimeFormat.ShortTime;
			}
			else {
				axisX.DateTimeGridAlignment = DateTimeMeasurementUnit.Minute;
				axisX.DateTimeMeasureUnit = DateTimeMeasurementUnit.Day;
				axisX.DateTimeOptions.Format = DateTimeFormat.ShortDate;
			}
			axisX.Range.SetMinMaxValues(min, max);
		}

		private void comboBoxEditSelProp_SelectedIndexChanged(object sender, EventArgs e) {
			if(null != OnSelectedPropertyChanged) {
				OnSelectedPropertyChanged();
			}
		}

	}
}
