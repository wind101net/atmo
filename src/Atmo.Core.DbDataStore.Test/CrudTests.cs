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
using System.Linq;
using Atmo.Data;
using Atmo.Units;
using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics;

namespace Atmo.Core.DbDataStore.Test {

	[TestFixture]
	public class CrudTests : CrudTestBase {

		private Data.DbDataStore CreateDbDataStore() {
			return new Data.DbDataStore(Connection);
		}

		private Random _rand = new Random((int)DateTime.Now.Ticks);

		private PackedReadingValues GenerateReadingValues() {
			return new PackedReadingValues(
				temperature: _rand.NextDouble() * 20.0,
				pressure: _rand.NextDouble() * ushort.MaxValue,
				humidity: _rand.NextDouble(),
				windDirection: _rand.NextDouble() * 359.99,
				windSpeed: _rand.NextDouble() * 10.0
			);
		}

		private PackedReading GenerateReading(DateTime stamp) {
			return new PackedReading(stamp, GenerateReadingValues());
		}

		private List<PackedReading> GenerateReadings(DateTime start, TimeSpan maxSpan) {
			TimeSpan oneSecond = new TimeSpan(0,0,0,1);
			var readings = new List<PackedReading>((int)maxSpan.TotalSeconds);
			for(TimeSpan span = new TimeSpan(); span < maxSpan; span = span.Add(oneSecond)) {
				readings.Add(GenerateReading(start.Add(span)));
			}
			return readings;
		}

		[Test]
		public void PushSyncStampTest() {
			using (var store = CreateDbDataStore()) {
				store.PushSyncStamp(new DateTime(2011, 10, 3));
				Assert.AreEqual(new DateTime(2011, 10, 3), store.GetMaxSyncStamp());
				store.PushSyncStamp(new DateTime(2011, 10, 5));
				Assert.AreEqual(new DateTime(2011, 10, 5), store.GetMaxSyncStamp());
				store.PushSyncStamp(new DateTime(2011, 10, 4));
				Assert.AreEqual(new DateTime(2011, 10, 5), store.GetMaxSyncStamp());
			}
		}

		[Test]
		public void AddSensorTest() {
			using (var store = CreateDbDataStore()) {
				store.AddSensor(new SensorInfo("Sensor1", SpeedUnit.MetersPerSec, TemperatureUnit.Celsius, PressureUnit.KiloPascals));
				var sensors1 = store.GetAllSensorInfos().ToList();
				Assert.That(sensors1, Has.Count.EqualTo(1));
				Assert.AreEqual("Sensor1",sensors1.First().Name);
				store.AddSensor(new SensorInfo("Sensor2", SpeedUnit.MetersPerSec, TemperatureUnit.Celsius, PressureUnit.KiloPascals));
				var sensors2 = store.GetAllSensorInfos().ToList();
				Assert.That(sensors2, Has.Count.EqualTo(2));
				Assert.AreEqual("Sensor1", sensors2.First().Name);
				Assert.AreEqual("Sensor2", sensors2.Skip(1).First().Name);
			}
		}

		[Test]
		public void BasicRecordPushTest() {
			using (var store = CreateDbDataStore()) {
				var sensor = new SensorInfo("Sensor1", SpeedUnit.MetersPerSec, TemperatureUnit.Celsius, PressureUnit.Pascals);
				store.AddSensor(sensor);
				var testTimeSpan = new TimeSpan(2, 0, 0, 0);
				var readings = GenerateReadings(
					new DateTime(2011, 11, 09),
					testTimeSpan
				);
				Stopwatch pushTime = new Stopwatch();
				pushTime.Start();
				Assert.That(store.Push(sensor.Name, readings));
				pushTime.Stop();
				Debug.WriteLine(String.Format("Push for {0} took {1}", testTimeSpan, pushTime.Elapsed));
				using(var countCmd = Connection.CreateTextCommand("SELECT COUNT(*) FROM Record")) {
					using(var countReader = countCmd.ExecuteReader()) {
						Assert.That(countReader.Read());
						Assert.AreEqual((int)testTimeSpan.TotalSeconds, countReader.GetInt32(0));
					}
					countCmd.CommandText = "SELECT COUNT(*) FROM HourRecord";
					using (var countReader = countCmd.ExecuteReader()) {
						Assert.That(countReader.Read());
						Assert.AreEqual((int)testTimeSpan.TotalHours, countReader.GetInt32(0));
					}
					countCmd.CommandText = "SELECT COUNT(*) FROM DayRecord";
					using (var countReader = countCmd.ExecuteReader()) {
						Assert.That(countReader.Read());
						Assert.AreEqual((int)testTimeSpan.TotalDays, countReader.GetInt32(0));
					}
				}
			}
		}

	}
}
