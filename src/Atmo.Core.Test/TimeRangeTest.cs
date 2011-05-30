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

using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Atmo.Test {
	
	[TestFixture]
	public class TimeRangeTest {

		private readonly List<DateTime> _times = new List<DateTime> {
			new DateTime(2011,4,5,12,12,12,12),
			new DateTime(1995,12,6,1,0,0),
			new DateTime(1908,1,1,1,1,1), 
		};

		public IEnumerable<DateTime> Times { get { return _times.AsReadOnly(); } }

		[Test]
		public void ConstructorLowHighSpanTest(
			[ValueSource(typeof(TimeRangeTest),"Times")]
			DateTime first,
			[ValueSource(typeof(TimeRangeTest), "Times")]
			DateTime second
		) {

			var range = new TimeRange(first, second);
			
			var low = first < second ? first : second;
			var high = first < second ? second : first;

			Assert.AreEqual(low, range.Low);
			Assert.AreEqual(high, range.High);
			Assert.AreEqual(high.Subtract(low), range.Span);
		}


	}
}
