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
using System.Data;
using System.Data.Common;
using System.Linq;
using Atmo.Stats;
using Atmo.Units;

namespace Atmo.Data {

	public class DbDataStore : IDataStore, IDisposable {

		private class DbSensorInfo : ISensorInfo {

			private readonly string _lastHardwareId;

			public DbSensorInfo(string name, string lastHardwareId) {
				Name = name;
				_lastHardwareId = lastHardwareId;
			}

			public string Name { get; private set; }

			public SpeedUnit SpeedUnit {
				get { return SpeedUnit.MetersPerSec; }
			}

			public TemperatureUnit TemperatureUnit {
				get { return TemperatureUnit.Celsius; }
			}

			public PressureUnit PressureUnit {
				get { return PressureUnit.Pascals; }
			}

			public override string ToString() {
				return Name + ' ' + _lastHardwareId;
			}

		}

		public struct PosixTimeRange {

			private readonly int _low;
			private readonly int _high;

			public PosixTimeRange(int a, int b) {
				if (a < b) {
					_low = a;
					_high = b;
				}
				else {
					_low = b;
					_high = a;
				}
			}

			public PosixTimeRange(DateTime a, DateTime b)
				: this(
				UnitUtility.ConvertToPosixTime(a),
				UnitUtility.ConvertToPosixTime(b)
			) { }

			public PosixTimeRange(TimeRange range)
				: this(range.Low, range.High) { }

			public int Low { get { return _low; } }

			public int High { get { return _high; } }

			public int Span { get { return _high - _low; } }

		}

		private IDbConnection _connection;

		public DbDataStore(IDbConnection connection) {
			if (null == connection) {
				throw new ArgumentNullException("connection");
			}
			_connection = connection;
		}

		#region Public Methods

		public DateTime GetMaxSyncStamp() {
			if (!ForceConnectionOpen()) {
				throw new Exception("Could not open database.");
			}
			DateTime minStamp = default(DateTime);
			using (IDbCommand command = _connection.CreateCommand()) {
				command.CommandText = "SELECT stamp FROM syncstamps ORDER BY stamp DESC LIMIT 1";
				command.CommandType = CommandType.Text;
				using (IDataReader reader = command.ExecuteReader()) {
					if (reader.Read()) {
						minStamp = UnitUtility.ConvertFromPosixTime(reader.GetInt32(0));
					}
				}
			}
			if (default(DateTime).Equals(minStamp)) {
				using (IDbCommand command = _connection.CreateCommand()) {
					command.CommandText = "SELECT stamp FROM dayrecord ORDER BY stamp DESC LIMIT 1";
					command.CommandType = CommandType.Text;
					using (IDataReader reader = command.ExecuteReader()) {
						if (reader.Read()) {
							minStamp = UnitUtility.ConvertFromPosixTime(reader.GetInt32(0));
						}
					}
				}
			}
			return minStamp;
		}

		public bool PushSyncStamp(DateTime stamp) {
			return PushSyncStamp(UnitUtility.ConvertToPosixTime(stamp));
		}

		private bool PushSyncStamp(int stamp) {
			using (IDbCommand command = _connection.CreateCommand()) {
				command.CommandText = "INSERT INTO syncstamps (stamp) VALUES (@stamp)";
				command.CommandType = CommandType.Text;
				DbParameter stampParam = command.CreateParameter() as DbParameter;
				stampParam.DbType = DbType.Int32;
				stampParam.ParameterName = "stamp";
				stampParam.Value = stamp;
				command.Parameters.Add(stampParam);
				return 1 == command.ExecuteNonQuery();
			}
		}

		public bool AdjustTimeStamps(
			string sensorName,
			TimeRange currentRange,
			TimeRange correctedRange
		) {
			return AdjustTimeStamps(
				sensorName,
				new PosixTimeRange(currentRange),
				new PosixTimeRange(correctedRange)
			);
		}

		public bool AdjustTimeStamps(
			string sensorName,
			PosixTimeRange currentRange,
			PosixTimeRange correctedRange
		) {
			int delta = correctedRange.Span - currentRange.Span;
			int offset = correctedRange.Low - currentRange.Low;
			if (delta == 0 && offset == 0) {
				return false;
			}
			if (!ForceConnectionOpen()) {
				throw new Exception("Could not open database.");
			}
			int sensorId = -1;
			using (IDbCommand command = _connection.CreateCommand()) {
				command.CommandText = "SELECT sensorID FROM Sensor WHERE nameKey = @nameKey";
				command.CommandType = CommandType.Text;
				DbParameter nameIdParam = command.CreateParameter() as DbParameter;
				nameIdParam.DbType = DbType.String;
				nameIdParam.ParameterName = "nameKey";
				nameIdParam.Value = sensorName;
				command.Parameters.Add(nameIdParam);
				using (IDataReader reader = command.ExecuteReader()) {
					if (reader.Read()) {
						sensorId = reader.GetInt32(0);
					}
				}
			}
			if (sensorId < 0) {
				throw new KeyNotFoundException("Sensor not found in datastore.");
			}
			if (correctedRange.Low < currentRange.Low || correctedRange.High > currentRange.High) {
				using (IDbCommand command = _connection.CreateCommand()) {
					command.CommandText = "SELECT COUNT(*) FROM Record WHERE sensorId = @sensorId";
					command.CommandType = CommandType.Text;
					DbParameter nameIdParam = command.CreateParameter() as DbParameter;
					nameIdParam.DbType = DbType.Int32;
					nameIdParam.ParameterName = "sensorId";
					nameIdParam.Value = sensorId;
					command.Parameters.Add(nameIdParam);

					List<string> orJoin = new List<string>();
					if (correctedRange.Low < currentRange.Low) {
						orJoin.Add("(stamp >= @corLow AND stamp < @curLow)");
					}
					DbParameter corLowParam = command.CreateParameter() as DbParameter;
					corLowParam.DbType = DbType.Int32;
					corLowParam.ParameterName = "corLow";
					corLowParam.Value = correctedRange.Low;
					DbParameter curLowParam = command.CreateParameter() as DbParameter;
					curLowParam.DbType = DbType.Int32;
					curLowParam.ParameterName = "curLow";
					curLowParam.Value = currentRange.Low;
					command.Parameters.Add(corLowParam);
					command.Parameters.Add(curLowParam);
					if (correctedRange.High > currentRange.High) {
						orJoin.Add("(stamp <= @corHigh AND stamp > @curHigh)");
					}
					DbParameter corHighParam = command.CreateParameter() as DbParameter;
					corHighParam.DbType = DbType.Int32;
					corHighParam.ParameterName = "corHigh";
					corHighParam.Value = correctedRange.High;
					DbParameter curHighParam = command.CreateParameter() as DbParameter;
					curHighParam.DbType = DbType.Int32;
					curHighParam.ParameterName = "curHigh";
					curHighParam.Value = currentRange.High;
					command.Parameters.Add(corHighParam);
					command.Parameters.Add(curHighParam);
					command.CommandText += String.Concat(" AND (", String.Join(" OR ", orJoin.ToArray()), ") AND NOT (stamp >= @curLow AND stamp <= @curHigh)");


					int count = 0;
					using (IDataReader reader = command.ExecuteReader()) {
						if (reader.Read()) {
							count = reader.GetInt32(0);
						}
					}
					if (count > 0) {
						throw new InvalidOperationException("Cannot overwrite existing datastore entries.");
					}
				}
			}

			// at this point there is space to shift or grow into
			if (delta < 0) {
				// shrinking
				bool ok = ShrinkRecords(sensorId, currentRange, delta);

				// then shift all if needed
				if (offset != 0) {
					ok = (0 != OffsetRecords(sensorId, new PosixTimeRange(currentRange.Low, currentRange.Low + correctedRange.Span), offset))
						| ok;
				}
				return ok;
			}
			else {
				bool ok = false;
				// need to shift the start into place
				if (offset != 0) {
					ok = 0 != OffsetRecords(sensorId, currentRange, offset);
				}
				// now we need to stretch the records
				if (delta > 0) {
					//ok |= InterpolateInsertRecords(sensorId, new PosixTimeRange(currentRange.High - offset + 1, correctedRange.High), correctedRange);
					ok |= ExpandRecords(sensorId, new PosixTimeRange(currentRange.Low + offset, currentRange.High + offset), delta);
				}
				return ok;
			}
			//return false;
		}

