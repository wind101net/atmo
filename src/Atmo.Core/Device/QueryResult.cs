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

using System.Collections.ObjectModel;

namespace Atmo.Device {
	public class QueryResult : KeyedCollection<long, MemoryRegionInfo> {

		public static readonly int MaxRegions = 6;

		public QueryResult()
			: base() {
			BytesPerPacket = 0;
		}

		public byte BytesPerPacket { get; set; }

		protected override long GetKeyForItem(MemoryRegionInfo item) {
			return item.Address;
		}

	}
}
