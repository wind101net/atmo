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
using System.Linq.Expressions;

namespace Atmo.Units {
	public class TemperatureConverter : ValueConverterBase<TemperatureUnit> {

		private static Expression CreateExpression(Expression input, TemperatureUnit from, TemperatureUnit to) {
			if (from == to) {
				return input;
			}
			else if (from == TemperatureUnit.Celsius) {
				return Expression.Add(
					Expression.Multiply(input, Expression.Constant(1.8)),
					Expression.Constant(32.0)
				);
			}
			else {
				return Expression.Divide(
					Expression.Subtract(input, Expression.Constant(32.0)),
					Expression.Constant(1.8)
				);
			}
		}

		public readonly Func<double, double> Conversion;

		public TemperatureConverter(TemperatureUnit from, TemperatureUnit to)
			: base(from,to)
		{
			var valueIn = Expression.Parameter(typeof(double), "value");
			Conversion = Expression.Lambda<Func<double, double>>(CreateExpression(valueIn, From, To), valueIn).Compile();
		}

		/// <inheritdoc/>
		public override double Convert(double value) {
			return Conversion(value);
		}

		/// <inheritdoc/>
		public override Expression GetConversionExpression(Expression input) {
			return CreateExpression(input, From, To);
		}
	}
}
