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
using Atmo.Stats;

namespace Atmo.Data {

	/// <summary>
	/// Defines operations for a data store.
	/// </summary>
	public interface IDataStore {
		/// <summary>
		/// Gets the information for all sensors in the data store.
		/// </summary>
		/// <returns>A collection of sensor infos.</returns>
		IEnumerable<ISensorInfo> GetAllSensorInfos();

		/// <summary>
		/// Gets sensor readings from the data store.
		/// </summary>
		/// <param name="sensor">The sensor name to get readings for.</param>
		/// <param name="from">The date to start retrieving from.</param>
		/// <param name="span">The direction and magnitude of time to retrieve records for, usually a negative value.</param>
		/// <returns>A collection of sensor readings.</returns>
		IEnumerable<IReading> GetReadings(string sensor, DateTime from, TimeSpan span);

		/// <summary>
		/// Gets summarized sensor readings.
		/// </summary>
		/// <param name="sensor">The sensor name to get readings for.</param>
		/// <param name="from">The date to start retrieving from.</param>
		/// <param name="span">The direction and magnitude of time to retrieve records for, usually a negative value.</param>
		/// <param name="summaryUnit">The time span of each summary.</param>
		/// <param name="desiredSummaryUnitSpan">The desired summary unit span.</param>
		/// <returns>A collection of sensor reading summaries.</returns>
		/// <remarks>Summaries are zero aligned.</remarks>
		IEnumerable<IReadingsSummary> GetReadingSummaries(string sensor, DateTime from, TimeSpan span, TimeSpan desiredSummaryUnitSpan);

		/// <summary>
		/// Returns a list of time spans supported by the data store.
		/// </summary>
		IEnumerable<TimeSpan> SupportedSummaryUnitSpans { get; }

		/// <summary>
		/// Adds a sensor info record to the data store.
		/// </summary>
		/// <param name="sensor">The sensor to add.</param>
		/// <returns><c>true</c> on success, <c>false</c> otherwise.</returns>
		bool AddSensor(ISensorInfo sensor);

		/// <summary>
		/// Deletes a sensor if found, including all data that is associated with it.
		/// </summary>
		/// <param name="name">The name of the sensor to delete.</param>
		/// <returns>True if found and deleted, otherwise, False.</returns>
		bool DeleteSensor(string name);

		/// <summary>
		/// Adds readings to the data store.
		/// </summary>
		/// <param name="sensor">The sensor name to add readings for.</param>
		/// <param name="readings">The readings to add.</param>
		/// <returns><c>true</c> on success, <c>false</c> otherwise.</returns>
		bool Push(string sensor, IEnumerable<IReading> readings);

		DateTime GetMaxSyncStamp();

		bool AdjustTimeStamps(
			string sensorName,
			TimeRange currentRange,
			TimeRange correctedRange
		);

		string GetLatestSensorNameForHardwareId(string hardwareId);

		void SetLatestSensorNameForHardwareId(string dbSensorName, string hardwareId);

		bool PushSyncStamp(DateTime stamp);

		bool Push<T>(string sensor, IEnumerable<T> readings) where T : IReading;

		bool Push<T>(string sensor, IEnumerable<T> readings, bool replace) where T : IReading;
	}

}
