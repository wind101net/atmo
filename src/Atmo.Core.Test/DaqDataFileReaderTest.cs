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

	}
}
