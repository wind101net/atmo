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

namespace Atmo.Units {

	public struct PosixTimeRange {

		private readonly PosixTime _low;
		private readonly PosixTime _high;

		public PosixTimeRange(PosixTime a, PosixTime b) {
			if (a < b) {
				_low = a;
				_high = b;
			}
			else {
				_low = b;
				_high = a;
			}
		}

		public PosixTimeRange(DateTime a, DateTime b)
			: this(
			new PosixTime(a),
			new PosixTime(b)
		) { }

		public PosixTimeRange(TimeRange range)
			: this(range.Low, range.High) { }

		public PosixTime Low { get { return _low; } }

		public PosixTime High { get { return _high; } }

		public int Span { get { return _high.Value - _low.Value; } }

	}
}
