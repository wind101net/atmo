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
    public class ReadingMinMaxCalculator<TReading> :
        ReadingValueMinMaxCalculator<TReading>,
        IAggregateCalculator<TReading, ReadingRangeAggregate>
		where TReading: IReading
	{

		private TimeRange _time;

		public ReadingMinMaxCalculator() : base() {
			_time = new TimeRange(default(DateTime), default(DateTime));
		}

		public new void Proccess(TReading input) {
			if (null == input || !input.IsValid) {
				return;
			}

			if (_time.Low == default(DateTime) && _time.High == default(DateTime)) {
				_time = new TimeRange(input.TimeStamp);
			}
			else {
				_time.Merge(input.TimeStamp);
			}

            base.Proccess(input);
		}

		public new ReadingRangeAggregate Result {
			get {
				return new ReadingRangeAggregate(
					_time,
					base.Result
				);
			}
		}
	}
}
