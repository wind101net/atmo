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

namespace Atmo {

	/// <summary>
	/// various reading from sensors.
	/// </summary>
	public interface IReadingValues {
		double Temperature { get; }
		double Pressure { get; }
		double Humidity { get; }
		/// <summary>
		/// Wind speed in metres/second .
		/// </summary>
		double WindSpeed { get; }
		/// <summary>
		/// Wind direction in degrees.
		/// </summary>
		double WindDirection { get; }

		/// <summary>
		/// Returns true if the reading is valid.
		/// </summary>
		bool IsValid { get; }
		/// <summary>
		/// Returns true when temperature is valid.
		/// </summary>
		bool IsTemperatureValid { get; }
		/// <summary>
		/// Returns true when pressure is valid.
		/// </summary>
		bool IsPressureValid { get; }
		/// <summary>
		/// Returns true when humidity is valid.
		/// </summary>
		bool IsHumidityValid { get; }
		/// <summary>
		/// Returns true when wind speed is valid;
		/// </summary>
		bool IsWindSpeedValid { get; }
		/// <summary>
		/// Returns true when wind direction is valid.
		/// </summary>
		bool IsWindDirectionValid { get; }
	}
}
