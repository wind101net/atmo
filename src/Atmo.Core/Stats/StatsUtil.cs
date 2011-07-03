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
using System.Collections.Generic;
using System.Linq;
using Atmo.Units;

namespace Atmo.Stats {
	public static class StatsUtil {

		public static IEnumerable<ReadingAggregate> AggregateMean<T>(IEnumerable<T> readings, TimeUnit unit)
			where T:IReading
		{
			int count;
			var enumerator = readings.OrderBy(r => r.TimeStamp).GetEnumerator();
			if(!enumerator.MoveNext()) {
				yield break;
			}
			var collectedReadings = new List<T> {enumerator.Current};
			var dateRangeLow = UnitUtility.StripToUnit(enumerator.Current.TimeStamp, unit);
			var dateRangeHigh = UnitUtility.IncrementByUnit(dateRangeLow, unit);
			while (enumerator.MoveNext()) {
				if (enumerator.Current.TimeStamp < dateRangeHigh) {
					collectedReadings.Add(enumerator.Current);
				}
				else {
					var values = AggregateMeanValues(collectedReadings, out count);
					yield return new ReadingAggregate(dateRangeLow, dateRangeHigh, values, count);

					collectedReadings.Clear();
					collectedReadings.Add(enumerator.Current);
					dateRangeLow = UnitUtility.StripToUnit(enumerator.Current.TimeStamp, unit);
					dateRangeHigh = UnitUtility.IncrementByUnit(dateRangeLow, unit);
				}
			}
			if (collectedReadings.Count > 0) {
				var values = AggregateMeanValues(collectedReadings, out count);
				yield return new ReadingAggregate(dateRangeLow, dateRangeHigh, values, count);
			}
		}

		public static ReadingValues AggregateMeanValues<T>(IEnumerable<T> readings)
			where T : IReadingValues
		{
			int c;
			return AggregateMeanValues<T>(readings, out c);
		}

		public static ReadingValues AggregateMeanValues<T>(IEnumerable<T> readings, out int count)
			where T:IReadingValues {
			var calculator = new ReadingValuesMeanCalculator<T>();
			foreach (var reading in readings) {
				calculator.Proccess(reading);
			}
			count = calculator.ProcessedCount;
			return calculator.Result;

		}

		public static ReadingAggregate AggregateMeanAggregates(List<ReadingAggregate> readings) {
			var calculator = new ReadingAggregateMeanCalculator();
			foreach(var reading in readings) {
				calculator.Proccess(reading);
			}
			return calculator.Result;
		}

		public static IEnumerable<ReadingAggregate> JoinParallelMeanReadings(IEnumerable<List<ReadingAggregate>> rawReadings)
		{
			var sortedEnumerators = new List<IEnumerator<ReadingAggregate>>(
				rawReadings
				.Select(
					e =>
						e.OrderBy(sr => sr.TimeStamp)
						.GetEnumerator()
				)
				.Where(e => e.MoveNext())
			);
			while (sortedEnumerators.Count > 0) {
				var readingsToCompile = new List<ReadingAggregate>();
				int count = 0;
				DateTime currentTimeReading = sortedEnumerators.Min(e => e.Current.TimeStamp);
				for (int i = 0; i < sortedEnumerators.Count; i++) {
					var currentEnumerator = sortedEnumerators[i];
					var currentReading = currentEnumerator.Current;
					if (currentReading.TimeStamp == currentTimeReading) {
						readingsToCompile.Add(currentReading);
						count++;
					}
					if (currentReading.TimeStamp <= currentTimeReading) {
						if (!currentEnumerator.MoveNext()) {
							sortedEnumerators.RemoveAt(i);
							i--;
						}
					}
				}
				if (readingsToCompile.Count > 0) {
					yield return AggregateMeanAggregates(readingsToCompile);
				}
			}
		}



	}
}
