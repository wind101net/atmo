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

		private const double DegToRadFactor = Math.PI / 180.0;
		private const double RadToDegFactor = 180.0 / Math.PI;

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
			int tempCount = 0;
			int presCount = 0;
			int humCount = 0;
			int speedCount = 0;
			int dirCount = 0;
			double tempSum = 0;
			double presSum = 0;
			double humSum = 0;
			double speedSum = 0;
			double dirSinSum = 0;
			double dirCosSum = 0;

			var tempCounts = new Dictionary<double, int>();
			var presCounts = new Dictionary<double, int>();
			var humCounts = new Dictionary<double, int>();
			var speedCounts = new Dictionary<double, int>();
			var dirCounts = new Dictionary<double, int>();
			
			int count;

			ReadingValues minValues;
			ReadingValues maxValues;
			ReadingValues meanValues;
			ReadingValues sampleStandardDeviationValues;

			if (readings.Count > 0) {

				minValues = new ReadingValues(readings[0]);
				maxValues = new ReadingValues(readings[0]);

				double value;

				for (int i = 0; i < readings.Count; i++) {
					T reading = readings[i];
					
					if (reading.IsTemperatureValid) {
						tempCount++;
						value = reading.Temperature;
						tempSum += value;

						if (minValues.Temperature > value) {
							minValues.Temperature = value;
						}
						else if (maxValues.Temperature < value) {
							maxValues.Temperature = value;
						}

						value = Math.Round(value);
						tempCounts[value] = tempCounts.TryGetValue(value, out count) ? count + 1 : 1;
					}
					if (reading.IsPressureValid) {
						presCount++;
						value = reading.Pressure;
						presSum += value;

						if (minValues.Pressure > value) {
							minValues.Pressure = value;
						}
						else if (maxValues.Pressure < value) {
							maxValues.Pressure = value;
						}

						value = Math.Round(value);
						presCounts[value] = presCounts.TryGetValue(value, out count) ? count + 1 : 1;
					}
					if (reading.IsHumidityValid) {
						humCount++;
						value = reading.Humidity;
						humSum += value;

						if (minValues.Humidity > value) {
							minValues.Humidity = value;
						}
						else if (maxValues.Humidity < value) {
							maxValues.Humidity = value;
						}

						value = Math.Round(value);
						humCounts[value] = humCounts.TryGetValue(value, out count) ? count + 1 : 1;
					}
					if (reading.IsWindSpeedValid) {
						speedCount++;
						value = reading.WindSpeed;
						speedSum += value;

						if (minValues.WindSpeed > value) {
							minValues.WindSpeed = value;
						}
						else if (maxValues.WindSpeed < value) {
							maxValues.WindSpeed = value;
						}

						value = Math.Round(value*2.0)/2; // round to nearest .5
						speedCounts[value] = speedCounts.TryGetValue(value, out count) ? count + 1 : 1;
					}
					if (reading.IsWindDirectionValid) {
						dirCount++;
						value = reading.WindDirection;
						//dirSum += value;

						double dirRad = value*DegToRadFactor;
						dirSinSum += Math.Sin(dirRad);
						dirCosSum += Math.Cos(dirRad);

						if (minValues.WindDirection > value) {
							minValues.WindDirection = value;
						}
						else if (maxValues.WindDirection < value) {
							maxValues.WindDirection = value;
						}

						value = Math.Round(value);
						dirCounts[value] = dirCounts.TryGetValue(value, out count) ? count + 1 : 1;
					}
				}

				meanValues = new ReadingValues(
					tempCount == 0 ? Double.NaN : (tempSum/tempCount),
					presCount == 0 ? Double.NaN : (presSum/presCount),
					humCount == 0 ? Double.NaN : (humSum/humCount),
					(dirCount == 0 || Double.IsNaN(dirSinSum) || Double.IsNaN(dirCosSum))
						? Double.NaN
						: UnitUtility.WrapDegree(Math.Atan2(dirSinSum / dirCount, dirCosSum / dirCount) * RadToDegFactor),
					speedCount == 0 ? Double.NaN : (speedSum/speedCount)
				);

				sampleStandardDeviationValues = new ReadingValues(0,0,0,0,0);

				for (int i = 0; i < readings.Count; i++) {
					T reading = readings[i];
					
					if (reading.IsTemperatureValid) {
						value = reading.Temperature - meanValues.Temperature;
						sampleStandardDeviationValues.Temperature += value * value;
					}
					if (reading.IsPressureValid) {
						value = reading.Pressure - meanValues.Pressure;
						sampleStandardDeviationValues.Pressure += value * value;
					}
					if (reading.IsHumidityValid) {
						value = reading.Humidity - meanValues.Humidity;
						sampleStandardDeviationValues.Humidity += value * value;
					}
					if (reading.IsWindSpeedValid) {
						value = reading.WindSpeed - meanValues.WindSpeed;
						sampleStandardDeviationValues.WindSpeed += value * value;
					}
					if (reading.IsWindDirectionValid) {
						value = reading.WindDirection - meanValues.WindDirection;
						sampleStandardDeviationValues.WindDirection += value * value;
					}
				}

				sampleStandardDeviationValues.Temperature = Math.Sqrt(sampleStandardDeviationValues.Temperature/tempCount);
				sampleStandardDeviationValues.Pressure = Math.Sqrt(sampleStandardDeviationValues.Pressure / presCount);
				sampleStandardDeviationValues.Humidity = Math.Sqrt(sampleStandardDeviationValues.Humidity / humCount);
				sampleStandardDeviationValues.WindSpeed = Math.Sqrt(sampleStandardDeviationValues.WindSpeed / speedCount);
				sampleStandardDeviationValues.WindDirection = Math.Sqrt(sampleStandardDeviationValues.WindDirection / dirCount);

			}else {
				minValues = ReadingValues.CreateInvalid();
				maxValues = ReadingValues.CreateInvalid();
				meanValues = ReadingValues.CreateInvalid();
				sampleStandardDeviationValues = ReadingValues.CreateInvalid();
			}

			return new ReadingsSummary(
				dateRangeLow,
				dateRangeHigh.Subtract(new TimeSpan(1)),
				minValues, maxValues,
				meanValues, sampleStandardDeviationValues,
				readings.Count,
				tempCounts,
				presCounts,
				humCounts,
				speedCounts,
				dirCounts
			);
		}

		public static ReadingsSummary Combine<TSummary>(List<TSummary> readings) where TSummary : IReadingsSummary {
			int totalTempCount = 0;
			int totalPresCount = 0;
			int totalHumCount = 0;
			int totalSpeedCount = 0;
			int totalDirCount = 0;
			int totalRecordCount = 0;
			DateTime minStamp = DateTime.MaxValue;
			DateTime maxStamp = DateTime.MinValue;
			double dirSinSum = 0;
			double dirCosSum = 0;

			var summary = new ReadingsSummary(
				default(DateTime),
				default(DateTime),
				ReadingValues.CreateInvalid(),
				ReadingValues.CreateInvalid(),
				ReadingValues.CreateInvalid(),
				ReadingValues.CreateInvalid(),
				0,
				new Dictionary<double, int>(),
				new Dictionary<double, int>(),
				new Dictionary<double, int>(),
				new Dictionary<double, int>(),
				new Dictionary<double, int>()
			);

			if (0 == readings.Count) {
				return summary;
			}

			summary.Min = new ReadingValues(Double.MaxValue, Double.MaxValue, Double.MaxValue, Double.MaxValue, Double.MaxValue);
			summary.Max = new ReadingValues(Double.MinValue, Double.MinValue, Double.MinValue, Double.MinValue, Double.MinValue);
			summary.Mean = new ReadingValues(0, 0, 0, 0, 0);
			summary.SampleStandardDeviation = new ReadingValues(0, 0, 0, 0, 0);

			foreach (var reading in readings) {

				if(reading.BeginStamp < minStamp) {
					minStamp = reading.BeginStamp;
				}
				if(reading.EndStamp > maxStamp) {
					maxStamp = reading.EndStamp;
				}

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

				int localTempCount = AppendCounts(summary.TemperatureCounts, reading.GetTemperatureCounts());
				int localPresCount = AppendCounts(summary.PressureCounts, reading.GetPressureCounts());
				int localHumCount = AppendCounts(summary.HumidityCounts, reading.GetHumidityCounts());
				int localSpeedCount = AppendCounts(summary.WindSpeedCounts, reading.GetWindSpeedCounts());
				int localDirCount = AppendCounts(summary.WindDirectionCounts, reading.GetWindDirectionCounts());

				totalTempCount += localTempCount;
				totalPresCount += localPresCount;
				totalHumCount += localHumCount;
				totalSpeedCount += localSpeedCount;
				totalDirCount += localDirCount;
				totalRecordCount += reading.Count;

				summary.Mean.Temperature += reading.Mean.Temperature * localTempCount;
				summary.Mean.Pressure += reading.Mean.Pressure * localPresCount;
				summary.Mean.Humidity += reading.Mean.Humidity * localHumCount;
				summary.Mean.WindSpeed += reading.Mean.WindSpeed * localSpeedCount;

				double dirRad = reading.Mean.WindDirection * DegToRadFactor;
				dirSinSum += Math.Sin(dirRad) * localDirCount;
				dirCosSum += Math.Cos(dirRad) * localDirCount;

			}

			if (0 != totalTempCount && 1 != totalTempCount) {
				summary.Mean.Temperature /= totalTempCount;
			}
			if (0 != totalPresCount && 1 != totalPresCount) {
				summary.Mean.Pressure /= totalPresCount;
			}
			if (0 != totalHumCount && 1 != totalHumCount) {
				summary.Mean.Humidity /= totalHumCount;
			}
			if (0 != totalSpeedCount && 1 != totalSpeedCount) {
				summary.Mean.WindSpeed /= totalSpeedCount;
			}
			summary.Mean.WindDirection = (totalDirCount == 0 || Double.IsNaN(dirSinSum) || Double.IsNaN(dirCosSum))
			    ? Double.NaN
			    : UnitUtility.WrapDegree(Math.Atan2(dirSinSum/totalDirCount, dirCosSum/totalDirCount)*RadToDegFactor);


			double dev;
			foreach (var reading in readings) {

				int localTempCount = reading.GetTemperatureCounts().Sum(c => c.Value);
				int localPresCount = reading.GetPressureCounts().Sum(c => c.Value);
				int localHumCount = reading.GetHumidityCounts().Sum(c => c.Value);
				int localSpeedCount = reading.GetWindSpeedCounts().Sum(c => c.Value);
				int localDirCount = reading.GetWindDirectionCounts().Sum(c => c.Value);

				dev = (reading.Mean.Temperature - summary.Mean.Temperature);
				summary.SampleStandardDeviation.Temperature += (dev*dev)*localTempCount;

				dev = (reading.Mean.Pressure - summary.Mean.Pressure);
				summary.SampleStandardDeviation.Pressure += (dev*dev)*localPresCount;

				dev = (reading.Mean.Humidity - summary.Mean.Humidity);
				summary.SampleStandardDeviation.Humidity += (dev*dev)*localHumCount;

				dev = (reading.Mean.WindSpeed - summary.Mean.WindSpeed);
				summary.SampleStandardDeviation.WindSpeed += (dev*dev)*localSpeedCount;

				dev = (reading.Mean.WindDirection - summary.Mean.WindDirection);
				summary.SampleStandardDeviation.WindDirection += (dev*dev)*localDirCount;
			}

			if (0 != totalTempCount && 1 != totalTempCount) {
				summary.SampleStandardDeviation.Temperature = Math.Sqrt(summary.SampleStandardDeviation.Temperature / totalTempCount);
			}
			if (0 != totalPresCount && 1 != totalPresCount) {
				summary.SampleStandardDeviation.Pressure = Math.Sqrt(summary.SampleStandardDeviation.Pressure / totalPresCount);
			}
			if (0 != totalHumCount && 1 != totalHumCount) {
				summary.SampleStandardDeviation.Humidity = Math.Sqrt(summary.SampleStandardDeviation.Humidity / totalHumCount);
			}
			if (0 != totalSpeedCount && 1 != totalSpeedCount) {
				summary.SampleStandardDeviation.WindSpeed = Math.Sqrt(summary.SampleStandardDeviation.WindSpeed / totalSpeedCount);
			}
			if (0 != totalDirCount && 1 != totalDirCount) {
				summary.SampleStandardDeviation.WindDirection = Math.Sqrt(summary.SampleStandardDeviation.WindDirection / totalDirCount);
			}

			summary.Count = totalRecordCount;
			summary.BeginStamp = minStamp;
			summary.EndStamp = maxStamp;
			return summary;
		}

		[Obsolete("Should replace this with a specialized version?")]
		public static ReadingsSummary Combine(List<ReadingsSummary> readings) {
			return Combine(readings.Cast<IReadingsSummary>().ToList());
		}

		/// <returns>The total number of counts added.</returns>
		public static int AppendCounts(Dictionary<double, int> destination, Dictionary<double, int> source) {
			int totalCount = 0;
			foreach (var kvp in source) {
				int count;
				if (destination.TryGetValue(kvp.Key, out count)) {
					destination[kvp.Key] = count + kvp.Value;
				}
				else {
					destination[kvp.Key] = kvp.Value;
				}
				totalCount += kvp.Value;
			}
			return totalCount;
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
