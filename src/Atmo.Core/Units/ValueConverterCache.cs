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
using System.Collections.Generic;

namespace Atmo.Units {
	public class ValueConverterCache<T> {

		public struct Key : IEquatable<Key> {
			public readonly T From;
			public readonly T To;

			public Key(T from, T to) {
				From = from;
				To = to;
			}

			public bool Equals(Key other) {
				return From.Equals(other.From)
					&& To.Equals(other.To);
			}

			public override bool Equals(object obj) {
				return obj is Key && Equals((Key) obj);
			}

			public override int GetHashCode() {
				return From.GetHashCode();
			}

			public override string ToString() {
				return String.Format("From {0} To {1}", From, To);
			}
		}

		private readonly Func<Key, ValueConverterBase<T>> _generator;
		private readonly Dictionary<Key, ValueConverterBase<T>> _cache;

		public ValueConverterCache(Func<Key, ValueConverterBase<T>> generator) {
			if(null == generator) {
				throw new ArgumentNullException("generator");
			}
			_generator = generator;
			_cache = new Dictionary<Key, ValueConverterBase<T>>();
		}

		public ValueConverterBase<T> Get(T from, T to) {
			return Get(new Key(from, to));
		}

		public ValueConverterBase<T> Get(Key key) {
			ValueConverterBase<T> converter;
			if (!_cache.TryGetValue(key, out converter)) {
				converter = _generator(key);
				_cache.Add(key, converter);
			}
			return converter;
		}

	}
}
