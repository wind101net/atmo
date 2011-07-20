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
	public interface IReadingsSummary {

		/// <summary>
		/// The first time stamp the summary contains.
		/// </summary>
		DateTime BeginStamp { get; }
		/// <summary>
		/// The last time stamp the summary contains.
		/// </summary>
		DateTime EndStamp { get; }
		/// <summary>
		/// The length of time the time span covers.
		/// </summary>
		TimeSpan TimeSpan { get; }
		/// <summary>
		/// The smallest values of all readings within the summary.
		/// </summary>
		IReadingValues Min { get; }
		/// <summary>
		/// The largest values of all readings within the summary.
		/// </summary>
		IReadingValues Max { get; }
		/// <summary>
		/// The mean values of all readings within the summary.
		/// </summary>
		IReadingValues Mean { get; }
		/// <summary>
		/// The median values of all readings within the summary.
		/// </summary>
		[Obsolete("Not currently used.")]
		IReadingValues Median { get; }

		/// <summary>
		/// The total number of records summarized
		/// </summary>
		int Count { get; }

		int GetTemperatureCount(double value);
		int GetPressureCount(double value);
		int GetHumidityCount(double value);
		int GetWindSpeedCount(double value);
		int GetWindDirectionCount(double value);

		Dictionary<double, int> GetTemperatureCounts();
		Dictionary<double, int> GetPressureCounts();
		Dictionary<double, int> GetHumidityCounts();
		Dictionary<double, int> GetWindSpeedCounts();
		Dictionary<double, int> GetWindDirectionCounts();
		
	}
}
