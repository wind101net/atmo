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

namespace Atmo.Data {
	public class DaqDataFileInfo {

		internal const int RecordSize = 8;
		internal const byte HeaderCodeByte = 0xa5;


		public static bool TryConvert7ByteDateTime(byte[] data, int offset, out DateTime stamp) {

			// TODO: test for an invalid date that falls within the ranges.

			if (data.Length - offset >= 7) {
				var year = (short)((data[offset] << 8) | data[offset + 1]);
				var month = data[offset + 2];
				var day = data[offset + 3];
				var hour = data[offset + 4];
				var min = data[offset + 5];
				var sec = data[offset + 6];
				if (
					year > 0
					&& month <= 12
					&& day <= 31
					&& hour <= 24
					&& min <= 59
					&& sec <= 59
					&& 0 != year
					&& 0 != month
					&& 0 != day
				) {
					stamp = new DateTime(year, month, day, hour, min, sec);
					return true;
				}
			}
			stamp = default(DateTime);
			return false;
		}

	}
}
