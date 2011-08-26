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

namespace Atmo.Stats {
	public class ReadingsSummary : IReadingsSummary, IReading {

		private static Dictionary<double, int> RoundAndCombine(Dictionary<double, int> values) {
			var result = new Dictionary<double, int>(values.Count);
			foreach (var kvp in values) {
				var roundKey = Math.Round(kvp.Key);
				int count;
				if (result.TryGetValue(roundKey, out count)) {
					result[roundKey] = count + kvp.Value;
				}else {
					result.Add(roundKey, kvp.Value);
				}
			}
			return result;
		}

		public DateTime BeginStamp;
		public DateTime EndStamp;
		public ReadingValues Min;
		public ReadingValues Max;
		public ReadingValues Mean;
		public ReadingValues SampleStandardDeviation;
		public int Count;
		public Dictionary<double, int> TemperatureCounts;
		public Dictionary<double, int> PressureCounts;
		public Dictionary<double, int> HumidityCounts;
		public Dictionary<double, int> WindSpeedCounts;
		public Dictionary<double, int> WindDirectionCounts;

		public ReadingsSummary(IReadingsSummary summary)
			: this(
			summary.BeginStamp, summary.EndStamp,
			new ReadingValues(summary.Min), new ReadingValues(summary.Max), new ReadingValues(summary.Mean), new ReadingValues(summary.SampleStandardDeviation), 
			summary.Count,
			summary.GetTemperatureCounts(),
			summary.GetPressureCounts(),
			summary.GetHumidityCounts(),
			summary.GetWindSpeedCounts(),
			summary.GetWindDirectionCounts())
		{ }

		public ReadingsSummary(
			DateTime beginStamp,
			DateTime endStamp,
			ReadingValues min,
			ReadingValues max,
			ReadingValues mean,
			ReadingValues sampleStandardDeviation,
			int count,
			Dictionary<double, int> tempCounts,
			Dictionary<double, int> presCounts,
			Dictionary<double, int> humCounts,
			Dictionary<double, int> speedCounts,
			Dictionary<double, int> dirCounts
		) {
			BeginStamp = beginStamp;
			EndStamp = endStamp;
			Min = min;
			Max = max;
			Mean = mean;
			SampleStandardDeviation = sampleStandardDeviation;
			Count = count;
			TemperatureCounts = RoundAndCombine(tempCounts);
			PressureCounts = RoundAndCombine(presCounts);
			HumidityCounts = RoundAndCombine(humCounts);
			WindSpeedCounts = RoundAndCombine(speedCounts);
			WindDirectionCounts = RoundAndCombine(dirCounts);
		}

		DateTime IReadingsSummary.BeginStamp {
			get { return BeginStamp; }
		}

		DateTime IReadingsSummary.EndStamp {
			get { return EndStamp; }
		}

		public TimeSpan TimeSpan {
			get { return EndStamp.AddTicks(1).Subtract(BeginStamp); }
		}

		IReadingValues IReadingsSummary.Min {
			get { return Min; }
		}

		IReadingValues IReadingsSummary.Max {
			get { return Max; }
		}

		IReadingValues IReadingsSummary.Mean {
			get { return Mean; }
		}

		IReadingValues IReadingsSummary.SampleStandardDeviation {
			get { return SampleStandardDeviation; }
		}

		int IReadingsSummary.Count {
			get { return Count; }
		}

		public DateTime TimeStamp {
			get { return BeginStamp; }
		}

		public double Temperature {
			get { return Mean.Temperature; }
		}

		public double Pressure {
			get { return Mean.Pressure; }
		}

		public double Humidity {
			get { return Mean.Humidity; }
		}

		public double WindSpeed {
			get { return Mean.WindSpeed; }
		}

		public double WindDirection {
			get { return Mean.WindDirection; }
		}

		public int GetTemperatureCount(double value) {
			int count;
			return (
				TemperatureCounts.TryGetValue(Math.Round(value), out count)
				? count
				: 0
			);
		}

		public int GetPressureCount(double value) {
			int count;
			return (
				PressureCounts.TryGetValue(Math.Round(value), out count)
				? count
				: 0
			);
		}

		public int GetHumidityCount(double value) {
			int count;
			return (
				HumidityCounts.TryGetValue(Math.Round(value), out count)
				? count
				: 0
			);
		}

		public int GetWindSpeedCount(double value) {
			int count;
			return (
				WindSpeedCounts.TryGetValue(Math.Round(value), out count)
				? count
				: 0
			);
		}

		public int GetWindDirectionCount(double value) {
			int count;
			return (
				WindDirectionCounts.TryGetValue(Math.Round(value), out count)
				? count
				: 0
			);
		}

		public Dictionary<double, int> GetTemperatureCounts() {
			return TemperatureCounts;
		}

		public Dictionary<double, int> GetPressureCounts() {
			return PressureCounts;
		}

		public Dictionary<double, int> GetHumidityCounts() {
			return HumidityCounts;
		}

		public Dictionary<double, int> GetWindSpeedCounts() {
			return WindSpeedCounts;
		}

		public Dictionary<double, int> GetWindDirectionCounts() {
			return WindDirectionCounts;
		}

		public bool IsValid {
			get {
				return Count > 0 && (
					IsTemperatureValid
					|| IsPressureValid
					|| IsHumidityValid
					|| IsWindSpeedValid
					|| IsWindDirectionValid
				);
			}
		}

		public bool IsTemperatureValid {
			get { return null != Mean && Mean.IsTemperatureValid; }
		}

		public bool IsPressureValid {
			get { return null != Mean && Mean.IsPressureValid; }
		}

		public bool IsHumidityValid {
			get { return null != Mean && Mean.IsHumidityValid; }
		}

		public bool IsWindSpeedValid {
			get { return null != Mean && Mean.IsWindSpeedValid; }
		}

		public bool IsWindDirectionValid {
			get { return null != Mean && Mean.IsWindDirectionValid; }
		}
	}
}
