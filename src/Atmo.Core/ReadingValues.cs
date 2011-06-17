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
	public class ReadingValues 
		: IReadingValues {

		/// <summary>
		/// Creates an invalid reading value set.
		/// </summary>
		/// <returns>A reading value set with invalid values.</returns>
		/// <remarks>All fields are set to Double.NaN.</remarks>
		public static ReadingValues CreateInvalid() {
			return new ReadingValues(Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN);
		}

		public ReadingValues(
			double temperature,
			double pressure,
			double humidity,
			double windDirection,
			double windSpeed
		) {
			Temperature = temperature;
			Pressure = pressure;
			Humidity = humidity;
			WindDirection = windDirection;
			WindSpeed = windSpeed;
		}

		public double Temperature { get; set; }

		public double Pressure { get; set; }

		public double Humidity { get; set; }

		public double WindSpeed { get; set; }

		public double WindDirection { get; set; }

		/// <inheritdoc/>
		public bool IsValid {
			get { throw new NotImplementedException(); }
		}

		/// <inheritdoc/>
		public bool IsTemperatureValid {
			get { throw new NotImplementedException(); }
		}

		/// <inheritdoc/>
		public bool IsPressureValid {
			get { throw new NotImplementedException(); }
		}

		/// <inheritdoc/>
		public bool IsHumidityValid {
			get { throw new NotImplementedException(); }
		}

		/// <inheritdoc/>
		public bool IsWindSpeedValid {
			get { throw new NotImplementedException(); }
		}

		/// <inheritdoc/>
		public bool IsWindDirectionValid {
			get { throw new NotImplementedException(); }
		}
	}
}
