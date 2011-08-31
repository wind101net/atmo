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

using System.Linq;

namespace Atmo.Device {

	/// <summary>
	/// A collection of memory regions.
	/// </summary>
	public class MemoryRegionDataCollection : System.Collections.ObjectModel.KeyedCollection<long, MemoryRegionData> {

		public void Union(long address, byte[] data) {

			var left = FindLeftRegion(address);
			if (null == left) {
				left = new MemoryRegionData(address, data);
				Add(left);
			}
			else {
				left.Data.AddRange(data);
			}
			long nextAddress = left.Address + left.Size;
			var right = this.FirstOrDefault(mrd => nextAddress == mrd.Address);
			if (null != right) {
				Remove(right);
				left.Data.AddRange(right.Data);
			}
		}

		private MemoryRegionData FindLeftRegion(long address) {
			foreach (var mrd in this) {
				if ((mrd.Address + mrd.Size) == address) {
					return mrd;
				}
			}
			return null;
		}

		protected override long GetKeyForItem(MemoryRegionData item) {
			return item.Address;
		}

	}
}