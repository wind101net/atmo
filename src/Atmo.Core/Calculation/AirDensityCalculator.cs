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
using Atmo.Units;

namespace Atmo.Calculation {
	public class AirDensityCalculator {

		public static PressureUnit PressureUnit {
			get { return PressureUnit.Pascals; }
		}

		/// <summary>
		/// Height above sea level in meters.
		/// </summary>
		public readonly double Height;
		/// <summary>
		/// The estimated pressure in Pa
		/// </summary>
		public readonly double EstimatedPressure;

		public AirDensityCalculator() : this(Double.NaN) { }

		/// <summary>
		/// Calculates air density.
		/// </summary>
		/// <param name="height">Height above sea level in meters.</param>
		public AirDensityCalculator(double height) {
			Height = height;
			if (Double.IsNaN(height)) {
				EstimatedPressure = Double.NaN;
			}
			else {
				try {
					// estimate the pressure
					EstimatedPressure = 101325 * Math.Pow((1.0 - ((0.0065*Height)/288.15)), 5.25578);
				}
				catch {
					EstimatedPressure = Double.NaN;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="temperature">The temperature in degrees C.</param>
		/// <param name="humidity">The relative humidity as a decimal (0.0 to 1.0).</param>
		/// <param name="pressure">The pressure in pascals.</param>
		/// <returns></returns>
		public static double AirDensity(double temperature, double humidity, double pressure) {
			var airDensity =
				pressure / 287.05 /
				(
					(temperature + 273.15)
					*
					(1 - (
						(0.378*humidity*610.78)
						/(pressure*Math.Pow(10, (7.5*temperature)/(237.3 + temperature)))
					))
				);

			return airDensity;

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="temperature">The temperature in degrees C.</param>
		/// <param name="humidity">The relative humidity as a decimal (0.0 to 1.0).</param>
		/// <returns></returns>
		public double AirDensity(double temperature, double humidity) {
			return AirDensity(temperature, humidity, EstimatedPressure);
		}

	}
}
