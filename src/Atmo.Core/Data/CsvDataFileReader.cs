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
using System.IO;
using System.Data.OleDb;

namespace Atmo.Data {
	public class CsvDataFileReader : IDisposable {

		private static string CreateCsvConnString(string filePath) {
			return String.Format(
				"Provider=Microsoft.Jet.OLEDB.4.0;"
				+ @"Data Source=""{0}"";"
				+ @"Extended Properties=""text;HDR=No;FMT=Delimited""",
				Path.GetDirectoryName(filePath)
			);
		}

		private static string CreateCsvTableName(string filePath) {
			return Path.GetFileName(filePath);
		}

		private readonly string _filePath;
		private readonly OleDbConnection _conn;

		public CsvDataFileReader(string filePath) {
			if(String.IsNullOrEmpty(filePath))
				throw new ArgumentException("filePath must be a file path.", "filePath");
			if(!File.Exists(filePath))
				throw new ArgumentException("filePath must be a file that exists.", "filePath");

			_filePath = filePath;
			_conn = new OleDbConnection(CreateCsvConnString(filePath));
			_conn.Open();
		}

		~CsvDataFileReader() {
			Dispose(false);
		}

		/// <inheritdoc/>
		public void Dispose() {
			Dispose(true);
		}

		private static DateTime AsDateTime(object o) {
			return (o is DateTime)
				? (DateTime)o
				: DateTime.Parse(o.ToString());
		}

		private static double AsDouble(object o) {
			if(o is double)
				return (double) o;
			if (null == o || o is DBNull)
				return Double.NaN;
			return Double.Parse(o.ToString());
		}

		public IEnumerable<Reading> ReadAll() {
			using(var cmd = _conn.CreateCommand()) {
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = String.Format("SELECT * FROM {0}", CreateCsvTableName(_filePath));
				using(var reader = cmd.ExecuteReader()) {
					if (null == reader)
						yield break;
					while(reader.Read()) {
						Reading reading;
						try {
							reading = new Reading(
								AsDateTime(reader.GetValue(0)),
								AsDouble(reader.GetValue(1)),
								AsDouble(reader.GetValue(2)),
								AsDouble(reader.GetValue(3)),
								AsDouble(reader.GetValue(5)),
								AsDouble(reader.GetValue(4))
							);
						}
						catch(Exception ex) {
							continue;
						}
						yield return reading;
					}
				}
			}
		}

		private void Dispose(bool disposing) {
			if(disposing)
				GC.SuppressFinalize(this);

			_conn.Dispose();
		}

	}
}
