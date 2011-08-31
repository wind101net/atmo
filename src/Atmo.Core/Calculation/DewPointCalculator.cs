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

namespace Atmo.Calculation {

	/// <summary>
	/// A calculator utility class that calculates the dewpoint.
	/// </summary>
	public static class DewPointCalculator {
		/// <summary>
		/// Calculates the dew point.
		/// </summary>
		/// <param name="temperature">The temperature in degrees C.</param>
		/// <param name="humidity">The relative humidity as a ratio (0.0 to 1.0).</param>
		/// <returns>The dew point.</returns>
		public static double DewPoint(double temperature, double humidity) {
			// some constants used by this equation
			const double m = 17.62;
			const double tn = 243.12;

			var h = ((Math.Log10(humidity*100.0) - 2.0)/0.4343)
				+ ((m*temperature)/(tn + temperature));
			return (tn * h) / (m - h);
		}
	}
}
