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
	public class SensorReadingsSummary : IReadingsSummary, IReading {

		#region Static Methods

		private static Dictionary<double, int> RoundAndCombine(Dictionary<double, int> values) {
			Dictionary<double, int> result = new Dictionary<double, int>(values.Count);
			if (null != values) {
				int count;
				double value;
				foreach (KeyValuePair<double, int> kvp in values) {
					value = Math.Round(kvp.Key);
					result[value] = (result.TryGetValue(value, out count)) ? (count + kvp.Value) : kvp.Value;
				}
			}
			return result;
		}

		#endregion

		#region Fields

		public DateTime BeginStamp;
		public DateTime EndStamp;
		public ReadingValues Min;
		public ReadingValues Max;
		public ReadingValues Mean;
		public ReadingValues Median;
		public int Count;
		public Dictionary<double, int> TemperatureCounts;
		public Dictionary<double, int> PressureCounts;
		public Dictionary<double, int> HumidityCounts;
		public Dictionary<double, int> WindSpeedCounts;
		public Dictionary<double, int> WindDirectionCounts;

		#endregion

		#region Constructors

		public SensorReadingsSummary(
			DateTime beginStamp,
			DateTime endStamp,
			ReadingValues min,
			ReadingValues max,
			ReadingValues mean,
			ReadingValues median,
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
			Median = median;
			Count = count;
			TemperatureCounts = RoundAndCombine(tempCounts);
			PressureCounts = RoundAndCombine(presCounts);
			HumidityCounts = RoundAndCombine(humCounts);
			WindSpeedCounts = RoundAndCombine(speedCounts);
			WindDirectionCounts = RoundAndCombine(dirCounts);
		}

		#endregion

		#region ISensorReadingsSummary Members

		DateTime IReadingsSummary.BeginStamp {
			get { return this.BeginStamp; }
		}

		DateTime IReadingsSummary.EndStamp {
			get { return this.EndStamp; }
		}

		public TimeSpan TimeSpan {
			get { return this.EndStamp.AddTicks(1).Subtract(this.BeginStamp); }
		}

		IReadingValues IReadingsSummary.Min {
			get { return this.Min; }
		}

		IReadingValues IReadingsSummary.Max {
			get { return this.Max; }
		}

		IReadingValues IReadingsSummary.Mean {
			get { return this.Mean; }
		}

		IReadingValues IReadingsSummary.Median {
			get { return this.Median; }
		}

		int IReadingsSummary.Count {
			get { return this.Count; }
		}

		#endregion

		#region ISensorReading Members

		public DateTime TimeStamp {
			get { return this.BeginStamp; }
		}

		#endregion

		#region ISensorReadingValues Members

		public double Temperature {
			get { return this.Mean.Temperature; }
		}

		public double Pressure {
			get { return this.Mean.Pressure; }
		}

		public double Humidity {
			get { return this.Mean.Humidity; }
		}

		public double WindSpeed {
			get { return this.Mean.WindSpeed; }
		}

		public double WindDirection {
			get { return this.Mean.WindDirection; }
		}

		#endregion

		#region ISensorReadingsSummary Members


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

		#endregion

		#region ISensorReadingsSummary Members


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

		#endregion

		#region ISensorReadingValues Members


		public bool IsValid {
			get { return null != Mean && Count > 0; }
		}

		#endregion

		#region ISensorReadingValues Members


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

		#endregion
	}
}
