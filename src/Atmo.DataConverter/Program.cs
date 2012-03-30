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
using System.IO;
using System.Linq;
using Atmo.Data;

namespace Atmo.DataConverter {
	class Program {

		static int Main(string[] args) {
			return (null == args || args.Length < 1)
				? DisplayUsage()
				: ProcessFiles(ParseFileArgs(args));
		}

		

		static int DisplayUsage() {
			Console.WriteLine(@"Usage: AtmoDataConverter ""C:\file\path\one.csv"" ""E:\file\path\two.dat""");
			return 0;
		}

		private static IEnumerable<FileInfo> ParseFileArgs(IEnumerable<string> args) {
			return args
				.Select(arg => new FileInfo(arg))
				.ToList();
		}

		private static int ProcessFiles(IEnumerable<FileInfo> files) {

			int result = 0;

			foreach(var file in files) {
				int localResult = ProcessFile(file);
				result = Math.Max(result, localResult);
			}

			return result;
		}

		private static int ProcessFile(FileInfo file) {

			if(!file.Exists) {
				Console.WriteLine("ERROR: file '{0}' not found.", file.FullName);
				return 1;
			}

			try {
				// try to convert daq to csv
				using(var daqDataFileReader = new DaqDataFileReader(file)) {
					return SaveCsv(ReadAll(daqDataFileReader), file.FullName);
				}
			}
			catch (IOException ex) {
				if(!file.Extension.EndsWith("csv",StringComparison.OrdinalIgnoreCase))
					Console.WriteLine("ERROR: csv may already exist.");
			}
			catch(Exception ex) {
				return 1;
			}

			try {
				using(var csvDataFileReader = new CsvDataFileReader(file.FullName)) {
					return SaveDat(CorrectTimeStamps(csvDataFileReader.ReadAll()), file.FullName);
				}
			}
			catch (Exception ex) {
				; // not a CSV file?
			}

			return 2;
		}

		private static IEnumerable<TReading[]> GroupByStamp<TReading>(IEnumerable<TReading> readings) where TReading : IReading {
			var buffer = new List<TReading>(61);
			foreach(var reading in readings) {
				if (buffer.Count == 0 || buffer[0].TimeStamp == reading.TimeStamp) {
					buffer.Add(reading);
				}
				else {
					yield return buffer.ToArray();
					buffer.Clear();
					buffer.Add(reading);
				}
			}
			if (buffer.Count != 0)
				yield return buffer.ToArray();
		}

		private static IEnumerable<Reading> CorrectTimeStamps<TReading>(IEnumerable<TReading> readings) where TReading: IReading {
			using(var groupEnumerator = GroupByStamp(readings).GetEnumerator()) {
				if (!groupEnumerator.MoveNext())
					yield break;
				var currentGroup = groupEnumerator.Current;
				if (currentGroup.Length == 1) {
					yield return new Reading(currentGroup[0]); // just use it as is
					if(groupEnumerator.MoveNext())
						currentGroup = groupEnumerator.Current; // we have more to do
					else
						yield break; // all done
				} else {
					var firstGroup = currentGroup;
					if(groupEnumerator.MoveNext()) {
						currentGroup = groupEnumerator.Current;
						var firstNextStamp = currentGroup.First().TimeStamp; // assumes a valid initial record
						for (int i = 0; i < firstGroup.Length; i++) {
							yield return new Reading(firstNextStamp.AddSeconds(i - firstGroup.Length), firstGroup[i]); // remove seconds going backwards, but keep records in order
						}
						// TODO: correct the times for first group from the first of current group
					}else {
						// just add seconds to the stamps, we have no further information to use
						for(int i = 0; i < firstGroup.Length; i++) {
							yield return new Reading(firstGroup[i].TimeStamp.AddSeconds(i), firstGroup[i]);
						}
						yield break; // nothing left
					}
				}
				// at this point, assume we have already done a MoveNext
				do {
					currentGroup = groupEnumerator.Current;
					if(currentGroup.Length == 1) {
						yield return new Reading(currentGroup[0]);
					}
					else if(currentGroup.Length > 0) {
						var firstStamp = currentGroup[0].TimeStamp;
						for(int i = 0; i < currentGroup.Length && i < 60; i++) {
							yield return new Reading(firstStamp.AddSeconds(i), currentGroup[i]);
						}
						for(int i = 60; i < currentGroup.Length; i++) {
							yield return new Reading(firstStamp.AddSeconds(59), currentGroup[i]);
						}
					}
				} while (groupEnumerator.MoveNext());
			}
		}

		private static IEnumerable<PackedReading> ReadAll(DaqDataFileReader reader)  {
			while(reader.MoveNext()) {
				yield return reader.Current;
			}
		}

		private static int SaveCsv<TReading>(IEnumerable<TReading> readings, string csvFileName) where TReading : IReading {
			csvFileName = Path.ChangeExtension(csvFileName, "csv");
			var iniFileName = Path.ChangeExtension(csvFileName, "ini");
			using (FileStream
				iniStream = File.Open(iniFileName, FileMode.Create, FileAccess.ReadWrite),
				csvStream = File.Open(csvFileName, FileMode.Create, FileAccess.ReadWrite)
			) {
				using (StreamWriter
					iniWriter = new StreamWriter(iniStream),
					csvWriter = new StreamWriter(csvStream)
				) {
					var dataWriter = new CsvDataFileWriter(
						csvWriter, iniWriter,
						Path.GetFileName(csvFileName));

					foreach (var reading in readings) {
						dataWriter.Write(reading);
					}

					Console.WriteLine("Saving to {0}", csvFileName);

					iniWriter.Flush();
					csvWriter.Flush();
				}
			}
			return 0;
		}

		private static int SaveDat(IEnumerable<Reading> readings, string datFileName) {
			datFileName = Path.ChangeExtension(datFileName, "dat");

			using(var writer = new DaqDataFileWriter(datFileName)) {
				foreach(var reading in readings) {
					writer.Write(reading);
				}
				Console.WriteLine("Saving to {0}", datFileName);
			}

			return 0;
		}

	}
}
