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

using Atmo.Units;
using NUnit.Framework;

namespace Atmo.Test {
	[TestFixture]
	public class ValueConverterTests {

		[Test]
		public void ConvertFromCelsiusToFahrenheitTest() {
			var converter = new TemperatureConverter(TemperatureUnit.Celsius, TemperatureUnit.Fahrenheit);
			Assert.AreEqual(32, converter.Convert(0));
			Assert.AreEqual(33.8, converter.Convert(1));
			Assert.AreEqual(89.6, converter.Convert(32));
			Assert.AreEqual(212, converter.Convert(100));
		}

		[Test]
		public void ConvertFromFahrenheitToCelsiusTest() {
			var converter = new TemperatureConverter(TemperatureUnit.Fahrenheit, TemperatureUnit.Celsius);
			Assert.AreEqual(-17.7777778, converter.Convert(0),0.00000005);
			Assert.AreEqual(0, converter.Convert(32), 0.0000000001);
			Assert.AreEqual(1, converter.Convert(33.8),0.0000000001);
			Assert.AreEqual(32, converter.Convert(89.6), 0.0000000001);
			Assert.AreEqual(100, converter.Convert(212), 0.0000000001);
		}

		[Test]
		public void ConvertFromMetersPerSecToMilesPerHourTest() {
			var converter = new SpeedConverter(SpeedUnit.MetersPerSec, SpeedUnit.MilesPerHour);
			Assert.AreEqual(0, converter.Convert(0));
			Assert.AreEqual(2.23693629, converter.Convert(1));
			Assert.AreEqual(4.47387258, converter.Convert(2));
		}

		[Test]
		public void ConvertFromMilesPerHourToMetersPerSecTest() {
			var converter = new SpeedConverter(SpeedUnit.MilesPerHour, SpeedUnit.MetersPerSec);
			Assert.AreEqual(0, converter.Convert(0));
			Assert.AreEqual(0.44704, converter.Convert(1));
			Assert.AreEqual(0.89408, converter.Convert(2));
		}

	}
}
