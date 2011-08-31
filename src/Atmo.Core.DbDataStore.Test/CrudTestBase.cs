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
using System.Data;
using System.IO;
using System.Reflection;
using System.Threading;
using Atmo.Data;
using NUnit.Framework;
using System.Collections.Generic;

namespace Atmo.Core.DbDataStore.Test {

	public class CrudTestBase {

		private string _fileName = null;
		private IDbConnection _connection = null;

		protected IDbConnection Connection {
			get { return _connection; }
		}

		public static Assembly ThisAsm {
			get { return typeof(CrudTests).Assembly; }
		}

		public static string AssemblyFolder {
			get {
				var asmPath = new Uri(ThisAsm.CodeBase).LocalPath;
				return Path.GetDirectoryName(asmPath);
			}
		}

		private static string GenerateDbFileName() {
			return Path.ChangeExtension(Guid.NewGuid().ToString(), "db");
		}

		private List<string> GetTableNames() {
			var tableNames = new List<string>();
			using (var cmd = _connection.CreateTextCommand(
				"SELECT name FROM"
				+ " (SELECT * FROM sqlite_master UNION ALL SELECT * FROM sqlite_temp_master)"
				+ " WHERE type='table'"
				+ " ORDER BY name"
			)) {
				using (var reader = cmd.ExecuteReader()) {
					while (reader.Read()) {
						string tableName = reader.GetString(0);
						if (!tableName.StartsWith("sqlite", StringComparison.OrdinalIgnoreCase)) {
							tableNames.Add(tableName);
						}
					}
				}
			}
			return tableNames;
		}

		private int TruncateTable(string name) {
			using(var cmd = _connection.CreateTextCommand(String.Format("DELETE FROM [{0}]",name))) {
				return cmd.ExecuteNonQuery();
			}
		}

		private IDbConnection GenerateConnection() {
			return new System.Data.SQLite.SQLiteConnection(
				String.Format(@"data source=""{0}"";page size=65536;journal mode=OFF;ignore_check_constraints=ON;synchronous=OFF", _fileName)
			);
		}

		[TestFixtureSetUp]
		public void TestFixtureSetUp() {
			_fileName = Path.ChangeExtension(Guid.NewGuid().ToString(), "db");
			using (var readStream = ThisAsm.GetManifestResourceStream("Atmo.Core.DbDataStore.Test.ClearStorage.db")) {
				using (var newFile = File.Create(_fileName)) {
					byte[] buffer = new byte[32768];
					int read;
					while ((read = readStream.Read(buffer, 0, buffer.Length)) > 0) {
						newFile.Write(buffer, 0, read);
					}
					newFile.Flush();
				}
			}
			Thread.Sleep(100);
		}

		[SetUp]
		public void SetUp() {
			_connection = GenerateConnection();
			Thread.Sleep(100);
			_connection.Open();
			Thread.Sleep(100);
			foreach(var tableName in GetTableNames()) {
				TruncateTable(tableName);
			}
			Thread.Sleep(100);
		}

		[Test(Description = "make sure that the temp database is there")]
		public void BaseTest() {
			Assert.AreEqual(ConnectionState.Open, _connection.State);
			Assert.That(File.Exists(_fileName));
			Assert.Greater(new FileInfo(_fileName).Length, 0);
		}

		[TearDown]
		public void TearDown() {
			Thread.Sleep(100);
			_connection.Dispose();
		}

		[TestFixtureTearDown]
		public void TestFixtureTearDown() {
			try {
				if (null != _connection && _connection.State != ConnectionState.Closed) {
					_connection.Dispose();
					_connection = null;
				}
			}
			catch {
				; // could not disconnect
			}
			try {
				File.Delete(_fileName);
			}
			catch {
				; // could not delete the file, left a mess
			}
		}

	}
}
