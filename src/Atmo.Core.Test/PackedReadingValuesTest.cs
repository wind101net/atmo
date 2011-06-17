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
	public class PackedReadingValuesTest {

		public byte[] DeviceSampleData;

		[TestFixtureSetUp]
		public void Setup() {
			DeviceSampleData = new byte[] { 0x39, 0xDF, 0xFD, 0x42, 0xBF, 0x37, 0xED, 0x1B };
		}

		[Test]
		public void FromDeviceRawBytesRawValuesTest() {
			var values = PackedReadingValues.FromDeviceBytes(DeviceSampleData, 0);

			Assert.AreEqual(0x73B, values.RawWindSpeed);
			Assert.AreEqual(0x1ff, values.RawWindDirection);
			Assert.AreEqual(0x285, values.RawTemperature);
			Assert.AreEqual(0x1f9, values.RawHumidity);
			Assert.AreEqual(0xbf68, values.RawPressure);
			Assert.AreEqual(
				PackedValuesFlags.AnemTemperatureSource
				| PackedValuesFlags.Humidity
				| PackedValuesFlags.Pressure
				| PackedValuesFlags.WindSpeed,
				values.RawFlags
			);
		}

		[Test]
		public void FromDeviceRawBytesValuesTest() {
			var values = PackedReadingValues.FromDeviceBytes(DeviceSampleData, 0);

			Assert.AreEqual(18.51, values.WindSpeed);
			Assert.AreEqual(511, values.WindDirection);
			Assert.AreEqual(24.5, values.Temperature);
			Assert.AreEqual(.505, values.Humidity);
			Assert.AreEqual(98000, values.Pressure);
			Assert.IsTrue(values.IsValid);
			Assert.IsTrue(values.IsHumidityValid);
			Assert.IsTrue(values.IsTemperatureValid);
			Assert.IsTrue(values.IsPressureValid);
			Assert.IsTrue(values.IsWindSpeedValid);
			Assert.IsFalse(values.IsWindDirectionValid);
		}

		[Test]
		public void FromDoubleValuesToRawValues() {
			var values = new PackedReadingValues(
				temperature: 24.5,
				pressure: 98000.0,
				humidity: 0.505,
				windDirection: 511.0,
				windSpeed: 18.51
			);

			Assert.AreEqual(0x73B, values.RawWindSpeed);
			Assert.AreEqual(0x1ff, values.RawWindDirection);
			Assert.AreEqual(0x285, values.RawTemperature);
			Assert.AreEqual(0x1f9, values.RawHumidity);
			Assert.AreEqual(0xbf68, values.RawPressure);
			Assert.IsTrue(values.IsValid);
			Assert.IsTrue(values.IsHumidityValid);
			Assert.IsTrue(values.IsTemperatureValid);
			Assert.IsTrue(values.IsPressureValid);
			Assert.IsTrue(values.IsWindSpeedValid);
			Assert.IsFalse(values.IsWindDirectionValid);
		}

	}
}
