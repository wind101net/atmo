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

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="readings"></param>
		/// <returns></returns>
		/// <remarks>
		/// Result is ordered by stamp in ascending order.
		/// </remarks>
		public static IEnumerable<ReadingsSummary> Summarize<T>(IEnumerable<T> readings, TimeUnit unit) where T : IReading {
			List<T> collectedReadings = new List<T>();
			IEnumerator<T> enumerator = readings.OrderBy(r => r.TimeStamp).GetEnumerator();
			if (enumerator.MoveNext()) {
				collectedReadings.Add(enumerator.Current);
				DateTime dateRangeLow = UnitUtility.StripToUnit(enumerator.Current.TimeStamp, unit);
				DateTime dateRangeHigh = UnitUtility.IncrementByUnit(dateRangeLow, unit);
				while (enumerator.MoveNext()) {
					if (enumerator.Current.TimeStamp < dateRangeHigh) {
						collectedReadings.Add(enumerator.Current);
					}
					else {
						yield return FormSummary<T>(dateRangeLow, dateRangeHigh, collectedReadings);
						collectedReadings.Clear();
						collectedReadings.Add(enumerator.Current);
						dateRangeLow = UnitUtility.StripToUnit(enumerator.Current.TimeStamp, unit);
						dateRangeHigh = UnitUtility.IncrementByUnit(dateRangeLow, unit);
					}
				}
				if (collectedReadings.Count > 0) {
					yield return FormSummary<T>(dateRangeLow, dateRangeHigh, collectedReadings);
				}
			}
		}

		private static ReadingsSummary FormSummary<T>(DateTime dateRangeLow, DateTime dateRangeHigh, List<T> readings) where T : IReadingValues {
			List<double> tempValues = new List<double>(readings.Count);
			List<double> presValues = new List<double>(readings.Count);
			List<double> humValues = new List<double>(readings.Count);
			List<double> speedValues = new List<double>(readings.Count);
			List<double> dirValues = new List<double>(readings.Count);
			double tempSum = 0;
			double presSum = 0;
			double humSum = 0;
			double speedSum = 0;
			double dirSum = 0;
			Dictionary<double, int> tempCounts = new Dictionary<double, int>();
			Dictionary<double, int> presCounts = new Dictionary<double, int>();
			Dictionary<double, int> humCounts = new Dictionary<double, int>();
			Dictionary<double, int> speedCounts = new Dictionary<double, int>();
			Dictionary<double, int> dirCounts = new Dictionary<double, int>();
			// TODO: later direction values should be calculated using vectors
			// TODO: take into account for missing sensors
			double value;
			int count;
			for (int i = 0; i < readings.Count; i++) {
				T reading = readings[i];
				if (reading.IsTemperatureValid) {
					tempValues.Add(value = reading.Temperature);
					tempSum += value;
					value = Math.Round(value);
					tempCounts[value] = (tempCounts.TryGetValue(value, out count)) ? count + 1 : 1;
				}
				if (reading.IsPressureValid) {
					presValues.Add(value = reading.Pressure);
					presSum += value;
					value = Math.Round(value);
					presCounts[value] = (presCounts.TryGetValue(value, out count)) ? count + 1 : 1;
				}
				if (reading.IsHumidityValid) {
					humValues.Add(value = reading.Humidity);
					humSum += value;
					value = Math.Round(value);
					humCounts[value] = (humCounts.TryGetValue(value, out count)) ? count + 1 : 1;
				}
				if (reading.IsWindSpeedValid) {
					speedValues.Add(value = reading.WindSpeed);
					speedSum += value;
					value = Math.Round(value * 2.0) / 2; // round to nearest .5
					speedCounts[value] = (speedCounts.TryGetValue(value, out count)) ? count + 1 : 1;
				}
				if (reading.IsWindDirectionValid) {
					dirValues.Add(value = reading.WindDirection);
					dirSum += value;
					value = Math.Round(value);
					dirCounts[value] = (dirCounts.TryGetValue(value, out count)) ? count + 1 : 1;
				}
			}
			tempValues.Sort();
			presValues.Sort();
			humValues.Sort();
			speedValues.Sort();
			dirValues.Sort();
			//int lasti = readings.Count - 1;
			//int midi = readings.Count / 2;
			//double dCount = (double)readings.Count;
			ReadingValues minValues = new ReadingValues(
				tempValues.Count == 0 ? Double.NaN : tempValues[0],
				presValues.Count == 0 ? Double.NaN : presValues[0],
				humValues.Count == 0 ? Double.NaN : humValues[0],
				speedValues.Count == 0 ? Double.NaN : speedValues[0],
				dirValues.Count == 0 ? Double.NaN : dirValues[0]
			);
			ReadingValues maxValues = new ReadingValues(
				tempValues.Count == 0 ? Double.NaN : tempValues[tempValues.Count - 1],
				presValues.Count == 0 ? Double.NaN : presValues[presValues.Count - 1],
				humValues.Count == 0 ? Double.NaN : humValues[humValues.Count - 1],
				speedValues.Count == 0 ? Double.NaN : speedValues[speedValues.Count - 1],
				dirValues.Count == 0 ? Double.NaN : dirValues[dirValues.Count - 1]
			);
			ReadingValues meanValues = new ReadingValues(
				tempValues.Count == 0 ? Double.NaN : (tempSum / (double)tempValues.Count),
				presValues.Count == 0 ? Double.NaN : (presSum / (double)presValues.Count),
				humValues.Count == 0 ? Double.NaN : (humSum / (double)humValues.Count),
				speedValues.Count == 0 ? Double.NaN : (speedSum / (double)speedValues.Count),
				dirValues.Count == 0 ? Double.NaN : (dirSum / (double)dirValues.Count)
			);
			ReadingValues medianValues = new ReadingValues(
				tempValues.Count == 0 ? Double.NaN : tempValues[tempValues.Count / 2],
				presValues.Count == 0 ? Double.NaN : presValues[presValues.Count / 2],
				humValues.Count == 0 ? Double.NaN : humValues[humValues.Count / 2],
				speedValues.Count == 0 ? Double.NaN : speedValues[speedValues.Count / 2],
				dirValues.Count == 0 ? Double.NaN : dirValues[dirValues.Count / 2]
			);
			return new ReadingsSummary(
				dateRangeLow,
				dateRangeHigh.Subtract(new TimeSpan(1)),
				minValues,
				maxValues, meanValues,
				medianValues,
				readings.Count,
				tempCounts,
				presCounts,
				humCounts,
				speedCounts,
				dirCounts
			);
		}

		public static ReadingsSummary Combine<TSummary>(List<TSummary> readings) where TSummary : IReadingsSummary {



			int totalCount = readings.Sum(r => r.Count);
			var summary = new ReadingsSummary(
				readings.Min(r => r.BeginStamp),
				readings.Max(r => r.EndStamp),
				new ReadingValues(Double.MaxValue, Double.MaxValue, Double.MaxValue, double.MaxValue, double.MaxValue),
				new ReadingValues(Double.MinValue, Double.MinValue, Double.MinValue, double.MinValue, double.MinValue),
				new ReadingValues(0, 0, 0, 0, 0),
				new ReadingValues(0, 0, 0, 0, 0), // TODO: median
				totalCount,
				new Dictionary<double, int>(),
				new Dictionary<double, int>(),
				new Dictionary<double, int>(),
				new Dictionary<double, int>(),
				new Dictionary<double, int>()
			);
			// TODO: use calculator?
			foreach (var reading in readings) {
				if (summary.Min.Temperature > reading.Min.Temperature) {
					summary.Min.Temperature = reading.Min.Temperature;
				}
				if (summary.Min.Pressure > reading.Min.Pressure) {
					summary.Min.Pressure = reading.Min.Pressure;
				}
				if (summary.Min.Humidity > reading.Min.Humidity) {
					summary.Min.Humidity = reading.Min.Humidity;
				}
				if (summary.Min.WindSpeed > reading.Min.WindSpeed) {
					summary.Min.WindSpeed = reading.Min.WindSpeed;
				}
				if (summary.Min.WindDirection > reading.Min.WindDirection) {
					summary.Min.WindDirection = reading.Min.WindDirection;
				}
				if (summary.Max.Temperature < reading.Max.Temperature) {
					summary.Max.Temperature = reading.Max.Temperature;
				}
				if (summary.Max.Pressure < reading.Max.Pressure) {
					summary.Max.Pressure = reading.Max.Pressure;
				}
				if (summary.Max.Humidity < reading.Max.Humidity) {
					summary.Max.Humidity = reading.Max.Humidity;
				}
				if (summary.Max.WindSpeed < reading.Max.WindSpeed) {
					summary.Max.WindSpeed = reading.Max.WindSpeed;
				}
				if (summary.Max.WindDirection < reading.Max.WindDirection) {
					summary.Max.WindDirection = reading.Max.WindDirection;
				}
				var readingCountD = (double)reading.Count;
				summary.Mean.Temperature += reading.Mean.Temperature * readingCountD;
				summary.Mean.Pressure += reading.Mean.Pressure * readingCountD;
				summary.Mean.Humidity += reading.Mean.Humidity * readingCountD;
				summary.Mean.WindSpeed += reading.Mean.WindSpeed * readingCountD;
				summary.Mean.WindDirection += reading.Mean.WindDirection * readingCountD;
				AppendCounts(summary.TemperatureCounts, reading.GetTemperatureCounts());
				AppendCounts(summary.PressureCounts, reading.GetPressureCounts());
				AppendCounts(summary.HumidityCounts, reading.GetHumidityCounts());
				AppendCounts(summary.WindSpeedCounts, reading.GetWindSpeedCounts());
				AppendCounts(summary.WindDirectionCounts, reading.GetWindDirectionCounts());
			}
			var totalCountD = (double)totalCount;
			if (0 != totalCountD && 1 != totalCountD) {
				summary.Mean.Temperature /= totalCountD;
				summary.Mean.Pressure /= totalCountD;
				summary.Mean.Humidity /= totalCountD;
				summary.Mean.WindSpeed /= totalCountD;
				summary.Mean.WindDirection /= totalCountD;
			}
			return summary;
		}

		public static ReadingsSummary Combine(List<ReadingsSummary> readings) {
			int totalCount = readings.Sum(r => r.Count);
			var summary = new ReadingsSummary(
				readings.Min(r => r.BeginStamp),
				readings.Max(r => r.EndStamp),
				new ReadingValues(Double.MaxValue, Double.MaxValue, Double.MaxValue, double.MaxValue, double.MaxValue),
				new ReadingValues(Double.MinValue, Double.MinValue, Double.MinValue, double.MinValue, double.MinValue),
				new ReadingValues(0, 0, 0, 0, 0),
				new ReadingValues(0, 0, 0, 0, 0), // TODO: median
				totalCount,
				new Dictionary<double, int>(),
				new Dictionary<double, int>(),
				new Dictionary<double, int>(),
				new Dictionary<double, int>(),
				new Dictionary<double, int>()
			);
			// TODO: use calculator?
			foreach (var reading in readings) {
				if (summary.Min.Temperature > reading.Min.Temperature) {
					summary.Min.Temperature = reading.Min.Temperature;
				}
				if (summary.Min.Pressure > reading.Min.Pressure) {
					summary.Min.Pressure = reading.Min.Pressure;
				}
				if (summary.Min.Humidity > reading.Min.Humidity) {
					summary.Min.Humidity = reading.Min.Humidity;
				}
				if (summary.Min.WindSpeed > reading.Min.WindSpeed) {
					summary.Min.WindSpeed = reading.Min.WindSpeed;
				}
				if (summary.Min.WindDirection > reading.Min.WindDirection) {
					summary.Min.WindDirection = reading.Min.WindDirection;
				}
				if (summary.Max.Temperature < reading.Max.Temperature) {
					summary.Max.Temperature = reading.Max.Temperature;
				}
				if (summary.Max.Pressure < reading.Max.Pressure) {
					summary.Max.Pressure = reading.Max.Pressure;
				}
				if (summary.Max.Humidity < reading.Max.Humidity) {
					summary.Max.Humidity = reading.Max.Humidity;
				}
				if (summary.Max.WindSpeed < reading.Max.WindSpeed) {
					summary.Max.WindSpeed = reading.Max.WindSpeed;
				}
				if (summary.Max.WindDirection < reading.Max.WindDirection) {
					summary.Max.WindDirection = reading.Max.WindDirection;
				}
				var readingCountD = (double)reading.Count;
				summary.Mean.Temperature += reading.Mean.Temperature * readingCountD;
				summary.Mean.Pressure += reading.Mean.Pressure * readingCountD;
				summary.Mean.Humidity += reading.Mean.Humidity * readingCountD;
				summary.Mean.WindSpeed += reading.Mean.WindSpeed * readingCountD;
				summary.Mean.WindDirection += reading.Mean.WindDirection * readingCountD;
				AppendCounts(summary.TemperatureCounts, reading.TemperatureCounts);
				AppendCounts(summary.PressureCounts, reading.PressureCounts);
				AppendCounts(summary.HumidityCounts, reading.HumidityCounts);
				AppendCounts(summary.WindSpeedCounts, reading.WindSpeedCounts);
				AppendCounts(summary.WindDirectionCounts, reading.WindDirectionCounts);
			}
			var totalCountD = (double)totalCount;
			if (0 != totalCountD && 1 != totalCountD) {
				summary.Mean.Temperature /= totalCountD;
				summary.Mean.Pressure /= totalCountD;
				summary.Mean.Humidity /= totalCountD;
				summary.Mean.WindSpeed /= totalCountD;
				summary.Mean.WindDirection /= totalCountD;
			}
			return summary;
		}

		public static void AppendCounts(Dictionary<double, int> destination, Dictionary<double, int> source) {
			foreach (var kvp in source) {
				int count;
				if (destination.TryGetValue(kvp.Key, out count)) {
					destination[kvp.Key] = count + kvp.Value;
				}
				else {
					destination[kvp.Key] = kvp.Value;
				}
			}
		}

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




		public static IEnumerable<ReadingsSummary> JoinReadingSummaryEnumerable<TSummary>(IEnumerable<IEnumerable<TSummary>> sensorReadings) where TSummary : IReadingsSummary {
			var sortedEnumerators = sensorReadings
				.Select(readingSet => readingSet.OrderBy(r => r.BeginStamp).GetEnumerator())
				.Where(e => e.MoveNext())
				.ToList();
			var forCompilation = new List<TSummary>();
			while(sortedEnumerators.Count > 0) {
				forCompilation.Clear();
				//int count = 0;
				var currentTimeReading = sortedEnumerators.Min(e => e.Current.BeginStamp);
				for(int i = 0; i < sortedEnumerators.Count; i++) {
					var currentEnumerator = sortedEnumerators[i];
					var currentReading = currentEnumerator.Current;
					if (currentReading.BeginStamp == currentTimeReading) {
						forCompilation.Add(currentReading);
						//count++;
					}
					if (currentReading.BeginStamp <= currentTimeReading) {
						if (!currentEnumerator.MoveNext()) {
							sortedEnumerators.RemoveAt(i);
							i--;
						}
					}
				}
				yield return Combine(forCompilation);
			}
		}
	}
}
