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
using System.Linq;
using System.Linq.Expressions;
using Atmo.Stats;

namespace Atmo.Units {

	public class ReadingValuesConverter<TValue> : ReadingValuesConverter<TValue,TValue>
		where TValue : IReadingValues
	{

		public ReadingValuesConverter(
			TemperatureConverter temperatureConverter,
			SpeedConverter speedConverter,
			PressureConverter pressureConverter
		) : base(temperatureConverter,speedConverter,pressureConverter) { }

		public void ConvertInline(List<TValue> values) {
			if(null == TemperatureConverter && null == SpeedConverter && null == PressureConverter) {
				return;
			}
			if(typeof(TValue).IsSubclassOf(typeof(ReadingValues))) {
				for(var i = 0; i < values.Count; i++) {
					var target = values[i] as ReadingValues;
					if (null != TemperatureConverter) {
						target.Temperature = TemperatureConverter.Conversion(target.Temperature);
					}
					if (null != PressureConverter) {
						target.Pressure = PressureConverter.Conversion(target.Pressure);
					}
					if (null != SpeedConverter) {
						target.WindSpeed = SpeedConverter.Conversion(target.WindSpeed);
					}
				}
				return;
			}
			for(var i = 0; i < values.Count; i++) {
				values[i] = Conversion(values[i]);
			}
		}

		public void ConvertInline(ref TValue value) {
			if (null == TemperatureConverter && null == SpeedConverter && null == PressureConverter) {
				return;
			}
			if (value is ReadingValues) {
				var target = value as ReadingValues;
				if (null != TemperatureConverter) {
					target.Temperature = TemperatureConverter.Conversion(target.Temperature);
				}
				if (null != PressureConverter) {
					target.Pressure = PressureConverter.Conversion(target.Pressure);
				}
				if (null != SpeedConverter) {
					target.WindSpeed = SpeedConverter.Conversion(target.WindSpeed);
				}
				return;
			}
			value = Conversion(value);			
		}

	}

	public class ReadingValuesConverter<TFrom, TTo>
		where TFrom : IReadingValues
		where TTo : IReadingValues
	{

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

			try {
				return Expression.New(
					typeof (TTo).GetConstructor(Enumerable.Repeat(typeof (double), 5).ToArray()),
					tempExp,
					pressExp,
					humExp,
					dirExp,
					speedExp
					);
			}catch {
				return null;
			}

		}

		public readonly Func<TFrom, TTo> Conversion;

		public ReadingValuesConverter(
			TemperatureConverter temperatureConverter,
			SpeedConverter speedConverter,
			PressureConverter pressureConverter
		) {
			TemperatureConverter = (null == temperatureConverter || temperatureConverter.From == temperatureConverter.To) ? null : temperatureConverter;
			SpeedConverter = (null == speedConverter || speedConverter.From == speedConverter.To) ? null : speedConverter;
			PressureConverter = (null == pressureConverter || pressureConverter.From == pressureConverter.To) ? null : pressureConverter;
			var recordIn = Expression.Parameter(typeof(TFrom), "record");
			var exp = CreateExpression(recordIn, TemperatureConverter, SpeedConverter, PressureConverter);
			if (null != exp) {
				Conversion = Expression.Lambda<Func<TFrom, TTo>>(exp, recordIn).Compile();
			}
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
