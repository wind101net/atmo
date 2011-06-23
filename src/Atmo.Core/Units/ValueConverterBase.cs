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

using System.Linq.Expressions;

namespace Atmo.Units {

	/// <summary>
	/// The base for a value converter implementation.
	/// </summary>
	/// <typeparam name="T">The unit type to convert.</typeparam>
	public abstract class ValueConverterBase<T> {

		/// <summary>
		/// Constructs a ValueConverterBase.
		/// </summary>
		/// <param name="from">The unit the converter converts from.</param>
		/// <param name="to">The unit the converter converts to.</param>
		protected ValueConverterBase(T from, T to) {
			From = from;
			To = to;
		}

		/// <summary>
		/// The unit the converter can convert from.
		/// </summary>
		public T From { get; private set; }
		/// <summary>
		/// The unit the converter can convert to.
		/// </summary>
		public T To { get; private set; }

		/// <summary>
		/// Converts a value from one unit to another.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted value.</returns>
		public abstract double Convert(double value);
		/// <summary>
		/// Generates an expression that will perform the conversion.
		/// </summary>
		/// <param name="input">The input to the conversion operation.</param>
		/// <returns>The conversion operation applied to the <paramref name="input"/>.</returns>
		public abstract Expression GetConversionExpression(Expression input);

	}

}
