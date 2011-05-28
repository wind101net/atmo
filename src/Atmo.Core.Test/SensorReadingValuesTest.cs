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

namespace Atmo.Test {
	[TestFixture]
	public class SensorReadingValuesTest {

		[Test]
		public void ConstructorGetTest() {

			var values = new SensorReadingValues(
				temperature: 11.1,
				pressure: 22.2,
				humidity: 33.3,
				windDirection: 44.4,
				windSpeed: 55.5
			);

			Assert.AreEqual(11.1, values.Temperature);
			Assert.AreEqual(22.2, values.Pressure);
			Assert.AreEqual(33.3, values.Humidity);
			Assert.AreEqual(44.4, values.WindDirection);
			Assert.AreEqual(55.5, values.WindSpeed);

		}

	}
}
