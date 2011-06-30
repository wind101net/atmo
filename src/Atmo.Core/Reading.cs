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
	public class Reading : ReadingValues, IReading {

		public Reading() : this(default(DateTime)) { }

		public Reading(DateTime timeStamp) : base() {
			TimeStamp = timeStamp;
		}

		public Reading(
			DateTime timeStamp,
			double temperature,
			double pressure,
			double humidity,
			double windDirection,
			double windSpeed
		)
			: base(temperature,pressure,humidity,windDirection,windSpeed)
		{
			TimeStamp = timeStamp;
		}

		public Reading(
			double temperature,
			double pressure,
			double humidity,
			double windDirection,
			double windSpeed
		)
			: base(temperature, pressure, humidity, windDirection, windSpeed)
		{
			TimeStamp = default(DateTime);
		}

		public Reading(DateTime timeStamp, ReadingValues values) : base(values) {
			TimeStamp = timeStamp;
		}

		public Reading(DateTime timeStamp, IReadingValues values)
			: base(values) {
			TimeStamp = timeStamp;
		}

		public Reading(IReading reading)
			: this(reading.TimeStamp,reading) { }

		public DateTime TimeStamp { get; set; }

	}
}
