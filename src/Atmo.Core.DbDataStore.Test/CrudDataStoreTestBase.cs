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
using Atmo.Units;
using NUnit.Framework;

namespace Atmo.Core.DbDataStore.Test {
	public abstract class CrudDataStoreTestBase : CrudTestBase {

		protected Data.DbDataStore CreateDbDataStore() {
			return new Data.DbDataStore(Connection);
		}

		private readonly Random _rand = new Random((int)DateTime.Now.Ticks);

		protected PackedReadingValues GenerateReadingValues() {
			return new PackedReadingValues(
				temperature: _rand.NextDouble() * 20.0,
				pressure: _rand.NextDouble() * ushort.MaxValue,
				humidity: _rand.NextDouble(),
				windDirection: _rand.NextDouble() * 359.99,
				windSpeed: _rand.NextDouble() * 10.0
			);
		}

		protected PackedReading GenerateReading(DateTime stamp) {
			return new PackedReading(stamp, GenerateReadingValues());
		}

		protected List<PackedReading> GenerateReadings(DateTime start, TimeSpan maxSpan) {
			var oneSecond = new TimeSpan(0, 0, 0, 1);
			var readings = new List<PackedReading>((int)maxSpan.TotalSeconds);
			for (var span = new TimeSpan(); span < maxSpan; span = span.Add(oneSecond)) {
				readings.Add(GenerateReading(start.Add(span)));
			}
			return readings;
		}

		protected Data.DbDataStore Store { get; private set; }

		protected SensorInfo AddSensor(
			Data.DbDataStore store = null,
			string name = "Sensor",
			SpeedUnit speedUnit = SpeedUnit.MetersPerSec,
			TemperatureUnit tempUnit = TemperatureUnit.Celsius,
			PressureUnit pressUnit = PressureUnit.KiloPascals
		) {
			if (null == store)
				store = Store;
			Assert.IsNotNull(store, "no store");
			var sensor = new SensorInfo(name, speedUnit, tempUnit, pressUnit);
			var sensorAdded = store.AddSensor(sensor);
			Assert.IsTrue(sensorAdded, "sensor add failed");
			return sensor;
		}

		[SetUp]
		public void SetUpStore() {
			Store = CreateDbDataStore();
		}

		[TearDown]
		public void TearDownStore() {
			var store = Store;
			Store = null;
			if(null != store) {
				store.Dispose();
			}
		}

	}
}
