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

	public struct PackedReading : IReading {

		public readonly DateTime TimeStamp;
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
	}
}
