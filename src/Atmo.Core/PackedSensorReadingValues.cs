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
// Contact: Jan Barani at jan@baranidesign.com
//
// ================================================================================

using System;

namespace Atmo {

	/// <summary>
	/// 
	/// </summary>
	public struct PackedSensorReadingValues
		: IReadingValues, IReadingRawValues
	{

		/// <summary>
		/// An invalid value.
		/// </summary>
		public static readonly PackedSensorReadingValues Invalid = new PackedSensorReadingValues(0, 0, 0);

		/// <summary>
		/// Generate a packed sensor reading value set from a data string.
		/// </summary>
		/// <param name="data">The data to read.</param>
		/// <param name="offset">The offset to start reading from.</param>
		/// <returns>A packed sensor reading values set.</returns>
		/// <remarks>The data must provide at least 8 bytes to read.</remarks>
		public static PackedSensorReadingValues FromDeviceBytes(byte[] data, int offset) {
			// TODO: unchecked(abc)
			// get the individual values
			var windSpeed = unchecked((ushort)((data[offset] << 5) | (data[offset + 1] >> 3))); // 13 b
			var windDirection = unchecked((ushort)(((data[offset + 1] & 0x07) << 6) | (data[offset + 2] >> 2))); // 9 b
			var temperature = unchecked((ushort)(((data[offset + 2] & 0x3) << 9) | (data[offset + 3] << 1) | (data[offset + 4] >> 7))); // 11 b
			var humidity = unchecked((ushort)(((data[offset + 4] & 0x7f) << 3) | (data[offset + 5] >> 5))); // 10b
			var pressureData = unchecked((ushort)(((data[offset + 5] & 0x1f) << 11) | (data[offset + 6] << 3) | (data[offset + 7] >> 5))); // 16b
			var flags = unchecked((byte)(data[offset + 7] & 0x1f)); // 5b
			
			// final format for storage:
			// pressureData = (ushort)((pressureData << 8) | (pressureData >> 8));
			// ushort dirAndHumData = (ushort)((windDirection) | (humidity << 9)); // 16 b
			// uint speedAndTempData = (uint)(windSpeed | (temperature << 13)); //24 b
			
			var tempFlagsData = unchecked((ushort)((temperature << 5) | flags));
			var humDirSpeedData = unchecked((uint)(((uint)(humidity) << 22) | ((uint)(windDirection) << 13) | windSpeed));
			return new PackedSensorReadingValues(pressureData, tempFlagsData, humDirSpeedData);
		}

		/// <summary>
		/// Stores the pressure data.
		/// </summary>
		private readonly ushort _pressureData;
		/// <summary>
		/// Stores the temperature and value flags data.
		/// </summary>
		private readonly ushort _temperatureAndFlags;
		/// <summary>
		/// Stores the humidity, direction, and speed.
		/// </summary>
		private readonly uint _humidityDirectionAndSpeed;

		/// <summary>
		/// Creates a new sensor reading value set given the raw data fields.
		/// </summary>
		/// <param name="pressureData"></param>
		/// <param name="temperatureAndFlags"></param>
		/// <param name="humidityDirectionAndSpeed"></param>
		private PackedSensorReadingValues(ushort pressureData, ushort temperatureAndFlags, uint humidityDirectionAndSpeed) {
			_pressureData = pressureData;
			_temperatureAndFlags = temperatureAndFlags;
			_humidityDirectionAndSpeed = humidityDirectionAndSpeed;
		}

		public PackedSensorReadingValues(
			double temperature,
			double pressure,
			double humidity,
			double windDirection,
			double windSpeed
		) {
			_pressureData = (ushort)(Math.Max(0, Math.Min(UInt16.MaxValue, pressure / 2.0)));
			_temperatureAndFlags = (ushort)(
				((ushort)(Math.Max(0, Math.Min(1023, (temperature + 40.0) * 10.0))) << 5)
				| 0x1f // TODO: this will need to be a real value
			);
			_humidityDirectionAndSpeed = (
				(uint)((uint)(Math.Max(0, Math.Min(1023, humidity * 1000.0))) << 22)
				| (uint)((ushort)(Math.Max(0, Math.Min(511, windDirection))) << 13) // TODO: should this cap to 360.0, or maybe even one step down from 360.0?
				| (uint)(Math.Max(0, Math.Min(8191, windSpeed * 100.0)))
			);
		}

		[CLSCompliant(false)]
		public ushort RawTemperature {
			get { return unchecked((ushort)(_temperatureAndFlags >> 5)); }
		}

		[CLSCompliant(false)]
		public ushort RawPressure {
			get { return _pressureData; }
		}

		[CLSCompliant(false)]
		public ushort RawHumidity {
			get { return unchecked((ushort)(_humidityDirectionAndSpeed >> 22)); }
		}

		[CLSCompliant(false)]
		public ushort RawWindSpeed {
			get { return unchecked((ushort)(_humidityDirectionAndSpeed & 0x1fff)); }
		}

		[CLSCompliant(false)]
		public ushort RawWindDirection {
			get { return unchecked((ushort)((_humidityDirectionAndSpeed >> 13) & 0x1ff)); }
		}

		public double Temperature {
			get { return unchecked((((ushort)(_temperatureAndFlags >> 5)) / 10.0) - 40.0); }
		}

		public double Pressure {
			get { return unchecked((double)(_pressureData * 2)); }
		}

		public double Humidity {
			get { return unchecked((double)((ushort)(_humidityDirectionAndSpeed >> 22)) / 1000.0); }
		}

		public double WindSpeed {
			get { return unchecked((double)((ushort)(_humidityDirectionAndSpeed & 0x1fff)) / 100.0); }
		}

		public double WindDirection {
			get { return unchecked((double)((ushort)((_humidityDirectionAndSpeed >> 13) & 0x1ff))); } // TODO: Is this ushort cast needed?
		}
	}
}
