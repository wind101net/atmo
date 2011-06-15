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
using System;

namespace Atmo.Test {
	[TestFixture]
	public class PackedReadingTest {

		public byte[] DeviceSampleData;
		public DateTime DateTimeSample = new DateTime(2011,5,15,10,45,19,891);

		[TestFixtureSetUp]
		public void Setup() {
			DeviceSampleData = new byte[] { 0x39, 0xDF, 0xFD, 0x42, 0xBF, 0x37, 0xED, 0x1B };
		}

		[Test]
		public void FromDeviceRawBytesRawValuesTest() {
			var reading = new PackedReading(
				DateTimeSample,
				PackedReadingValues.FromDeviceBytes(DeviceSampleData, 0)
			);
			Assert.AreEqual(0x73B, reading.Values.RawWindSpeed);
			Assert.AreEqual(0x1ff, reading.Values.RawWindDirection);
			Assert.AreEqual(0x285, reading.Values.RawTemperature);
			Assert.AreEqual(0x1f9, reading.Values.RawHumidity);
			Assert.AreEqual(0xbf68, reading.Values.RawPressure);
		}

		[Test]
		public void FromDeviceRawBytesValuesTest() {
			var reading = new PackedReading(
				DateTimeSample,
				PackedReadingValues.FromDeviceBytes(DeviceSampleData, 0)
			);
			Assert.AreEqual(18.51, reading.WindSpeed);
			Assert.AreEqual(511, reading.WindDirection);
			Assert.AreEqual(24.5, reading.Temperature);
			Assert.AreEqual(.505, reading.Humidity);
			Assert.AreEqual(98000, reading.Pressure);
		}

		[Test]
		public void FromDoubleValuesToRawValues() {
			var reading = new PackedReading(
				DateTimeSample,
				new PackedReadingValues(
					temperature: 24.5,
					pressure: 98000.0,
					humidity: 0.505,
					windDirection: 511.0,
					windSpeed: 18.51
				)
			);
			Assert.AreEqual(0x73B, reading.Values.RawWindSpeed);
			Assert.AreEqual(0x1ff, reading.Values.RawWindDirection);
			Assert.AreEqual(0x285, reading.Values.RawTemperature);
			Assert.AreEqual(0x1f9, reading.Values.RawHumidity);
			Assert.AreEqual(0xbf68, reading.Values.RawPressure);
		}

	}
}
