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
using System.Linq;
using System.Windows.Forms;
using Atmo.Data;
using Atmo.Stats;
using Atmo.UI.WinForms.Controls;
using Atmo.Units;

namespace Atmo.UI.DevEx {
	public partial class MainForm : Form {

		private IDaqConnection _deviceConnection = null;
		private MemoryDataStore _memoryDataStore = null;
		private SensorViewPanelController _sensorViewPanelControler = null;

		public MainForm() {
			InitializeComponent();

			_deviceConnection = new Demo.DemoDaqConnection();

			_memoryDataStore = new MemoryDataStore();

			_sensorViewPanelControler = new SensorViewPanelController(panelSensors) {
				DefaultSelected = true
			};

		}

		public TemperatureUnit TemperatureUnit { get; set; }
		public SpeedUnit SpeedUnit { get; set; }
		public PressureUnit PressureUnit { get; set; }

		private void timerTesting_Tick(object sender, EventArgs e) {
			
			// current live state
			var now = DateTime.Now;
			var sensors = _deviceConnection.ToList();

			// get readings
			var readings = new Dictionary<ISensor, IReading>();
			foreach (var sensor in sensors) {
				var reading = sensor.GetCurrentReading();
				if(reading.IsValid) {
					readings.Add(sensor, reading);
				}
			}

			// save to memory
			foreach(KeyValuePair<ISensor, IReading> reading in readings) {
				if(!_memoryDataStore.GetAllSensorInfos().Any(si => si.Name.Equals(reading.Key.Name))) {
					_memoryDataStore.AddSensor(reading.Key);
				}
				_memoryDataStore.Push(reading.Key.Name, new [] { reading.Value });
			}

			// update the sensor controls
			_sensorViewPanelControler.UpdateView(sensors);

			// the current sensor views
			var sensorViews = _sensorViewPanelControler.SensorViews.ToList();

			// determine which sensors are enabled
			var enabledSensors = new List<ISensor>();
			for(int i = 0; i < sensors.Count && i < sensorViews.Count; i++) {
				if(sensorViews[i].IsSelected) {
					enabledSensors.Add(sensors[i]);
				}
			}
			
			var liveDataEnabled = true;
			var liveDataTimeSpan = new TimeSpan(0, 1, 0);

			if(liveDataEnabled) {
				var enabledSensorsLiveMeans = new List<List<ReadingAggregate>>(enabledSensors.Count);
				foreach(var sensor in enabledSensors) {
					var recentReadings = _memoryDataStore.GetReadings(sensor.Name, now, TimeSpan.Zero.Subtract(liveDataTimeSpan));
					var means = StatsUtil.AggregateMean(recentReadings, TimeUnit.Second).ToList();
					enabledSensorsLiveMeans.Add(means);
				}
				var enabledSensorsCompiledMeans = StatsUtil.JoinParallelMeanReadings(enabledSensorsLiveMeans);
				liveAtmosphericGraph.SetDataSource(enabledSensorsCompiledMeans);
				liveAtmosphericGraph.FormatTimeAxis(liveDataTimeSpan);
				liveAtmosphericGraph.SetLatest(enabledSensorsCompiledMeans.LastOrDefault());
			}


			/*foreach (SensorDeviceView sensorDeviceView in sensorViewContainer.Controls.OfType<SensorDeviceView>()) {
				sensorDeviceView.UpdateSensors(readings);
				sensorDeviceView.SetPreferedUnits(desiredTempUnit, desiredSpeedUnit, desiredPressureUnit);
			}*/


		}

	}
}
