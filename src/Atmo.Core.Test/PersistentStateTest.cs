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

		private string _xml234567 = null;

		[SetUp]
		public void SetUp() {
			var state = new PersistentState {
				MinValueRange = new ReadingValues(1, 2, 3, 4, 5),
				MinDewPointRange = 6,
				MinAirDensityRange = 7
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
			Assert.AreEqual(1, readState.MinValueRange.Temperature);
			Assert.AreEqual(2, readState.MinValueRange.Pressure);
			Assert.AreEqual(3, readState.MinValueRange.Humidity);
			Assert.AreEqual(4, readState.MinValueRange.WindDirection);
			Assert.AreEqual(5, readState.MinValueRange.WindSpeed);
			Assert.AreEqual(6, readState.MinDewPointRange);
			Assert.AreEqual(7, readState.MinAirDensityRange);

		}

	}
}
