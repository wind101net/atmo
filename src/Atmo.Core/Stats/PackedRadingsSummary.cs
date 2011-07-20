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
	public abstract class PackedReadingsSummary : IReadingsSummary {

        internal static readonly TimeSpan OneTick = new TimeSpan(1);

		public DateTime BeginStamp;
		public int Count;
		public PackedReadingValues Min;
		public PackedReadingValues Max;
		public PackedReadingValues Mean;
		public PackedReadingValues Median;
		public Dictionary<ushort, int> TemperatureCounts;
		public Dictionary<ushort, int> PressureCounts;
		public Dictionary<ushort, int> HumidityCounts;
		public Dictionary<ushort, int> WindSpeedCounts;
		public Dictionary<ushort, int> WindDirectionCounts;

		protected PackedReadingsSummary(
			DateTime beginStamp,
			PackedReadingValues min,
			PackedReadingValues max,
			PackedReadingValues mean,
			PackedReadingValues median,
			int count
		) {
			BeginStamp = beginStamp;
			Min = min;
			Max = max;
			Mean = mean;
			Median = median;
			Count = count;
			TemperatureCounts = new Dictionary<ushort, int>();
			PressureCounts = new Dictionary<ushort, int>();
			HumidityCounts = new Dictionary<ushort, int>();
			WindSpeedCounts = new Dictionary<ushort, int>();
			WindDirectionCounts = new Dictionary<ushort, int>();
		}

		DateTime IReadingsSummary.BeginStamp {
			get { return BeginStamp; }
		}

	    public abstract DateTime EndStamp { get; }

		public abstract TimeSpan TimeSpan { get; }

		IReadingValues IReadingsSummary.Min {
			get { return Min; }
		}

		IReadingValues IReadingsSummary.Max {
			get { return Max; }
		}

		IReadingValues IReadingsSummary.Mean {
			get { return Mean; }
		}

		IReadingValues IReadingsSummary.Median {
			get { return Median; }
		}

		int IReadingsSummary.Count {
			get { return Count; }
		}

		public int GetTemperatureCount(double value) {
			int count;
			return (
				TemperatureCounts.TryGetValue((ushort)(Math.Round(Math.Max(0, Math.Min(1023, (value + 40.0) * 10.0)))), out count)
				? count
				: 0
			);
		}

		public int GetPressureCount(double value) {
			int count;
			return (
				PressureCounts.TryGetValue((ushort)(Math.Round(Math.Max(0, Math.Min(65535, value / 2.0)))), out count)
				? count
				: 0
			);
		}

		public int GetHumidityCount(double value) {
			int count;
			return (
				HumidityCounts.TryGetValue((ushort)(Math.Round(Math.Max(0, Math.Min(1023, value * 1000.0)))), out count)
				? count
				: 0
			);
		}

		public int GetWindSpeedCount(double value) {
			int count;
			return (
				WindSpeedCounts.TryGetValue((ushort)(Math.Round(Math.Max(0, Math.Min(8191, value * 100.0)))), out count)
				? count
				: 0
			);
		}

		public int GetWindDirectionCount(double value) {
			int count;
			return (
				WindDirectionCounts.TryGetValue((ushort)(Math.Round(Math.Max(0, Math.Min(511, value)))), out count)
				? count
				: 0
			);
		}

		public Dictionary<double, int> GetTemperatureCounts() {
			var result = new Dictionary<double, int>(TemperatureCounts.Count);
			foreach (var kvp in TemperatureCounts) {
				result.Add((kvp.Key / 10.0) - 40.0, kvp.Value);
			}
			return result;
		}

		public Dictionary<double, int> GetPressureCounts() {
			var result = new Dictionary<double, int>(PressureCounts.Count);
			foreach (var kvp in PressureCounts) {
				result.Add(kvp.Key * 2.0, kvp.Value);
			}
			return result;
		}

		public Dictionary<double, int> GetHumidityCounts() {
			var result = new Dictionary<double, int>(HumidityCounts.Count);
			foreach (var kvp in HumidityCounts) {
				result.Add(kvp.Key / 1000.0, kvp.Value);
			}
			return result;
		}

		public Dictionary<double, int> GetWindSpeedCounts() {
			var result = new Dictionary<double, int>(WindSpeedCounts.Count);
			foreach (var kvp in WindSpeedCounts) {
				result.Add(kvp.Key / 100.0, kvp.Value);
			}
			return result;
		}

		public Dictionary<double, int> GetWindDirectionCounts() {
			var result = new Dictionary<double, int>(WindDirectionCounts.Count);
			foreach (var kvp in WindDirectionCounts) {
				result.Add(kvp.Key, kvp.Value);
			}
			return result;
		}

	}
}
