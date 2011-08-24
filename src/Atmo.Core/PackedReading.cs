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

namespace Atmo {

	/// <inheritdoc/>
	public struct PackedReading : IReading {

		public static readonly int SizeOf = PackedReadingValues.SizeOf + sizeof(long);

		/// <summary>
		/// The time stamp of the reading.
		/// </summary>
		public readonly DateTime TimeStamp;
		/// <summary>
		/// The various sensor values for the reading.
		/// </summary>
		public readonly PackedReadingValues Values;

		public PackedReading(DateTime stamp, PackedReadingValues values) {
            TimeStamp = stamp;
            Values = values;
        }

		/// <inheritdoc/>
		DateTime IReading.TimeStamp {
			get { return TimeStamp; }
		}

		/// <inheritdoc/>
		public double Temperature {
			get { return Values.Temperature; }
		}

		/// <inheritdoc/>
		public double Pressure {
			get { return Values.Pressure; }
		}

		/// <inheritdoc/>
		public double Humidity {
			get { return Values.Humidity; }
		}

		/// <inheritdoc/>
		public double WindSpeed {
			get { return Values.WindSpeed; }
		}

		/// <inheritdoc/>
		public double WindDirection {
			get { return Values.WindDirection; }
		}

		/// <inheritdoc/>
		public bool IsValid {
			get { return Values.IsValid; }
		}

		/// <inheritdoc/>
		public bool IsTemperatureValid {
			get { return Values.IsTemperatureValid; }
		}

		/// <inheritdoc/>
		public bool IsPressureValid {
			get { return Values.IsPressureValid; }
		}

		/// <inheritdoc/>
		public bool IsHumidityValid {
			get { return Values.IsHumidityValid; }
		}

		/// <inheritdoc/>
		public bool IsWindSpeedValid {
			get { return Values.IsWindSpeedValid; }
		}

		/// <inheritdoc/>
		public bool IsWindDirectionValid {
			get { return Values.IsWindDirectionValid; }
		}
	}
}
