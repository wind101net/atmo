﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Atmo.Data {

	public class CsvDataFileWriter
	{

		public static void WriteIni(StreamWriter iniWriter, string csvFileName) {
			iniWriter.Write('[');
			iniWriter.Write(csvFileName);
			iniWriter.WriteLine(']');
			iniWriter.WriteLine("ColnameHeader=True");
			iniWriter.WriteLine("Format=CSVDelimited");
			iniWriter.WriteLine("DateTimeFormat=yyyy/MM/dd HH:nn:ss");
			iniWriter.WriteLine("Col1=Stamp DateTime");
			iniWriter.WriteLine("Col2=Temperature Double");
			iniWriter.WriteLine("Col3=Pressure Double");
			iniWriter.WriteLine("Col4=Humidity Double");
			iniWriter.WriteLine("Col5=Speed Double");
			iniWriter.WriteLine("Col6=Direction Double");
		}

		private static void WriteQuoted(StreamWriter writer, string text) {
			writer.Write('\"');
			writer.Write(text);
			writer.Write('\"');
		}

		private static void WriteCommaSeperateQuoted(StreamWriter writer, IEnumerable<string> items) {
			var enumerator = items.GetEnumerator();
			if(enumerator.MoveNext()) {
				WriteQuoted(writer, enumerator.Current);
				while(enumerator.MoveNext()) {
					writer.Write(',');
					WriteQuoted(writer, enumerator.Current);
				}
			}
		}

		private static readonly string[] HeaderItems = new[] { "Stamp", "Temperature", "Pressure", "Humidity", "Speed", "Direction" };

		public void WriteHeaderRow(StreamWriter csvWriter) {
			WriteCommaSeperateQuoted(csvWriter, HeaderItems);
			csvWriter.WriteLine();
		}

		private static IEnumerable<string> ToStringValues(IReading reading) {
			yield return reading.TimeStamp.ToString("yyyy/MM/dd HH:mm:ss");
			yield return reading.Temperature.ToString();
			yield return reading.Pressure.ToString();
			yield return reading.Humidity.ToString();
			yield return reading.WindSpeed.ToString();
			yield return reading.WindDirection.ToString();
		}

		private readonly StreamWriter _csvWriter;
		private readonly StreamWriter _iniWriter;
		private readonly string _csvFileName;
		private readonly bool _headerRow;

		private Action<IReading> _writeAction;

		public CsvDataFileWriter(StreamWriter csvWriter)
			: this(csvWriter, true) {}

		public CsvDataFileWriter(StreamWriter csvWriter, bool headerRow)
			: this(csvWriter, headerRow, null, null) { }

		public CsvDataFileWriter(StreamWriter csvWriter, StreamWriter iniWriter, string csvFileName)
			: this(csvWriter, true, iniWriter, csvFileName) { }

		public CsvDataFileWriter(StreamWriter csvWriter, bool headerRow, StreamWriter iniWriter, string csvFileName) {
			if(null == csvWriter)
				throw new ArgumentNullException("csvWriter");

			if((null != iniWriter) ^ (null != csvFileName))
				throw new ArgumentException("iniWriter and csvFileName must be both specified together.");

			_csvWriter = csvWriter;
			_iniWriter = iniWriter;
			_csvFileName = csvFileName;
			_headerRow = headerRow;
			_writeAction = InitAndWrite;
		}

		private void Init() {
			if (null != _iniWriter)
				WriteIni(_iniWriter, _csvFileName);
			if (_headerRow)
				WriteHeaderRow(_csvWriter);
		}

		public void Write(IReading reading) {
			_writeAction(reading);
		}

		private void WriteCore(IReading reading) {
			WriteCommaSeperateQuoted(_csvWriter, ToStringValues(reading));
			_csvWriter.WriteLine();
		}

		private void InitAndWrite(IReading reading) {
			Init();
			WriteCore(reading);
			_writeAction = WriteCore;
		}





	}

}
