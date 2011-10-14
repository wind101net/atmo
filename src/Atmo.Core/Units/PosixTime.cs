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

namespace Atmo.Units {

	/// <summary>
	/// A 32bit signed posix formated time.
	/// </summary>
	public struct PosixTime :
		IConvertible,
		IEquatable<PosixTime>,
		IEquatable<int>,
		IEquatable<DateTime>,
		IComparable<PosixTime>
	{

		public static bool operator > (PosixTime a, PosixTime b) {
			return a.Value > b.Value;
		}

		public static bool operator < (PosixTime a, PosixTime b) {
			return a.Value < b.Value;
		}

		public static bool operator ==(PosixTime a, PosixTime b) {
			return a.Value == b.Value;
		}

		public static bool operator !=(PosixTime a, PosixTime b) {
			return a.Value != b.Value;
		}

		public static implicit operator int(PosixTime t) {
			return t.Value;
		}

		public static implicit operator PosixTime(int i) {
			return new PosixTime(i);
		}

		public static implicit operator DateTime(PosixTime t) {
			return t.AsDateTime;
		}

		public static implicit operator PosixTime(DateTime t) {
			return new PosixTime(t);
		}

		public readonly int Value;

		public PosixTime(int value) {
			Value = value;
		}

		public PosixTime(DateTime dateTime) {
			Value = UnitUtility.ConvertToPosixTime(dateTime);
		}

		public DateTime AsDateTime { get { return UnitUtility.ConvertFromPosixTime(Value); } }

		public TypeCode GetTypeCode() {
			return TypeCode.DateTime;
		}

		public bool ToBoolean(IFormatProvider provider) {
			return Convert.ToBoolean(ToInt32(provider), provider);
		}

		public byte ToByte(IFormatProvider provider) {
			return Convert.ToByte(ToInt32(provider), provider);
		}

		public char ToChar(IFormatProvider provider) {
			return Convert.ToChar(ToInt32(provider), provider);
		}

		public DateTime ToDateTime(IFormatProvider provider) {
			return AsDateTime;
		}

		public decimal ToDecimal(IFormatProvider provider) {
			return Convert.ToDecimal(ToInt32(provider), provider);
		}

		public double ToDouble(IFormatProvider provider) {
			return Convert.ToDouble(ToInt32(provider), provider);
		}

		public short ToInt16(IFormatProvider provider) {
			return Convert.ToInt16(ToInt32(provider), provider);
		}

		public int ToInt32(IFormatProvider provider) {
			return Value;
		}

		public long ToInt64(IFormatProvider provider) {
			return Value;
		}

		public sbyte ToSByte(IFormatProvider provider) {
			return Convert.ToSByte(ToInt32(provider), provider);
		}

		public float ToSingle(IFormatProvider provider) {
			return Convert.ToSingle(ToInt32(provider), provider);
		}

		public string ToString(IFormatProvider provider) {
			return Convert.ToString(ToInt32(provider), provider);
		}

		public object ToType(Type conversionType, IFormatProvider provider) {
			return Convert.ChangeType(ToInt32(provider), conversionType);
		}

		public ushort ToUInt16(IFormatProvider provider) {
			return Convert.ToUInt16(ToInt32(provider), provider);
		}

		public uint ToUInt32(IFormatProvider provider) {
			return Convert.ToUInt32(ToInt32(provider), provider);
		}

		public ulong ToUInt64(IFormatProvider provider) {
			return Convert.ToUInt64(ToInt32(provider), provider);
		}

		public bool Equals(PosixTime other) {
			return Value == other.Value;
		}

		public bool Equals(int other) {
			return Value == other;
		}

		public bool Equals(DateTime other) {
			return AsDateTime == other;
		}

		public int CompareTo(PosixTime other) {
			return Value.CompareTo(other.Value);
		}

		public override bool Equals(object obj) {
			return obj is PosixTime
				? Equals((PosixTime)obj)
				: obj is DateTime
				? Equals((DateTime)obj)
				: obj is int
				? Equals((int)obj)
				: false;
		}

		public override int GetHashCode() {
			return Value ^ -92837840;
		}

		public override string ToString() {
			return AsDateTime.ToString();
		}
	}
}
