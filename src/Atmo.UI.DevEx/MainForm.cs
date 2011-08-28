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
using System.IO;
using System.Linq;
using System.Net;
using Atmo.Daq.Win32;
using Atmo.Data;
using Atmo.Stats;
using Atmo.UI.DevEx.Controls;
using Atmo.UI.WinForms.Controls;
using Atmo.Units;
using DevExpress.XtraEditors;
using System.Windows.Forms;

namespace Atmo.UI.DevEx {
	public partial class MainForm : XtraForm {

		private IDaqConnection _deviceConnection = null;
		private MemoryDataStore _memoryDataStore = null;
		private SensorViewPanelController _sensorViewPanelControler = null;
		private HistoricSensorViewPanelController _historicSensorViewPanelController = null;
		private System.Data.SQLite.SQLiteConnection _dbConnection = null;
		private IDataStore _dbStore;
		private bool _updateHistorical = false;

		private ProgramContext AppContext { get; set; }

		public MainForm(ProgramContext appContext) {
			if (null == appContext) {
				throw new ArgumentNullException();
			}
			AppContext = appContext;

			InitializeComponent();

			ConverterCache = ReadingValuesConverterCache<IReadingValues, ReadingValues>.Default;
			ConverterCacheReadingValues = ReadingValuesConverterCache<ReadingValues>.Default;
			ConverterCacheReadingAggregate = ReadingValuesConverterCache<ReadingAggregate>.Default;
			liveAtmosphericGraph.ConverterCacheReadingValues = ConverterCacheReadingValues;

			_deviceConnection = new UsbDaqConnection(); // new Demo.DemoDaqConnection();

			_dbConnection = new System.Data.SQLite.SQLiteConnection(
				@"data source=ClearStorage.db;page size=4096;cache size=4000;journal mode=Off"
			);
			_dbStore = new DbDataStore(_dbConnection);

			_memoryDataStore = new MemoryDataStore();

			_sensorViewPanelControler = new SensorViewPanelController(groupControlSensors) {
				DefaultSelected = true
			};
			_historicSensorViewPanelController = new HistoricSensorViewPanelController(groupControlDbList) {
				DefaultSelected = true,
			};
			_historicSensorViewPanelController.OnDeleteRequested += OnDeleteRequested;

			historicalTimeSelectHeader.CheckEdit.CheckedChanged += histNowChk_CheckedChanged;
			ReloadHistoric();

			historicalTimeSelectHeader.TimeRange.SelectedIndex = historicalTimeSelectHeader.TimeRange.FindNearestIndex(AppContext.PersistentState.HistoricalTimeScale);
			liveAtmosphericHeader.TimeRange.SelectedIndex = liveAtmosphericHeader.TimeRange.FindNearestIndex(AppContext.PersistentState.LiveTimeScale);

			historicalTimeSelectHeader.TimeRange.ValueChanged += historicalTimeSelectHeader_TimeRangeValueChanged;
			liveAtmosphericHeader.TimeRange.ValueChanged += liveTimeSelectHeader_TimeRangeValueChanged;

			foreach(var view in _historicSensorViewPanelController.Views) {
				bool selected = false;
				if(
					null != view && null != view.SensorInfo
					&& AppContext.PersistentState.SelectedDatabases.Contains(view.SensorInfo.Name)
				) {
					selected = true;
				}
				view.IsSelected = selected;
			}

			HandleRapidFireSetup();

		}

		private void HandleRapidFireSetup() {
			if (AppContext.PersistentState.PwsEnabled) {
				StartRapidFire();
			}else {
				CancelRapidFire("Not enabled.");
			}
		}

		public void historicalTimeSelectHeader_TimeRangeValueChanged(object sender, EventArgs args) {
			AppContext.PersistentState.HistoricalTimeScale = historicalTimeSelectHeader.TimeRange.SelectedSpan;
			AppContext.PersistentState.IsDirty = true;
		}

		public void liveTimeSelectHeader_TimeRangeValueChanged(object sender, EventArgs args) {
			AppContext.PersistentState.LiveTimeScale = liveAtmosphericHeader.TimeRange.SelectedSpan;
			AppContext.PersistentState.IsDirty = true;
		}

		public TemperatureUnit TemperatureUnit { get { return AppContext.PersistentState.TemperatureUnit; } }
		public SpeedUnit SpeedUnit { get { return AppContext.PersistentState.SpeedUnit; } }
		public PressureUnit PressureUnit { get { return AppContext.PersistentState.PressureUnit; } }

