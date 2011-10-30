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

	/// <summary>
	/// A time range between two time values.
	/// </summary>
	public struct TimeRange {

		/// <summary>
		/// The lowest time value in the range.
		/// </summary>
		private DateTime _low;
		/// <summary>
		/// The highest time value in the range.
		/// </summary>
		private DateTime _high;

		public TimeRange(DateTime value) {
			_low = _high = value;
		}

		public TimeRange(DateTime a, DateTime b) {
			if (a < b) {
				_low = a;
				_high = b;
			}
			else {
				_low = b;
				_high = a;
			}
		}

		public TimeRange(TimeRange range) {
			_low = range._low;
			_high = range._high;
		}

		public DateTime Low { get { return _low; } }

		public DateTime High { get { return _high; } }

		public TimeSpan Span { get { return High.Subtract(Low); } }

		public void Merge(DateTime time) {
			if (time < _low) {
				_low = time;
			}else if(time > _high) {
				_high = time;
			}
		}

		public void Merge(TimeRange timeRange) {
			if(timeRange._low < _low) {
				_low = timeRange._low;
			}
			if(timeRange._high > _high) {
				_high = timeRange._high;
			}
		}

		public bool Intersects(TimeRange range) {
			return (Low <= range.High) && (range.Low <= High);
		}

	}
}
