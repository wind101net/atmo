using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
			catch (Exception ex) {
				; // not a DAQ file?
			}

			return 0;
		}

		private static IEnumerable<IReading> ReadAll(DaqDataFileReader reader) {
			while(reader.MoveNext()) {
				yield return reader.Current;
			}
		}

		private static int SaveCsv(IEnumerable<IReading> iEnumerable, string csvFileName) {
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

					foreach (var reading in iEnumerable) {
						dataWriter.Write(reading);
					}

					iniWriter.Flush();
					csvWriter.Flush();
				}
			}
			return 0;
		}

	}
}