		private int OffsetRecords(int sensorId, PosixTimeRange range, int offset) {
			int c = 0;
			if (0 == offset) {
				return 0;
			}
			if (!ForceConnectionOpen()) {
				throw new Exception("Could not open database.");
			}
			using (IDbCommand command = _connection.CreateCommand()) {
				command.Transaction = _connection.BeginTransaction();
				command.CommandText = "UPDATE Record SET stamp = stamp + @offset WHERE sensorId = @sensorId AND stamp = @stamp";
				command.CommandType = CommandType.Text;
				DbParameter sensorIdParam = command.CreateParameter() as DbParameter;
				sensorIdParam.DbType = DbType.Int32;
				sensorIdParam.ParameterName = "sensorId";
				sensorIdParam.Value = sensorId;
				command.Parameters.Add(sensorIdParam);
				DbParameter stampParam = command.CreateParameter() as DbParameter;
				stampParam.DbType = DbType.Int32;
				stampParam.ParameterName = "stamp";
				stampParam.Value = 0;
				command.Parameters.Add(stampParam);
				DbParameter offsetParam = command.CreateParameter() as DbParameter;
				offsetParam.DbType = DbType.Int32;
				offsetParam.ParameterName = "offset";
				offsetParam.Value = offset;
				command.Parameters.Add(offsetParam);
				if (offset < 0) {
					for (int i = range.Low; i <= range.High; i++) {
						stampParam.Value = i;
						c += command.ExecuteNonQuery();
					}
				}
				else {
					for (int i = range.High; i >= range.Low; i--) {
						stampParam.Value = i;
						c += command.ExecuteNonQuery();
					}
				}
				command.Transaction.Commit();
			}
			return c;
		}

		private bool ExpandRecords(int sensorId, PosixTimeRange range, int delta) {

			if (delta < 0) {
				return false;
			}
			if (delta == 0) {
				return true;
			}

			int totalInserts = delta;
			int rangeSpan = range.Span;
			int resultingSpan = rangeSpan + delta;
			if (totalInserts >= resultingSpan) {
				return false;
			}

			int splitParts = Math.Min(rangeSpan, delta + 1);


			using (IDbCommand command = _connection.CreateCommand()) {
				command.Transaction = _connection.BeginTransaction();
				command.CommandText = "UPDATE OR REPLACE Record SET stamp = (stamp + @offset) WHERE sensorId = @sensorId AND stamp = @stamp";
				command.CommandType = CommandType.Text;
				DbParameter sensorIdParam = command.CreateParameter() as DbParameter;
				sensorIdParam.DbType = DbType.Int32;
				sensorIdParam.ParameterName = "sensorId";
				sensorIdParam.Value = sensorId;
				command.Parameters.Add(sensorIdParam);
				/*DbParameter minStampParam = command.CreateParameter() as DbParameter;
				minStampParam.DbType = DbType.Int32;
				minStampParam.ParameterName = "minStamp";
				minStampParam.Value = 0;
				command.Parameters.Add(minStampParam);
				DbParameter maxStampParam = command.CreateParameter() as DbParameter;
				maxStampParam.DbType = DbType.Int32;
				maxStampParam.ParameterName = "maxStamp";
				maxStampParam.Value = 0;
				command.Parameters.Add(maxStampParam);*/
				DbParameter offsetParam = command.CreateParameter() as DbParameter;
				offsetParam.DbType = DbType.Int32;
				offsetParam.ParameterName = "offset";
				offsetParam.Value = 0;
				command.Parameters.Add(offsetParam);
				DbParameter stampParam = command.CreateParameter() as DbParameter;
				stampParam.DbType = DbType.Int32;
				stampParam.ParameterName = "stamp";
				stampParam.Value = 0;
				command.Parameters.Add(stampParam);
				//for (int partIndex = 0; partIndex < splitParts; partIndex++)
				for (int partIndex = splitParts - 1; partIndex >= 0; partIndex--) {

					int curPartIndex = ((partIndex * rangeSpan) / splitParts);
					int nextPartIndex = Math.Min(((partIndex + 1) * rangeSpan) / splitParts, rangeSpan);
					int destIndex = (curPartIndex * (resultingSpan - 1)) / (rangeSpan - 1);
					int shift = (destIndex + range.Low) - (curPartIndex + range.Low);

					// move all between ["curPartIndex","nextPartIndex") up by "shift"
					//minStampParam.Value = curPartIndex;
					//maxStampParam.Value = nextPartIndex;
					offsetParam.Value = shift;
					for (int i = nextPartIndex + range.Low - 1; i >= curPartIndex + range.Low; i--) {
						stampParam.Value = i;
						command.ExecuteNonQuery();
					}

				}
				command.Transaction.Commit();
			}
			return true;

		}

