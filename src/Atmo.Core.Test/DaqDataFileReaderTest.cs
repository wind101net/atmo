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
using NUnit.Framework;

namespace Atmo.Test {
	[TestFixture]
	public class DaqDataFileReaderTest {

		[Test]
		public void FirstHeaderDateTimeTest() {
			var firstDate = default(DateTime);
			var recordsRead = -999;
			using(var stream = DaqDataFileTestUtility.CreateSampleDaqFileStream()) {
				using(var reader = new DaqDataFileReader(stream)) {
					recordsRead = reader.ReadNextValidStamp(out firstDate);
				}
			}

			Assert.AreNotEqual(-999, recordsRead, "Tested method was not executed.");
			Assert.AreNotEqual(-1,firstDate, "Valid date was not found.");
			Assert.AreNotEqual(default(DateTime), firstDate, "Valid date was not found.");

			Assert.AreEqual(new DateTime(2010,3,15,19,30,23),firstDate, "Wrong date was read.");
		}

		/// <summary>
		/// Test the first 3 records from a file.
		/// </summary>
		/// <remarks>
		/// The first record in the file is invalid so the time of the first valid records is one second after the time stamp in the header.
		/// </remarks>
		[Test]
		public void ReadFirstThreeRecordsTest() {
			DateTime stamp;
			// setup the stamp variable to compare
			Assert.True(DaqDataFileInfo.TryConvert7ByteDateTime(
				new byte[] { 0x07, 0xda, 0x03, 0x0f, 0x13, 0x1e, 0x17, 0xa5 },
				0, out stamp
			));
			using (var stream = DaqDataFileTestUtility.CreateSampleDaqFileStream()) {
				using (var reader = new DaqDataFileReader(stream)) {


					Assert.True(reader.MoveNext());
					Assert.AreEqual(
						PackedReadingValues.FromDeviceBytes(new byte[] {
                            0x00, 0x01, 0x09, 0x41, 0x33, 0x60, 0x00, 0x1f                   	
						}, 0),
						reader.Current.Values
					);
					Assert.AreEqual(stamp.Add(new TimeSpan(0, 0, 0, 1)), reader.Current.TimeStamp);

					Assert.True(reader.MoveNext());
					Assert.AreEqual(
						PackedReadingValues.FromDeviceBytes(new byte[] {
                            0x00, 0x01, 0x05, 0x41, 0x33, 0x60, 0x00, 0x1f                   	
						}, 0),
						reader.Current.Values
					);
					Assert.AreEqual(stamp.Add(new TimeSpan(0, 0, 0, 2)), reader.Current.TimeStamp);

					Assert.True(reader.MoveNext());
					Assert.AreEqual(
						PackedReadingValues.FromDeviceBytes(new byte[] {
                            0x00, 0x01, 0x09, 0x41, 0x33, 0x60, 0x00, 0x1f                   	
						}, 0),
						reader.Current.Values
					);
					Assert.AreEqual(stamp.Add(new TimeSpan(0, 0, 0, 3)), reader.Current.TimeStamp);

				}
			}
		}

	}
}
