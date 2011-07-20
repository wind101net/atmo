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
using Atmo.Stats;
using NUnit.Framework;

namespace Atmo.Test
{
    [TestFixture]
    public class PackedReadingsHourSummaryTest
    {

        private ReadingValues[] _values;
        private ReadingValues _min;
        private ReadingValues _max;
        private ReadingValues _mean;
        private ReadingValues _median;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _values = new[] {
                new ReadingValues(1.0,	2.0,		Double.NaN,180,	4.0),
				new ReadingValues(2.0,	Double.NaN,	Double.NaN,45,	6.0),
				new ReadingValues(3.0,	6.0,		Double.NaN,315,	Double.NaN),
				new ReadingValues(Double.NaN,Double.NaN,Double.NaN,Double.NaN,Double.NaN)
			};
            var minMaxCalc = new ReadingValueMinMaxCalculator<ReadingValues>();
            var meanCalc = new ReadingValuesMeanCalculator<ReadingValues>();
            foreach(var readingValues in _values)
            {
                minMaxCalc.Proccess(readingValues);
                meanCalc.Proccess(readingValues);
            }
            _min = minMaxCalc.Result.Min;
            _max = minMaxCalc.Result.Max;
            _mean = meanCalc.Result;
            _median = ReadingValues.CreateInvalid();
        }

        [Test]
        public void ConstructionVerificationTest()
        {
            var startTime = new DateTime(2011, 2, 3, 8, 0, 0);
            var summary = new PackedReadingsHourSummary(
                startTime,
                new PackedReadingValues(_min),
                new PackedReadingValues(_max),
                new PackedReadingValues(_mean),
                new PackedReadingValues(_median),
                _values.Length
            );

            Assert.AreEqual(startTime, summary.BeginStamp);
            Assert.AreEqual(new TimeSpan(0,1,0,0), summary.TimeSpan);
            Assert.AreEqual(startTime.AddHours(1).Subtract(new TimeSpan(1)), summary.EndStamp);

            Assert.AreEqual(_min, summary.Min);
            Assert.AreEqual(_max, summary.Max);
            Assert.AreEqual(_mean, summary.Mean);
            Assert.AreEqual(_values.Length, summary.Count);


        }
    }
}
