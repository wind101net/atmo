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
			where T:IReadingValues
		{
			const double degToRadFactor = Math.PI/180.0;
			const double radToDegFactor = 180.0/Math.PI;

			double temperatureValue = 0;
			double pressureValue = 0;
			double humidityValue = 0;
			double speedValue = 0;
			double dirSinSum = 0;
			double dirCosSum = 0;

			int temperatureCount = 0;
			int pressureCount = 0;
			int humidityCount = 0;
			int speedCount = 0;
			int dirCount = 0;

			count = 0;
			
			foreach(var reading in readings) {
				if(reading.IsValid) {
					if(reading.IsTemperatureValid) {
						temperatureValue += reading.Temperature;
						temperatureCount++;
					}
					if(reading.IsPressureValid) {
						pressureValue += reading.Pressure;
						pressureCount++;
					}
					if(reading.IsHumidityValid) {
						humidityValue += reading.Humidity;
						humidityCount++;
					}
					if(reading.IsWindSpeedValid) {
						speedValue += reading.WindSpeed;
						speedCount++;
					}
					if(reading.IsWindDirectionValid) {
						var dir = reading.WindDirection*degToRadFactor;
						dirSinSum += Math.Sin(dir);
						dirCosSum += Math.Cos(dir);
						dirCount++;
					}
					count++;
				}
			}

			temperatureValue = temperatureCount > 0
				? temperatureValue / temperatureCount
				: Double.NaN;

			pressureValue = pressureCount > 0
				? pressureValue / pressureCount
				: Double.NaN;

			humidityValue = humidityCount > 0
				? humidityValue / humidityCount
				: Double.NaN;

			speedValue = speedCount > 0
				? speedValue / speedCount
				: Double.NaN;

			var directionValue = dirCount > 0
				? UnitUtility.WrapDegree(Math.Atan2(dirSinSum/dirCount, dirCosSum/dirCount) * radToDegFactor)
				: Double.NaN;

			return new ReadingValues(temperatureValue, pressureValue, humidityValue, directionValue, speedValue);
		}

		public static ReadingAggregate AggregateMeanAggregates(List<ReadingAggregate> readings)
		{
			const double degToRadFactor = Math.PI / 180.0;
			const double radToDegFactor = 180.0 / Math.PI;

			double temperatureValue = 0;
			double pressureValue = 0;
			double humidityValue = 0;
			double speedValue = 0;
			double dirSinSum = 0;
			double dirCosSum = 0;

			int temperatureCount = 0;
			int pressureCount = 0;
			int humidityCount = 0;
			int speedCount = 0;
			int dirCount = 0;

			int count = 0;

			var minStart = DateTime.MaxValue;
			var maxEnd = DateTime.MinValue;

			foreach (var reading in readings) {
				if (reading.IsValid) {
					var localCount = reading.Count;

					if(reading.BeginStamp < minStart) {
						minStart = reading.BeginStamp;
					}
					if(reading.EndStamp > maxEnd) {
						maxEnd = reading.EndStamp;
					}

					if (reading.IsTemperatureValid) {
						temperatureValue += (reading.Temperature * localCount);
						temperatureCount += localCount;
					}
					if (reading.IsPressureValid) {
						pressureValue += (reading.Pressure * localCount);
						pressureCount += localCount;
					}
					if (reading.IsHumidityValid) {
						humidityValue += (reading.Humidity * localCount);
						humidityCount += localCount;
					}
					if (reading.IsWindSpeedValid) {
						speedValue += (reading.WindSpeed * localCount);
						speedCount += localCount;
					}
					if (reading.IsWindDirectionValid) {
						var dir = reading.WindDirection * degToRadFactor;
						dirSinSum += (Math.Sin(dir) * localCount);
						dirCosSum += (Math.Cos(dir) * localCount);
						dirCount += localCount;
					}
					count += localCount;
				}
			}

			temperatureValue = temperatureCount > 0
				? temperatureValue / temperatureCount
				: Double.NaN;

			pressureValue = pressureCount > 0
				? pressureValue / pressureCount
				: Double.NaN;

			humidityValue = humidityCount > 0
				? humidityValue / humidityCount
				: Double.NaN;

			speedValue = speedCount > 0
				? speedValue / speedCount
				: Double.NaN;

			var directionValue = dirCount > 0
				? UnitUtility.WrapDegree(Math.Atan2(dirSinSum / dirCount, dirCosSum / dirCount) * radToDegFactor)
				: Double.NaN;

			return new ReadingAggregate(
				minStart, maxEnd,
				new ReadingValues(temperatureValue, pressureValue, humidityValue, directionValue, speedValue),
				count
			);
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
