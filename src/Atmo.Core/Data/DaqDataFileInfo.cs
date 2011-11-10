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
using System.Text.RegularExpressions;

namespace Atmo.Data {

	/// <summary>
	/// Information about DAQ data files.
	/// </summary>
	/// <remarks>
	/// This class may take the form of a static helper class.
	/// </remarks>
	public class DaqDataFileInfo {

		public static readonly Regex AnemFileNameRegex = new Regex(@"([A-D])(\d\d)(\d\d)(\d\d)[.]DAT", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		internal const int RecordSize = 8;
		internal const byte HeaderCodeByte = 0xa5;

		/// <summary>
		/// Converts 7 bytes of data as found in a DAQ file into a date time.
		/// </summary>
		/// <param name="data">The raw data.</param>
		/// <param name="offset">The offset within the raw data.</param>
		/// <param name="stamp">The result.</param>
		/// <returns>True if successful.</returns>
		public static bool TryConvert7ByteDateTime(byte[] data, int offset, out DateTime stamp) {

			// TODO: test for an invalid date that falls within the ranges.
			if (data.Length - offset >= 7) {
				var year = (short)((data[offset] << 8) | data[offset + 1]);
				var month = data[offset + 2];
				var day = data[offset + 3];
				var hour = data[offset + 4];
				var min = data[offset + 5];
				var sec = data[offset + 6];
				if (
					year > 0
					&& month <= 12
					&& day <= 31
					&& hour <= 24
					&& min <= 59
					&& sec <= 59
					&& 0 != year
					&& 0 != month
					&& 0 != day
				) {
					try {
						stamp = new DateTime(year, month, day, hour, min, sec);
						return true;
					}
					catch {;}
				}
			}
			stamp = default(DateTime);
			return false;
		}

		public static byte[] ConvertDateTimeTo7ByteForm(DateTime stamp) {
			var dateChunk = new byte[7];
			ConvertDateTimeTo7ByteForm(stamp, dateChunk, 0);
			return dateChunk;
		}

		public static void ConvertDateTimeTo7ByteForm(DateTime stamp, byte[] data, int offset) {
			var year = (short)stamp.Year;
			data[offset] = checked((byte)(year >> 8)); // if it is the year 65535 we should probably fail now
			data[offset + 1] = unchecked((byte)(year));
			data[offset + 2] = unchecked((byte)stamp.Month);
			data[offset + 3] = unchecked((byte)stamp.Day);
			data[offset + 4] = unchecked((byte)stamp.Hour);
			data[offset + 5] = unchecked((byte)stamp.Minute);
			data[offset + 6] = unchecked((byte)stamp.Second);
		}

		public static bool TryConvertFrom7ByteDateTimeForce(byte[] data, int offset, out DateTime stamp) {
			try {
				if (data.Length - offset >= 7) {
					var year = Math.Max((short)0,(short)((data[offset] << 8) | data[offset + 1]));
					var month =Math.Max((byte)1, Math.Min((byte)12,data[offset + 2]));
					var day = Math.Max((byte)1,Math.Min((byte)31,data[offset + 3]));
					var hour = Math.Min((byte)23,data[offset + 4]);
					var min = Math.Min((byte)59,data[offset + 5]);
					var sec = Math.Min((byte)59,data[offset + 6]);
					while (day > 0) {
						try {
							stamp = new DateTime(year, month, day, hour, min, sec);
							return true;
						}catch {
							day--;
							if(day < 28) {
								break;
							}
						}
					}
				}
			}
			catch {;}
			stamp = default(DateTime);
			return false;
		}

		public static DaqDataFileInfo Create(FileInfo fileInfo) {
			int nid = 0;

			string idValue = AnemFileNameRegex.Match(fileInfo.Name).Groups[1].Value;
			if (String.IsNullOrEmpty(idValue) || 1 != idValue.Length) {
				return null;
			}
			
			char idChar = Char.ToUpperInvariant(idValue[0]);
			if (idChar >= 'A' && idChar <= 'Z') {
				nid = idChar - 'A';
			}
			else {
				return null;
			}

			try {
				using (FileStream fs = File.Open(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
					DateTime stamp;
					int recordsPassed = -1;
					using (var reader = new DaqDataFileReader(fs)) {
						recordsPassed = reader.ReadNextValidStamp(out stamp);
					}
					if (recordsPassed >= 0) {
						if (0 != recordsPassed) {
							stamp = stamp.Subtract(new TimeSpan(0, 0, recordsPassed));
						}
						return new DaqDataFileInfo(fileInfo, nid, stamp);
					}
					return null;
				}
			}
			catch (IOException ioEx) {
				return null;
			}
		}

		private readonly FileInfo _fileInfo;
        private int _nid;
        private DateTime _firstStamp;

		private DaqDataFileInfo(FileInfo fileInfo, int nid, DateTime firstStamp) {
            if (null == fileInfo) {
                throw new ArgumentNullException("fileInfo");
            }
            _fileInfo = fileInfo;
            _nid = nid;
            _firstStamp = firstStamp;
        }

        public int Nid {
            get { return _nid; }
        }

        public DateTime FirstStamp {
            get { return _firstStamp; }
        }

        public FileInfo Path {
            get { return _fileInfo; }
        }

        public override string ToString() {
            return String.Concat(_fileInfo.Name, " (", _nid, ',', _firstStamp, ')');
        }

		public DaqDataFileReader CreateReader() {
			return new DaqDataFileReader(_fileInfo);
		}

	}
}
