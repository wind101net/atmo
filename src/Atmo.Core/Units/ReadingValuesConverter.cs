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
using System.Linq;
using System.Linq.Expressions;

namespace Atmo.Units {
	public class ReadingValuesConverter<TFrom, TTo>
		where TFrom : IReadingValues
		where TTo : IReadingValues {

		private static Expression CreateExpression(
			Expression input,
			ValueConverterBase<TemperatureUnit> temperatureConverter,
			ValueConverterBase<SpeedUnit> speedConverter,
			ValueConverterBase<PressureUnit> pressureConverter
		) {
			if(null == temperatureConverter && null == speedConverter && null == pressureConverter) {
				if (typeof(TFrom) == typeof(TTo)) {
					return input;
				}
				return Expression.New(
					typeof(TTo).GetConstructor(new []{ typeof(TFrom) })
					?? typeof(TTo).GetConstructor(new []{ typeof(IReadingValues) }),
					input
				);
			}
			Expression tempExp = Expression.Property(input, "Temperature");
			Expression pressExp = Expression.Property(input, "Pressure");
			Expression humExp = Expression.Property(input, "Humidity");
			Expression speedExp = Expression.Property(input, "WindSpeed");
			Expression dirExp = Expression.Property(input, "WindDirection");
			if(null != temperatureConverter) {
				tempExp = temperatureConverter.GetConversionExpression(tempExp);
			}
			if(null != speedConverter) {
				speedExp = speedConverter.GetConversionExpression(speedExp);
			}
			if(null != pressureConverter) {
				pressExp = pressureConverter.GetConversionExpression(pressExp);
			}

			return Expression.New(
				typeof(TTo).GetConstructor(Enumerable.Repeat(typeof(double), 5).ToArray()),
				tempExp,
				pressExp,
				humExp,
				dirExp,
				speedExp
			);
		}

		public readonly Func<TFrom, TTo> Conversion;

		public ReadingValuesConverter(
			TemperatureConverter temperatureConverter,
			SpeedConverter speedConverter,
			PressureConverter pressureConverter
		) {
			// TOOD: rebuild conversion function
			TemperatureConverter = temperatureConverter;
			SpeedConverter = speedConverter;
			PressureConverter = pressureConverter;
			var recordIn = Expression.Parameter(typeof(TFrom), "record");
			Conversion = Expression.Lambda<Func<TFrom, TTo>>(CreateExpression(recordIn, TemperatureConverter, SpeedConverter, PressureConverter), recordIn).Compile();
		}

		public TemperatureConverter TemperatureConverter { get; private set; }
		public SpeedConverter SpeedConverter { get; private set; }
		public PressureConverter PressureConverter { get; private set; }

		public TTo Convert(TFrom value) {
			return Conversion(value);
		}

		public Expression GetConversionExpression(Expression input) {
			return CreateExpression(input, TemperatureConverter, SpeedConverter, PressureConverter);
		}

	}
}