		private bool ShrinkRecords(int sensorId, PosixTimeRange range, int delta) {
			if (delta > 0) {
				return false;
			}
			if (0 == delta) {
				return true;
			}

			int totalDeletions = -delta;
			int rangeSpan = range.Span;
			if (rangeSpan < totalDeletions) {
				return false;
			}

			using (IDbCommand command = _connection.CreateCommand()) {
				command.Transaction = _connection.BeginTransaction();
				command.CommandText = "UPDATE OR REPLACE Record SET stamp = stamp - @offset WHERE sensorId = @sensorId AND stamp = @stamp";
				command.CommandType = CommandType.Text;
				DbParameter sensorIdParam = command.CreateParameter() as DbParameter;
				sensorIdParam.DbType = DbType.Int32;
				sensorIdParam.ParameterName = "sensorId";
				sensorIdParam.Value = sensorId;
				command.Parameters.Add(sensorIdParam);
				DbParameter stampParam = command.CreateParameter() as DbParameter;
				stampParam.DbType = DbType.Int32;
				stampParam.ParameterName = "stamp";
				stampParam.Value = 0;
				command.Parameters.Add(stampParam);
				DbParameter offsetParam = command.CreateParameter() as DbParameter;
				offsetParam.DbType = DbType.Int32;
				offsetParam.ParameterName = "offset";
				offsetParam.Value = 0;
				command.Parameters.Add(offsetParam);
				for (int delNumber = 0; delNumber < totalDeletions; delNumber++) {
					// scoot it down
					int delIndex = (rangeSpan * (delNumber + 1)) / ((rangeSpan - totalDeletions) + 1);
					int nextIndex = (rangeSpan * (delNumber + 2)) / ((rangeSpan - totalDeletions) + 1);
					nextIndex = System.Math.Min(nextIndex, range.High);
					//minStampParam.Value = delIndex;
					//maxStampParam.Value = nextIndex;
					offsetParam.Value = delNumber + 1;
					for (int i = delIndex; i < nextIndex; i++) {
						stampParam.Value = i;
						command.ExecuteNonQuery();
					}
				}


				command.CommandText = "DELETE FROM Record WHERE sensorId = @sensorId AND stamp > @minStamp AND stamp <= @maxStamp";
				command.CommandType = CommandType.Text;
				command.Parameters.Clear();
				/*DbParameter sensorIdParam = command.CreateParameter() as DbParameter;
				sensorIdParam.DbType = DbType.Int32;
				sensorIdParam.ParameterName = "sensorId";
				sensorIdParam.Value = sensorId;*/
				command.Parameters.Add(sensorIdParam);
				DbParameter minStampParam = command.CreateParameter() as DbParameter;
				minStampParam.DbType = DbType.Int32;
				minStampParam.ParameterName = "minStamp";
				minStampParam.Value = range.High - totalDeletions;
				command.Parameters.Add(minStampParam);
				DbParameter maxStampParam = command.CreateParameter() as DbParameter;
				maxStampParam.DbType = DbType.Int32;
				maxStampParam.ParameterName = "maxStamp";
				maxStampParam.Value = range.High;
				command.Parameters.Add(maxStampParam);
				command.ExecuteNonQuery();
				command.Transaction.Commit();
			}

			return true;
		}

		[Obsolete]
		private bool DeleteRecords(int sensorId, PosixTimeRange deletionRange) {
			if (!ForceConnectionOpen()) {
				throw new Exception("Could not open database.");
			}
			return false;
		}

		[Obsolete]
		private bool InterpolateInsertRecords(int sensorId, PosixTimeRange insertionRange, PosixTimeRange existingDataRange) {
			if (!ForceConnectionOpen()) {
				throw new Exception("Could not open database.");
			}
			byte[] values = null;
			using (IDbCommand command = _connection.CreateCommand()) {
				command.CommandText = "SElECT Record.[values] FROM Record WHERE sensorId = @sensorId AND stamp >= @minStamp AND stamp <= @maxStamp ORDER BY stamp DESC LIMIT 1";
				command.CommandType = CommandType.Text;
				DbParameter sensorIdParam = command.CreateParameter() as DbParameter;
				sensorIdParam.DbType = DbType.Int32;
				sensorIdParam.ParameterName = "sensorId";
				sensorIdParam.Value = sensorId;
				command.Parameters.Add(sensorIdParam);
				DbParameter minStampParam = command.CreateParameter() as DbParameter;
				minStampParam.DbType = DbType.Int32;
				minStampParam.ParameterName = "minStamp";
				minStampParam.Value = existingDataRange.Low;
				command.Parameters.Add(minStampParam);
				DbParameter maxStampParam = command.CreateParameter() as DbParameter;
				maxStampParam.DbType = DbType.Int32;
				maxStampParam.ParameterName = "maxStamp";
				maxStampParam.Value = existingDataRange.High;
				command.Parameters.Add(maxStampParam);
				using (IDataReader reader = command.ExecuteReader()) {
					if (reader.Read()) {
						values = reader.GetValue(0) as byte[];
					}
				}
			}
			if (null == values) {
				return false;
			}

			using (IDbCommand command = _connection.CreateCommand()) {
				command.CommandText = "INSERT INTO Record (sensorId,stamp,[values]) VALUES (@sensorId,@stamp,@values)";
				command.CommandType = CommandType.Text;
				DbParameter sensorIdParam = command.CreateParameter() as DbParameter;
				sensorIdParam.DbType = DbType.Int32;
				sensorIdParam.ParameterName = "sensorId";
				sensorIdParam.Value = sensorId;
				command.Parameters.Add(sensorIdParam);
				DbParameter stampParam = command.CreateParameter() as DbParameter;
				stampParam.DbType = DbType.Int32;
				stampParam.ParameterName = "stamp";
				stampParam.Value = insertionRange.Low;
				command.Parameters.Add(stampParam);
				DbParameter valuesParam = command.CreateParameter() as DbParameter;
				valuesParam.DbType = DbType.Binary;
				valuesParam.ParameterName = "values";
				valuesParam.Value = values;
				command.Parameters.Add(valuesParam);
				for (int stamp = insertionRange.Low; stamp <= insertionRange.High; stamp++) {
					stampParam.Value = stamp;
					command.ExecuteNonQuery();
				}
			}

			return true;
		}

		public string GetLatestSensorNameForHardwareId(string hardwareId) {
			if (!ForceConnectionOpen()) {
				throw new Exception("Could not open database.");
			}
			using (IDbCommand command = _connection.CreateCommand()) {
				command.CommandText = "SELECT nameKey FROM Sensor LEFT JOIN dayrecord on dayrecord.sensorId = Sensor.sensorId WHERE Sensor.lastLoadHardwareId = @hardwareId GROUP BY dayrecord.sensorId ORDER BY MAX(dayrecord.stamp) DESC";
				command.CommandType = CommandType.Text;
				DbParameter hardwareIdParam = command.CreateParameter() as DbParameter;
				hardwareIdParam.DbType = DbType.String;
				hardwareIdParam.ParameterName = "hardwareId";
				hardwareIdParam.Value = hardwareId;
				command.Parameters.Add(hardwareIdParam);
				using (IDataReader reader = command.ExecuteReader()) {
					if (reader.Read()) {
						return reader.GetValue(0) as string;
					}
				}
			}
			return null;
		}

		public void SetLatestSensorNameForHardwareId(string dbSensorName, string hardwareId) {
			if (!ForceConnectionOpen()) {
				throw new Exception("Could not open database.");
			}
			using (IDbCommand command = _connection.CreateCommand()) {
				command.CommandText = "UPDATE Sensor SET lastLoadHardwareId = @hardwareId WHERE nameKey = @nameKey";
				command.CommandType = CommandType.Text;
				DbParameter hardwareIdParam = command.CreateParameter() as DbParameter;
				hardwareIdParam.DbType = DbType.String;
				hardwareIdParam.ParameterName = "hardwareId";
				hardwareIdParam.Value = hardwareId;
				command.Parameters.Add(hardwareIdParam);
				DbParameter dbIdParam = command.CreateParameter() as DbParameter;
				dbIdParam.DbType = DbType.String;
				dbIdParam.ParameterName = "nameKey";
				dbIdParam.Value = dbSensorName;
				command.Parameters.Add(dbIdParam);
				command.ExecuteNonQuery();
			}

		}

		#endregion

		#region Private Methods

		private bool ForceConnectionOpen() {
			if (_connection.State == ConnectionState.Closed) {
				_connection.Open();
				return _connection.State == ConnectionState.Open || _connection.State == ConnectionState.Connecting;
			}
			return true;
		}

		#endregion

		#region IDataStore Members

