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
using Atmo.Data;
using Atmo.Units;
using NUnit.Framework;
using System.Collections.Generic;

namespace Atmo.Core.DbDataStore.Test {
	[TestFixture]
	public class RecordAdjustTests : CrudDataStoreTestBase {


		//
		// start     mark1                   mark2    end
		//   |---------|-----------------------|-------|
		// 
		//                 ... one minute ...

		protected readonly TimeSpan ShortLeg;
		protected readonly TimeSpan HalfShortLeg;
		protected readonly TimeSpan LongLeg;
		protected readonly TimeSpan HalfLongLeg;
		protected readonly TimeSpan TotalLength;
		
		protected readonly DateTime TimeStart;
		protected readonly DateTime TimeMark1;
		protected readonly DateTime TimeMark2;
		protected readonly DateTime TimeEnd;

		protected readonly TimeRange TestPeriodRange;
		protected readonly TimeRange MarkPeriodRange;


		public RecordAdjustTests() {
			TotalLength = new TimeSpan(0, 1, 0);
			ShortLeg = new TimeSpan(0,0,10);
			HalfShortLeg = new TimeSpan(ShortLeg.Ticks / 2);
			LongLeg = TotalLength - ShortLeg - ShortLeg;
			HalfLongLeg = new TimeSpan(LongLeg.Ticks / 2);
			TimeStart = new DateTime(2011, 11, 09, 9, 0, 0);
			TimeMark1 = TimeStart + ShortLeg;
			TimeMark2 = TimeMark1 + LongLeg;
			TimeEnd = TimeStart + TotalLength;

			TestPeriodRange = new TimeRange(TimeStart, TimeEnd);
			MarkPeriodRange = new TimeRange(TimeMark1, TimeMark2);

		}

		protected List<PackedReading> GenerateTestPeriodRecords() {
			return GenerateReadings(TimeStart, TotalLength);
		}

		private List<PackedReading> PopulateTestPeriodRecords(string sensorName) {
			var fullReadings = GenerateTestPeriodRecords();
			Assert.That(Store.Push(sensorName, fullReadings));
			using (var countCmd = Connection.CreateTextCommand("SELECT COUNT(*) FROM Record")) {
				using (var countReader = countCmd.ExecuteReader()) {
					Assert.That(countReader.Read());
					Assert.AreEqual((int)TotalLength.TotalSeconds, countReader.GetInt32(0));
				}
			}
			return fullReadings;
		}

		[Test]
		public void ShrinkAndShiftBackTest() {
			var sensor = AddSensor();

			var fullReadings = PopulateTestPeriodRecords(sensor.Name);

			var modMark1 = TimeStart + HalfShortLeg; // shift back
			var modMark2 = modMark1 + HalfLongLeg; // shrink
			var moveToRange = new TimeRange(modMark1, modMark2);

			Assert.Throws(
				typeof (InvalidOperationException),
			    () => Store.AdjustTimeStamps(sensor.Name, MarkPeriodRange, moveToRange, false)
			);

			Assert.That(Store.AdjustTimeStamps(sensor.Name, MarkPeriodRange, moveToRange, true));
			using (var countCmd = Connection.CreateTextCommand("SELECT COUNT(*) FROM Record")) {
				using (var countReader = countCmd.ExecuteReader()) {
					Assert.That(countReader.Read());
					Assert.AreEqual((int)((HalfShortLeg + HalfLongLeg + ShortLeg).TotalSeconds),
						countReader.GetInt32(0));
				}
			}
		}

		[Test]
		public void GrowAndShiftBackTest() {
			var sensor = AddSensor();

			var fullReadings = PopulateTestPeriodRecords(sensor.Name);

			var modMark1 = TimeMark1 + ShortLeg; // to shift back
			var modMark2 = TimeMark2 - ShortLeg; // to grow it
			var moveFromRange = new TimeRange(modMark1, modMark2);
			var moveToRange = new TimeRange(TimeMark1, TimeMark2);

			Assert.Throws(
				typeof(InvalidOperationException),
				() => Store.AdjustTimeStamps(sensor.Name, moveFromRange, moveToRange, false)
			);

			Assert.That(Store.AdjustTimeStamps(sensor.Name, moveFromRange, moveToRange, true));
			using (var countCmd = Connection.CreateTextCommand("SELECT COUNT(*) FROM Record")) {
				using (var countReader = countCmd.ExecuteReader()) {
					Assert.That(countReader.Read());
					Assert.AreEqual((int)((ShortLeg + ShortLeg + HalfLongLeg).TotalSeconds),
						countReader.GetInt32(0));
				}
			}
		}

		[Test]
		public void ShiftBackTest() {
			var sensor = AddSensor();

			var fullReadings = PopulateTestPeriodRecords(sensor.Name);

			var modMark1 = TimeMark1 - HalfShortLeg; // shift back
			var modMark2 = modMark1 + LongLeg;
			var moveToRange = new TimeRange(modMark1, modMark2);

			Assert.Throws(
				typeof(InvalidOperationException),
				() => Store.AdjustTimeStamps(sensor.Name, MarkPeriodRange, moveToRange, false)
			);

			Assert.That(Store.AdjustTimeStamps(sensor.Name, MarkPeriodRange, moveToRange, true));
			using (var countCmd = Connection.CreateTextCommand("SELECT COUNT(*) FROM Record")) {
				using (var countReader = countCmd.ExecuteReader()) {
					Assert.That(countReader.Read());
					Assert.AreEqual((int)((TestPeriodRange.Span - HalfShortLeg).TotalSeconds),
						countReader.GetInt32(0));
				}
			}
		}