		public ReadingValuesConverterCache<IReadingValues, ReadingValues> ConverterCache { get; set; }
		public ReadingValuesConverterCache<ReadingValues> ConverterCacheReadingValues { get; set; }
		public ReadingValuesConverterCache<ReadingAggregate> ConverterCacheReadingAggregate { get; set; }

		private void OnDeleteRequested(ISensorInfo sensorInfo) {
			if (null == sensorInfo || String.IsNullOrEmpty(sensorInfo.Name) || null == _dbStore) {
				MessageBox.Show("Invalid target", "Error");
				return;
			}

			var result = MessageBox.Show(
				String.Format("Are you sure you want to delete the sensor named '{0}'?", sensorInfo.Name), "Delete?",
			    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question
			);
			if(result != DialogResult.Yes) {
				return;
			}
			result = MessageBox.Show(
				String.Format("Are you really sure you want to delete the sensor named '{0}'? All data for this sensor will be removed without possibility of retrieval.", sensorInfo.Name), "DELETE?",
				MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation
			);
			if (result != DialogResult.Yes) {
				return;
			}
			_dbStore.DeleteSensor(sensorInfo.Name);
			ReloadHistoric();
		}

		private void timerTesting_Tick(object sender, EventArgs e) {
			
			// current live state
			var now = DateTime.Now;
			var sensors = _deviceConnection.Where(s => s.IsValid).ToList();

			// get readings
			var readings = new Dictionary<ISensor, IReading>();
			foreach (var sensor in sensors) {
				var reading = sensor.GetCurrentReading();
				if(reading.IsValid) {
					readings.Add(sensor, reading);
				}
			}

			// save to memory
			foreach(var reading in readings) {
				if(!_memoryDataStore.GetAllSensorInfos().Any(si => si.Name.Equals(reading.Key.Name))) {
					_memoryDataStore.AddSensor(reading.Key);
				}
				_memoryDataStore.Push(reading.Key.Name, new [] { reading.Value });
			}

			// update the sensor controls
			_sensorViewPanelControler.UpdateView(sensors);

			// the current sensor views
			var sensorViews = _sensorViewPanelControler.Views.ToList();

			// determine which sensors are enabled
			var enabledSensors = new List<ISensor>();
			for(int i = 0; i < sensors.Count && i < sensorViews.Count; i++) {
				if(sensorViews[i].IsSelected) {
					enabledSensors.Add(sensors[i]);
				}
			}
			
			var liveDataEnabled = true;
			var liveDataTimeSpan = liveAtmosphericHeader.TimeRange.SelectedSpan;

			// pass it off to the live data graphs/tables
			if(liveDataEnabled) {
				
				// gather the data for each selected sensor
				var enabledSensorsLiveMeans = new List<List<ReadingAggregate>>(enabledSensors.Count);
				foreach(var sensor in enabledSensors) {
					
					// get the recent readings
					var recentReadings = _memoryDataStore.GetReadings(sensor.Name, now, TimeSpan.Zero.Subtract(liveDataTimeSpan));
					
					// calculate the mean data
					var means = StatsUtil.AggregateMean(recentReadings, TimeUnit.Second).ToList();

					// convert the units for display
					var converter = ConverterCacheReadingAggregate.Get(
						sensor.TemperatureUnit, TemperatureUnit,
						sensor.SpeedUnit, SpeedUnit,
						sensor.PressureUnit, PressureUnit
					);
					converter.ConvertInline(means);
					
					// add it to the presentation list
					enabledSensorsLiveMeans.Add(means);
				}

				// compile it all together into one set
				var enabledSensorsCompiledMeans = StatsUtil.JoinParallelMeanReadings(enabledSensorsLiveMeans);

				// present the data set
				liveAtmosphericGraph.TemperatureUnit = TemperatureUnit;
				liveAtmosphericGraph.PressureUnit = PressureUnit;
				liveAtmosphericGraph.SpeedUnit = SpeedUnit;
				liveAtmosphericGraph.FormatTimeAxis(liveDataTimeSpan);
				liveAtmosphericGraph.SetLatest(enabledSensorsCompiledMeans.LastOrDefault());
				liveAtmosphericGraph.State = AppContext.PersistentState;
				liveAtmosphericGraph.SetDataSource(enabledSensorsCompiledMeans);
			}

			var histTimeRangeSelector = historicalTimeSelectHeader.TimeRange;
			var histDateChooser = historicalTimeSelectHeader.DateEdit;
			var histNowChk = historicalTimeSelectHeader.CheckEdit;
			var histTimeChooser = historicalTimeSelectHeader.TimeEdit;

			var histTimeSpan = histTimeRangeSelector.SelectedSpan;
			var histStartDate = histNowChk.Checked
			    ? now.Subtract(histTimeSpan)
			    : histDateChooser.DateTime.Date.Add(histTimeChooser.Time.TimeOfDay);

			var cumTimeInfo = HistoricalGraphBreakdown.GetCumulativeWindows(
				histStartDate.Add(histTimeSpan),
				histTimeSpan,
				!histNowChk.Checked
			);

			var historicalSelected = _dbStore.GetAllSensorInfos().Where(si => _historicSensorViewPanelController.IsSensorSelected(si)).ToList();
			var sensorReadings = historicalSelected.Select(
				sensor =>
				_dbStore.GetReadingSummaries(sensor.Name, cumTimeInfo.MaxStamp, cumTimeInfo.MinStamp - cumTimeInfo.MaxStamp, UnitUtility.ChooseBestSummaryUnit(histTimeSpan))
			);

			var historicalSummaries = StatsUtil.JoinReadingSummaryEnumerable(sensorReadings).ToList();

			windResourceGraph.TemperatureUnit = TemperatureUnit;
			windResourceGraph.PressureUnit = PressureUnit;
			windResourceGraph.SpeedUnit = SpeedUnit;

			windResourceGraph.SetDataSource(historicalSummaries);

			historicalGraphBreakdown.TemperatureUnit = TemperatureUnit;
			historicalGraphBreakdown.PressureUnit = PressureUnit;
			historicalGraphBreakdown.SpeedUnit = SpeedUnit;
			historicalGraphBreakdown.SelectedAttributeType = ReadingAttributeType.WindSpeed;
			historicalGraphBreakdown.StepBack = !histNowChk.Checked;
			historicalGraphBreakdown.DrillStartDate = histStartDate;
			historicalGraphBreakdown.CumulativeTimeSpan = histTimeSpan;

			historicalGraphBreakdown.SetDataSource(historicalSummaries);

		}