		public IEnumerable<ISensorInfo> GetAllSensorInfos() {
			if (!ForceConnectionOpen()) {
				throw new Exception("Could not open database.");
			}
			List<ISensorInfo> sensorInfos = new List<ISensorInfo>();
			using (IDbCommand command = _connection.CreateCommand()) {
				command.CommandText = "SELECT nameKey,lastLoadHardwareId FROM sensor";
				command.CommandType = CommandType.Text;
				using (IDataReader reader = command.ExecuteReader()) {
					while (reader.Read()) {
						sensorInfos.Add(new DbSensorInfo(
							reader.GetValue(0) as string,
							reader.GetValue(1) as string
						));
					}
				}
			}
			return sensorInfos;
		}

		public IEnumerable<PackedReading> GetReadings(string sensor, DateTime from, TimeSpan span) {
			DateTime to = from.Add(span);
			if (to < from) {
				DateTime s = to;
				to = from;
				from = s;
			}
			if (!ForceConnectionOpen()) {
				throw new Exception("Could not open database.");
			}
			using (IDbCommand command = _connection.CreateCommand()) {
				command.CommandType = CommandType.Text;
				command.CommandText = "SELECT stamp,[values] FROM Record"
				+ " INNER JOIN Sensor ON (Sensor.sensorId = Record.sensorId)"
				+ " WHERE Sensor.nameKey = @sensorNameKey"
				+ " AND Record.stamp >= @minPosixStamp"
				+ " AND Record.stamp <= @maxPosixStamp"
				+ " ORDER BY stamp " + ((span < TimeSpan.Zero) ? "DESC" : "ASC");
				{
					DbParameter sensorStampMinParam;
					command.Parameters.Add(sensorStampMinParam = command.CreateParameter() as DbParameter);
					sensorStampMinParam.DbType = DbType.Int32;
					sensorStampMinParam.ParameterName = "minPosixStamp";
					sensorStampMinParam.Value = UnitUtility.ConvertToPosixTime(from);
				}
				{
					DbParameter sensorStampMaxParam;
					command.Parameters.Add(sensorStampMaxParam = command.CreateParameter() as DbParameter);
					sensorStampMaxParam.DbType = DbType.Int32;
					sensorStampMaxParam.ParameterName = "maxPosixStamp";
					sensorStampMaxParam.Value = UnitUtility.ConvertToPosixTime(to);
				}
				{
					DbParameter nameKeyParam;
					command.Parameters.Add(nameKeyParam = command.CreateParameter() as DbParameter);
					nameKeyParam.DbType = DbType.String;
					nameKeyParam.ParameterName = "sensorNameKey";
					nameKeyParam.Value = sensor;
				}

				using (IDataReader reader = command.ExecuteReader()) {
					int ordStamp = reader.GetOrdinal("stamp");
					int ordValues = reader.GetOrdinal("values");
					byte[] values = new byte[8];
					while (reader.Read()) {
						reader.GetBytes(ordValues, 0, values, 0, values.Length);
						yield return new PackedReading(
							UnitUtility.ConvertFromPosixTime(reader.GetInt32(ordStamp)),
							PackedReadingValues.ConvertFromPackedBytes(values,0)
						);
					}
				}
			}
		}

		private IEnumerable<PackedReadingsDaySummary> GetDaySummaries(string sensor, DateTime from, TimeSpan span) {
			DateTime to = from.Add(span);
			if (to < from) {
				DateTime s = to;
				to = from;
				from = s;
			}
			if (!ForceConnectionOpen()) {
				throw new Exception("Could not open database.");
			}
			using (IDbCommand command = _connection.CreateCommand()) {
				command.CommandType = CommandType.Text;
				command.CommandText = "SELECT stamp,minValues,maxValues,meanValues,medianValues,recordCount"
				+ ",tempCount,pressCount,humCount,speedCount,dirCount"
				+ " FROM DayRecord"
				+ " INNER JOIN Sensor ON (Sensor.sensorId = DayRecord.sensorId)"
				+ " WHERE Sensor.nameKey = @sensorNameKey"
				+ " AND DayRecord.stamp >= @minPosixStamp"
				+ " AND DayRecord.stamp < @maxPosixStamp"
				+ " ORDER BY stamp " + ((span < TimeSpan.Zero) ? "DESC" : "ASC");
				{
					DbParameter sensorStampMinParam;
					command.Parameters.Add(sensorStampMinParam = command.CreateParameter() as DbParameter);
					sensorStampMinParam.DbType = DbType.Int32;
					sensorStampMinParam.ParameterName = "minPosixStamp";
					sensorStampMinParam.Value = UnitUtility.ConvertToPosixTime(from);
				}
				{
					DbParameter sensorStampMaxParam;
					command.Parameters.Add(sensorStampMaxParam = command.CreateParameter() as DbParameter);
					sensorStampMaxParam.DbType = DbType.Int32;
					sensorStampMaxParam.ParameterName = "maxPosixStamp";
					sensorStampMaxParam.Value = UnitUtility.ConvertToPosixTime(to);
				}
				{
					DbParameter nameKeyParam;
					command.Parameters.Add(nameKeyParam = command.CreateParameter() as DbParameter);
					nameKeyParam.DbType = DbType.String;
					nameKeyParam.ParameterName = "sensorNameKey";
					nameKeyParam.Value = sensor;
				}

				using (IDataReader reader = command.ExecuteReader()) {
					int ordStamp = reader.GetOrdinal("stamp");
					int ordMinValues = reader.GetOrdinal("minValues");
					int ordMaxValues = reader.GetOrdinal("maxValues");
					int ordMeanValues = reader.GetOrdinal("meanValues");
					int ordMedianValues = reader.GetOrdinal("medianValues");
					int ordRecordCount = reader.GetOrdinal("recordCount");
					int ordTempCount = reader.GetOrdinal("tempCount");
					int ordPressCount = reader.GetOrdinal("pressCount");
					int ordHumCount = reader.GetOrdinal("humCount");
					int ordSpeedCount = reader.GetOrdinal("speedCount");
					int ordDirCount = reader.GetOrdinal("dirCount");

					byte[] values = new byte[8];
					//return ReadAsSensorReadings(reader);
					while (reader.Read()) {

						reader.GetBytes(ordMinValues, 0, values, 0, values.Length);
						PackedReadingValues minValues = PackedReadingValues.ConvertFromPackedBytes(values);
						reader.GetBytes(ordMaxValues, 0, values, 0, values.Length);
						PackedReadingValues maxValues = PackedReadingValues.ConvertFromPackedBytes(values);
						reader.GetBytes(ordMeanValues, 0, values, 0, values.Length);
						PackedReadingValues meanValues = PackedReadingValues.ConvertFromPackedBytes(values);
						reader.GetBytes(ordMedianValues, 0, values, 0, values.Length);
						PackedReadingValues medianValues = PackedReadingValues.ConvertFromPackedBytes(values);
						PackedReadingsDaySummary summary = new PackedReadingsDaySummary(
							UnitUtility.ConvertFromPosixTime(reader.GetInt32(ordStamp)),
							minValues,
							maxValues,
							meanValues,
							medianValues,
							reader.GetInt32(ordRecordCount)
						);

						summary.TemperatureCounts = PackedReadingValues.PackedCountsToHashUnsigned16(reader.GetValue(ordTempCount) as byte[]);
						summary.PressureCounts = PackedReadingValues.PackedCountsToHashUnsigned16(reader.GetValue(ordPressCount) as byte[]);
						summary.HumidityCounts = PackedReadingValues.PackedCountsToHashUnsigned16(reader.GetValue(ordHumCount) as byte[]);
						summary.WindSpeedCounts = PackedReadingValues.PackedCountsToHashUnsigned16(reader.GetValue(ordSpeedCount) as byte[]);
						summary.WindDirectionCounts = PackedReadingValues.PackedCountsToHashUnsigned16(reader.GetValue(ordDirCount) as byte[]);

						yield return summary;


					}
				}
			}
		}

