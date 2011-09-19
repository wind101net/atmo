// ================================================================================
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
			while(_cumulativeCharts.Count < numberGraphsToMake) {
				var chart = new ChartControl();
				chart.Show();
				chart.DataSource = bindingSourceReadingSummary;
				chart.Size = new Size(200, 200);
				chart.Dock = DockStyle.Fill;
				chart.Legend.Visible = false;
				chart.Titles.Add(new ChartTitle());
				chart.Series.Add(new Series());
				_cumulativeCharts.Add(chart);
				tableLayout.Controls.Add(chart);
			}


			for (int i = 0; i < numberGraphsToMake; i++) {
				
				var chart = _cumulativeCharts[i];


				var series = chart.Series[0];
				series.ArgumentDataMember = "TimeStamp";
				series.ArgumentScaleType = ScaleType.DateTime;
				series.ValueDataMembersSerializable = SelectedAttributeType.ToString().Replace(" ", "");
				series.ValueScaleType = ScaleType.Numerical;
				var seriesView = new LineSeriesView(){
					AxisYName = "Pressure AxisY",
					Color = Color.Black,
					//LineTensionPercent = 50,
					PaneName = "UserAndPressure"
				};
				seriesView.LineMarkerOptions.Visible = false;
				//(series.View as AreaSeriesView).MarkerOptions.Visible = false;

				series.View = seriesView;

				series.Label.Visible = false;

				var chartTitle = chart.Titles[0];
				chartTitle.Text = "";
				chartTitle.Visible = true;
				chartTitle.Font = new Font(chart.Titles[0].Font.FontFamily, 10.0f);

				var axisX = (chart.Diagram as XYDiagram).AxisX;
				var axisY = (chart.Diagram as XYDiagram).AxisY;
				axisX.Reverse = false;
				axisX.GridLines.Visible = true;
				if (timeSpanPerGraph < new TimeSpan(0, 1, 0)) {
					axisX.DateTimeMeasureUnit = axisX.DateTimeGridAlignment = DateTimeMeasurementUnit.Second;
					axisX.DateTimeOptions.Format = DateTimeFormat.ShortTime;
				}
				else if (timeSpanPerGraph < new TimeSpan(1, 0, 0)) {
					axisX.DateTimeMeasureUnit = axisX.DateTimeGridAlignment = DateTimeMeasurementUnit.Minute;
					axisX.DateTimeOptions.Format = DateTimeFormat.ShortTime;
				}
				else if (timeSpanPerGraph < new TimeSpan(1, 0, 0, 0)) {
					axisX.DateTimeMeasureUnit = axisX.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
					axisX.DateTimeOptions.Format = DateTimeFormat.ShortTime;
				}
				else if (timeSpanPerGraph < new TimeSpan(31, 0, 0, 1)) {
					axisX.DateTimeMeasureUnit = axisX.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
					axisX.DateTimeOptions.Format = DateTimeFormat.ShortDate;
				}
				else {
					axisX.DateTimeMeasureUnit = axisX.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
					axisX.DateTimeOptions.Format = DateTimeFormat.ShortDate;
				}
				
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

			Func<ReadingsSummary, double> targetValueFunction = null;
			switch(SelectedAttributeType) {
			case ReadingAttributeType.WindSpeed:
				targetValueFunction = (r) => (r.WindSpeed);
				break;
			case ReadingAttributeType.Temperature:
				targetValueFunction = (r) => (r.Temperature);
				break;
			case ReadingAttributeType.Humidity:
				targetValueFunction = (r) => (r.Humidity);
				break;
			case ReadingAttributeType.Pressure:
				targetValueFunction = (r) => (r.Pressure);
				break;
			default:
				throw new NotSupportedException(String.Format("selectedAttributeType {0} is not supported", SelectedAttributeType));
			}

			var min = 0.0;
			var max = 0.0;
			if(items.Count > 0) {
				min = max = targetValueFunction(items[0]);
				for(int i = 1; i < items.Count; i++) {
					var value = targetValueFunction(items[i]);
					if(value < min) {
						min = value;
					}
					else if(value > max) {
						max = value;
					}
				}
			}

			RecreateCumulativeGraphs(); // TODO: only when they need to change!!!!!

			for(int i = 0; i < cumTimeInfo.Count; i++) {
				var window = cumTimeInfo[i];
				var chartReadings = items.Where(r => r.TimeStamp >= window.min && r.TimeStamp <= window.max).ToList();
				_cumulativeCharts[i].DataSource = chartReadings;
				_cumulativeCharts[i].Invalidate();
				_cumulativeCharts[i].Titles[0].Text = window.Name;
				AxisX axisX = (_cumulativeCharts[i].Diagram as XYDiagram).AxisX;

				AxisY axisY = (_cumulativeCharts[i].Diagram as XYDiagram).AxisY;
				if (Double.MaxValue == min || Double.MinValue == max || min == max) {
					axisY.Range.Auto = true;
				}
				else {
					axisY.Range.Auto = false;
					axisY.Range.SetMinMaxValues(min, max);
				}

				if (window.Span <= new TimeSpan(1, 0, 0, 0)) {
					axisX.DateTimeOptions.Format = DateTimeFormat.ShortTime;
					axisX.DateTimeScaleMode = DateTimeScaleMode.Manual;
					axisX.DateTimeGridAlignment = DateTimeMeasurementUnit.Minute;
					axisX.DateTimeMeasureUnit = DateTimeMeasurementUnit.Minute;
				}
				else {
					axisX.DateTimeOptions.Format = DateTimeFormat.ShortDate;
					axisX.DateTimeScaleMode = DateTimeScaleMode.Manual;
					axisX.DateTimeGridAlignment = DateTimeMeasurementUnit.Day;
					axisX.DateTimeMeasureUnit = DateTimeMeasurementUnit.Day;
				}
				axisX.Range.SetMinMaxValues(window.min, window.max);
			}


		}

		private void comboBoxEditSelProp_SelectedIndexChanged(object sender, EventArgs e) {
			if(null != OnSelectedPropertyChanged) {
				OnSelectedPropertyChanged();
			}
		}

	}
}
