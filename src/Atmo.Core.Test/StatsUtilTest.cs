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
using System.Linq;
using Atmo.Units;
using NUnit.Framework;

namespace Atmo.Test {
	[TestFixture]
	public class StatsUtilTest {

		[Test]
		public void AggregateMeanValuesTest() {
			var values = new[] {
                new ReadingValues(1.0,	2.0,		Double.NaN,180,	4.0),
				new ReadingValues(2.0,	Double.NaN,	Double.NaN,45,	6.0),
				new ReadingValues(3.0,	4.0,		Double.NaN,315,	Double.NaN),
				new ReadingValues(Double.NaN,Double.NaN,Double.NaN,Double.NaN,Double.NaN)
			};

			var result = Stats.StatsUtil.AggregateMeanValues(values);

			Assert.AreEqual(2, result.Temperature);
			Assert.AreEqual(3, result.Pressure);
			Assert.AreEqual(Double.NaN, result.Humidity);
			Assert.AreEqual(0, result.WindDirection, 0.000000000001);
			Assert.AreEqual(5, result.WindSpeed);

		}

		[Test]
		public void AggregateMeanTest() {

			var start = new DateTime(2011, 6, 30, 22, 18, 58);
			var values = new[] {
                new ReadingValues(1.0,	2.0,		Double.NaN,180,	4.0),
				new ReadingValues(2.0,	Double.NaN,	Double.NaN,45,	6.0),
				new ReadingValues(3.0,	4.0,		Double.NaN,315,	Double.NaN),
				new ReadingValues(Double.NaN,Double.NaN,Double.NaN,Double.NaN,Double.NaN)
			};
			var readings = new Reading[(values.Length*2)+2];
			for(var i = 0; i < readings.Length; i++) {
				readings[i] = new Reading(
					start.Add(new TimeSpan(0,0,0,i)),
					values[i % values.Length]
				);
			}

			var means = Stats.StatsUtil.AggregateMean(readings, TimeUnit.Minute).ToArray();
			
			Assert.That(means,Has.Length.EqualTo(2));

			Assert.AreEqual(1.5, means[0].Temperature);
			Assert.AreEqual(2, means[0].Pressure);
			Assert.AreEqual(Double.NaN, means[0].Humidity);
			Assert.AreEqual(
				(values[0].WindDirection + values[1].WindDirection) / 2.0,
				means[0].WindDirection, 0.000000000001
			);
			Assert.AreEqual(5, means[0].WindSpeed);
			Assert.AreEqual(new DateTime(2011,6,30,22,18,0), means[0].BeginStamp);
			Assert.AreEqual(new DateTime(2011,6,30,22,19,0), means[0].EndStamp);

			Assert.AreEqual(2, means[1].Temperature);
			Assert.AreEqual(3, means[1].Pressure);
			Assert.AreEqual(Double.NaN, means[1].Humidity);
			Assert.AreEqual(0, means[1].WindDirection, 0.000000000001);
			Assert.AreEqual(5, means[1].WindSpeed);
			Assert.AreEqual(new DateTime(2011, 6, 30, 22, 19, 0), means[1].BeginStamp);
			Assert.AreEqual(new DateTime(2011, 6, 30, 22, 20, 0), means[1].EndStamp);

		}

	}
}
