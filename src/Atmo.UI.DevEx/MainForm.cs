// ================================================================================
//
// Atmo 2
// Copyright (C) 2011  BARANI DESIGN
//AutoStartRapidFire
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
using System.Threading;
using Atmo.Calculation;
using Atmo.Daq.Win32;
using Atmo.Data;
using Atmo.Stats;
using Atmo.UI.DevEx.Controls;
using Atmo.UI.WinForms.Controls;
using Atmo.Units;
using DevExpress.XtraEditors;
using System.Windows.Forms;
using log4net;
using System.Security.Cryptography;
using System.Text;

//rp
using System.Web;
using System.Net.Sockets;

namespace Atmo.UI.DevEx {



	public partial class MainForm : XtraForm
	{

		private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		private static readonly string DatabaseFileName = @"ClearStorage.db";  //rp @

		private IDaqConnection _deviceConnection = null;
		private MemoryDataStore _memoryDataStore = null;
		private SensorViewPanelController _sensorViewPanelControler = null;
		private HistoricSensorViewPanelController _historicSensorViewPanelController = null;
		private System.Data.SQLite.SQLiteConnection _dbConnection = null;
		private IDataStore _dbStore;
		private bool _updateHistorical = false;
		private object _historicalUpdateThreadMutex = new object();
		private DateTime _lastLiveUpdate = default(DateTime);
		private TimeSpan _lastLiveUpdateInterval = default(TimeSpan);

		private ProgramContext AppContext { get; set; }



        //rp
        private AdvancedSensorValues[] _advancedSensorValues;
<<<<<<< HEAD

        public double time_correction = 0;
        private static System.Timers.Timer timerWU;


        //rp
        public  InternetStreamingStatistics _internetStreamingStatistics;

=======
        public double time_correction = 0;
        private static System.Timers.Timer timerWU;

>>>>>>> 0cad5a8d70d0696eeb938de59d096b4c3dd3e023
        private  void OnTimedEventWU(object source, System.Timers.ElapsedEventArgs e)
        {

               timerRapidFire_TickWU(null, null);


                try
                {
                    ISensor[] sensors = _deviceConnection.ToArray();

                    for (int i = 0; i < sensors.Length; i++)
                    {
                        if (sensors[i].IsValid)
                        {
                            var reading = sensors[i].GetCurrentReading();
                            if (reading.IsWindSpeedValid == true)
                            {
                                _advancedSensorValues[i].AddValue_WU_WindSpeed(reading.WindSpeed);
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex.ToString());
                }

        }



		public MainForm(ProgramContext appContext) {
			if (null == appContext) {
				throw new ArgumentNullException();
			}

			AppContext = appContext;
			InitializeComponent();







            timerWU = new System.Timers.Timer(); 
<<<<<<< HEAD
            timerWU.Interval = 5000; //pov2500
=======
            timerWU.Interval = 2500; 
>>>>>>> 0cad5a8d70d0696eeb938de59d096b4c3dd3e023
            timerWU.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEventWU);
            timerWU.Enabled = false;
            timerWU.Start();


            _advancedSensorValues = new AdvancedSensorValues[10];
            _advancedSensorValues[0] = new AdvancedSensorValues();
            _advancedSensorValues[1] = new AdvancedSensorValues();
            _advancedSensorValues[2] = new AdvancedSensorValues();
            _advancedSensorValues[3] = new AdvancedSensorValues();
            _advancedSensorValues[4] = new AdvancedSensorValues();
            _advancedSensorValues[5] = new AdvancedSensorValues();
            _advancedSensorValues[6] = new AdvancedSensorValues();
            _advancedSensorValues[7] = new AdvancedSensorValues();
            _advancedSensorValues[8] = new AdvancedSensorValues();
            _advancedSensorValues[9] = new AdvancedSensorValues();





			Text = ProgramContext.ProgramFriendlyName;

			ConverterCache = ReadingValuesConverterCache<IReadingValues, ReadingValues>.Default;
			ConverterCacheReadingValues = ReadingValuesConverterCache<ReadingValues>.Default;
			ConverterCacheReadingAggregate = ReadingValuesConverterCache<ReadingAggregate>.Default;
			liveAtmosphericGraph.ConverterCacheReadingValues = ConverterCacheReadingValues;

			_deviceConnection = new UsbDaqConnection(); // new Demo.DemoDaqConnection();

			// create the DB file using the application privilages instead of using the installer
			if (!File.Exists(DatabaseFileName)) {
				using(var dbTemplateDataStream = typeof (DbDataStore).Assembly.GetManifestResourceStream("Atmo." + DatabaseFileName))
				using(var outDbTemplate = File.Create(DatabaseFileName)) {
					var buffer = new byte[1024];
					int totalRead;
					while(0 < (totalRead = dbTemplateDataStream.Read(buffer, 0, buffer.Length)))
						outDbTemplate.Write(buffer,0, totalRead);
				}
				
				Thread.Sleep(250); // just to be safe
				Log.InfoFormat("Core DB file created at: {0}", Path.GetFullPath(DatabaseFileName));
			}
			try {
				_dbConnection = new System.Data.SQLite.SQLiteConnection(
					@"data source=" + DatabaseFileName + @";page size=4096;cache size=4000;journal mode=Off");
				_dbConnection.Open();
			}
			catch(Exception ex) {
				Log.Error("Database connection failure.", ex);
				throw;
			}
			_dbStore = new DbDataStore(_dbConnection);

			_memoryDataStore = new MemoryDataStore();

			_sensorViewPanelControler = new SensorViewPanelController(groupControlSensors) {
				DefaultSelected = true
			};
			_historicSensorViewPanelController = new HistoricSensorViewPanelController(groupControlDbList) {
				DefaultSelected = true,
			};
			_historicSensorViewPanelController.OnDeleteRequested += OnDeleteRequested;
			_historicSensorViewPanelController.OnRenameRequested += OnRenameRequested;

			historicalTimeSelectHeader.CheckEdit.CheckedChanged += histNowChk_CheckedChanged;

			historicalTimeSelectHeader.TimeRange.SelectedIndex = historicalTimeSelectHeader.TimeRange.FindNearestIndex(AppContext.PersistentState.HistoricalTimeScale);
			liveAtmosphericHeader.TimeRange.SelectedIndex = liveAtmosphericHeader.TimeRange.FindNearestIndex(AppContext.PersistentState.LiveTimeScale);

			historicalTimeSelectHeader.TimeRange.ValueChanged += historicalTimeSelectHeader_TimeRangeValueChanged;
			liveAtmosphericHeader.TimeRange.ValueChanged += liveTimeSelectHeader_TimeRangeValueChanged;

			foreach(var view in _historicSensorViewPanelController.Views.Where(v => null != v)) {
				bool selected = false;
				if(
					null != view.SensorInfo
					&& AppContext.PersistentState.SelectedDatabases.Contains(view.SensorInfo.Name)
				) {
					selected = true;
				}
				view.IsSelected = selected;
			}

			//HandleRapidFireSetup();
			HandleDaqTemperatureSourceSet(_deviceConnection.UsingDaqTemp);



            //rp
            HandleWFSetup();

            //rp
            HandleAWSetup();

            //rp
            HandleWeatherSetup();

            //rp
            _internetStreamingStatistics = new InternetStreamingStatistics();

			ReloadHistoric();
			_historicSensorViewPanelController.OnSelectionChanged += RequestHistoricalUpdate;
			historicalGraphBreakdown.OnSelectedPropertyChanged += RequestHistoricalUpdate;
			historicalTimeSelectHeader.OnTimeRangeChanged += RequestHistoricalUpdate;
			windResourceGraph.OnWeibullParamChanged += RequestHistoricalUpdate;
#if DEBUG
			barSubItemDebug.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
#endif

		}

		private void HandleRapidFireSetup() {

            if (AppContext.PersistentState.PwsEnabled) {
				StartRapidFire();
			}else {
				CancelRapidFire("Not enabled.");
			}
		}


        //rp
        private void HandleWFSetup()
        {
            if (AppContext.PersistentState.PwfEnabled)
            {
                StartWindFinder();
            }
            else
            {
                CancelWindFinder("Not enabled.");
            }
        }
        //rp

        //rp
        private void HandleAWSetup()
        {
            if (AppContext.PersistentState.PawEnabled)
            {
                StartAwekas();
            }
            else
            {
                CancelAwekas("Not enabled.");
            }
        }


        //rp
        private void HandleWeatherSetup()
        {
            if (AppContext.PersistentState.WeatherEnabled)
            {
                StartWeather();
            }
            else
            {
                CancelWeather("Not enabled.");
            }
        }
        
        public string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
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

			try {
				if (_dbStore.DeleteSensor(sensorInfo.Name))
					Log.InfoFormat("Sensor '{0}' was deleted by the user.", sensorInfo.Name);
				else
					Log.ErrorFormat("Sensor '{0}' deletion failed and was requested by the user.", sensorInfo.Name);
			}
			catch(Exception ex) {
				Log.Error("Error deleting sensor '" + sensorInfo.Name + "'.", ex);
			}

			ReloadHistoric();
		}

