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

using System.IO;
using System;

namespace Atmo.Data {
	
	/// <summary>
	/// Reads a DAQ file.
	/// </summary>
	public class DaqDataFileReader : IDisposable {

		private const int LastRecordIndex = DaqDataFileInfo.RecordSize - 1;

		private Stream _stream;
		private readonly bool _ownsStream;
		private bool _isClosed;
		private DateTime _currentBaseTime;
		private bool _timeBaseFromReadAhead;
		private int _chunkRecordCounter;
		private PackedReading _current;

		public DaqDataFileReader(FileInfo fileInfo)
			: this(fileInfo.FullName) { }

		public DaqDataFileReader(string filePath)
			: this(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), true) { }

		/// <summary>
		/// Constructs a new DAQ file reader using the given <paramref name="stream"/>.
		/// </summary>
		/// <param name="stream">The stream to read DAQ data from.</param>
		public DaqDataFileReader(Stream stream) : this(stream, false) { }

		private DaqDataFileReader(Stream stream, bool ownsStream) {
			_stream = stream;
			_ownsStream = ownsStream;
			_isClosed = false;
			_currentBaseTime = default(DateTime);
			_current = default(PackedReading);
			_timeBaseFromReadAhead = false;
			_chunkRecordCounter = 0;
			if(null == _stream) {
				throw new ArgumentNullException("stream");
			}
		}

		~DaqDataFileReader() {
			Dispose(false);
		}

		/// <inheritdoc/>
		public void Dispose() {
			Dispose(true);
		}

		private void Dispose(bool disposing) {
			_isClosed = true;
			if(disposing) {
				GC.SuppressFinalize(this);
			}
			if(null != _stream && _ownsStream) {
				_stream.Dispose();
			}
			_stream = null;
		}

		/// <summary>
		/// The current reading.
		/// </summary>
		public PackedReading Current {
			get {
				return _current;
			}
		}

		/// <summary>
		/// Reads the first header encountered with a valid date-time stamp.
		/// </summary>
		/// <param name="firstValidStamp">The date-time that was encountered.</param>
		/// <returns>the number of records before the first valid date-time stamp was encountered.</returns>
		/// <remarks>
		/// When no valid header is found the method will return -1 and <paramref name="firstValidStamp"/> will be set to default(DateTime).
		/// </remarks>
		public int ReadNextValidStamp(out DateTime firstValidStamp) {

			// TODO: if the stream is not at a multiple of 8, scoot it back
			if (_isClosed || _stream.Position >= _stream.Length) {
				_isClosed = true;
				firstValidStamp = default(DateTime);
				return -1;
			}
			int recordCount = 0;
			var headerBuffer = new byte[DaqDataFileInfo.RecordSize];
			while(DaqDataFileInfo.RecordSize == _stream.Read(headerBuffer, 0, DaqDataFileInfo.RecordSize)) {
				if (DaqDataFileInfo.HeaderCodeByte == headerBuffer[LastRecordIndex]) {
					if(DaqDataFileInfo.TryConvert7ByteDateTime(headerBuffer, 0, out firstValidStamp)) {
						return recordCount;
					}
				}else {
					recordCount++;
				}
			}
			firstValidStamp = default(DateTime);
			return -1;
		}

		/// <summary>
		/// Moves to the next record in the DAQ file.
		/// </summary>
		/// <returns>true when the move was successful.</returns>
		public bool MoveNext() {
			if (_isClosed || _stream.Position >= _stream.Length) {
				_isClosed = true;
				return false;
			}
			var recordData = new byte[DaqDataFileInfo.RecordSize];
			while(true) {
				if (_stream.Position >= _stream.Length ||
				    DaqDataFileInfo.RecordSize != _stream.Read(recordData, 0, DaqDataFileInfo.RecordSize)) {
					_current = new PackedReading();
					_isClosed = true;
					return false;
				}
				
				if (DaqDataFileInfo.HeaderCodeByte == recordData[DaqDataFileInfo.RecordSize - 1]) {
					// a header record
					if (DaqDataFileInfo.TryConvert7ByteDateTime(recordData, 0, out _currentBaseTime)) {
						_timeBaseFromReadAhead = false;
						_chunkRecordCounter = 0;
					}
					else {
						// invalid time stamp
						if (!_timeBaseFromReadAhead) {
							// if we havnt already read ahead for a good stamp we
							// need to read ahead to get the next valid time stamp
							var positionRestore = _stream.Position;
							var dataRecordsRead = ReadNextValidStamp(out _currentBaseTime);
							if (dataRecordsRead < 0) {
								return false; // failed to find any more time stamp records, quit
							}
							// skipped over some data records
							if (0 != dataRecordsRead) {
								// need to adjust the stamp we got for those records
								_currentBaseTime = _currentBaseTime.Subtract(
									new TimeSpan(0, 0, dataRecordsRead)
									);
								// also need to put the stream back how found it
								_stream.Seek(positionRestore, SeekOrigin.Begin);
							}
							// this is the start of a new "virtual" chunk
							_chunkRecordCounter = 0;
							// virtual because invalid headers immediately following this will be ignored
							_timeBaseFromReadAhead = true;
						}
						else {
							; // just skip it, pretend it doesen't exist because we already read ahead
						}
					}
				}
				else {
					// a data record
					var values = PackedReadingValues.FromDeviceBytes(recordData, 0);
					if (!values.IsValid) {
						_chunkRecordCounter++;
						continue;
					}
					_current = new PackedReading(
						_currentBaseTime.Add(new TimeSpan(0, 0, _chunkRecordCounter)),
						values
						);
					_chunkRecordCounter++;
					return true;
				}
				
			}
		}
	}

}
