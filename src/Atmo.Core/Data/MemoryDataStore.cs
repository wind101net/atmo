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
using Atmo.Units;

namespace Atmo.Data {
	public class MemoryDataStore : IDataStore {

		private class SensorStorage : List<Reading>, ISensor {

			private readonly string _name;
			private readonly SpeedUnit _speedUnit;
			private readonly TemperatureUnit _tempUnit;
			private readonly PressureUnit _presUnit;

			public SensorStorage(ISensorInfo sensorInfo) : this(sensorInfo.Name, sensorInfo.SpeedUnit, sensorInfo.TemperatureUnit, sensorInfo.PressureUnit) { }

			public SensorStorage(string name, SpeedUnit speedUnit, TemperatureUnit tempUnit, PressureUnit presUnit) : this(name, speedUnit, tempUnit, presUnit, null) { }

			public SensorStorage(string name, SpeedUnit speedUnit, TemperatureUnit tempUnit, PressureUnit presUnit, IEnumerable<Reading> readings)
				: base((null == readings) ? (new Reading[0]) : readings) {
				_name = name;
				_speedUnit = speedUnit;
				_tempUnit = tempUnit;
				_presUnit = presUnit;
			}

			public Reading GetCurrentReading() {
				return this.OrderByDescending(sr => sr.TimeStamp).FirstOrDefault();
			}

			IReading ISensor.GetCurrentReading() {
				return GetCurrentReading();
			}

			public string Name {
				get { return _name; }
			}

			public SpeedUnit SpeedUnit {
				get { return _speedUnit; }
			}

			public TemperatureUnit TemperatureUnit {
				get { return _tempUnit; }
			}

			public PressureUnit PressureUnit {
				get { return _presUnit; }
			}

			public bool IsValid {
				get { return true; }
			}

		}

		private List<SensorStorage> _sensors;

		public MemoryDataStore() {
			_sensors = new List<SensorStorage>();
		}

		public bool Push(string sensor, IEnumerable<Reading> readings) {
			SensorStorage sensorStorage = this.GetSensorStorageByName(sensor);
			if (null == sensorStorage) {
				return false;
			}
			sensorStorage.AddRange(readings);
			return true;
		}

		public void PurgeBefore(DateTime minStamp) {
			foreach (SensorStorage currentSenor in _sensors) {
				currentSenor.RemoveAll(sr => sr.TimeStamp <= minStamp);
			}
		}

		private SensorStorage GetSensorStorageByName(string sensorName) {
			if (null == sensorName) {
				sensorName = String.Empty;
			}
			return _sensors.FirstOrDefault(ss => sensorName.Equals(ss.Name));
		}

		public IEnumerable<ISensorInfo> GetAllSensorInfos() {
			return _sensors.Cast<ISensorInfo>();
		}

		public IEnumerable<Reading> GetReadings(string sensor, DateTime from, TimeSpan span) {
			SensorStorage sensorStorage = this.GetSensorStorageByName(sensor);
			if (null == sensorStorage) {
				return Enumerable.Empty<Reading>();
			}
			DateTime to = from.Add(span);
			return (
				(to < from)
				? (
					sensorStorage
					.Where(ss => ss.TimeStamp >= to && ss.TimeStamp <= from)
					.OrderByDescending(ss => ss.TimeStamp)
				)
				: (
					sensorStorage
					.Where(ss => ss.TimeStamp >= from && ss.TimeStamp <= to)
					.OrderBy(ss => ss.TimeStamp)
				)
			);
		}

		/*
		IEnumerable<IReading> IDataStore.GetReadings(string sensor, DateTime from, TimeSpan span) {
			return this.GetReadings(sensor, from, span).OfType<IReading>();
		}

		public IEnumerable<ReadingsSummary> GetReadingSummaries(string sensor, DateTime from, TimeSpan span, TimeUnit summaryUnit) {
			return StatsUtility.Summarize<Reading>(this.GetReadings(sensor, from, span), summaryUnit);
		}

		IEnumerable<IReadingsSummary> IDataStore.GetReadingSummaries(string sensor, DateTime from, TimeSpan span, TimeUnit summaryUnit) {
			return this.GetReadingSummaries(sensor, from, span, summaryUnit).OfType<IReadingsSummary>();
		}*/

		public bool AddSensor(ISensorInfo sensor) {
			if (null == sensor) {
				return false; // throw new ArgumentNullException("sensor");
			}
			if (null == sensor.Name) {
				return false; // throw new ArgumentException("Sensor name is null.", "sensor");
			}
			if (_sensors.Any(ss => sensor.Name.Equals(ss.Name))) {
				return false; // throw new ArgumentException("A sensor with that name already exists within the data store.", "sensor");
			}
			_sensors.Add(new SensorStorage(sensor));
			return true;
		}

		public bool Push(string sensor, IEnumerable<IReading> readings) {
			var sensorStorage = this.GetSensorStorageByName(sensor);
			if (null == sensorStorage) {
				return false;
			}
			sensorStorage.AddRange(readings.Select(sr => new Reading(sr)));
			return true;
		}

		public DateTime GetMaxSyncStamp() {
			return default(DateTime);
		}

		public bool AdjustTimeStamps(string sensorName, TimeRange currentRange, TimeRange correctedRange) {
			return false;
		}

		public string GetLatestSensorNameForHardwareId(string hardwareId) {
			return null;
		}

		public bool PushSyncStamp(DateTime stamp) {
			return false;
		}

		public bool Push<T>(string sensor, IEnumerable<T> readings) where T : IReading {
			return false;
		}

		public bool Push<T>(string sensor, IEnumerable<T> readings, bool replace) where T : IReading {
			return Push<T>(sensor, readings);
		}

		public void SetLatestSensorNameForHardwareId(string dbSensorName, string hardwareId) {
			;
		}

	}
}