		private IEnumerable<PackedReadingsHourSummary> GetHourSummaries(string sensor, DateTime from, TimeSpan span) {
			DateTime to = from.Add(span);
			if (to < from) {
				DateTime s = to;
				to = from;
				from = s;
			}
			if (!ForceConnectionOpen()) {
				throw new Exception("Could not open database.");
			}
			using (IDbCommand command = _connection.CreateCommand()) {
				command.CommandType = CommandType.Text;
				command.CommandText = "SELECT stamp,minValues,maxValues,meanValues,medianValues,recordCount"
				+ ",tempCount,pressCount,humCount,speedCount,dirCount"
				+ " FROM HourRecord"
				+ " INNER JOIN Sensor ON (Sensor.sensorId = HourRecord.sensorId)"
				+ " WHERE Sensor.nameKey = @sensorNameKey"
				+ " AND HourRecord.stamp >= @minPosixStamp"
				+ " AND HourRecord.stamp < @maxPosixStamp"
				+ " ORDER BY stamp " + ((span < TimeSpan.Zero) ? "DESC" : "ASC");
				{
					DbParameter sensorStampMinParam;
					command.Parameters.Add(sensorStampMinParam = command.CreateParameter() as DbParameter);
					sensorStampMinParam.DbType = DbType.Int32;
					sensorStampMinParam.ParameterName = "minPosixStamp";
					sensorStampMinParam.Value = UnitUtility.ConvertToPosixTime(from);
				}
				{
					DbParameter sensorStampMaxParam;
					command.Parameters.Add(sensorStampMaxParam = command.CreateParameter() as DbParameter);
					sensorStampMaxParam.DbType = DbType.Int32;
					sensorStampMaxParam.ParameterName = "maxPosixStamp";
					sensorStampMaxParam.Value = UnitUtility.ConvertToPosixTime(to);
				}
				{
					DbParameter nameKeyParam;
					command.Parameters.Add(nameKeyParam = command.CreateParameter() as DbParameter);
					nameKeyParam.DbType = DbType.String;
					nameKeyParam.ParameterName = "sensorNameKey";
					nameKeyParam.Value = sensor;
				}

				using (IDataReader reader = command.ExecuteReader()) {
					int ordStamp = reader.GetOrdinal("stamp");
					int ordMinValues = reader.GetOrdinal("minValues");
					int ordMaxValues = reader.GetOrdinal("maxValues");
					int ordMeanValues = reader.GetOrdinal("meanValues");
					int ordMedianValues = reader.GetOrdinal("medianValues");
					int ordRecordCount = reader.GetOrdinal("recordCount");
					int ordTempCount = reader.GetOrdinal("tempCount");
					int ordPressCount = reader.GetOrdinal("pressCount");
					int ordHumCount = reader.GetOrdinal("humCount");
					int ordSpeedCount = reader.GetOrdinal("speedCount");
					int ordDirCount = reader.GetOrdinal("dirCount");

					byte[] values = new byte[8];
					//return ReadAsSensorReadings(reader);
					while (reader.Read()) {

						reader.GetBytes(ordMinValues, 0, values, 0, values.Length);
						PackedReadingValues minValues = PackedReadingValues.ConvertFromPackedBytes(values);
						reader.GetBytes(ordMaxValues, 0, values, 0, values.Length);
						PackedReadingValues maxValues = PackedReadingValues.ConvertFromPackedBytes(values);
						reader.GetBytes(ordMeanValues, 0, values, 0, values.Length);
						PackedReadingValues meanValues = PackedReadingValues.ConvertFromPackedBytes(values);
						reader.GetBytes(ordMedianValues, 0, values, 0, values.Length);
						PackedReadingValues medianValues = PackedReadingValues.ConvertFromPackedBytes(values);
						PackedReadingsHourSummary summary = new PackedReadingsHourSummary(
							UnitUtility.ConvertFromPosixTime(reader.GetInt32(ordStamp)),
							minValues,
							maxValues,
							meanValues,
							medianValues,
							reader.GetInt32(ordRecordCount)
						);

						summary.TemperatureCounts = PackedReadingValues.PackedCountsToHashUnsigned16(reader.GetValue(ordTempCount) as byte[]);
						summary.PressureCounts = PackedReadingValues.PackedCountsToHashUnsigned16(reader.GetValue(ordPressCount) as byte[]);
						summary.HumidityCounts = PackedReadingValues.PackedCountsToHashUnsigned16(reader.GetValue(ordHumCount) as byte[]);
						summary.WindSpeedCounts = PackedReadingValues.PackedCountsToHashUnsigned16(reader.GetValue(ordSpeedCount) as byte[]);
						summary.WindDirectionCounts = PackedReadingValues.PackedCountsToHashUnsigned16(reader.GetValue(ordDirCount) as byte[]);

						yield return summary;


					}
				}
			}
		}

		IEnumerable<IReading> IDataStore.GetReadings(string sensor, DateTime from, TimeSpan span) {
			return GetReadings(sensor, from, span).OfType<IReading>();
		}

		public IEnumerable<IReadingsSummary> GetReadingSummaries(string sensor, DateTime from, TimeSpan span, TimeUnit summaryUnit) {
			if (TimeUnit.Second == summaryUnit || TimeUnit.Minute == summaryUnit) {
				return StatsUtil.Summarize<PackedReading>(GetReadings(sensor, from, span), summaryUnit).OfType<IReadingsSummary>();
			}
			else if (TimeUnit.Hour == summaryUnit) {
				return this.GetHourSummaries(sensor, from, span).OfType<IReadingsSummary>();
			}
			else if (TimeUnit.Day == summaryUnit) {
				return this.GetDaySummaries(sensor, from, span).OfType<IReadingsSummary>();
			}
			else if (TimeUnit.Month == summaryUnit || TimeUnit.Year == summaryUnit) {
				return this.GetDaySummaries(sensor, from, span).OfType<IReadingsSummary>();
			}
			else {
				throw new NotSupportedException("The supplied summary time unit is not supported.");
			}
		}