		[Test]
		public void ShrinkAndShiftForwardTest() {
			var sensor = AddSensor();

			var fullReadings = PopulateTestPeriodRecords(sensor.Name);

			var modMark1 = TimeMark1 + HalfLongLeg; // shift forward
			var modMark2 = TimeMark2 + HalfShortLeg; // shrink
			var moveToRange = new TimeRange(modMark1, modMark2);

			Assert.Throws(
				typeof(InvalidOperationException),
				() => Store.AdjustTimeStamps(sensor.Name, MarkPeriodRange, moveToRange, false)
			);

			Assert.That(Store.AdjustTimeStamps(sensor.Name, MarkPeriodRange, moveToRange, true));
			using (var countCmd = Connection.CreateTextCommand("SELECT COUNT(*) FROM Record")) {
				using (var countReader = countCmd.ExecuteReader()) {
					Assert.That(countReader.Read());
					Assert.AreEqual((int)((TestPeriodRange.Span - HalfLongLeg).TotalSeconds),
						countReader.GetInt32(0));
				}
			}
		}

		[Test]
		public void GrowAndShiftForwardTest() {
			var sensor = AddSensor();

			var fullReadings = PopulateTestPeriodRecords(sensor.Name);

			var modMark1 = TimeMark1 + HalfShortLeg; // shift forward
			var modMark2 = TimeEnd; // grow
			var moveToRange = new TimeRange(modMark1, modMark2);

			Assert.Throws(
				typeof(InvalidOperationException),
				() => Store.AdjustTimeStamps(sensor.Name, MarkPeriodRange, moveToRange, false)
			);

			Assert.That(Store.AdjustTimeStamps(sensor.Name, MarkPeriodRange, moveToRange, true));
			using (var countCmd = Connection.CreateTextCommand("SELECT COUNT(*) FROM Record")) {
				using (var countReader = countCmd.ExecuteReader()) {
					Assert.That(countReader.Read());
					Assert.AreEqual((int)((ShortLeg + LongLeg).TotalSeconds),
						countReader.GetInt32(0));
				}
			}
		}

		[Test]
		public void ShiftForwardTest() {
			var sensor = AddSensor();

			var fullReadings = PopulateTestPeriodRecords(sensor.Name);

			var modMark1 = TimeMark1 + HalfShortLeg; // shift forward
			var modMark2 = TimeMark2 + HalfShortLeg; // shift forward
			var moveToRange = new TimeRange(modMark1, modMark2);

			Assert.Throws(
				typeof(InvalidOperationException),
				() => Store.AdjustTimeStamps(sensor.Name, MarkPeriodRange, moveToRange, false)
			);

			Assert.That(Store.AdjustTimeStamps(sensor.Name, MarkPeriodRange, moveToRange, true));
			using (var countCmd = Connection.CreateTextCommand("SELECT COUNT(*) FROM Record")) {
				using (var countReader = countCmd.ExecuteReader()) {
					Assert.That(countReader.Read());
					Assert.AreEqual((int)((TestPeriodRange.Span - HalfShortLeg).TotalSeconds),
						countReader.GetInt32(0));
				}
			}
		}

		[Test]
		public void ShrinkTest() {
			var sensor = AddSensor();

			var fullReadings = PopulateTestPeriodRecords(sensor.Name);

			var modMark1 = TimeMark1;
			var modMark2 = TimeMark1 + HalfLongLeg; // shrink
			var moveToRange = new TimeRange(modMark1, modMark2);


			Assert.That(Store.AdjustTimeStamps(sensor.Name, MarkPeriodRange, moveToRange, false));
			using (var countCmd = Connection.CreateTextCommand("SELECT COUNT(*) FROM Record")) {
				using (var countReader = countCmd.ExecuteReader()) {
					Assert.That(countReader.Read());
					Assert.AreEqual((int)((TestPeriodRange.Span - HalfLongLeg).TotalSeconds),
						countReader.GetInt32(0));
				}
			}
		}

		[Test]
		public void GrowTest() {
			var sensor = AddSensor();

			var fullReadings = PopulateTestPeriodRecords(sensor.Name);

			var modMark1 = TimeMark1;
			var modMark2 = TimeMark2 + HalfShortLeg; // grow
			var moveToRange = new TimeRange(modMark1, modMark2);

			Assert.Throws(
				typeof(InvalidOperationException),
				() => Store.AdjustTimeStamps(sensor.Name, MarkPeriodRange, moveToRange, false)
			);

			Assert.That(Store.AdjustTimeStamps(sensor.Name, MarkPeriodRange, moveToRange, true));
			using (var countCmd = Connection.CreateTextCommand("SELECT COUNT(*) FROM Record")) {
				using (var countReader = countCmd.ExecuteReader()) {
					Assert.That(countReader.Read());
					Assert.AreEqual((int)((TestPeriodRange.Span - HalfShortLeg).TotalSeconds),
						countReader.GetInt32(0));
				}
			}
		}

		[Test]
		public void NoChangeTest() {
			var sensor = AddSensor();

			var fullReadings = PopulateTestPeriodRecords(sensor.Name);

			Assert.False(Store.AdjustTimeStamps(sensor.Name, MarkPeriodRange, MarkPeriodRange, true));
			Assert.False(Store.AdjustTimeStamps(sensor.Name, MarkPeriodRange, MarkPeriodRange, false));

			using (var countCmd = Connection.CreateTextCommand("SELECT COUNT(*) FROM Record")) {
				using (var countReader = countCmd.ExecuteReader()) {
					Assert.That(countReader.Read());
					Assert.AreEqual((int)((TestPeriodRange.Span).TotalSeconds),
						countReader.GetInt32(0));
				}
			}
		}

	}
}
