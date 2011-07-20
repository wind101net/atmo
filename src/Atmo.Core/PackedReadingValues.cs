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
using System.Collections.Generic;

namespace Atmo {

	/// <inheritdoc/>
	public struct PackedReadingValues :
        IReadingValues,
        IEquatable<IReadingValues>,
        IEquatable<PackedReadingValues>
	{

		/// <summary>
		/// An invalid value.
		/// </summary>
		public static readonly PackedReadingValues Invalid = new PackedReadingValues(0, 0, 0);

		/// <summary>
		/// Generate a packed sensor reading value set from a data string.
		/// </summary>
		/// <param name="data">The data to read.</param>
		/// <param name="offset">The offset to start reading from.</param>
		/// <returns>A packed sensor reading values set.</returns>
		/// <remarks>The data must provide at least 8 bytes to read.</remarks>
		public static PackedReadingValues FromDeviceBytes(byte[] data, int offset) {
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
			return new PackedReadingValues(pressureData, tempFlagsData, humDirSpeedData);
		}

		public static byte[] ConvertToPackedBytes(IReadingValues values) {
			return ConvertToPackedBytes(
				values is PackedReadingValues
				? (PackedReadingValues)values
				: (
					values is PackedReading
					? ((PackedReading)values).Values
					: new PackedReadingValues(values)
				)
			);
		}

		public static byte[] ConvertToPackedBytes(PackedReadingValues values) {
			var data = new byte[8];
			Array.Copy(BitConverter.GetBytes(values._pressureData), data, 2);
			Array.Copy(BitConverter.GetBytes(values._temperatureAndFlags), 0, data, 2, 2);
			Array.Copy(BitConverter.GetBytes(values._humidityDirectionAndSpeed), 0, data, 4, 4);
			return data;
		}

		public static PackedReadingValues ConvertFromPackedBytes(byte[] data) {
			var pressureData = BitConverter.ToUInt16(data, 0);
			var tempFlagsData = BitConverter.ToUInt16(data, 2);
			var humDirSpeedData = BitConverter.ToUInt32(data, 4);
			return new PackedReadingValues(pressureData, tempFlagsData, humDirSpeedData);
		}

		public static PackedReadingValues ConvertFromPackedBytes(byte[] data, int offset) {
			var pressureData = BitConverter.ToUInt16(data, offset);
			var tempFlagsData = BitConverter.ToUInt16(data, offset + 2);
			var humDirSpeedData = BitConverter.ToUInt32(data, offset + 4);
			return new PackedReadingValues(pressureData, tempFlagsData, humDirSpeedData);
		}

		#region Count Byte Conversions

		public static byte[] ConvertTemperatureCountsToPackedBytes(Dictionary<double, int> tempValues) {
			Dictionary<ushort, int> ushortValues = new Dictionary<ushort, int>(tempValues.Count);
			int count;
			foreach (KeyValuePair<double, int> kvp in tempValues) {
				ushort cmpVal = (ushort)(Math.Round(Math.Max(0, Math.Min(1023, (kvp.Key + 40.0) * 10.0))));
				ushortValues[cmpVal] = ushortValues.TryGetValue(cmpVal, out count) ? count + kvp.Value : kvp.Value;
			}
			return ConvertToPackedBytes(ushortValues);
		}

		public static byte[] ConvertPressureCountsToPackedBytes(Dictionary<double, int> tempValues) {
			Dictionary<ushort, int> ushortValues = new Dictionary<ushort, int>(tempValues.Count);
			int count;
			foreach (KeyValuePair<double, int> kvp in tempValues) {
				ushort cmpVal = (ushort)(Math.Round(Math.Max(0, Math.Min(65535, kvp.Key / 2.0))));
				ushortValues[cmpVal] = ushortValues.TryGetValue(cmpVal, out count) ? count + kvp.Value : kvp.Value;
			}
			return ConvertToPackedBytes(ushortValues);
		}

		public static byte[] ConvertHumidityCountsToPackedBytes(Dictionary<double, int> tempValues) {
			Dictionary<ushort, int> byteValues = new Dictionary<ushort, int>(tempValues.Count);
			int count;
			foreach (KeyValuePair<double, int> kvp in tempValues) {
				ushort cmpVal = (ushort)(Math.Round(Math.Max(0, Math.Min(1023, kvp.Key * 1000))));
				byteValues[cmpVal] = byteValues.TryGetValue(cmpVal, out count) ? count + kvp.Value : kvp.Value;
			}
			return ConvertToPackedBytes(byteValues);
		}

		public static byte[] ConvertWindSpeedCountsToPackedBytes(Dictionary<double, int> tempValues) {
			Dictionary<ushort, int> ushortValues = new Dictionary<ushort, int>(tempValues.Count);
			int count;
			foreach (KeyValuePair<double, int> kvp in tempValues) {
				ushort cmpVal = (ushort)(Math.Round(Math.Max(0, Math.Min(8191, kvp.Key * 100.0))));
				ushortValues[cmpVal] = ushortValues.TryGetValue(cmpVal, out count) ? count + kvp.Value : kvp.Value;
			}
			return ConvertToPackedBytes(ushortValues);
		}

		public static byte[] ConvertWindDirectionCountsToPackedBytes(Dictionary<double, int> tempValues) {
			Dictionary<ushort, int> ushortValues = new Dictionary<ushort, int>(tempValues.Count);
			int count;
			foreach (KeyValuePair<double, int> kvp in tempValues) {
				ushort cmpVal = (ushort)(Math.Round(Math.Max(0, Math.Min(511, kvp.Key))));
				ushortValues[cmpVal] = ushortValues.TryGetValue(cmpVal, out count) ? count + kvp.Value : kvp.Value;
			}
			return ConvertToPackedBytes(ushortValues);
		}

		private static byte[] ConvertToPackedBytes(Dictionary<ushort, int> countValues) {
			int stride = sizeof(ushort) + sizeof(int);
			byte[] result = new byte[countValues.Count * stride];
			IEnumerator<KeyValuePair<ushort, int>> enumerator = countValues.GetEnumerator();
			for (int i = 0; enumerator.MoveNext(); i++) {
				byte[] usData = BitConverter.GetBytes(enumerator.Current.Key);
				Array.Copy(usData, 0, result, i * stride, usData.Length);
				byte[] iData = BitConverter.GetBytes(enumerator.Current.Value);
				Array.Copy(iData, 0, result, (i * stride) + usData.Length, iData.Length);
			}
			return result;
		}

		private static byte[] ConvertToPackedBytes(Dictionary<byte, int> countValues) {
			int stride = sizeof(byte) + sizeof(int);
			byte[] result = new byte[countValues.Count * stride];
			IEnumerator<KeyValuePair<byte, int>> enumerator = countValues.GetEnumerator();
			for (int i = 0; enumerator.MoveNext(); i++) {
				result[i * stride] = enumerator.Current.Key;
				byte[] iData = BitConverter.GetBytes(enumerator.Current.Value);
				Array.Copy(iData, 0, result, (i * stride) + 1, iData.Length);
			}
			return result;
		}

		public static Dictionary<ushort, int> PackedCountsToHashUnsigned16(byte[] data) {
			int stride = sizeof(ushort) + sizeof(int);
			Dictionary<ushort, int> result = new Dictionary<ushort, int>(data.Length / stride);
			for (int i = 0; i < data.Length; i += stride) {
				ushort key = BitConverter.ToUInt16(data, i);
				int value = BitConverter.ToInt32(data, i + sizeof(ushort));
				result[key] = value;
			}
			return result;
		}

		public static Dictionary<byte, int> PackedCountsToHashUnsigned8(byte[] data) {
			int stride = sizeof(byte) + sizeof(int);
			Dictionary<byte, int> result = new Dictionary<byte, int>(data.Length / stride);
			for (int i = 0; i < data.Length; i += stride) {
				byte key = data[i];
				int value = BitConverter.ToInt32(data, i + sizeof(byte));
				result[key] = value;
			}
			return result;
		}

		#endregion

        public static bool operator==(PackedReadingValues a, PackedReadingValues b)
        {
            return a.Equals(b);
        }

        public static bool operator!=(PackedReadingValues a, PackedReadingValues b)
        {
            return !a.Equals(b);
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
		private PackedReadingValues(ushort pressureData, ushort temperatureAndFlags, uint humidityDirectionAndSpeed) {
			_pressureData = pressureData;
			_temperatureAndFlags = temperatureAndFlags;
			_humidityDirectionAndSpeed = humidityDirectionAndSpeed;
		}

		public PackedReadingValues(
			double temperature,
			double pressure,
			double humidity,
			double windDirection,
			double windSpeed
		) {
			/*var flags = PackedValuesFlags.WindSpeed | PackedValuesFlags.Humidity | PackedValuesFlags.Pressure;
			if(windDirection >= 0 && windDirection <= 360) {
				flags |= PackedValuesFlags.WindDirection;
			}*/

		    var flags = PackedValuesFlags.None;
            if(!Double.IsNaN(pressure))
                flags |= PackedValuesFlags.Pressure;
            if (!Double.IsNaN(humidity))
                flags |= PackedValuesFlags.Humidity;
            if (!Double.IsNaN(windDirection))
                flags |= PackedValuesFlags.WindDirection;
            if (!Double.IsNaN(windSpeed))
                flags |= PackedValuesFlags.WindSpeed;

			_pressureData = (ushort)(Math.Max(0, Math.Min(UInt16.MaxValue, pressure / 2.0)));

			_temperatureAndFlags = (ushort)(
				((ushort)(Math.Max(0, Math.Min(1023, (temperature + 40.0) * 10.0))) << 5)
				| (ushort)flags
			);
			_humidityDirectionAndSpeed = (
				(uint)((uint)(Math.Max(0, Math.Min(1023, humidity * 1000.0))) << 22)
				| (uint)((ushort)(Math.Max(0, Math.Min(511, windDirection))) << 13) // TODO: should this cap to 360.0, or maybe even one step down from 360.0?
				| (uint)(Math.Max(0, Math.Min(8191, windSpeed * 100.0)))
			);
		}

		public PackedReadingValues(IReadingValues values)
			: this(values.Temperature, values.Pressure, values.Humidity, values.WindDirection, values.WindSpeed) { }

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

		public PackedValuesFlags RawFlags {
			get { return (PackedValuesFlags) (_temperatureAndFlags & 0x1f); }
		}

		public double Temperature {
			get {
				return IsTemperatureValid
					? unchecked((((ushort)(_temperatureAndFlags >> 5)) / 10.0) - 40.0)
					: Double.NaN
				;
			}
		}

		public double Pressure {
			get {
				return IsPressureValid
					? unchecked((double)(_pressureData * 2))
					: Double.NaN
				;
			}
		}

		public double Humidity {
			get {
				return IsHumidityValid
				    ? unchecked((double) ((ushort) (_humidityDirectionAndSpeed >> 22))/1000.0)
				    : Double.NaN
				;
			}
		}

		public double WindSpeed {
			get {
				return IsWindSpeedValid
					? unchecked((double)((ushort)(_humidityDirectionAndSpeed & 0x1fff)) / 100.0)
					: Double.NaN
				;
			}
		}

		public double WindDirection {
			get {
				// TODO: Is this ushort cast needed?
				return IsWindDirectionValid
					? unchecked((double)((ushort)((_humidityDirectionAndSpeed >> 13) & 0x1ff)))
					: Double.NaN
				;
			}
		}


		public bool IsValid {
			get { return 0 != ((ushort)PackedValuesFlags.AllDataFlags & _temperatureAndFlags); }
		}

		public bool IsTemperatureValid {
			get {
				return (0 != ((ushort) PackedValuesFlags.AnemTemperatureSource & _temperatureAndFlags))
					|| (0 != (ushort)(_temperatureAndFlags >> 5))
				;
			}
		}

		public bool IsPressureValid {
			get { return 0 != ((ushort)PackedValuesFlags.Pressure & _temperatureAndFlags); }
		}

		public bool IsHumidityValid {
			get { return 0 != ((ushort)PackedValuesFlags.Humidity & _temperatureAndFlags); }
		}

		public bool IsWindSpeedValid {
			get { return 0 != ((ushort)PackedValuesFlags.WindSpeed & _temperatureAndFlags); }
		}

		public bool IsWindDirectionValid {
			get { return 0 != ((ushort)PackedValuesFlags.WindDirection & _temperatureAndFlags); }
		}

        public bool Equals(IReadingValues other)
        {
            return null != other
                && (IsTemperatureValid ? other.Temperature == Temperature : !other.IsTemperatureValid)
                && (IsPressureValid ? other.Pressure == Pressure : !other.IsPressureValid)
                && (IsHumidityValid ? other.Humidity == Humidity : !other.IsHumidityValid)
                && (IsWindDirectionValid ? other.WindDirection == WindDirection : !other.IsWindDirectionValid)
                && (IsWindSpeedValid ? other.WindSpeed == WindSpeed : !other.IsWindSpeedValid)
            ;
        }

        public bool Equals(PackedReadingValues other)
        {
            return _humidityDirectionAndSpeed == other._humidityDirectionAndSpeed
                && _pressureData == other._pressureData
                && _temperatureAndFlags == other._temperatureAndFlags;
        }

        public override bool Equals(object obj)
        {
            return obj is PackedReadingValues
                ? Equals((PackedReadingValues)obj)
                : Equals(obj as IReadingValues)
            ;
        }

        public override int GetHashCode()
        {
            return _humidityDirectionAndSpeed.GetHashCode();
        }

        public override string ToString()
        {
            return String.Format(
                "T:{0} P:{1} H:{2} D:{3} S:{4}",
                Temperature, Pressure, Humidity, WindDirection, WindSpeed
            );
        }

    }
}