		public bool AddSensor(ISensorInfo sensor) {
			if (null == sensor) {
				throw new ArgumentNullException("sensor");
			}
			if (!ForceConnectionOpen()) {
				throw new Exception("Could not open database.");
			}
			int? sensorRecordId = null;
			using (IDbCommand getSensorCmd = _connection.CreateCommand()) {
				getSensorCmd.CommandType = CommandType.Text;
				getSensorCmd.CommandText = "SELECT sensorId FROM Sensor WHERE nameKey = '" + sensor.Name + "'";
				using (IDataReader getSensorReader = getSensorCmd.ExecuteReader()) {
					if (getSensorReader.Read()) {
						sensorRecordId = getSensorReader.GetInt32(getSensorReader.GetOrdinal("sensorId"));
					}
				}
			}
			if (!sensorRecordId.HasValue) {
				using (IDbCommand addSensorCmd = _connection.CreateCommand()) {
					addSensorCmd.CommandType = CommandType.Text;
					addSensorCmd.CommandText = "INSERT INTO Sensor(nameKey,lastLoadHardwareId) VALUES "
						+ "(@nameKey,@lastHardwareId)";
					DbParameter param;
					addSensorCmd.Parameters.Add(param = addSensorCmd.CreateParameter() as DbParameter);
					param.DbType = DbType.String;
					param.ParameterName = "nameKey";
					param.Value = sensor.Name;
					addSensorCmd.Parameters.Add(param = addSensorCmd.CreateParameter() as DbParameter);
					param.DbType = DbType.String;
					param.Value = "";
					param.ParameterName = "lastHardwareId";
					addSensorCmd.ExecuteNonQuery();
				}
				using (IDbCommand getSensorCmd = _connection.CreateCommand()) {
					getSensorCmd.CommandType = CommandType.Text;
					getSensorCmd.CommandText = "SELECT sensorId FROM Sensor WHERE nameKey = '" + sensor.Name + "'";
					using (IDataReader getSensorReader = getSensorCmd.ExecuteReader()) {
						if (getSensorReader.Read()) {
							sensorRecordId = getSensorReader.GetInt32(getSensorReader.GetOrdinal("sensorId"));
							return true;
						}
					}
				}
			}
			return false;
		}

		bool IDataStore.Push(string sensor, IEnumerable<IReading> readings) {
			return Push<IReading>(sensor, readings);
		}

		public bool Push<T>(string sensor, IEnumerable<T> readings) where T : IReading {
			return Push<T>(sensor, readings, false);
		}

