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
	public class ReadingValuesConverterTest {

		[Test]
		public void ReadingValuesConverterTestA() {
			var converter = new ReadingValuesConverter<ReadingValues, ReadingValues>(
				new TemperatureConverter(TemperatureUnit.Celsius,TemperatureUnit.Fahrenheit),
				new SpeedConverter(SpeedUnit.MetersPerSec, SpeedUnit.MetersPerSec), 
				new PressureConverter(PressureUnit.Millibar, PressureUnit.KiloPascals)
			);

			var inputA = new ReadingValues(0, 0, 0, 0, 0);
			var expectedA = new ReadingValues(32, 0, 0, 0, 0);

			Assert.AreEqual(expectedA, converter.Convert(inputA));

			var inputB = new ReadingValues(30, 1000, .9, 98, 14);
			var expectedB = new ReadingValues(86, 100, .9, 98, 14);

			Assert.AreEqual(expectedB, converter.Convert(inputB));

			Assert.AreNotEqual(expectedA, expectedB);
		}

	}
}
