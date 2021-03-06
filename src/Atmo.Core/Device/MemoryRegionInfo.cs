﻿// ================================================================================
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

namespace Atmo.Device {

	/// <summary>
	/// Attributes that define a chunk of memory.
	/// </summary>
	public class MemoryRegionInfo {

		public byte TypeFlag;
		public long Address;
		public long Length;

		public MemoryRegionInfo(byte typeFlag, long address, long length) {
			TypeFlag = typeFlag;
			Address = address;
			Length = length;
		}

	}
}
