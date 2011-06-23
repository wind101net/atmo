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
	public class PressureConverter : ValueConverterBase<PressureUnit> {

		private static Expression CreateExpression(Expression input, PressureUnit from, PressureUnit to) {
			if (from == to) {
				return input;
			}
			switch (from) {
			case PressureUnit.InchOfMercury:
				switch (to) {
				case PressureUnit.KiloPascals:
					return Expression.Multiply(input, Expression.Constant(3.38600));
				case PressureUnit.Pascals:
					return Expression.Multiply(input, Expression.Constant(3386));
				case PressureUnit.Millibar:
					return Expression.Multiply(input, Expression.Constant(33.86));
				default:
					throw new ArgumentOutOfRangeException("to");
				}
			case PressureUnit.KiloPascals:
				switch (to) {
				case PressureUnit.InchOfMercury:
					return Expression.Multiply(input,Expression.Constant(0.295333727));
				case PressureUnit.Pascals:
					return Expression.Multiply(input,Expression.Constant(1000.0));
				case PressureUnit.Millibar:
					return Expression.Multiply(input, Expression.Constant(10.0));
				default:
					throw new ArgumentOutOfRangeException("to");
				}
			case PressureUnit.Pascals:
				switch (to) {
				case PressureUnit.InchOfMercury:
					return Expression.Multiply(input, Expression.Constant(0.295333727/1000.0));
				case PressureUnit.KiloPascals:
					return Expression.Divide(input,Expression.Constant(1000.0));
				case PressureUnit.Millibar:
					return Expression.Multiply(input, Expression.Constant(10.0/1000.0));
				default:
					throw new ArgumentOutOfRangeException("to");
				}
			case PressureUnit.Millibar:
				switch (to) {
				case PressureUnit.InchOfMercury:
					return Expression.Multiply(input, Expression.Constant(0.0295333727));
				case PressureUnit.Pascals:
					return Expression.Multiply(input, Expression.Constant(0.1 * 1000.0));
				case PressureUnit.KiloPascals:
					return Expression.Multiply(input, Expression.Constant(0.1));
				default:
					throw new ArgumentOutOfRangeException("to");
				}
			default:
				throw new ArgumentOutOfRangeException("from");
			}
		}

		public readonly Func<double, double> Conversion;

		public PressureConverter(PressureUnit from, PressureUnit to)
			: base(from,to)
		{
			var valueIn = Expression.Parameter(typeof(double), "value");
			Conversion = Expression.Lambda<Func<double, double>>(CreateExpression(valueIn, From, To), valueIn).Compile();
		}

		public override double Convert(double value) {
			return Conversion(value);
		}

		public override Expression GetConversionExpression(Expression input) {
			return CreateExpression(input, From, To);
		}
	}
}
