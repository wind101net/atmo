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
using System.IO;

namespace Atmo.Data {
	public class DaqDataFileWriter : IDisposable {

		private Stream _stream;
		private readonly bool _ownsStream;
		private bool _isClosed;

		private int _recordsInThisChunk;
		private DateTime _chunkStamp;


		public DaqDataFileWriter(FileInfo fileInfo)
			: this(fileInfo.FullName) { }

		public DaqDataFileWriter(string filePath)
			: this(File.Open(filePath, FileMode.Create, FileAccess.Write, FileShare.None), true) { }

		/// <summary>
		/// Constructs a new DAQ file reader using the given <paramref name="stream"/>.
		/// </summary>
		/// <param name="stream">The stream to read DAQ data from.</param>
		public DaqDataFileWriter(Stream stream) : this(stream, false) { }

		private DaqDataFileWriter(Stream stream, bool ownsStream) {
			_stream = stream;
			_ownsStream = ownsStream;
			_isClosed = false;

			_recordsInThisChunk = 0;
			_chunkStamp = default(DateTime);
		}

		~DaqDataFileWriter() {
			Dispose(false);
		}

		/// <inheritdoc/>
		public void Dispose() {
			Dispose(true);
		}

		private void Dispose(bool disposing) {
			_isClosed = true;
			if(disposing)
				GC.SuppressFinalize(this);

			if (null != _stream && _ownsStream) {
				_stream.Flush();
				_stream.Dispose();
			}
			_stream = null;
		}

		public void Write(IReading reading) {
			Write(reading.TimeStamp, new PackedReadingValues(reading));
		}

		public void Write(Reading reading) {
			Write(reading.TimeStamp, new PackedReadingValues(reading));
		}

		public void Write(DateTime stamp, PackedReadingValues values) {
			if(_isClosed || null == _stream)
				throw new InvalidOperationException("Writer is not valid.");

			var data = new byte[DaqDataFileInfo.RecordSize];

			// do we needa new chunk?
			var needsAChunkHeader = false;
			var expectedStamp = _chunkStamp.Add(new TimeSpan(TimeSpan.TicksPerSecond * _recordsInThisChunk));
			if(expectedStamp != stamp)
				needsAChunkHeader = true;
			if (_recordsInThisChunk == 0)
				needsAChunkHeader = true;
			if (_recordsInThisChunk >= 255)
				needsAChunkHeader = true;

			// make a chunk if needed
			if(needsAChunkHeader) {
				DaqDataFileInfo.ConvertDateTimeTo7ByteForm(stamp, data, 0);
				data[DaqDataFileInfo.RecordSize - 1] = DaqDataFileInfo.HeaderCodeByte;
				_stream.Write(data, 0, data.Length);
				_chunkStamp = stamp;
				_recordsInThisChunk = 0;
			}

			// write the record
			PackedReadingValues.ToDeviceBytes(values, data, 0);
			_stream.Write(data, 0, data.Length);
			_recordsInThisChunk++;

		}

	}
}
