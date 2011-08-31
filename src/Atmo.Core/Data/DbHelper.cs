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
using System.Data.Common;

namespace Atmo.Data {

	/// <summary>
	/// Some helper methods for database operations.
	/// </summary>
	public static class DbHelper {

		/// <summary>
		/// Adds and returns a new parameter to a command.
		/// </summary>
		/// <param name="command">The command to add the parameter to.</param>
		/// <param name="name">The name to give the parameter.</param>
		/// <param name="dbType">The database value type to assign to.</param>
		/// <param name="value">The actual value to store.</param>
		/// <returns>The newly created and added parameter.</returns>
		public static DbParameter AddParameter(this IDbCommand command, string name, DbType dbType, object value) {
			var parameter = command.CreateParameter() as DbParameter;
			if (null == parameter)
				throw new ArgumentException("command does not create parameters of type DbParameter.", "command");

			parameter.DbType = dbType;
			parameter.ParameterName = name;
			parameter.Value = value;
			command.Parameters.Add(parameter);
			return parameter;
		}

		/// <summary>
		/// Create a text command with the given SQL command text.
		/// </summary>
		/// <param name="connection">The connection to create the command from.</param>
		/// <param name="commandText">The command text to use.</param>
		/// <returns>A new text command with the given command text.</returns>
		public static IDbCommand CreateTextCommand(this IDbConnection connection, string commandText) {
			var command = connection.CreateCommand();
			command.CommandType = CommandType.Text;
			command.CommandText = commandText ?? String.Empty;
			return command;
		}

	}
}