		private void OnRenameRequested(ISensorInfo sensorInfo) {
			if (null == sensorInfo || String.IsNullOrEmpty(sensorInfo.Name) || null == _dbStore) {
				MessageBox.Show("Invalid target", "Error");
				return;
			}

			var renameForm = new RenameForm() {
				Value = sensorInfo.Name,
				Text = "Rename " + sensorInfo.Name
			};
			var renameRes = renameForm.ShowDialog(this);
			if (renameRes != DialogResult.OK)
				return;
			if(renameForm.Value == sensorInfo.Name)
				return;

			try {
				if (_dbStore.RenameSensor(sensorInfo.Name, renameForm.Value))
					Log.InfoFormat("Sensor '{0}' renamed to '{1}' by user.", sensorInfo.Name, renameForm.Value);
				else
					MessageBox.Show("Rename failed.", "Error");
			}
			catch(Exception ex) {
				Log.Error("Error renaming sensor '" + sensorInfo.Name + "' to '" + renameForm.Value + "'.", ex);
			}

			ReloadHistoric();
		}

		private void timerLive_Tick(object sender, EventArgs e) {
			if(!backgroundWorkerLiveGraph.IsBusy)
				backgroundWorkerLiveGraph.RunWorkerAsync();
			//UpdateLiveGraph();
		}

		private void barButtonItemPrefs_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
			try {
<<<<<<< HEAD

                var settingsForm = new SettingsForm(AppContext.PersistentState, _internetStreamingStatistics);
               



                if (settingsForm.ShowDialog(this) == DialogResult.OK)
                {
                    //time_correction = settingsForm.time_correction;
                }
=======
				var settingsForm = new SettingsForm(AppContext.PersistentState);

                settingsForm.time_correction = time_correction;
                if (settingsForm.ShowDialog(this) == DialogResult.OK)
                {
                    time_correction = settingsForm.time_correction;
                }

>>>>>>> 0cad5a8d70d0696eeb938de59d096b4c3dd3e023


                //rp
                // send event after close SettingsForm
                timerAwekas_Tick(null, null);
                timerWindFinder_Tick(null,null);
                timerRapidFire_TickWU(null, null);

            }
			catch(Exception ex) {
				Log.Error("Settings change failed.", ex);
				MessageBox.Show("Settings change failed.", "Failure");
			}

            //rp zak
			// HandleRapidFireSetup();
		}

		private void simpleButtonFindSensors_Click(object sender, EventArgs e) {
			FindSensors();
		}

