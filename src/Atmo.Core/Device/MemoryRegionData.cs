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

using System.Collections.Generic;

namespace Atmo.Device {

	/// <summary>
	/// Represents a region of memory on a device that has or can store data.
	/// </summary>
	public class MemoryRegionData {

		public long Address;
		public List<byte> Data;

		public MemoryRegionData() : this(0) { }

		public MemoryRegionData(long address) : this(address, null) { }

		public MemoryRegionData(long address, IEnumerable<byte> data) {
			Address = address;
			Data = null == data ? new List<byte>() : new List<byte>(data);
		}

		public long Size {
			get {
				return null == Data ? 0 : Data.Count;
			}
		}

		public long LastAddress {
			get {
				return Address + Size - 1;
			}
		}

	}
}