		public bool Push<T>(string sensor, IEnumerable<T> readings, bool replace) where T : IReading {
			if (null == sensor) {
				throw new ArgumentNullException("sensor");
			}
			if (!ForceConnectionOpen()) {
				throw new Exception("Could not open database.");
			}
			int? sensorRecordId = null;
			using (IDbCommand getSensorCmd = _connection.CreateCommand()) {
				getSensorCmd.CommandType = CommandType.Text;
				getSensorCmd.CommandText = "SELECT sensorId FROM Sensor WHERE nameKey = '" + sensor + "'";
				using (IDataReader getSensorReader = getSensorCmd.ExecuteReader()) {
					if (getSensorReader.Read()) {
						sensorRecordId = getSensorReader.GetInt32(getSensorReader.GetOrdinal("sensorId"));
					}
				}
			}
			if (!sensorRecordId.HasValue) {
				return false;
			}
			DateTime minStamp = new DateTime(9999, 1, 1);
			DateTime maxStamp = new DateTime(0);
			using (IDbCommand pushRecordCommand = _connection.CreateCommand()) {
				pushRecordCommand.CommandType = CommandType.Text;
				pushRecordCommand.CommandText = "INSERT OR " + (replace ? "REPLACE" : "IGNORE") +
				                                " INTO Record (sensorId,stamp,[values]) VALUES " +
				                                "(@sensorId,@stamp,@values)";
				DbParameter sensorIdParam;
				pushRecordCommand.Parameters.Add(sensorIdParam = pushRecordCommand.CreateParameter() as DbParameter);
				sensorIdParam.DbType = DbType.Int32;
				sensorIdParam.ParameterName = "sensorId";
				sensorIdParam.Value = sensorRecordId.Value;
				DbParameter stampParam = pushRecordCommand.CreateParameter() as DbParameter;
				pushRecordCommand.Parameters.Add(stampParam);
				stampParam.DbType = DbType.Int32;
				stampParam.ParameterName = "stamp";
				DbParameter valuesParam = pushRecordCommand.CreateParameter() as DbParameter;
				pushRecordCommand.Parameters.Add(valuesParam);
				valuesParam.DbType = DbType.Binary;
				valuesParam.Size = 8;
				valuesParam.ParameterName = "values";
				pushRecordCommand.Transaction = _connection.BeginTransaction();

				int counter = 0;
				if (typeof (T) == typeof (PackedReading)) {
					foreach (PackedReading reading in readings.OfType<PackedReading>()) {
						stampParam.Value = UnitUtility.ConvertToPosixTime(reading.TimeStamp);
						valuesParam.Value = PackedReadingValues.ConvertToPackedBytes(reading.Values);
						pushRecordCommand.ExecuteNonQuery();
						counter++;
						if (counter >= 65536) {
							counter = 0;
							pushRecordCommand.Transaction.Commit();
							pushRecordCommand.Transaction = _connection.BeginTransaction();
						}
						if (reading.TimeStamp < minStamp) {
							minStamp = reading.TimeStamp;
						}
						if (reading.TimeStamp > maxStamp) {
							maxStamp = reading.TimeStamp;
						}
					}
				}
				else {
					foreach (IReading reading in readings) {
						stampParam.Value = UnitUtility.ConvertToPosixTime(reading.TimeStamp);
						valuesParam.Value = PackedReadingValues.ConvertToPackedBytes(reading);
						pushRecordCommand.ExecuteNonQuery();
						counter++;
						if (counter >= 65536) {
							counter = 0;
							pushRecordCommand.Transaction.Commit();
							pushRecordCommand.Transaction = _connection.BeginTransaction();
						}
						if (reading.TimeStamp < minStamp) {
							minStamp = reading.TimeStamp;
						}
						if (reading.TimeStamp > maxStamp) {
							maxStamp = reading.TimeStamp;
						}
					}
				}

				/*DateTime hourAlignedMinStamp = UnitUtility.StripToUnit(minStamp, TimeUnit.Hour);
				DateTime hourAlignedMaxStamp = UnitUtility.StripToUnit(maxStamp, TimeUnit.Hour).AddHours(1.0);
				DateTime dayAlignedMinStamp = UnitUtility.StripToUnit(minStamp, TimeUnit.Day);
				DateTime dayAlignedMaxStamp = UnitUtility.StripToUnit(maxStamp, TimeUnit.Day).AddDays(1.0);*/
				DateTime dayAlignedMinStamp = UnitUtility.StripToUnit(minStamp, TimeUnit.Day);
				DateTime dayAlignedMaxStamp = UnitUtility.StripToUnit(maxStamp, TimeUnit.Day).AddDays(1.0);

				List<SensorReadingsSummary> hourSummaries = new List<SensorReadingsSummary>(
					StatsUtil.Summarize<PackedReading>(
						GetReadings(sensor, dayAlignedMinStamp, dayAlignedMaxStamp.Subtract(dayAlignedMinStamp)), TimeUnit.Hour)
					);

				List<SensorReadingsSummary> updates = new List<SensorReadingsSummary>();

				pushRecordCommand.CommandType = CommandType.Text;
				pushRecordCommand.CommandText = "SELECT stamp FROM HourRecord"
				                                + " INNER JOIN Sensor ON (Sensor.sensorId = HourRecord.sensorId)"
				                                + " WHERE Sensor.nameKey = @sensorNameKey"
				                                + " AND HourRecord.stamp >= @minPosixStamp"
				                                + " AND HourRecord.stamp <= @maxPosixStamp";
				{
					DbParameter sensorStampMinParam;
					pushRecordCommand.Parameters.Add(sensorStampMinParam = pushRecordCommand.CreateParameter() as DbParameter);
					sensorStampMinParam.DbType = DbType.Int32;
					sensorStampMinParam.ParameterName = "minPosixStamp";
					sensorStampMinParam.Value = UnitUtility.ConvertToPosixTime(dayAlignedMinStamp);
				}
				{
					DbParameter sensorStampMaxParam;
					pushRecordCommand.Parameters.Add(sensorStampMaxParam = pushRecordCommand.CreateParameter() as DbParameter);
					sensorStampMaxParam.DbType = DbType.Int32;
					sensorStampMaxParam.ParameterName = "maxPosixStamp";
					sensorStampMaxParam.Value = UnitUtility.ConvertToPosixTime(dayAlignedMaxStamp);
				}
				{
					DbParameter nameKeyParam;
					pushRecordCommand.Parameters.Add(nameKeyParam = pushRecordCommand.CreateParameter() as DbParameter);
					nameKeyParam.DbType = DbType.String;
					nameKeyParam.ParameterName = "sensorNameKey";
					nameKeyParam.Value = sensor;
				}

				DbParameter minValuesParam = pushRecordCommand.CreateParameter() as DbParameter;
				pushRecordCommand.Parameters.Add(minValuesParam);
				minValuesParam.DbType = DbType.Binary;
				minValuesParam.Size = 8;
				minValuesParam.ParameterName = "minValues";
				DbParameter maxValuesParam = pushRecordCommand.CreateParameter() as DbParameter;
				pushRecordCommand.Parameters.Add(maxValuesParam);
				maxValuesParam.DbType = DbType.Binary;
				maxValuesParam.Size = 8;
				maxValuesParam.ParameterName = "maxValues";
				DbParameter meanValuesParam = pushRecordCommand.CreateParameter() as DbParameter;
				pushRecordCommand.Parameters.Add(meanValuesParam);
				meanValuesParam.DbType = DbType.Binary;
				meanValuesParam.Size = 8;
				meanValuesParam.ParameterName = "meanValues";
				DbParameter medianValuesParam = pushRecordCommand.CreateParameter() as DbParameter;
				pushRecordCommand.Parameters.Add(medianValuesParam);
				medianValuesParam.DbType = DbType.Binary;
				medianValuesParam.Size = 8;
				medianValuesParam.ParameterName = "medianValues";
				DbParameter recordCountParam = pushRecordCommand.CreateParameter() as DbParameter;
				pushRecordCommand.Parameters.Add(recordCountParam);
				recordCountParam.DbType = DbType.Int32;
				recordCountParam.ParameterName = "recordCount";
				DbParameter tempCountParam = pushRecordCommand.CreateParameter() as DbParameter;
				pushRecordCommand.Parameters.Add(tempCountParam);
				tempCountParam.DbType = DbType.Binary;
				tempCountParam.Size = 0;
				tempCountParam.ParameterName = "tempCount";
				DbParameter pressCountParam = pushRecordCommand.CreateParameter() as DbParameter;
				pushRecordCommand.Parameters.Add(pressCountParam);
				pressCountParam.DbType = DbType.Binary;
				pressCountParam.Size = 0;
				pressCountParam.ParameterName = "pressCount";
				DbParameter humCountParam = pushRecordCommand.CreateParameter() as DbParameter;
				pushRecordCommand.Parameters.Add(humCountParam);
				humCountParam.DbType = DbType.Binary;
				humCountParam.Size = 0;
				humCountParam.ParameterName = "humCount";
				DbParameter speedCountParam = pushRecordCommand.CreateParameter() as DbParameter;
				pushRecordCommand.Parameters.Add(speedCountParam);
				speedCountParam.DbType = DbType.Binary;
				speedCountParam.Size = 0;
				speedCountParam.ParameterName = "speedCount";
				DbParameter dirCountParam = pushRecordCommand.CreateParameter() as DbParameter;
				pushRecordCommand.Parameters.Add(dirCountParam);
				dirCountParam.DbType = DbType.Binary;
				dirCountParam.Size = 0;
				dirCountParam.ParameterName = "dirCount";

				{

					List<DateTime> existingHourStamps = new List<DateTime>();
					using (IDataReader existingDatesReader = pushRecordCommand.ExecuteReader()) {
						int ordStamp = existingDatesReader.GetOrdinal("stamp");
						while (existingDatesReader.Read()) {
							existingHourStamps.Add(UnitUtility.ConvertFromPosixTime(existingDatesReader.GetInt32(ordStamp)));
						}
					}

					pushRecordCommand.CommandText =
						"INSERT INTO HourRecord(sensorId,stamp,minValues,maxValues,meanValues,medianValues,recordCount,tempCount,pressCount,humCount,speedCount,dirCount) VALUES " +
						"(@sensorId,@stamp,@minValues,@maxValues,@meanValues,@medianValues,@recordCount,@tempCount,@pressCount,@humCount,@speedCount,@dirCount)";

					// todo: try to get this into a single query as above they are.

					foreach (SensorReadingsSummary summary in hourSummaries) {
						if (existingHourStamps.Contains(summary.BeginStamp)) {
							updates.Add(summary);
						}
						else {

							stampParam.Value = UnitUtility.ConvertToPosixTime(summary.BeginStamp);
							minValuesParam.Value = PackedReadingValues.ConvertToPackedBytes(summary.Min);
							maxValuesParam.Value = PackedReadingValues.ConvertToPackedBytes(summary.Max);
							meanValuesParam.Value = PackedReadingValues.ConvertToPackedBytes(summary.Mean);
							medianValuesParam.Value = PackedReadingValues.ConvertToPackedBytes(summary.Median);
							recordCountParam.Value = summary.Count;

							byte[] data;
							tempCountParam.Value =
								data = PackedReadingValues.ConvertTemperatureCountsToPackedBytes(summary.TemperatureCounts);
							tempCountParam.Size = data.Length;
							pressCountParam.Value = data = PackedReadingValues.ConvertPressureCountsToPackedBytes(summary.PressureCounts);
							pressCountParam.Size = data.Length;
							humCountParam.Value = data = PackedReadingValues.ConvertHumidityCountsToPackedBytes(summary.HumidityCounts);
							humCountParam.Size = data.Length;
							speedCountParam.Value = data = PackedReadingValues.ConvertWindSpeedCountsToPackedBytes(summary.WindSpeedCounts);
							speedCountParam.Size = data.Length;
							dirCountParam.Value =
								data = PackedReadingValues.ConvertWindDirectionCountsToPackedBytes(summary.WindDirectionCounts);
							dirCountParam.Size = data.Length;

							pushRecordCommand.ExecuteNonQuery();
						}
					}

					if (updates.Count > 0) {
						pushRecordCommand.CommandText = "UPDATE HourRecord SET"
						                                +
						                                " minValues=@minValues,maxValues=@maxValues,meanValues=@meanValues,medianValues=@medianValues,recordCount=@recordCount"
						                                + " WHERE sensorId=@sensorId AND stamp=@stamp";
						foreach (SensorReadingsSummary summary in updates) {
							stampParam.Value = UnitUtility.ConvertToPosixTime(summary.BeginStamp);
							minValuesParam.Value = PackedReadingValues.ConvertToPackedBytes(summary.Min);
							maxValuesParam.Value = PackedReadingValues.ConvertToPackedBytes(summary.Max);
							meanValuesParam.Value = PackedReadingValues.ConvertToPackedBytes(summary.Mean);
							medianValuesParam.Value = PackedReadingValues.ConvertToPackedBytes(summary.Median);
							recordCountParam.Value = summary.Count;

							byte[] data;
							tempCountParam.Value =
								data = PackedReadingValues.ConvertTemperatureCountsToPackedBytes(summary.TemperatureCounts);
							tempCountParam.Size = data.Length;
							pressCountParam.Value = data = PackedReadingValues.ConvertPressureCountsToPackedBytes(summary.PressureCounts);
							pressCountParam.Size = data.Length;
							humCountParam.Value = data = PackedReadingValues.ConvertHumidityCountsToPackedBytes(summary.HumidityCounts);
							humCountParam.Size = data.Length;
							speedCountParam.Value = data = PackedReadingValues.ConvertWindSpeedCountsToPackedBytes(summary.WindSpeedCounts);
							speedCountParam.Size = data.Length;
							dirCountParam.Value =
								data = PackedReadingValues.ConvertWindDirectionCountsToPackedBytes(summary.WindDirectionCounts);
							dirCountParam.Size = data.Length;

							pushRecordCommand.ExecuteNonQuery();
						}
					}

				}

				updates.Clear();



				List<SensorReadingsSummary> daySummaries = new List<SensorReadingsSummary>();
				{
					List<List<SensorReadingsSummary>> dailyHourSummaries = new List<List<SensorReadingsSummary>>();
					List<SensorReadingsSummary> currentHourSummaries = new List<SensorReadingsSummary>();
					foreach (SensorReadingsSummary hourSummary in hourSummaries) {
						if (
							currentHourSummaries.Count == 0
							||
							currentHourSummaries[0].BeginStamp.Date.Equals(hourSummary.BeginStamp.Date)
							) {
							currentHourSummaries.Add(hourSummary);
						}
						else {
							dailyHourSummaries.Add(currentHourSummaries);
							currentHourSummaries = new List<SensorReadingsSummary>();
						}
					}
					if (currentHourSummaries.Count > 0) {
						dailyHourSummaries.Add(currentHourSummaries);
					}
					foreach (List<SensorReadingsSummary> hourlyForCondensing in dailyHourSummaries) {
						SensorReadingsSummary summary = StatsUtil.Combine(hourlyForCondensing);
						summary.BeginStamp = summary.BeginStamp.Date;
						summary.EndStamp = summary.BeginStamp.AddDays(1).AddTicks(-1);
						daySummaries.Add(summary);
					}
				}
				{
					pushRecordCommand.CommandText = "SELECT stamp FROM DayRecord"
					                                + " INNER JOIN Sensor ON (Sensor.sensorId = DayRecord.sensorId)"
					                                + " WHERE Sensor.nameKey = @sensorNameKey"
					                                + " AND DayRecord.stamp >= @minPosixStamp"
					                                + " AND DayRecord.stamp <= @maxPosixStamp";

					List<DateTime> existingDayStamps = new List<DateTime>();
					using (IDataReader existingDatesReader = pushRecordCommand.ExecuteReader()) {
						int ordStamp = existingDatesReader.GetOrdinal("stamp");
						while (existingDatesReader.Read()) {
							existingDayStamps.Add(UnitUtility.ConvertFromPosixTime(existingDatesReader.GetInt32(ordStamp)));
						}
					}

					pushRecordCommand.CommandText =
						"INSERT INTO DayRecord(sensorId,stamp,minValues,maxValues,meanValues,medianValues,recordCount,tempCount,pressCount,humCount,speedCount,dirCount) VALUES " +
						"(@sensorId,@stamp,@minValues,@maxValues,@meanValues,@medianValues,@recordCount,@tempCount,@pressCount,@humCount,@speedCount,@dirCount)";
					foreach (SensorReadingsSummary summary in daySummaries) {
						if (existingDayStamps.Contains(summary.BeginStamp)) {
							updates.Add(summary);
						}
						else {

							stampParam.Value = UnitUtility.ConvertToPosixTime(summary.BeginStamp);
							minValuesParam.Value = PackedReadingValues.ConvertToPackedBytes(summary.Min);
							maxValuesParam.Value = PackedReadingValues.ConvertToPackedBytes(summary.Max);
							meanValuesParam.Value = PackedReadingValues.ConvertToPackedBytes(summary.Mean);
							medianValuesParam.Value = PackedReadingValues.ConvertToPackedBytes(summary.Median);
							recordCountParam.Value = summary.Count;

							byte[] data;
							tempCountParam.Value =
								data = PackedReadingValues.ConvertTemperatureCountsToPackedBytes(summary.TemperatureCounts);
							tempCountParam.Size = data.Length;
							pressCountParam.Value = data = PackedReadingValues.ConvertPressureCountsToPackedBytes(summary.PressureCounts);
							pressCountParam.Size = data.Length;
							humCountParam.Value = data = PackedReadingValues.ConvertHumidityCountsToPackedBytes(summary.HumidityCounts);
							humCountParam.Size = data.Length;
							speedCountParam.Value = data = PackedReadingValues.ConvertWindSpeedCountsToPackedBytes(summary.WindSpeedCounts);
							speedCountParam.Size = data.Length;
							dirCountParam.Value =
								data = PackedReadingValues.ConvertWindDirectionCountsToPackedBytes(summary.WindDirectionCounts);
							dirCountParam.Size = data.Length;

							pushRecordCommand.ExecuteNonQuery();
						}
					}

					if (updates.Count > 0) {
						pushRecordCommand.CommandText = "UPDATE DayRecord SET"
						                                +
						                                " minValues=@minValues,maxValues=@maxValues,meanValues=@meanValues,medianValues=@medianValues,recordCount=@recordCount"
						                                + " WHERE sensorId=@sensorId AND stamp=@stamp";
						foreach (SensorReadingsSummary summary in updates) {
							stampParam.Value = UnitUtility.ConvertToPosixTime(summary.BeginStamp);
							minValuesParam.Value = PackedReadingValues.ConvertToPackedBytes(summary.Min);
							maxValuesParam.Value = PackedReadingValues.ConvertToPackedBytes(summary.Max);
							meanValuesParam.Value = PackedReadingValues.ConvertToPackedBytes(summary.Mean);
							medianValuesParam.Value = PackedReadingValues.ConvertToPackedBytes(summary.Median);
							recordCountParam.Value = summary.Count;

							byte[] data;
							tempCountParam.Value =
								data = PackedReadingValues.ConvertTemperatureCountsToPackedBytes(summary.TemperatureCounts);
							tempCountParam.Size = data.Length;
							pressCountParam.Value = data = PackedReadingValues.ConvertPressureCountsToPackedBytes(summary.PressureCounts);
							pressCountParam.Size = data.Length;
							humCountParam.Value = data = PackedReadingValues.ConvertHumidityCountsToPackedBytes(summary.HumidityCounts);
							humCountParam.Size = data.Length;
							speedCountParam.Value = data = PackedReadingValues.ConvertWindSpeedCountsToPackedBytes(summary.WindSpeedCounts);
							speedCountParam.Size = data.Length;
							dirCountParam.Value =
								data = PackedReadingValues.ConvertWindDirectionCountsToPackedBytes(summary.WindDirectionCounts);
							dirCountParam.Size = data.Length;

							pushRecordCommand.ExecuteNonQuery();
						}
					}
				}


				pushRecordCommand.Transaction.Commit();
			}
			return true;
		}

		#endregion

		public void Dispose() {
			if (_connection is IDisposable) {
				(_connection as IDisposable).Dispose();
			}
		}

	}
}