		private void barButtonItemSensorSetup_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
			FindSensors();
		}

		private void FindSensors() {
			//var rapidFireEnabled = timerRapidFire.Enabled;
            var rapidFireEnabled = timerWU.Enabled;  

			try {
				if (rapidFireEnabled) {
					//timerRapidFire.Enabled = false;
                    timerWU.Enabled = false;
				}
				timerLive.Enabled = false;
				var findSensorForm = new FindSensorsDialog(_deviceConnection);
				findSensorForm.ShowDialog(this);
			}
			catch (Exception ex) {
				Log.Error("Failed to find sensors.", ex);
			}
			finally{
				timerLive.Enabled = true;
				if (rapidFireEnabled) {
					//timerRapidFire.Enabled = true;
                    timerWU.Enabled = true;
				}
			}
		}

		private void barButtonItemImport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
			DownloadDataDialog();
		}

		private void simpleButtonDownloadData_Click(object sender, EventArgs e) {
			DownloadDataDialog(true);
		}

		private void DownloadDataDialog(bool auto = false) {
			try {
				var importForm = new ImportDataForm(_dbStore, _deviceConnection){
					AutoImport = true,
					PersistentState = AppContext.PersistentState
				};
				importForm.ShowDialog(this);
			}
			catch(Exception ex) {
				Log.Error("Download data failed.", ex);
			}
			ReloadHistoric();
		}

		public void ReloadHistoric() {
			var historicSensors = _dbStore.GetAllSensorInfos();
			_historicSensorViewPanelController.UpdateView(historicSensors, AppContext.PersistentState);
			RequestHistoricalUpdate();
		}

		private void histNowChk_CheckedChanged(object sender, EventArgs e) {
			RequestHistoricalUpdate();
		}

		private void barButtonItemTimeCorrection_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
			try {
				var timeCorrectionDialog = new TimeCorrection(_dbStore);
				timeCorrectionDialog.ShowDialog(this);
			}
			catch(Exception ex) {
				Log.Error("Time correction failed.", ex);
			}
			ReloadHistoric();
		}

		private void barButtonItemExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
			try {
				var exportForm = new ExportForm(_dbStore);
				exportForm.ShowDialog(this);
			}
			catch(Exception ex) {
				Log.Error("Export failed.", ex);
			}
		}

		private void barButtonItemTimeSync_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
			ShowTimeSyncDialog();
		}

		private void ShowTimeSyncDialog() {
			try {
				var timeSync = new TimeSync(_deviceConnection, _dbStore);
				timeSync.ShowDialog(this);
			}
			catch(Exception ex) {
				Log.Error("Time sync failed.", ex);
			}
		}

		private void barButtonItemExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
			Close();
		}

		private void barButtonItemFirmwareUpdate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
			if(!(_deviceConnection is UsbDaqConnection)) {
				MessageBox.Show("Device is not supported.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			try {
				timerQueryTime.Stop();
				timerLive.Stop();
				var patchForm = new PatcherForm(_deviceConnection as UsbDaqConnection);
				patchForm.ShowDialog();
			}
			catch(Exception ex) {
				Log.Error("Firmware update failed.", ex);
			}
			finally {
				timerLive.Start();
				timerQueryTime.Start();
			}
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

		private readonly TimeSpan MaxClockUpdateFrequency = new TimeSpan(0,0,0,30);
		private DateTime LastClockUpdate = default(DateTime);

		private void timerQueryTime_Tick(object sender, EventArgs e) {

			UpdateClockStats();
			UpdateDaqStats();

		}

		private static string DisplayDateTime(DateTime v) {
			// dont use string format for date, let the culture handle that
			return v.ToShortDateString() + ' ' + v.ToString("HH:mm:ss");
		}

		private void UpdateClockStats() {
			var now = DateTime.Now;
			labelLocalTime.Text = DisplayDateTime(now);
			if (null != _deviceConnection) {
				var deviceTime = _deviceConnection.QueryClock();
				if (default(DateTime) != deviceTime) {
					labelDaqTime.Text = DisplayDateTime(deviceTime);
					var diff = deviceTime.Subtract(now);
					if (diff < TimeSpan.Zero) {
						diff = TimeSpan.Zero - diff;
					}

					bool outOfSync = diff >= new TimeSpan(0, 0, 0, 3, 0);
					labelDaqTime.ForeColor = ForeColor;

					if (outOfSync && AppContext.PersistentState.AutoSyncClock
						&& (now - LastClockUpdate) >= MaxClockUpdateFrequency
					) {
						System.Diagnostics.Debug.WriteLine("BeginClockSync");
						if (_deviceConnection.SetClock(DateTime.Now)) {
							labelDaqTime.ForeColor = ForeColor;
							System.Diagnostics.Debug.WriteLine("EndClockSync: OK!");
							LastClockUpdate = DateTime.Now;
							return;
						}
						System.Diagnostics.Debug.WriteLine("EndClockSync: Fail");
					}

					if (outOfSync) {
						labelDaqTime.ForeColor = now.Second % 2 == 0 ? Color.Red : ForeColor;
					}
					else {
						labelDaqTime.ForeColor = ForeColor;
					}
					return;
				}
			}
			labelDaqTime.Text = "N/A";
			labelDaqTime.ForeColor = ForeColor;
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

		private void simpleButtonPwsAction_Click(object sender, EventArgs e) {


           // if (timerRapidFire.Enabled)
            if (timerWU.Enabled)
            {
                CancelWeather("User canceled.");
            }
            else
            {
                AutoStartWeather();
            }
     	}

        /*
		private void AutoStartRapidFire() {

            var stationName = AppContext.PersistentState.StationNameWeather;
            var stationPassword = AppContext.PersistentState.StationPasswordWeather;
			bool isValid = !String.IsNullOrEmpty(stationPassword) 
				&& !String.IsNullOrEmpty(stationName);

			if (timerRapidFire.Enabled != isValid) {
				if(isValid) {
					StartRapidFire();
					timerRapidFire_Tick(null, null);
				}
			}
		}
        */

		private const string RunningText = "Running";

		private void StartRapidFire() {


			timerRapidFire.Enabled = true;
			AppContext.PersistentState.PwsEnabled = true;




			//labelControlPwsStatus.Text = "Running";
            labelControlPwsStatus.SetPropertyThreadSafe(() => labelControlPwsStatus.Text, RunningText + ", but no server answer");
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
			Log.InfoFormat("PWS rapid fire canceled due to '{0}'.", message);
		}







<<<<<<< HEAD
=======
        private UriBuilder builder_wu = null;
>>>>>>> 0cad5a8d70d0696eeb938de59d096b4c3dd3e023
        private DateTime timeActual;
        private DateTime timeTemp;
        private int count_to_correction = 10000;
        double dc = 0;

        
    	private void timerRapidFire_TickWU(object sender, EventArgs e) {

            int sensor_num;
            sensor_num = AppContext.PersistentState.StationSensorIndexWeather;
            string str_not_corr_sensor = "Not correct sensor selected!";

<<<<<<< HEAD
=======


>>>>>>> 0cad5a8d70d0696eeb938de59d096b4c3dd3e023
            try
            {


                ISensor[] sensors = _deviceConnection.ToArray();

                if (sensors[sensor_num].IsValid)
                {
                    if (labelControlPwsStatus.Text == str_not_corr_sensor)
                    {
                        //labelControlPwsStatus.Text = "Correct sensor selected";
                        labelControlPwsStatus.SetPropertyThreadSafe(() => labelControlPwsStatus.Text, "Correct sensor selected");
                    }
                }
                else
                {
                    // CancelWindFinder("Not correct sensor selected!");
                    labelControlPwsStatus.SetPropertyThreadSafe(() => labelControlPwsStatus.Text, str_not_corr_sensor);
                    return;
                }




                var reading = sensors[sensor_num].GetCurrentReading();




                var id = AppContext.PersistentState.StationNameWeather;
                var stationPassword = AppContext.PersistentState.StationPasswordWeather;



                Dictionary<string, string> queryParams = new Dictionary<string, string>();
                queryParams.Add("action", "updateraw");
                queryParams.Add("ID", id);
                queryParams.Add("PASSWORD", stationPassword);





                //timeActual = DateTime.UtcNow;
<<<<<<< HEAD
                timeActual = timeTemp.AddMilliseconds(5000);//pov5000
                timeTemp = timeActual;
                timerWU.Interval = 5000;//pov2500
=======
                timeActual = timeTemp.AddMilliseconds(2500);
                timeTemp = timeActual;
                timerWU.Interval = 2500;
>>>>>>> 0cad5a8d70d0696eeb938de59d096b4c3dd3e023

                

                // is time to time correction ?
<<<<<<< HEAD
                if (++count_to_correction > 200) //2.5*200[s]
=======
                if (++count_to_correction > 10)
>>>>>>> 0cad5a8d70d0696eeb938de59d096b4c3dd3e023
                {
                    count_to_correction = 0;

                    dc = GetTimeCorrection(DateTime.UtcNow);



                    // Error by time snchronization ?
                    if (dc == -123456)
                    {
                       // timeActual = DateTime.UtcNow.AddMilliseconds(0);
                    }
                    else 
                    {
                        timeActual = DateTime.UtcNow.AddMilliseconds(dc);
                    }

                    timeTemp = timeActual;

<<<<<<< HEAD


                    File.AppendAllText("weatherlog_out.txt", timeTemp + 
                                               " - TIME CORRECTION: " + dc.ToString() + 
                                                 Environment.NewLine);
=======
                    //     File.AppendAllText("weatherlog_out.txt",
                    //                           "TIME CORRECTION: " + dc.ToString() + 
                    //                             Environment.NewLine);
>>>>>>> 0cad5a8d70d0696eeb938de59d096b4c3dd3e023

                }

                
                
               



                string datetimestr = "";
                datetimestr += timeActual.ToString("yyyy-MM-dd+");
                datetimestr += timeActual.ToString("HH:");// + "%3A";
                datetimestr += timeActual.ToString("mm:");// + "%3A";
                datetimestr += timeActual.ToString("ss");

                queryParams.Add("dateutc", datetimestr); // format casu: priklad 18:50:20


                if (reading.IsWindDirectionValid && reading.WindDirection >= 0 && reading.WindDirection <= 360.0)
                {
                    queryParams.Add("winddir", ((int)(reading.WindDirection)).ToString());
                }



                if (reading.IsWindSpeedValid)
                {
                    var speedConverter = ReadingValuesConverterCache<Reading>.SpeedCache
                        .Get(sensors[sensor_num].SpeedUnit, SpeedUnit.MilesPerHour);
                    var speed = speedConverter.Convert(reading.WindSpeed);
                    queryParams.Add("windspeedmph", speed.ToString());
                }

<<<<<<< HEAD

=======
>>>>>>> 0cad5a8d70d0696eeb938de59d096b4c3dd3e023
              //  queryParams.Add("windgustmph", "0,0");
                if (reading.IsWindSpeedValid)
                {
                    queryParams.Add("windgustmph", _advancedSensorValues[sensor_num].m_windgust_WU.ToString() );
                }

                if (reading.IsTemperatureValid)
                {
                    var tempConverter = ReadingValuesConverterCache<Reading>.TemperatureCache
                        .Get(sensors[sensor_num].TemperatureUnit, TemperatureUnit.Fahrenheit);
                    var temperature = tempConverter.Convert(reading.Temperature);
                    queryParams.Add("tempf", temperature.ToString());
                }

              //  queryParams.Add("rainin", "");

                if (reading.IsPressureValid)
                {
                    var pressConverter = ReadingValuesConverterCache<Reading>.PressCache
                        .Get(sensors[sensor_num].PressureUnit, PressureUnit.InchOfMercury);
                    var pressure = pressConverter.Convert(reading.Pressure);
                    queryParams.Add("baromin", pressure.ToString());
                }
<<<<<<< HEAD
                else
                {
                    queryParams.Add("baromin", "NaN");
                }
=======
>>>>>>> 0cad5a8d70d0696eeb938de59d096b4c3dd3e023

                if (reading.IsHumidityValid && reading.IsTemperatureValid)
                {
                    var tempConverterCelcius = ReadingValuesConverterCache<Reading>.TemperatureCache
                        .Get(sensors[sensor_num].TemperatureUnit, TemperatureUnit.Celsius);
                    var tempConverterCToF = ReadingValuesConverterCache<Reading>.TemperatureCache
                        .Get(TemperatureUnit.Celsius, TemperatureUnit.Fahrenheit);

                    var tempC = tempConverterCelcius.Convert(reading.Temperature);
                    var dewPointC = DewPointCalculator.DewPoint(tempC, reading.Humidity);
                    var dewPointF = tempConverterCToF.Convert(dewPointC);
                    queryParams.Add("dewptf", dewPointF.ToString());
                }
<<<<<<< HEAD
                else
                {
                    queryParams.Add("dewptf", "NaN");
                }

=======
>>>>>>> 0cad5a8d70d0696eeb938de59d096b4c3dd3e023



                if (reading.IsHumidityValid)
                {
                    queryParams.Add("humidity", (reading.Humidity * 100.0).ToString());
                }
<<<<<<< HEAD
                else
                {
                    queryParams.Add("humidity", "NaN");
                }
=======
>>>>>>> 0cad5a8d70d0696eeb938de59d096b4c3dd3e023

//               queryParams.Add("weather", "");

//               queryParams.Add("clouds", "");


                queryParams.Add(
                    "softwaretype",
                    "Atmo"
                );

                queryParams.Add("realtime", "1");


                // pokus z pov 2.5
<<<<<<< HEAD
                queryParams.Add("rtfreq", "5,0"); //pov2.5


/* rp
 * old
 *             if (builder_wu == null)
                {
                    builder_wu = new UriBuilder("http://rtupdate.wunderground.com/weatherstation/updateweatherstation.php");
                }
*/

                UriBuilder builder_wu;
                builder_wu = new UriBuilder("http://rtupdate.wunderground.com/weatherstation/updateweatherstation.php");
=======
                queryParams.Add("rtfreq", "2,5");


                if (builder_wu == null)
                {
                    builder_wu = new UriBuilder("http://rtupdate.wunderground.com/weatherstation/updateweatherstation.php");
                }
>>>>>>> 0cad5a8d70d0696eeb938de59d096b4c3dd3e023

                builder_wu.Query = String.Join(
                    "&",
                    queryParams
                        .Select(kvp => String.Concat(Uri.EscapeDataString(kvp.Key), '=', Uri.EscapeDataString(kvp.Value)))
                        .ToArray()
                );
<<<<<<< HEAD


                try
                {

                    var reqSent = HttpWebRequest.Create(builder_wu.Uri);
                    reqSent.BeginGetResponse(HandleRapidFireResult, reqSent);






                    //rp
                    //debug
                    //Log.Warn("WA - sendet packet: " + builder_wu.Uri);





                    // rp
                    // for statistic
                    _internetStreamingStatistics.m_StreamingServers[0].IncrementSendetPacekets();

=======
>>>>>>> 0cad5a8d70d0696eeb938de59d096b4c3dd3e023


                try
                {

                    var reqSent = HttpWebRequest.Create(builder_wu.Uri);
                    reqSent.BeginGetResponse(HandleRapidFireResult, reqSent);

                }
                catch (Exception ex)
                {
<<<<<<< HEAD
                    Log.Warn("PWS rapid fire failure.1.", ex);
=======
                    Log.Warn("PWS rapid fire failure.", ex);
>>>>>>> 0cad5a8d70d0696eeb938de59d096b4c3dd3e023

                }
         
            
            }
            catch (Exception ex)
            {
<<<<<<< HEAD
                Log.Warn("PWS rapid fire failure.2.", ex);
=======
                Log.Warn("PWS rapid fire failure.", ex);
>>>>>>> 0cad5a8d70d0696eeb938de59d096b4c3dd3e023
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
				try {
					if (_deviceConnection is IDisposable) {
						(_deviceConnection as IDisposable).Dispose();
					}
				}
				catch (Exception ex) {
					Log.Warn("Shutdown problem: device connection.", ex);
				}

				try {
					if (_dbStore is IDisposable) {
						(_dbStore as IDisposable).Dispose();
					}
				}
				catch (Exception ex) {
					Log.Warn("Shutdown problem: database.", ex);
				}
			}
			base.Dispose(disposing);
		}

		private void simpleButtonTimeSync_Click(object sender, EventArgs e) {
			ShowTimeSyncDialog();
		}

		private void simpleButtonTempSource_Click(object sender, EventArgs e) {
			var newState = !_deviceConnection.UsingDaqTemp;
			_deviceConnection.UseDaqTemp(newState);
		}

		private void HandleDaqTemperatureSourceSet(bool isDaqTempSource) {
			string text;
			Image image;
			if (isDaqTempSource) {
				text = "Temperature Sensor: DAQ (click to change)";
				image = Properties.Resources.Temp_Sensor_01;
			}
			else {
				text = "Temperature Sensor: Anemometer (click to change)";
				image = Properties.Resources.Temp_Sensor_02;
			}
			simpleButtonTempSource.SetPropertyThreadSafe(() => simpleButtonTempSource.Text, text);
			simpleButtonTempSource.SetPropertyThreadSafe(() => simpleButtonTempSource.Image, image);
		}

		private void barButtonItemAbout_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
			var aboutForm = new AboutForm();
			aboutForm.ShowDialog(this);
		}



		private void backgroundWorkerLiveGraph_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e) {
			UpdateLiveGraph();
		}

		public void UpdateLiveGraph() {

			HandleDaqTemperatureSourceSet(_deviceConnection.UsingDaqTemp);
			// current live state
			var now = DateTime.Now;
			var sensors = _deviceConnection.Where(s => s.IsValid).ToList();
			// get readings
			var readings = new Dictionary<ISensor, IReading>();
			foreach (var sensor in sensors) {
				var reading = sensor.GetCurrentReading();
				if (reading.IsValid) {
					readings.Add(sensor, reading);
				}
			}

			// save to memory
			foreach (var reading in readings) {
				if (!_memoryDataStore.GetAllSensorInfos().Any(si => si.Name.Equals(reading.Key.Name))) {
					_memoryDataStore.AddSensor(reading.Key);
				}
				_memoryDataStore.Push(reading.Key.Name, new[]{reading.Value});
			}

			// the current sensor views
			var sensorViews = _sensorViewPanelControler.Views.ToList();

			// determine which sensors are enabled
			var enabledSensors = new List<ISensor>();
			for (int i = 0; i < sensors.Count && i < sensorViews.Count; i++) {
				if (sensorViews[i].IsSelected) {
					enabledSensors.Add(sensors[i]);
				}
			}

			var liveDataTimeSpan = liveAtmosphericHeader.TimeRange.SelectedSpan;
			TimeSpan liveUpdateInterval;
			if(liveDataTimeSpan > new TimeSpan(1,0,0))
				liveUpdateInterval = new TimeSpan(0,1,0);
			else if(liveDataTimeSpan >= new TimeSpan(1,0,0))
				liveUpdateInterval = new TimeSpan(0,0,15);
			else
				liveUpdateInterval = new TimeSpan(0,0,0,0,500);

			var liveDataEnabled =
				(_lastLiveUpdateInterval != liveUpdateInterval)
				|| (now - liveUpdateInterval > _lastLiveUpdate);

			_lastLiveUpdateInterval = liveUpdateInterval;

			// pass it off to the live data graphs/tables
			if (liveDataEnabled) {
				_lastLiveUpdate = now;
				TimeUnit meanLiveUnit;
				if(liveDataTimeSpan >= new TimeSpan(1,0,0))
					meanLiveUnit = TimeUnit.Minute;
				else
					meanLiveUnit = TimeUnit.Second;

				// gather the data for each selected sensor
				var enabledSensorsLiveMeans = new List<List<ReadingAggregate>>(enabledSensors.Count);
				foreach (var sensor in enabledSensors) {

					// get the recent readings
					var recentReadings = _memoryDataStore.GetReadings(sensor.Name, now, TimeSpan.Zero.Subtract(liveDataTimeSpan));

					// calculate the mean data
					var means = StatsUtil.AggregateMean(recentReadings, meanLiveUnit).ToList();

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
				liveAtmosphericGraph.Invoke(new Action(() => {
					liveAtmosphericGraph.TemperatureUnit = TemperatureUnit;
					liveAtmosphericGraph.PressureUnit = PressureUnit;
					liveAtmosphericGraph.SpeedUnit = SpeedUnit;
					liveAtmosphericGraph.FormatTimeAxis(liveDataTimeSpan);
					liveAtmosphericGraph.SetLatest(enabledSensorsCompiledMeans.LastOrDefault());
					liveAtmosphericGraph.State = AppContext.PersistentState;
					liveAtmosphericGraph.SetDataSource(enabledSensorsCompiledMeans);
					// update the sensor controls
				}));

			}

			Invoke(new Action(() => { _sensorViewPanelControler.UpdateView(sensors, AppContext.PersistentState); }));
		}

		private void backgroundWorkerLiveGraph_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e) {
			//System.Diagnostics.Debug.WriteLine("Live Data End");
		}

		public void RequestHistoricalUpdate() {
			if(Monitor.TryEnter(_historicalUpdateThreadMutex)) {
				Monitor.Exit(_historicalUpdateThreadMutex);
				ThreadPool.QueueUserWorkItem(HistoricalDataUpdate);
			}
		}

		private void HistoricalDataUpdate(object junk) {
			if(!Visible) {
				lock (_historicalUpdateThreadMutex) {
					Thread.Sleep(100);
				}
				RequestHistoricalUpdate();
				return;
			}

			lock (_historicalUpdateThreadMutex) {
				System.Diagnostics.Debug.WriteLine("Begin Historical Data");

				var now = DateTime.Now;
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

				var historicalSelected =
					_dbStore.GetAllSensorInfos()
					.Where(si => _historicSensorViewPanelController.IsSensorSelected(si))
					.ToList();

				var sensorReadings = historicalSelected.Select(
					sensor =>
					_dbStore.GetReadingSummaries(
						sensor.Name, cumTimeInfo.MaxStamp,
						cumTimeInfo.MinStamp - cumTimeInfo.MaxStamp,
					    UnitUtility.ChooseBestSummaryUnit(histTimeSpan)
					)
				);

				var historicalSummaries = StatsUtil
					.JoinReadingSummaryEnumerable(sensorReadings)
					.ToList();

				if (!IsDisposed) {
					BeginInvoke(new Action(() => {
					    windResourceGraph.TemperatureUnit = TemperatureUnit;
					    windResourceGraph.PressureUnit = PressureUnit;
					    windResourceGraph.SpeedUnit = SpeedUnit;


					    windResourceGraph.SetDataSource(historicalSummaries);

					    historicalGraphBreakdown.TemperatureUnit = TemperatureUnit;
					    historicalGraphBreakdown.PressureUnit = PressureUnit;
					    historicalGraphBreakdown.SpeedUnit = SpeedUnit;
					    historicalGraphBreakdown.StepBack = !histNowChk.Checked;
					    historicalGraphBreakdown.DrillStartDate = histStartDate;
					    historicalGraphBreakdown.CumulativeTimeSpan = histTimeSpan;

					    historicalGraphBreakdown.SetDataSource(historicalSummaries);
					}));
				}

			}
		}

		private IEnumerable<PackedReading> GenerateFakeReadings(TimeSpan backSpan, DateTime timeFrom) {
			var oneSecond = new TimeSpan(0, 0, 0, 1);
			var timeTo = timeFrom - backSpan;
			var rand = new Random((int)DateTime.Now.Ticks);
			IReadingValues lastReading = null;
			for (var curTime = timeTo; curTime <= timeFrom; curTime += oneSecond) {
				var curReading = Demo.DemoDaqConnection.DemoSensor.GetCurrentReading(curTime, rand, lastReading);
				yield return curReading;
				lastReading = curReading;
			}
			yield break;
		}

		private void barButtonItemAdd24Hours_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
			DateTime now = DateTime.Now;
			const int maxSensors = 2;
			var sensors = _deviceConnection.Take(maxSensors).ToList();
			foreach (var sensor in sensors) {
				var span = new TimeSpan(0, 24, 0, 0);
				var readings = GenerateFakeReadings(span, now);
				_memoryDataStore.Push(sensor.Name, readings.Select(r => new Reading(r)));
				Log.DebugFormat("Added {0} of data to '{1}' ({2}).", span, sensor.Name, sensor);
			}
		}

		private void barButtonItemFirmwareUpdateV2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
			if(!(_deviceConnection is UsbDaqConnection)) {
				MessageBox.Show("Device is not supported.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			try {
				timerQueryTime.Stop();
				timerLive.Stop();
				var patchForm = new PatcherForm2(_deviceConnection as UsbDaqConnection);
				patchForm.ShowDialog();
			}
			catch(Exception ex) {
				Log.Error("Firmware update failed.", ex);
			}
			finally {
				timerLive.Start();
				timerQueryTime.Start();
			}
		}



        //rp>>
        private void StartWindFinder()
        {
            AppContext.PersistentState.PwfEnabled = true;


            pom_inverval = 60 * 60;
            timerWindFinder.Enabled = true;

            //labelControlPwsStatus.Text = "Running";
            labelControlWindFinder.SetPropertyThreadSafe(() => labelControlWindFinder.Text, RunningText);
            //simpleButtonPwsAction.Text = "Running";
            simpleButtonWindFinderAction.SetPropertyThreadSafe(() => simpleButtonWindFinderAction.Text, RunningText + " (click to stop).");
            //simpleButtonPwsAction.BackColor = Color.LightGreen;
            simpleButtonWindFinderAction.SetPropertyThreadSafe(() => simpleButtonWindFinderAction.ForeColor, ForeColor);

        }

        //rp>>
        private void StartAwekas()
        {
            AppContext.PersistentState.PawEnabled = true;


            pom_inverval_aw = 60 * 60;
            timerAwekas.Enabled = true;

            //labelControlPwsStatus.Text = "Running";
            labelControlAwekas.SetPropertyThreadSafe(() => labelControlAwekas.Text, RunningText + ", but no server answer");
            //simpleButtonPwsAction.Text = "Running";
            simpleButtonAwekas.SetPropertyThreadSafe(() => simpleButtonAwekas.Text, RunningText + " (click to stop).");
            //simpleButtonPwsAction.BackColor = Color.LightGreen;
            simpleButtonAwekas.SetPropertyThreadSafe(() => simpleButtonAwekas.ForeColor, ForeColor);

       }

        //rp>>
        public int pom_inverval_weather = 0;
        private void StartWeather()
        {

            
            AppContext.PersistentState.WeatherEnabled = true;


            pom_inverval_weather = 60 * 60;

            timerWU.Enabled = true;
            
            timeActual = DateTime.UtcNow;
            timeTemp = timeActual;
            
            //labelControlPwsStatus.Text = "Running";
            labelControlPwsStatus.SetPropertyThreadSafe(() => labelControlPwsStatus.Text, RunningText + ", but no server answer");
            //simpleButtonPwsAction.Text = "Running";
            simpleButtonPwsAction.SetPropertyThreadSafe(() => simpleButtonPwsAction.Text, RunningText + " (click to stop).");
            //simpleButtonPwsAction.BackColor = Color.LightGreen;
            simpleButtonPwsAction.SetPropertyThreadSafe(() => simpleButtonPwsAction.ForeColor, ForeColor);

        }
        private void CancelWeather(string message)
        {
            //timerRapidFire.Enabled = false;

            timerWU.Enabled = false;

            //rp !!!!!!!!!!!!
            //  AppContext.PersistentState.PwfEnabled = false;
            
            //labelControlPwsStatus.Text = message;
            labelControlPwsStatus.SetPropertyThreadSafe(() => labelControlPwsStatus.Text, message);
            //simpleButtonPwsAction.Text = "Disabled";
            simpleButtonPwsAction.SetPropertyThreadSafe(() => simpleButtonPwsAction.Text, "Stopped (click to start).");
            //simpleButtonPwsAction.BackColor = Color.LightPink;
            simpleButtonPwsAction.SetPropertyThreadSafe(() => simpleButtonPwsAction.ForeColor, Color.Red);
            Log.InfoFormat("Weather canceled due to '{0}'.", message);
        }

        private void CancelWindFinder(string message)
        {
            timerWindFinder.Enabled = false;


          //rp !!!!!!!!!!!!
          //  AppContext.PersistentState.PwfEnabled = false;



            //labelControlPwsStatus.Text = message;
            labelControlWindFinder.SetPropertyThreadSafe(() => labelControlWindFinder.Text, message);
            //simpleButtonPwsAction.Text = "Disabled";
            simpleButtonWindFinderAction.SetPropertyThreadSafe(() => simpleButtonWindFinderAction.Text, "Stopped (click to start).");
            //simpleButtonPwsAction.BackColor = Color.LightPink;
            simpleButtonWindFinderAction.SetPropertyThreadSafe(() => simpleButtonWindFinderAction.ForeColor, Color.Red);
            Log.InfoFormat("WindFinder canceled due to '{0}'.", message);
        }


        private void CancelAwekas(string message)
        {
            timerAwekas.Enabled = false;


            //rp !!!!!!!!!!!!
            //  AppContext.PersistentState.PawEnabled = false;



            //labelControlPwsStatus.Text = message;
            labelControlAwekas.SetPropertyThreadSafe(() => labelControlAwekas.Text, message);
            //simpleButtonPwsAction.Text = "Disabled";
            simpleButtonAwekas.SetPropertyThreadSafe(() => labelControlAwekas.Text, "Stopped (click to start).");
            //simpleButtonPwsAction.BackColor = Color.LightPink;
            simpleButtonAwekas.SetPropertyThreadSafe(() => labelControlAwekas.ForeColor, Color.Red);
            Log.InfoFormat("Awekas canceled due to '{0}'.", message);
        }


        private void simpleButtonWindFinderAction_Click(object sender, EventArgs e)
        {

            if (timerWindFinder.Enabled)
            {
                CancelWindFinder("User canceled.");
            }
            else
            {

              //  AppContext.PersistentState.StationNameWF = "meno";
                //AppContext.PersistentState.StationPasswordWF = "heslo";
                AutoStartWindFinder();

            }

        }



        public int pom_inverval = 0;

        public void timerWindFinder_Tick(object sender, EventArgs e)
        {
            int sensor_num;
            sensor_num = AppContext.PersistentState.StationSensorIndexWF;

            string str_not_corr_sensor = "Not correct sensor selected!";


            try
            {


                ISensor[] sensors = _deviceConnection.ToArray();

                if (sensors[sensor_num].IsValid)
                {
                    if (labelControlWindFinder.Text == str_not_corr_sensor)
                    {
                       //labelControlWindFinder.Text = "Correct sensor selected";
                       labelControlWindFinder.SetPropertyThreadSafe(() => labelControlWindFinder.Text, "Correct sensor selected");
                    }
                }
                else
                {
                    // CancelWindFinder("Not correct sensor selected!");
                    labelControlWindFinder.SetPropertyThreadSafe(() => labelControlWindFinder.Text, str_not_corr_sensor);
                    return;
                }

                pom_inverval++;
                if (pom_inverval < AppContext.PersistentState.StationIntervalWF * 6) return;

                pom_inverval = 0;



                var reading = sensors[sensor_num].GetCurrentReading();

                // Log.DebugFormat("readed from sensor: " + reading.ToString() );
                //   System.Diagnostics.Debug.WriteLine("readed from sensor: " + reading.Humidity.ToString() + " " + reading.Temperature.ToString() +
                //     " " + reading.Pressure.ToString() + " " + reading.TimeStamp.ToString() + " " + reading.WindDirection.ToString() + " " + reading.WindSpeed.ToString());







                Dictionary<string, string> queryParams = new Dictionary<string, string>();
                queryParams.Add("sender_id", AppContext.PersistentState.StationNameWF);
                queryParams.Add("password", AppContext.PersistentState.StationPasswordWF);
                DateTime utcStamp = reading.TimeStamp.ToUniversalTime();
                //queryParams.Add("date", utcStamp.ToString("yyyy-MM-dd hh:mm:ss"));

                queryParams.Add("date", utcStamp.ToString("dd.MM.yyyy"));
                queryParams.Add("time", utcStamp.ToString("hh:mm"));


                if (reading.IsTemperatureValid)
                {
                    var tempConverter = ReadingValuesConverterCache<Reading>.TemperatureCache
                        .Get(sensors[sensor_num].TemperatureUnit, TemperatureUnit.Celsius);
                    var temperature = tempConverter.Convert(reading.Temperature);
                    queryParams.Add("airtemp", temperature.ToString());
                }

                if (reading.IsWindSpeedValid)
                {
                    var speedConverter = ReadingValuesConverterCache<Reading>.SpeedCache
                        .Get(sensors[sensor_num].SpeedUnit, SpeedUnit.MetersPerSec);
                    var speed = speedConverter.Convert(reading.WindSpeed / 3.6);
                    queryParams.Add("windspeed", speed.ToString());
                }

                if (reading.IsWindDirectionValid && reading.WindDirection >= 0 && reading.WindDirection <= 360.0)
                {
                    queryParams.Add("winddir", ((int)(reading.WindDirection)).ToString());
                }


                if (reading.IsPressureValid)
                {
                    var pressConverter = ReadingValuesConverterCache<Reading>.PressCache
                        .Get(sensors[sensor_num].PressureUnit, PressureUnit.KiloPascals);
                    var pressure = pressConverter.Convert(reading.Pressure);
                    queryParams.Add("pressure", pressure.ToString());
                }


                /*
                if (reading.IsHumidityValid)
                {
                    queryParams.Add("humidity", (reading.Humidity * 100.0).ToString());
                }
              
                if (reading.IsHumidityValid && reading.IsTemperatureValid)
                {
                    var tempConverterCelcius = ReadingValuesConverterCache<Reading>.TemperatureCache
                        .Get(sensors[sensor_num].TemperatureUnit, TemperatureUnit.Celsius);
                    var tempConverterCToF = ReadingValuesConverterCache<Reading>.TemperatureCache
                        .Get(TemperatureUnit.Celsius, TemperatureUnit.Fahrenheit);

                    var tempC = tempConverterCelcius.Convert(reading.Temperature);
                    var dewPointC = DewPointCalculator.DewPoint(tempC, reading.Humidity);
                    var dewPointF = tempConverterCToF.Convert(dewPointC);
                    queryParams.Add("dewptf", dewPointF.ToString());
                }
             
              
               */

                var builder = new UriBuilder("http://www.windfinder.com/wind-cgi/httpload.pl?");


                //sender_id="++"&password=&date=19.5.2011&time=17:13&airtemp=20&windspeed=12&gust=14&winddir=180&pressure=1012&rain=5");


                builder.Query = String.Join(
                    "&",
                    queryParams
                        .Select(kvp => String.Concat(Uri.EscapeDataString(kvp.Key), '=', Uri.EscapeDataString(kvp.Value)))
                        .ToArray()
                );
                try
                {
                    var reqSent = HttpWebRequest.Create(builder.Uri);
                    reqSent.BeginGetResponse(HandlWFResult, reqSent);

                    System.Diagnostics.Debug.WriteLine("\r\nRiadok: " + builder.Uri);

                }
                catch (Exception ex)
                {
                    Log.Warn("WindFinder failure.", ex);
                }


            }
            catch (Exception mainex)
            {
                Log.Warn("WindFinder failure main: ", mainex);
            }
             
            




        }

        private void HandleRapidFireResult(IAsyncResult result)
        {

            var req = (WebRequest)result.AsyncState;
            try
            {
                using (WebResponse res = req.EndGetResponse(result))
                {
                    using (var reader = new StreamReader(res.GetResponseStream()))
                    {
                        string responseMessage = reader.ReadToEnd().ToUpperInvariant();


                        // rp
                        // for statistic
                        _internetStreamingStatistics.m_StreamingServers[0].IncrementReadedPacekets();
                        



                        if (responseMessage.StartsWith("SUCCESS"))
                        {
                            ; // OK
                            labelControlPwsStatus.SetPropertyThreadSafe(() => labelControlPwsStatus.Text, "Running");

                        }
                        else if (responseMessage.Contains("PASSWORD"))
                        {

                            //rp??????????????? - pokus aby neskoncil pri chybe passwordu a poracoval dalej
                            // CancelRapidFire("ID or password is invalid.");
                            labelControlPwsStatus.SetPropertyThreadSafe(() => labelControlPwsStatus.Text, "ID or password is invalid.");

                        }
                        else
                        {
                            labelControlPwsStatus.SetPropertyThreadSafe(() => labelControlPwsStatus.Text, "Weather underground  protocol error.");
//                            CancelRapidFire("PWS protocol error.");
                        }
                    }
                }
            }
            catch (WebException webEx)
            {
                //CancelRapidFire("PWS Communication failure.");
                labelControlPwsStatus.SetPropertyThreadSafe(() => labelControlPwsStatus.Text, "Connection error!");

                Log.Warn("Weather underground rapid fire was disabled due to an error.", webEx);
            }
        }
        
        private void HandleAwResult(IAsyncResult result)
        {
            var req = (WebRequest)result.AsyncState;
            try
            {
                using (WebResponse res = req.EndGetResponse(result))
                {
                    using (var reader = new StreamReader(res.GetResponseStream()))
                    {
                        string responseMessage = reader.ReadToEnd().ToUpperInvariant();



                        if (responseMessage.Contains("OK"))
                        {
                            ; // OK


                            DateTime current = DateTime.Now;
                            System.Diagnostics.Debug.WriteLine(current.ToShortTimeString() + "Prijate AWEKAS: " + responseMessage);
   //                         MessageBox.Show("AWEKAS PRISLO OK - ("+current.ToShortTimeString() + ")"  + responseMessage);

                            labelControlAwekas.SetPropertyThreadSafe(() => labelControlAwekas.Text, "Running");

                        }
                        else if (responseMessage.Contains("PASSWORT"))
                        {
                            CancelAwekas("Username or password is invalid.");
                        }
                        else
                        {
                            labelControlAwekas.SetPropertyThreadSafe(() => labelControlAwekas.Text, "Awekas protocol error.");
                        //    CancelAwekas("Awekas protocol error.");
                        }
                    }
                }
            }
            catch (WebException webEx)
            {
                // CancelAwekas("Awekas Communication failure.");
                //rp
                labelControlAwekas.SetPropertyThreadSafe(() => labelControlAwekas.Text, "Connection error!");

                Log.Warn("Awekas was disabled due to an error.", webEx);
            }
        }

        private void HandlWFResult(IAsyncResult result)
        {
            var req = (WebRequest)result.AsyncState;
            try
            {
                using (WebResponse res = req.EndGetResponse(result))
                {
                    using (var reader = new StreamReader(res.GetResponseStream()))
                    {
                        string responseMessage = reader.ReadToEnd().ToUpperInvariant();
                        if (responseMessage.Contains("OK\n"))
                        {
                            //                            MessageBox.Show(responseMessage);
                            System.Diagnostics.Debug.WriteLine("WINDFINDER - OK - responseMessage: " + responseMessage);
                            labelControlWindFinder.SetPropertyThreadSafe(() => labelControlWindFinder.Text, "Running");
                        }
                        else if (responseMessage.Contains("PASSWORD"))
                        {
                            CancelWindFinder("Station ID or password is invalid.");
                        }
                        else
                        {
                            //CancelWindFinder("WindFinder protocol error.");
                            labelControlWindFinder.SetPropertyThreadSafe(() => labelControlWindFinder.Text, "WindFinder protocol error.");

                            System.Diagnostics.Debug.WriteLine("WINDFINDER - responseMessage: " + responseMessage);
                        }
                    }
                }
            }
            catch (WebException webEx)
            {
                //CancelWindFinder("WindFinder Communication failure.");
                Log.Warn("WindFinder was disabled due to an error.", webEx);

                labelControlWindFinder.SetPropertyThreadSafe(() => labelControlWindFinder.Text, "Connection error!");
                

            }
        }



        private void AutoStartWeather()
        {
                        
            var stationName = AppContext.PersistentState.StationNameWeather;
            var stationPassword = AppContext.PersistentState.StationPasswordWeather;
            bool isValid = !String.IsNullOrEmpty(stationName) &&
                !String.IsNullOrEmpty(stationPassword);


            //MessageBox.Show(stationName + stationPassword);

            //     if (timerRapidFire.Enabled != isValid)
            if (timerWU.Enabled != isValid)
            
            {
                if (isValid)
                {
                    StartWeather();
                    this.pom_inverval_weather = 60 * 60;  // aby to preslo cez casovac prvy krat
                    timerRapidFire_TickWU(null, null);
                }
            }
              
        }


        private void AutoStartAwekas()
        {
            var stationName = AppContext.PersistentState.StationNameAw;
            var stationPassword = AppContext.PersistentState.StationPasswordAw;
            bool isValid = !String.IsNullOrEmpty(stationName) &&
                !String.IsNullOrEmpty(stationPassword);


            //MessageBox.Show(stationName + stationPassword);

            if (timerAwekas.Enabled != isValid)
            {
                if (isValid)
                {
                    StartAwekas();
                    this.pom_inverval_aw = 60 * 60;  // aby to preslo cez casovac prvy krat
                    timerAwekas_Tick(null, null);
                }
            }
        }

   		private void AutoStartWindFinder() {
			var stationName = AppContext.PersistentState.StationNameWF;
            var stationPassword = AppContext.PersistentState.StationPasswordWF;
            bool isValid = !String.IsNullOrEmpty(stationName) && 
				!String.IsNullOrEmpty(stationPassword);


            //MessageBox.Show(stationName + stationPassword);

            if (timerWindFinder.Enabled != isValid)
            {
				if(isValid) {
                    StartWindFinder();
                    this.pom_inverval = 60 * 60;
                    timerWindFinder_Tick(null, null);
				}
			}
		}

        private void simpleButtonAwekasaction_Click(object sender, EventArgs e)
        {

           if (timerAwekas.Enabled)
            {
                CancelAwekas("User canceled.");
            }
            else
            {

                AutoStartAwekas();

            }
        }

        public int pom_inverval_aw = 0;
        private void timerAwekas_Tick(object sender, EventArgs e)
        {

            string str_not_corr_sensor = "Not correct sensor selected!";

            int sensor_num;





            try
            {





                sensor_num = AppContext.PersistentState.StationSensorIndexAw;


                ISensor[] sensors = _deviceConnection.ToArray();

                if (sensors[sensor_num].IsValid)
                {
                    if (labelControlAwekas.Text == str_not_corr_sensor)
                    {
                        labelControlAwekas.SetPropertyThreadSafe(() => labelControlAwekas.Text, "Correct sensor selected");
                    }
                }
                else
                {
                    // CancelAwekas("Not correct sensor selected!");
                    labelControlAwekas.SetPropertyThreadSafe(() => labelControlAwekas.Text, str_not_corr_sensor);

                    return;
                }

                pom_inverval_aw++;

                var reading = sensors[sensor_num].GetCurrentReading();



                DateTime current = DateTime.Now;
                //System.Diagnostics.Debug.WriteLine(current.ToShortTimeString() + "Timer prerusenie - AWEKAS: " + pom_inverval_aw.ToString() );




                if (pom_inverval_aw < (AppContext.PersistentState.StationIntervalAW * 6)) return;
                pom_inverval_aw = 0;




                DateTime utcStamp = reading.TimeStamp.ToUniversalTime();
                string str_temp = "";
                if (reading.IsTemperatureValid)
                {
                    var tempConverter = ReadingValuesConverterCache<Reading>.TemperatureCache
                        .Get(sensors[sensor_num].TemperatureUnit, TemperatureUnit.Celsius);
                    var temperature = tempConverter.Convert(reading.Temperature);
                    str_temp = temperature.ToString();
                }
                string str_hum = "";
                if (reading.IsHumidityValid)
                {
                    str_hum = (reading.Humidity * 100.0).ToString();
                }
                string str_press = "";
                if (reading.IsPressureValid)
                {
                    var pressConverter = ReadingValuesConverterCache<Reading>.PressCache
                        .Get(sensors[sensor_num].PressureUnit, PressureUnit.Pascals);
                    var pressure = pressConverter.Convert(reading.Pressure)/100;
                    str_press = pressure.ToString();
                }
                string str_windspeed = "";
                if (reading.IsWindSpeedValid)
                {
                    var speedConverter = ReadingValuesConverterCache<Reading>.SpeedCache
                        .Get(sensors[sensor_num].SpeedUnit, SpeedUnit.MetersPerSec);
                    var speed = speedConverter.Convert(reading.WindSpeed / 3.6);
                    str_windspeed = speed.ToString();
                }


                string str_windir = "";
                if (reading.IsWindDirectionValid && reading.WindDirection >= 0 && reading.WindDirection <= 360.0)
                {
                    str_windir = ((int)(reading.WindDirection)).ToString();
                }

                var builder = new UriBuilder("http://www.awekas.at/extern/eingabe_pruefung.php?val=" + AppContext.PersistentState.StationNameAw.ToString() + ";" +
                      CalculateMD5Hash(AppContext.PersistentState.StationPasswordAw.ToString()) + ";" +
                      utcStamp.ToString("dd.MM.yyyy") + ";" +
                      utcStamp.ToString("hh:mm") + ";" +
                      str_temp + ";" +
                      str_hum + ";" +
                      str_press + ";" +
                      ";" + // precipiation 
                      str_windspeed + ";" +
                      str_windir + ";" +
                      ";" + // 11
                      ";" + // 12
                      ";" + // 13
                      "en" + ";" +//14
                      "0" + ";" +//15
                      "0" + ";" //16

                );



                current = DateTime.Now;
                System.Diagnostics.Debug.WriteLine(current.ToShortTimeString() + "Poslane AWEKAS: " + builder.Uri);


                try
                {
                    var reqSent = HttpWebRequest.Create(builder.Uri);
                    reqSent.BeginGetResponse(HandleAwResult, reqSent);

                }
                catch (Exception ex)
                {
                   Log.Warn("Awekas failure.", ex );
                }



            }
            catch (Exception exmain)
            {
                Log.Warn("Awekas failure main: ", exmain );
            }

        }






        private double GetTimeCorrection( DateTime nDT)
        {
            //rp - time sychronization
<<<<<<< HEAD
            string ntpServer1 = "pool.ntp.org";
            string ntpServer2 = "time.windows.com";

            double correct = 0;

            DateTime dt = GetNetworkTime(ntpServer1);
=======

            DateTime dt = GetNetworkTime();
            double correct=0;
>>>>>>> 0cad5a8d70d0696eeb938de59d096b4c3dd3e023

            // nepodarilo sa synchronizovat
            if (dt.Year == 1905)
            {
<<<<<<< HEAD
//                return -123456;
                dt = GetNetworkTime(ntpServer2);
            }

            // podarilo sa synchronizovat
            if (dt.Year != 1905)
            {
                correct = ( dt - nDT ).TotalMilliseconds;
                return correct;
            }

            return -123456;

=======
                return -123456;
            }

            // podarilo sa synchronizovat
            else
            {
                correct = ( dt - nDT ).TotalMilliseconds;
            }

            return correct;
