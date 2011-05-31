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

using NUnit.Framework;
using System.IO;

namespace Atmo.Test {

	[TestFixture]
	public class DaqDataFileTestUtility {

		private const string SampleDaqFileResourceName = "Atmo.Test.A110516.DAT";

		public static Stream CreateSampleDaqFileStream() {
			return typeof(DaqDataFileTestUtility)
				.Assembly
				.GetManifestResourceStream(SampleDaqFileResourceName);
		}

		[Test]
		public void SampleDaqFileReadToEndTest() {
			var bytesRead = 0;
			using(var stream = CreateSampleDaqFileStream()) {
				while(stream.ReadByte() >= 0) {
					bytesRead++;
				}
			}
			Assert.AreEqual(39600, bytesRead);
		}

		[Test]
		public void SampleDaqFileHeaderRecordTest() {
			var headerData = new byte[8];
			using (var stream = CreateSampleDaqFileStream()) {
				Assert.AreEqual(
					headerData.Length,
					stream.Read(headerData,0,headerData.Length)
				);
			}
			Assert.That(
				headerData,
				Is.EqualTo(
					new byte[]{0x07, 0xda, 0x03, 0x0f, 0x13, 0x1e, 0x17, 0xa5}
				)
			);
		}

	}
}
