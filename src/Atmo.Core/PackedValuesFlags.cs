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

	[Flags]
	public enum PackedValuesFlags : byte {
		/// <summary>
		/// Set when the pressure value is valid.
		/// </summary>
		Pressure = 0x01,
		/// <summary>
		/// Set when the humidity value is valid.
		/// </summary>
		Humidity = 0x02,
		/// <summary>
		/// Set when the wind direction value is valid.
		/// </summary>
		WindDirection = 0x04,
		/// <summary>
		/// Set when the wind speed value is valid.
		/// </summary>
		WindSpeed = 0x08,
		/// <summary>
		/// The binary combination of Pressure, Humidity, WindDirection, and WindSpeed .
		/// </summary>
		AllDataFlags = Pressure | Humidity | WindDirection | WindSpeed,
		/// <summary>
		/// Set when the temperature was recieved from an anemometer.
		/// </summary>
		AnemTemperatureSource = 0x10,
		/// <summary>
		/// Set for a record that acts as a header for a larger group of data records.
		/// </summary>
		/// <remarks>
		/// The binary combination of pressure and wind direction was chosen for this value as no current firmware/hardware sets this value for data records.
		/// </remarks>
		Header = Pressure | WindDirection

	}
}