		private void barButtonItemPrefs_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
			var settingsForm = new SettingsForm(AppContext.PersistentState);
			settingsForm.ShowDialog(this);
			HandleRapidFireSetup();
		}

		private void simpleButtonFindSensors_Click(object sender, EventArgs e) {
			FindSensors();
		}

		private void barButtonItemSensorSetup_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
			FindSensors();
		}

		private void FindSensors() {
			var rapidFireEnabled = timerRapidFire.Enabled;
			if(rapidFireEnabled) {
				timerRapidFire.Enabled = false;
			}
			timerTesting.Enabled = false;
			var findSensorForm = new FindSensorsDialog(_deviceConnection);
			findSensorForm.ShowDialog(this);
			timerTesting.Enabled = true;
			if(rapidFireEnabled) {
				timerRapidFire.Enabled = true;
			}
		}

		private void barButtonItemImport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
			DownloadDataDialog();
		}

		private void simpleButtonDownloadData_Click(object sender, EventArgs e) {
			DownloadDataDialog(true);
		}

		private void DownloadDataDialog(bool auto = false) {
			var importForm = new ImportDataForm(_dbStore, _deviceConnection) {
				AutoImport = true
			};
			importForm.PersistentState = AppContext.PersistentState;
			importForm.ShowDialog(this);
			ReloadHistoric();
		}

		public void ReloadHistoric() {
			var historicSensors = _dbStore.GetAllSensorInfos();
			_historicSensorViewPanelController.UpdateView(historicSensors);
			TriggerHistoricalUpdate();
		}

		private void TriggerHistoricalUpdate() {
			_updateHistorical = true;
		}

		private void histNowChk_CheckedChanged(object sender, EventArgs e) {
			TriggerHistoricalUpdate();
		}

		private void barButtonItemTimeCorrection_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
			var timeCorrectionDialog = new TimeCorrection(_dbStore);
			timeCorrectionDialog.ShowDialog(this);
		}

		private void barButtonItemExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
			var exportForm = new ExportForm(_dbStore);
			exportForm.ShowDialog(this);
		}

		private void barButtonItemTimeSync_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
			var timeSync = new TimeSync(_deviceConnection,_dbStore);
			timeSync.ShowDialog(this);
		}

		private void barButtonItemExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
			Close();
		}

		private void barButtonItemFirmwareUpdate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
			if(!(_deviceConnection is UsbDaqConnection)) {
				MessageBox.Show("Device is not supported", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			var patchForm = new PatcherForm(_deviceConnection as UsbDaqConnection);
			patchForm.ShowDialog();
		}

		private void MainForm_FormClosed(object sender, FormClosedEventArgs e) {
			
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
			if(null != AppContext) {
				var selectedNames = _historicSensorViewPanelController.Views
					.Where(v => v.IsSelected && null != v.SensorInfo)
					.Select(v => v.SensorInfo.Name)
					.ToList();

				AppContext.PersistentState.SelectedDatabases = selectedNames;
				AppContext.PersistentState.IsDirty = true;
			}
		}

		private void timerQueryTime_Tick(object sender, EventArgs e) {

			UpdateDaqTimes();
			UpdateDaqStats();

		}

		private void UpdateDaqStats() {
			const string na = "N/A";

			var temp = _deviceConnection.Temperature;
			var volBat = _deviceConnection.VoltageBattery;
			var volUsb = _deviceConnection.VoltageUsb;

			labelTmpDaq.Text = Double.IsNaN(temp) ? na : temp.ToString("F2");
			labelVolBat.Text = Double.IsNaN(volBat) ? na : volBat.ToString("F2");
			labelVolUsb.Text = Double.IsNaN(volUsb) ? na : volUsb.ToString("F2");
		}

		private void UpdateDaqTimes() {
			var now = DateTime.Now;
			labelLocalTime.Text = now.ToString();

			if(null != _deviceConnection) {
				var deviceTime = _deviceConnection.QueryClock();
				if(default(DateTime) != deviceTime) {
					labelDaqTime.Text = deviceTime.ToString();
					var diff = deviceTime.Subtract(now);
					if (diff < TimeSpan.Zero) {
						diff = TimeSpan.Zero - diff;
					}
					labelDaqTime.ForeColor = (
						(diff >= new TimeSpan(0, 0, 5) && (0 == (now.Second % 2)))
						? Color.Red
						: ForeColor
					);
					return;
				}
			}

			labelDaqTime.Text = "N/A";
			labelDaqTime.ForeColor = ForeColor;
		}

		private void simpleButtonPwsAction_Click(object sender, EventArgs e) {
			if(timerRapidFire.Enabled) {
				CancelRapidFire("User canceled.");
			}
			else {
				AutoStartRapidFire();
			}
		}

		private void AutoStartRapidFire() {
			var stationNames = AppContext.PersistentState.StationNames;
			var stationPassword = AppContext.PersistentState.StationPassword;
			bool isValid = stationNames.Any(sn => !String.IsNullOrEmpty(sn))
				&& !String.IsNullOrEmpty(stationPassword);

			if (timerRapidFire.Enabled != isValid) {
				if(isValid) {
					StartRapidFire();
					timerRapidFire_Tick(null, null);
				}
			}
		}

		private const string RunningText = "Running";

		private void StartRapidFire() {
			timerRapidFire.Enabled = true;
			AppContext.PersistentState.PwsEnabled = true;

			//labelControlPwsStatus.Text = "Running";
			labelControlPwsStatus.SetPropertyThreadSafe(() => labelControlPwsStatus.Text, RunningText);
			//simpleButtonPwsAction.Text = "Running";
			simpleButtonPwsAction.SetPropertyThreadSafe(() => simpleButtonPwsAction.Text, RunningText + " (click to stop).");
			//simpleButtonPwsAction.BackColor = Color.LightGreen;
			simpleButtonPwsAction.SetPropertyThreadSafe(() => simpleButtonPwsAction.ForeColor, ForeColor);

		}

		private void CancelRapidFire(string message) {
			timerRapidFire.Enabled = false;
			AppContext.PersistentState.PwsEnabled = false;

			//labelControlPwsStatus.Text = message;
			labelControlPwsStatus.SetPropertyThreadSafe(() => labelControlPwsStatus.Text, message);
			//simpleButtonPwsAction.Text = "Disabled";
			simpleButtonPwsAction.SetPropertyThreadSafe(() => labelControlPwsStatus.Text, "Stopped (click to start).");
			//simpleButtonPwsAction.BackColor = Color.LightPink;
			simpleButtonPwsAction.SetPropertyThreadSafe(() => labelControlPwsStatus.ForeColor, Color.Red);
		}

		private void timerRapidFire_Tick(object sender, EventArgs e) {
			ISensor[] sensors = _deviceConnection.ToArray();

			IReading[] readings = sensors
				.Select(sensor => sensor.IsValid ? sensor.GetCurrentReading() : null)
				.ToArray()
			;

			if(readings.All(r => null == r)) {
				labelControlPwsStatus.Text = "No sensors.";
			}else {
				labelControlPwsStatus.Text = RunningText;
			}

			var stationNames = AppContext.PersistentState.StationNames;
			var stationPassword = AppContext.PersistentState.StationPassword;
			if (String.IsNullOrEmpty(stationPassword)) {
				CancelRapidFire("No PWS password.");
				return;
			}
			if(stationNames == null || stationNames.Count == 0) {
				CancelRapidFire("No PWS stations.");
				return;
			}
			for (int i = 0; i < readings.Length; i++) {
				string id = stationNames[i];
				if (String.IsNullOrEmpty(id)) {
					continue;
				}
				IReading reading = readings[i];
				if (null == reading || !reading.IsValid) {
					continue;
				}
				Dictionary<string, string> queryParams = new Dictionary<string, string>();
				queryParams.Add("action", "updateraw");
				queryParams.Add("ID", id);
				queryParams.Add("PASSWORD", stationPassword);
				DateTime utcStamp = reading.TimeStamp.ToUniversalTime();
				queryParams.Add("dateutc", utcStamp.ToString("yyyy-MM-dd hh:mm:ss"));
				queryParams.Add("realtime", "1");
				queryParams.Add("rtfreq", ((timerRapidFire.Interval / 1000.0).ToString()));
				if (reading.IsWindDirectionValid && reading.WindDirection <= 360.0) {
					queryParams.Add("winddir", ((int)(reading.WindDirection)).ToString());
				}
				if (reading.IsWindSpeedValid) {
					queryParams.Add(
						"windspeedmph",
						UnitUtility.ConvertUnit(reading.WindSpeed, sensors[i].SpeedUnit, SpeedUnit.MilesPerHour).ToString()
					);
				}
				if (reading.IsHumidityValid) {
					queryParams.Add("humidity", (reading.Humidity * 100.0).ToString());
				}
				if (reading.IsTemperatureValid) {
					queryParams.Add(
						"tempf",
						UnitUtility.ConvertUnit(reading.Temperature, sensors[i].TemperatureUnit, TemperatureUnit.Fahrenheit).ToString()
					);
				}
				if (reading.IsPressureValid) {
					queryParams.Add(
						"baromin",
						UnitUtility.ConvertUnit(reading.Pressure, sensors[i].PressureUnit, PressureUnit.InchOfMercury).ToString()
					);
				}
				queryParams.Add(
					"softwaretype",
					"Atmo"
				);

				var builder = new UriBuilder("http://rtupdate.wunderground.com/weatherstation/updateweatherstation.php");
				builder.Query = String.Join(
					"&",
					queryParams
						.Select(kvp => String.Concat(Uri.EscapeDataString(kvp.Key), '=', Uri.EscapeDataString(kvp.Value)))
						.ToArray()
				);

				var reqSent = HttpWebRequest.Create(builder.Uri);
				reqSent.BeginGetResponse(HandleRapidFireResult, reqSent);
			}

		}

		private void HandleRapidFireResult(IAsyncResult result) {
			var req = (WebRequest)result.AsyncState;
			try {
				using (WebResponse res = req.EndGetResponse(result)) {
					using (var reader = new StreamReader(res.GetResponseStream())) {
						string responseMessage = reader.ReadToEnd().ToUpperInvariant();
						if (responseMessage.StartsWith("SUCCESS")) {
							; // OK
						}
						else if (responseMessage.Contains("PASSWORD")) {
							CancelRapidFire("PWS Station ID or password is invalid.");
						}
						else {
							CancelRapidFire("PWS protocol error.");
						}
					}
				}
			}
			catch (WebException webEx) {
				CancelRapidFire("PWS Communication failure.");
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
				if(_deviceConnection is IDisposable) {
					(_deviceConnection as IDisposable).Dispose();
				}
			}
			base.Dispose(disposing);
		}


	}
}
