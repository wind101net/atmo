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
	public class ReadingValues :
		IReadingValues,
		IEquatable<ReadingValues>,
		IEquatable<IReadingValues>
	{

		/// <summary>
		/// Creates an invalid reading value set.
		/// </summary>
		/// <returns>A reading value set with invalid values.</returns>
		/// <remarks>All fields are set to Double.NaN.</remarks>
		public static ReadingValues CreateInvalid() {
			return new ReadingValues(Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN);
		}

		public ReadingValues() {
			Temperature = 0;
			Pressure = 0;
			Humidity = 0;
			WindDirection = 0;
			WindSpeed = 0;
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

		public ReadingValues(IReadingValues values) {
			if(null == values) {
				Temperature = Double.NaN;
				Pressure = Double.NaN;
				Humidity = Double.NaN;
				WindDirection = Double.NaN;
				WindSpeed = Double.NaN;
			}else {
				Temperature = values.Temperature;
				Pressure = values.Pressure;
				Humidity = values.Humidity;
				WindDirection = values.WindDirection;
				WindSpeed = values.WindSpeed;
			}
		}

		/// <inheritdoc/>
		public double Temperature { get; set; }

		/// <inheritdoc/>
		public double Pressure { get; set; }

		/// <inheritdoc/>
		public double Humidity { get; set; }

		/// <inheritdoc/>
		public double WindSpeed { get; set; }

		/// <inheritdoc/>
		public double WindDirection { get; set; }

		/// <inheritdoc/>
		public bool IsValid {
			get { return IsTemperatureValid || IsPressureValid || IsHumidityValid || IsWindSpeedValid || IsWindDirectionValid; }
		}

		/// <inheritdoc/>
		public bool IsTemperatureValid {
			get { return !Double.IsNaN(Temperature); }
		}

		/// <inheritdoc/>
		public bool IsPressureValid {
			get { return !Double.IsNaN(Pressure); }
		}

		/// <inheritdoc/>
		public bool IsHumidityValid {
			get { return !Double.IsNaN(Humidity); }
		}

		/// <inheritdoc/>
		public bool IsWindSpeedValid {
			get { return !Double.IsNaN(WindSpeed); }
		}

		/// <inheritdoc/>
		public bool IsWindDirectionValid {
			get { return !Double.IsNaN(WindDirection); }
		}

		/// <inheritdoc/>
		public bool Equals(ReadingValues other) {
			return null != other
			    && Temperature == other.Temperature
			    && Pressure == other.Pressure
			    && Humidity == other.Humidity
			    && WindSpeed == other.WindSpeed
			    && WindDirection == other.WindDirection
			;
		}

		/// <inheritdoc/>
		public bool Equals(IReadingValues other) {
			return null != other
				&& Temperature == other.Temperature
				&& Pressure == other.Pressure
				&& Humidity == other.Humidity
				&& WindSpeed == other.WindSpeed
				&& WindDirection == other.WindDirection
			;
		}

		/// <inheritdoc/>
		public override bool Equals(object obj) {
			return null != obj && (
				(obj is ReadingValues) ? Equals(obj as ReadingValues)
				: (obj is IReadingValues) && Equals(obj as IReadingValues)
			);
		}

		public override int GetHashCode() {
			return WindSpeed.GetHashCode() ^ Temperature.GetHashCode();
		}

		public override string ToString() {
			return String.Format(
				"T:{0} P:{1} H:{2} D:{3} S:{4}",
				Temperature, Pressure, Humidity, WindDirection, WindSpeed
			);
		}

	}
}
