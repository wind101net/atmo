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
	public class CrudTests : CrudDataStoreTestBase {

		[Test]
		public void PushSyncStampTest() {
			Store.PushSyncStamp(new DateTime(2011, 10, 3));
			Assert.AreEqual(new DateTime(2011, 10, 3), Store.GetMaxSyncStamp());
			Store.PushSyncStamp(new DateTime(2011, 10, 5));
			Assert.AreEqual(new DateTime(2011, 10, 5), Store.GetMaxSyncStamp());
			Store.PushSyncStamp(new DateTime(2011, 10, 4));
			Assert.AreEqual(new DateTime(2011, 10, 5), Store.GetMaxSyncStamp());
		}

		[Test]
		public void AddSensorTest() {
			AddSensor(null,"Sensor1");
			var sensors1 = Store.GetAllSensorInfos().ToList();
			Assert.That(sensors1, Has.Count.EqualTo(1));
			Assert.AreEqual("Sensor1",sensors1.First().Name);
			AddSensor(null, "Sensor2");
			var sensors2 = Store.GetAllSensorInfos().ToList();
			Assert.That(sensors2, Has.Count.EqualTo(2));
			Assert.AreEqual("Sensor1", sensors2.First().Name);
			Assert.AreEqual("Sensor2", sensors2.Skip(1).First().Name);
		}

		[Test]
		public void BasicRecordPushTest() {
			var sensor = AddSensor();
			var testTimeStart = new DateTime(2011, 11, 09);
			var testTimeSpan = new TimeSpan(2, 0, 0, 0);
			var readings = GenerateReadings(testTimeStart,testTimeSpan);
			var pushTime = new Stopwatch();
			pushTime.Start();
			Assert.That(Store.Push(sensor.Name, readings));
			pushTime.Stop();
			Debug.WriteLine(String.Format("Push for {0} took {1}", testTimeSpan, pushTime.Elapsed));
			using(var countCmd = Connection.CreateTextCommand("SELECT COUNT(*) FROM Record")) {
				using(var countReader = countCmd.ExecuteReader()) {
					Assert.That(countReader.Read());
					Assert.AreEqual((int)testTimeSpan.TotalSeconds, countReader.GetInt32(0));
				}
				countCmd.CommandText = "SELECT COUNT(*) FROM MinuteRecord";
				using (var countReader = countCmd.ExecuteReader()) {
					Assert.That(countReader.Read());
					Assert.AreEqual((int)testTimeSpan.TotalMinutes, countReader.GetInt32(0));
				}
				countCmd.CommandText = "SELECT COUNT(*) FROM TenminuteRecord";
				using (var countReader = countCmd.ExecuteReader()) {
					Assert.That(countReader.Read());
					Assert.AreEqual((int)(testTimeSpan.TotalMinutes / 10.0), countReader.GetInt32(0));
				}
			}

			var readingsInDb = Store.GetReadings(sensor.Name, testTimeStart, testTimeSpan)
				.ToList();
				
			var firstReading = readingsInDb.First();
			Assert.AreEqual(readings[0], firstReading);

			var tenMinSummaries = Store.GetReadingSummaries(sensor.Name, testTimeStart, testTimeSpan, new TimeSpan(0, 0, 10, 0))
				.ToList();

			Assert.That(tenMinSummaries, Has.Count.EqualTo((int)(testTimeSpan.TotalMinutes / 10.0)));
		}

	}
}
