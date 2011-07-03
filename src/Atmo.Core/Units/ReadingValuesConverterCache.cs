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

	public class ReadingValuesConverterCache<TValue>
		where TValue : IReadingValues
	{

		public static readonly ReadingValuesConverterCache<TValue> Default;
		public static readonly ValueConverterCache<TemperatureUnit> TemperatureCache;
		public static readonly ValueConverterCache<SpeedUnit> SpeedCache;
		public static readonly ValueConverterCache<PressureUnit> PressCache;

		static ReadingValuesConverterCache() {
			TemperatureCache = new ValueConverterCache<TemperatureUnit>(k => new TemperatureConverter(k.From, k.To));
			SpeedCache = new ValueConverterCache<SpeedUnit>(k => new SpeedConverter(k.From, k.To));
			PressCache = new ValueConverterCache<PressureUnit>(k => new PressureConverter(k.From, k.To));
			Default = new ReadingValuesConverterCache<TValue>(
				k => new ReadingValuesConverter<TValue>(
					TemperatureCache.Get(k.Temp) as TemperatureConverter,
					SpeedCache.Get(k.Speed) as SpeedConverter,
					PressCache.Get(k.Press) as PressureConverter
				)
			);
		}

		public struct Key : IEquatable<Key> {

			public readonly ValueConverterCache<TemperatureUnit>.Key Temp;
			public readonly ValueConverterCache<SpeedUnit>.Key Speed;
			public readonly ValueConverterCache<PressureUnit>.Key Press;

			public Key(ValueConverterCache<TemperatureUnit>.Key temp, ValueConverterCache<SpeedUnit>.Key speed, ValueConverterCache<PressureUnit>.Key press) {
				Temp = temp;
				Speed = speed;
				Press = press;
			}

			public bool Equals(Key other) {
				return Temp.Equals(other.Temp)
					&& Speed.Equals(other.Speed)
					&& Press.Equals(other.Press)
				;
			}

			public override bool Equals(object obj) {
				return obj is Key && Equals((Key)obj);
			}

			public override int GetHashCode() {
				return Temp.GetHashCode() ^ -(Speed.GetHashCode()) ^ Press.GetHashCode();
			}

			public override string ToString() {
				return String.Format("T ({0}) S ({1}) P ({2})", Temp, Speed, Press);
			}
		}

		private readonly Func<Key, ReadingValuesConverter<TValue>> _generator;
		private readonly Dictionary<Key, ReadingValuesConverter<TValue>> _cache;

		public ReadingValuesConverterCache(Func<Key, ReadingValuesConverter<TValue>> generator) {
			if (null == generator) {
				throw new ArgumentNullException("generator");
			}
			_generator = generator;
			_cache = new Dictionary<Key, ReadingValuesConverter<TValue>>();
		}

		public ReadingValuesConverter<TValue> Get(
			TemperatureUnit tempFrom, TemperatureUnit tempTo,
			SpeedUnit speedFrom, SpeedUnit speedTo,
			PressureUnit pressFrom, PressureUnit pressTo
		) {
			return Get(
				new ValueConverterCache<TemperatureUnit>.Key(tempFrom, tempTo),
				new ValueConverterCache<SpeedUnit>.Key(speedFrom, speedTo),
				new ValueConverterCache<PressureUnit>.Key(pressFrom, pressTo)
			);
		}

		public ReadingValuesConverter<TValue> Get(ValueConverterCache<TemperatureUnit>.Key temp, ValueConverterCache<SpeedUnit>.Key speed, ValueConverterCache<PressureUnit>.Key press) {
			return Get(new Key(temp, speed, press));
		}

		public ReadingValuesConverter<TValue> Get(Key key) {
			ReadingValuesConverter<TValue> converter;
			if (!_cache.TryGetValue(key, out converter)) {
				converter = _generator(key);
				_cache.Add(key, converter);
			}
			return converter;
		}

	}

	public class ReadingValuesConverterCache<TFrom, TTo>
		where TFrom : IReadingValues
		where TTo : IReadingValues {

		public static readonly ReadingValuesConverterCache<TFrom, TTo> Default;
		public static readonly ValueConverterCache<TemperatureUnit> TemperatureCache;
		public static readonly ValueConverterCache<SpeedUnit> SpeedCache;
		public static readonly ValueConverterCache<PressureUnit> PressCache;

		static ReadingValuesConverterCache() {
			TemperatureCache = new ValueConverterCache<TemperatureUnit>(k => new TemperatureConverter(k.From, k.To));
			SpeedCache = new ValueConverterCache<SpeedUnit>(k => new SpeedConverter(k.From, k.To));
			PressCache = new ValueConverterCache<PressureUnit>(k => new PressureConverter(k.From, k.To));
			Default = new ReadingValuesConverterCache<TFrom, TTo>(
				k => new ReadingValuesConverter<TFrom, TTo>(
					TemperatureCache.Get(k.Temp) as TemperatureConverter,
					SpeedCache.Get(k.Speed) as SpeedConverter,
					PressCache.Get(k.Press) as PressureConverter
				)
			);
		}

		public struct Key : IEquatable<Key> {

			public readonly ValueConverterCache<TemperatureUnit>.Key Temp;
			public readonly ValueConverterCache<SpeedUnit>.Key Speed;
			public readonly ValueConverterCache<PressureUnit>.Key Press;

			public Key(ValueConverterCache<TemperatureUnit>.Key temp, ValueConverterCache<SpeedUnit>.Key speed, ValueConverterCache<PressureUnit>.Key press) {
				Temp = temp;
				Speed = speed;
				Press = press;
			}

			public bool Equals(Key other) {
				return Temp.Equals(other.Temp)
					&& Speed.Equals(other.Speed)
					&& Press.Equals(other.Press)
				;
			}

			public override bool Equals(object obj) {
				return obj is Key && Equals((Key)obj);
			}

			public override int GetHashCode() {
				return Temp.GetHashCode() ^ -(Speed.GetHashCode()) ^ Press.GetHashCode();
			}

			public override string ToString() {
				return String.Format("T ({0}) S ({1}) P ({2})", Temp, Speed, Press);
			}
		}

		private readonly Func<Key, ReadingValuesConverter<TFrom, TTo>> _generator;
		private readonly Dictionary<Key, ReadingValuesConverter<TFrom, TTo>> _cache;

		public ReadingValuesConverterCache(Func<Key, ReadingValuesConverter<TFrom, TTo>> generator) {
			if(null == generator) {
				throw new ArgumentNullException("generator");
			}
			_generator = generator;
			_cache = new Dictionary<Key, ReadingValuesConverter<TFrom, TTo>>();
		}

		public ReadingValuesConverter<TFrom, TTo> Get(
			TemperatureUnit tempFrom, TemperatureUnit tempTo,
			SpeedUnit speedFrom, SpeedUnit speedTo,
			PressureUnit pressFrom, PressureUnit pressTo
		) {
			return Get(
				new ValueConverterCache<TemperatureUnit>.Key(tempFrom, tempTo),
				new ValueConverterCache<SpeedUnit>.Key(speedFrom, speedTo),
				new ValueConverterCache<PressureUnit>.Key(pressFrom, pressTo)
			);
		}

		public ReadingValuesConverter<TFrom, TTo> Get(ValueConverterCache<TemperatureUnit>.Key temp, ValueConverterCache<SpeedUnit>.Key speed, ValueConverterCache<PressureUnit>.Key press) {
			return Get(new Key(temp, speed, press));
		}

		public ReadingValuesConverter<TFrom, TTo> Get(Key key) {
			ReadingValuesConverter<TFrom, TTo> converter;
			if (!_cache.TryGetValue(key, out converter)) {
				converter = _generator(key);
				_cache.Add(key, converter);
			}
			return converter;
		}

	}
}
