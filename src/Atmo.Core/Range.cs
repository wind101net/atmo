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
	public struct Range {

		public static bool operator ==(Range a, Range b) {
			return a.Equals(b);
		}

		public static bool operator !=(Range a, Range b) {
			return !a.Equals(b);
		}

		private double _low;
		private double _high;

		public Range(double value) {
			_low = _high = value;
		}

		public Range(double a, double b) {
			if (b < a) {
				_low = b;
				_high = a;
			}
			else {
				_low = a;
				_high = b;
			}
		}

		public Range(Range range) {
			_low = range._low;
			_high = range._high;
		}

		public double Low { get { return _low; } }

		public double High { get { return _high; } }

		public double Mid { get { return (_low + _high)/2.0; } }

		public double Size { get { return _high - _low; } }

		public void Merge(double value) {
			if(Double.IsNaN(_low) || Double.IsNaN(_high)) {
				_low = _high = value;
			}else {
				if(value < _low) {
					_low = value;
				}else if(value > _high) {
					_high = value;
				}
			}
		}

		public void Recenter(double mid) {
			var len = _high - _low;
			_high = mid + (len / 2.0);
			_low = _high - len;
		}
	}
}
