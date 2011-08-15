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

namespace Atmo.Stats {
	public class PackedReadingsTenMinutesSummary : PackedReadingsSummary {

		private static readonly TimeSpan TenMinutes = new TimeSpan(0, 10, 0);

		public PackedReadingsTenMinutesSummary(
			DateTime beginStamp,
			PackedReadingValues min,
			PackedReadingValues max,
			PackedReadingValues mean,
			PackedReadingValues median,
			int count
		)
			: base(
				beginStamp,
				min,
				max,
				mean,
				median,
				count
				) { }

		public override DateTime EndStamp {
			get { return BeginStamp.AddMinutes(10).Subtract(OneTick); }
		}

		public override TimeSpan TimeSpan {
			get { return TenMinutes; }
		}

	}
}
