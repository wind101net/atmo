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

using System.IO;
using System.Text;
using Atmo.Data;
using NUnit.Framework;

namespace Atmo.Test {
	[TestFixture]
	public class PersistentStateTest {

		private string _xml234567;

		[SetUp]
		public void SetUp() {
			var state = new PersistentState {
				MinRangeSizes = new ReadingValues(1, 2, 3, 4, 5),
				MinRangeSizeDewPoint = 6,
				MinRangeSizeAirDensity = 7,
				HeightAboveSeaLevel = 8,
				UserGraphAttribute = PersistentState.UserCalculatedAttribute.AirDensity
			};

			var sb = new StringBuilder();
			using (var xmlWriter = new StringWriter(sb)) {
				PersistentState.Serializer.Serialize(xmlWriter, state);
			}
			_xml234567 = sb.ToString();
		}

		[Test]
		public void XmlSerializeTest() {
			var readState = PersistentState.Serializer.Deserialize(new StringReader(_xml234567)) as PersistentState;

			Assert.IsNotNull(readState);
			Assert.AreEqual(1, readState.MinRangeSizes.Temperature);
			Assert.AreEqual(2, readState.MinRangeSizes.Pressure);
			Assert.AreEqual(3, readState.MinRangeSizes.Humidity);
			Assert.AreEqual(4, readState.MinRangeSizes.WindDirection);
			Assert.AreEqual(5, readState.MinRangeSizes.WindSpeed);
			Assert.AreEqual(6, readState.MinRangeSizeDewPoint);
			Assert.AreEqual(7, readState.MinRangeSizeAirDensity);
			Assert.AreEqual(8, readState.HeightAboveSeaLevel);
			Assert.AreEqual(PersistentState.UserCalculatedAttribute.AirDensity, readState.UserGraphAttribute);

		}

	}
}
