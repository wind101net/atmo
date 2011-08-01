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

namespace Atmo.Device {
	public static class HexParser {

		public static MemoryRegionDataCollection ParseToMemoryRegions(TextReader reader) {
			string currentLine = null;
			long currentExtendedOffset = 0;
			bool eofReached = false;

			var memoryRegions = new MemoryRegionDataCollection();

			while (!String.IsNullOrEmpty(currentLine = reader.ReadLine())) {
				if (currentLine[0] != ':') {
					throw new InvalidDataException("Hex line does not begin with ':'. File is invalid or corrupted.");
				}
				byte recordType = Convert.ToByte(currentLine.Substring(7, 2), 16);

				if (recordType == 0x04 || recordType == 0x00) {

					byte recordLength = Convert.ToByte(currentLine.Substring(1, 2), 16);
					ushort address = Convert.ToUInt16(currentLine.Substring(3, 4), 16);

					uint checkSum = (uint)(recordLength + (address & 0xff) + (0xff & (address >> 8)) + recordType);
					byte[] data = new byte[recordLength];
					for (int i = 0; i < data.Length; i++) {
						byte value = Convert.ToByte(currentLine.Substring(9 + (2 * i), 2), 16);
						checkSum += value;
						data[i] = value;
					}
					checkSum = ~checkSum + 1;

					byte expectedCheckSum = Convert.ToByte(currentLine.Substring(9 + (data.Length * 2), 2), 16);

					if (expectedCheckSum != (checkSum & 0xff)) {
						throw new InvalidDataException("Checksum mismatch. File is invalid or corrupted.");
					}

					if (recordType == 0x04) {
						currentExtendedOffset = ((long)BitConverter.ToUInt16(data, 0) << 16);
					}
					else {
						long chunkAddress = currentExtendedOffset + address;
						memoryRegions.Union(chunkAddress, data);
					}
				}
				else if (recordType == 0x01) {
					eofReached = true;
					byte recordLength = Convert.ToByte(currentLine.Substring(1, 2), 16);
					ushort address = Convert.ToUInt16(currentLine.Substring(3, 4), 16);
					byte expectedCheckSum = Convert.ToByte(currentLine.Substring(9, 2), 16);
					if (recordLength != 0 || address != 0 || expectedCheckSum != 0xff) {
						throw new InvalidDataException("Hex EOF corrupted.");
					}
					break;
				}
				else {
					;//throw new NotSupportedException(String.Concat("Hex parser does not support records of type 0x", recordType.ToString("X2")));
				}
			}

			if (!eofReached) {
				throw new InvalidDataException("No EOF record found. Hex data may be invalid or corrupted.");
			}

			return memoryRegions;

		}

	}
}
