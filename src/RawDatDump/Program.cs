using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Atmo;
using Atmo.Data;

namespace RawDatDump
{
	class Program
	{
		static int Main(string[] args) {
			return (null == args || args.Length < 1)
				? DisplayUsage()
				: ProcessFiles(ParseFileArgs(args));
		}

		static int DisplayUsage() {
			Console.WriteLine(@"Usage: RawDatDump ""E:\file\path\file.dat""");
			return 0;
		}

		private static IEnumerable<FileInfo> ParseFileArgs(IEnumerable<string> args) {
			return args
				.Select(arg => new FileInfo(arg))
				.ToList();
		}

		private static int ProcessFiles(IEnumerable<FileInfo> files) {

			int result = 0;

			foreach (var file in files) {
				int localResult = ProcessFile(file);
				result = Math.Max(result, localResult);
			}

			return result;
		}

		private static int ProcessFile(FileInfo file) {

			if (!file.Exists) {
				Console.WriteLine("ERROR: file '{0}' not found.", file.FullName);
				return 1;
			}

			try {
				// try to convert daq to csv
				using (var daqDataFileReader = new DaqDataFileReader(file)) {
					return SaveDump(ReadAll(daqDataFileReader), file.FullName);
				}
			}
			catch (IOException ex) {
				if (!file.Extension.EndsWith("csv", StringComparison.OrdinalIgnoreCase))
					Console.WriteLine("ERROR: csv may already exist.");
			}
			catch (Exception ex) {
				return 1;
			}


			return 2;
		}

		private static IEnumerable<PackedReading> ReadAll(DaqDataFileReader reader) {
			while (reader.MoveNext()) {
				yield return reader.Current;
			}
		}

		private static int SaveDump(IEnumerable<PackedReading> readings, string txtFileName) {
			txtFileName = Path.ChangeExtension(txtFileName, "txt");
			using (var txtStream = File.Open(txtFileName, FileMode.Create, FileAccess.ReadWrite))
			using (var txtWriter = new StreamWriter(txtStream)) {
				foreach (var reading in readings) {
					txtWriter.Write(reading.TimeStamp);
					txtWriter.Write("\tT: {0} {1} 0x{1:X}", reading.Values.Temperature, reading.Values.RawTemperature);
					txtWriter.Write("\tP: {0} {1} 0x{1:X}", reading.Values.Pressure, reading.Values.RawPressure);
					txtWriter.Write("\tH: {0} {1} 0x{1:X}", reading.Values.Humidity, reading.Values.RawHumidity);
					txtWriter.Write("\tS: {0} {1} 0x{1:X}", reading.Values.WindSpeed, reading.Values.RawWindSpeed);
					txtWriter.Write("\tD: {0} {1} 0x{1:X}", reading.Values.WindDirection, reading.Values.RawWindDirection);
					txtWriter.WriteLine("\tF: {0} {1} 0x{1:X}", reading.Values.RawFlags, (int)reading.Values.RawFlags);
				}
				Console.WriteLine("Saving to {0}", txtFileName);
				txtWriter.Flush();
			}
			return 0;
		}


	}
}
