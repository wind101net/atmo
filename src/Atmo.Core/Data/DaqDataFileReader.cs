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
	
	public class DaqDataFileReader : IDisposable {

		private const int LastRecordIndex = DaqDataFileInfo.RecordSize - 1;

		private Stream _stream;
		private readonly bool _ownsStream;

		public DaqDataFileReader(Stream stream) : this(stream, false) { }

		private DaqDataFileReader(Stream stream, bool ownsStream) {
			_stream = stream;
			_ownsStream = ownsStream;
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
			if(disposing) {
				GC.SuppressFinalize(this);
			}
			if(null != _stream && _ownsStream) {
				_stream.Dispose();
			}
			_stream = null;
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

			int recordCount = 0;
			var headerBuffer = new byte[DaqDataFileInfo.RecordSize];
			while(DaqDataFileInfo.RecordSize == _stream.Read(headerBuffer, 0, DaqDataFileInfo.RecordSize)) {
				if (DaqDataFileInfo.HeaderCodeByte == headerBuffer[LastRecordIndex]) {
					if(DaqDataFileInfo.TryConvert7byteDateTime(headerBuffer,0,out firstValidStamp)) {
						return recordCount;
					}
				}else {
					recordCount++;
				}
			}
			firstValidStamp = default(DateTime);
			return -1;
		}
	}

}