>>>>>>> 0cad5a8d70d0696eeb938de59d096b4c3dd3e023
        }




<<<<<<< HEAD
        public DateTime GetNetworkTime( string ntpServer)
=======
        public DateTime GetNetworkTime()
>>>>>>> 0cad5a8d70d0696eeb938de59d096b4c3dd3e023
        {

            try
            {

                //default Windows time server
<<<<<<< HEAD
                //const string ntpServer = "time.windows.com";
                //const string ntpServer = "pool.ntp.org";
=======
                const string ntpServer = "time.windows.com";
>>>>>>> 0cad5a8d70d0696eeb938de59d096b4c3dd3e023

                // NTP message size - 16 bytes of the digest (RFC 2030)
                var ntpData = new byte[48];

                //Setting the Leap Indicator, Version Number and Mode values
                ntpData[0] = 0x1B; //LI = 0 (no warning), VN = 3 (IPv4 only), Mode = 3 (Client Mode)

                var addresses = Dns.GetHostEntry(ntpServer).AddressList;

                //The UDP port number assigned to NTP is 123
                var ipEndPoint = new IPEndPoint(addresses[0], 123);
                //NTP uses UDP
                var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                //rp
//                socket.SendTimeout = ;
<<<<<<< HEAD
                socket.ReceiveTimeout = 1000;//pov200
                socket.Connect(ipEndPoint);
                socket.Send(ntpData);
=======
                socket.ReceiveTimeout = 200;
                socket.Connect(ipEndPoint);
                socket.Send(ntpData);
                //socket.set
>>>>>>> 0cad5a8d70d0696eeb938de59d096b4c3dd3e023
                socket.Receive(ntpData);
                socket.Close();

                //Offset to get to the "Transmit Timestamp" field (time at which the reply 
                //departed the server for the client, in 64-bit timestamp format."
                const byte serverReplyTime = 40;

                //Get the seconds part
                ulong intPart = BitConverter.ToUInt32(ntpData, serverReplyTime);

                //Get the seconds fraction
                ulong fractPart = BitConverter.ToUInt32(ntpData, serverReplyTime + 4);

                //Convert From big-endian to little-endian
                intPart = SwapEndianness(intPart);
                fractPart = SwapEndianness(fractPart);

                var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);

                //**UTC** time
                var networkDateTime = (new DateTime(1900, 1, 1)).AddMilliseconds((long)milliseconds);

                return networkDateTime;
            }
            catch (Exception ex)
            {
              //  MessageBox.Show("Error with time synchronization !!!");

                return new DateTime(1905, 2, 2, 2, 2, 2);
            }
        }

        // stackoverflow.com/a/3294698/162671
        static uint SwapEndianness(ulong x)
        {
            return (uint)(((x & 0x000000ff) << 24) +
                           ((x & 0x0000ff00) << 8) +
                           ((x & 0x00ff0000) >> 8) +
                           ((x & 0xff000000) >> 24));
        }

<<<<<<< HEAD

=======
        private void timerSynchronizeTime_Tick(object sender, EventArgs e)
        {



        }
>>>>>>> 0cad5a8d70d0696eeb938de59d096b4c3dd3e023


	}

}
