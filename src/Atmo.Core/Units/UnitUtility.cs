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
using System.Text;

namespace Atmo.Units {
	public static class UnitUtility {
		
		public static string TimeSpanLabelString(TimeSpan span) {
			var builder = new StringBuilder();
			// year/week/day
			if(span.Days >= 365) {
				if(0 == span.Days % 365) {
					builder.AppendFormat("{0} Year{1} ", span.Days/365, span.Days == 365 ? String.Empty : "s");
				}else {
					builder.AppendFormat("{0} Days ", span.Days);
				}
			}else if(span.Days >= 7) {
				if(0 == span.Days % 7) {
					builder.AppendFormat("{0} Week{1} ", span.Days / 7, span.Days == 7 ? String.Empty : "s");
				}else {
					builder.AppendFormat("{0} Days ", span.Days);
				}
			}else if(span.Days > 0) {
				builder.AppendFormat("{0} Day{1} ", span.Days, span.Days == 1 ? String.Empty : "s");
			}
			// hour
			if(0 != span.Hours) {
				builder.AppendFormat("{0} Hour{1} ", span.Hours, span.Hours == 1 ? String.Empty : "s");
			}
			// min
			if (0 != span.Minutes) {
				builder.AppendFormat("{0} Minute{1} ", span.Minutes, span.Minutes == 1 ? String.Empty : "s");
			}
			// sec
			if (0 != span.Seconds) {
				builder.AppendFormat("{0} Second{1} ", span.Seconds, span.Seconds == 1 ? String.Empty : "s");
			}
			// cleanup
			if(builder.Length > 0 && builder[builder.Length-1] == ' ') {
				builder.Remove(builder.Length - 1, 1);
			}
			// final
			return builder.ToString();
		}

		public static double WrapDegree(double degrees) {
			const double degreePeriod = 360.0;
			while (degrees < 0) {
				degrees += degreePeriod;	
			}
			while (degrees >= degreePeriod) {
				degrees -= degreePeriod;
			}
			return degrees;
		}

		public static string GetFriendlyName(SpeedUnit unit) {
			switch (unit) {
			case SpeedUnit.MetersPerSec: return "m/s";
			case SpeedUnit.MilesPerHour: return "mph";
			default: return unit.ToString();
			}
		}
		

		public static string GetFriendlyName(TemperatureUnit unit) {
			switch (unit) {
			case TemperatureUnit.Celsius: return "C";
			case TemperatureUnit.Fahrenheit: return "F";
			default: return unit.ToString();
			}
		}
		
		public static string GetFriendlyName(PressureUnit unit) {
			switch (unit) {
			case PressureUnit.InchOfMercury: return "inHg";
			case PressureUnit.KiloPascals: return "kPa";
			case PressureUnit.Pascals: return "Pa";
			case PressureUnit.Millibar: return "hPa";//"mBar";
			default: return unit.ToString();
			}
		}

		[Obsolete("Should use a converter")]
		public static double ConvertUnit(double value, TemperatureUnit from, TemperatureUnit to) {
			if (from == to) {
				return value;
			}
			if (from == TemperatureUnit.Celsius) {
				return (value * 1.8) + 32.0;
			}
			return (value - 32.0) / 1.8;
		}

		[Obsolete("Should use a converter")]
		public static double ConvertUnit(double value, SpeedUnit from, SpeedUnit to) {
			if (from == to) {
				return value;
			}
			if (from == SpeedUnit.MetersPerSec) {
				return 2.23693629 * value;
			}
			return 0.44704 * value;
		}

		[Obsolete("Should use a converter")]
		public static double ConvertUnit(double value, PressureUnit from, PressureUnit to) {
			if (from == to) {
				return value;
			}

			switch (from) {
				case PressureUnit.InchOfMercury:
					switch (to) {
						case PressureUnit.KiloPascals:
							return value * 3.38600;
						case PressureUnit.Pascals:
							return value * (3.38600 * 1000.0);
						case PressureUnit.Millibar:
							return value * 33.86;
						default:
							throw new ArgumentOutOfRangeException("to");
					}
				case PressureUnit.KiloPascals:
					switch (to) {
						case PressureUnit.InchOfMercury:
							return 0.295333727 * value;
						case PressureUnit.Pascals:
							return value * 1000.0;
						case PressureUnit.Millibar:
							return 10.0 * value;
						default:
							throw new ArgumentOutOfRangeException("to");
					}
				case PressureUnit.Pascals:
					switch (to) {
						case PressureUnit.InchOfMercury:
							return (0.295333727 / 1000.0) * value;
						case PressureUnit.KiloPascals:
							return value / 1000.0;
						case PressureUnit.Millibar:
							return (10.0 / 1000.0) * value;
						default:
							throw new ArgumentOutOfRangeException("to");
					}
				case PressureUnit.Millibar:
					switch (to) {
						case PressureUnit.InchOfMercury:
							return value * 0.0295333727;
						case PressureUnit.Pascals:
							return value * (0.1 * 1000.0);
						case PressureUnit.KiloPascals:
							return value * 0.1;
						default:
							throw new ArgumentOutOfRangeException("to");
					}
				default:
					throw new ArgumentOutOfRangeException("from");
			}
		}


		/*
		public static IEnumerable<Reading> ConvertUnits(
			IEnumerable<Reading> readings,
			TemperatureUnit desiredTempUnit,
			SpeedUnit desiredSpeedUnit,
			PressureUnit desiredPressUnit,
			ISensorInfo sensorInfo
		) {
			return ConvertUnits(readings, sensorInfo.TemperatureUnit, desiredTempUnit, sensorInfo.SpeedUnit, desiredSpeedUnit, sensorInfo.PressureUnit, desiredPressUnit);
		}

		public static IEnumerable<SensorReading> ConvertUnits(
			IEnumerable<ISensorReading> readings,
			TemperatureUnit desiredTempUnit,
			SpeedUnit desiredSpeedUnit,
			PressureUnit desiredPressUnit,
			ISensorInfo sensorInfo
		) {
			return ConvertUnits(readings, sensorInfo.TemperatureUnit, desiredTempUnit, sensorInfo.SpeedUnit, desiredSpeedUnit, sensorInfo.PressureUnit, desiredPressUnit);
		}

		public static IEnumerable<ReadingsSummary> ConvertUnits(
			IEnumerable<ISensorReadingsSummary> readings,
			TemperatureUnit desiredTempUnit,
			SpeedUnit desiredSpeedUnit,
			PressureUnit desiredPressUnit,
			ISensorInfo sensorInfo
		) {
			return ConvertUnits(readings, sensorInfo.TemperatureUnit, desiredTempUnit, sensorInfo.SpeedUnit, desiredSpeedUnit, sensorInfo.PressureUnit, desiredPressUnit);
		}

		public static IEnumerable<SensorReading> ConvertUnits(
			IEnumerable<SensorReading> readings,
			TemperatureUnit dataTempUnit,
			TemperatureUnit desiredTempUnit,
			SpeedUnit dataSpeedUnit,
			SpeedUnit desiredSpeedUnit,
			PressureUnit dataPressUnit,
			PressureUnit desiredPressUnit
		) {
			double conversionFactor = 0;
			IEnumerable<SensorReading> result = null;
			if (dataTempUnit != desiredTempUnit) {
				conversionFactor = ConvertUnit(1.0, dataTempUnit, desiredTempUnit);
				result = readings.Select(sr => new SensorReading(sr.TimeStamp, sr.Temperature * conversionFactor, sr.Pressure, sr.Humidity, sr.WindSpeed, sr.WindDirection));
			}
			if (dataSpeedUnit != desiredSpeedUnit) {
				conversionFactor = ConvertUnit(1.0, dataSpeedUnit, desiredSpeedUnit);
				result = readings.Select(sr => new SensorReading(sr.TimeStamp, sr.Temperature, sr.Pressure, sr.Humidity, sr.WindSpeed * conversionFactor, sr.WindDirection));
			}
			if (dataPressUnit != desiredPressUnit) {
				conversionFactor = ConvertUnit(1.0, dataPressUnit, desiredPressUnit);
				result = readings.Select(sr => new SensorReading(sr.TimeStamp, sr.Temperature, sr.Pressure * conversionFactor, sr.Humidity, sr.WindSpeed, sr.WindDirection));
			}
			return result;
		}

		public static IEnumerable<SensorReading> ConvertUnits(
			IEnumerable<ISensorReading> readings,
			TemperatureUnit dataTempUnit,
			TemperatureUnit desiredTempUnit,
			SpeedUnit dataSpeedUnit,
			SpeedUnit desiredSpeedUnit,
			PressureUnit dataPressUnit,
			PressureUnit desiredPressUnit
		) {
			double conversionFactor = 0;
			IEnumerable<SensorReading> result = null;
			if (dataTempUnit != desiredTempUnit) {
				conversionFactor = ConvertUnit(1.0, dataTempUnit, desiredTempUnit);
				result = readings.Select(sr => new SensorReading(sr.TimeStamp, sr.Temperature * conversionFactor, sr.Pressure, sr.Humidity, sr.WindSpeed, sr.WindDirection));
			}
			if (dataSpeedUnit != desiredSpeedUnit) {
				conversionFactor = ConvertUnit(1.0, dataSpeedUnit, desiredSpeedUnit);
				result = readings.Select(sr => new SensorReading(sr.TimeStamp, sr.Temperature, sr.Pressure, sr.Humidity, sr.WindSpeed * conversionFactor, sr.WindDirection));
			}
			if (dataPressUnit != desiredPressUnit) {
				conversionFactor = ConvertUnit(1.0, dataPressUnit, desiredPressUnit);
				result = readings.Select(sr => new SensorReading(sr.TimeStamp, sr.Temperature, sr.Pressure * conversionFactor, sr.Humidity, sr.WindSpeed, sr.WindDirection));
			}
			return result;
		}

		private static Dictionary<TemperatureUnit, Dictionary<TemperatureUnit, Func<double, double>>> _tempFuncs
			= new Dictionary<TemperatureUnit, Dictionary<TemperatureUnit, Func<double, double>>>();

		public static Func<double, double> GetTempConvFunc(TemperatureUnit from, TemperatureUnit to) {
			Dictionary<TemperatureUnit, Func<double, double>> toLookup;
			if (!_tempFuncs.TryGetValue(from, out toLookup)) {
				toLookup = new Dictionary<TemperatureUnit, Func<double, double>>();
				_tempFuncs.Add(from, toLookup);
			}
			Func<double, double> func;
			if (!toLookup.TryGetValue(to, out func)) {
				ParameterExpression p = Expression.Parameter(typeof(double), "value");
				func = Expression.Lambda<Func<double, double>>(
					ConversionOperation.CreateConvertTempValue(p, from, to),
					p
				).Compile();
			}
			return func;
		}
		
		*/

		/*

		private class ConversionOperation {

			public readonly Func<double, double> ConvertTempValue;
			public readonly Func<double, double> ConvertSpeedValue;
			public readonly Func<double, double> ConvertPressValue;
			public readonly Func<ISensorReadingValues, SensorReadingValues> ConvertSensorReadingValue;
			public readonly Func<ISensorReadingsSummary, ReadingsSummary> ConvertSensorReadingsSummary;

			public readonly TemperatureUnit dataTempUnit;
			public readonly TemperatureUnit desiredTempUnit;
			public readonly SpeedUnit dataSpeedUnit;
			public readonly SpeedUnit desiredSpeedUnit;
			public readonly PressureUnit dataPressUnit;
			public readonly PressureUnit desiredPressUnit;

			public ConversionOperation(
				TemperatureUnit dataTempUnit,
				TemperatureUnit desiredTempUnit,
				SpeedUnit dataSpeedUnit,
				SpeedUnit desiredSpeedUnit,
				PressureUnit dataPressUnit,
				PressureUnit desiredPressUnit
			) {

				this.dataTempUnit = dataTempUnit;
				this.desiredTempUnit = desiredTempUnit;
				this.dataSpeedUnit = dataSpeedUnit;
				this.desiredSpeedUnit = desiredSpeedUnit;
				this.dataPressUnit = dataPressUnit;
				this.desiredPressUnit = desiredPressUnit;

				ParameterExpression valueIn = ParameterExpression.Parameter(typeof(double), "value");

				ConvertTempValue = Expression.Lambda<Func<double, double>>(CreateConvertTempValue(valueIn), valueIn).Compile();
				ConvertSpeedValue = Expression.Lambda<Func<double, double>>(CreateConvertSpeedValue(valueIn), valueIn).Compile();
				ConvertPressValue = Expression.Lambda<Func<double, double>>(CreateConvertPressValue(valueIn), valueIn).Compile();

				ParameterExpression readingIn = ParameterExpression.Parameter(typeof(ISensorReadingValues), "readingValues");

				ConvertSensorReadingValue = Expression.Lambda<Func<ISensorReadingValues, SensorReadingValues>>(
					CreateConvertOneExpression(readingIn), readingIn
				).Compile();

				ParameterExpression readingSummaryIn = ParameterExpression.Parameter(typeof(ISensorReadingsSummary), "readingSummary");

				ConvertSensorReadingsSummary = Expression.Lambda<Func<ISensorReadingsSummary, ReadingsSummary>>(
					CreateConvertOneSummary(readingSummaryIn), readingSummaryIn
				).Compile();


			}

			internal static Expression CreateConvertTempValue(Expression tempIn, TemperatureUnit from, TemperatureUnit to) {
				if (from != to) {
					if (from == TemperatureUnit.Celsius) {
						return Expression.Add(
							Expression.Multiply(tempIn, Expression.Constant(1.8)),
							Expression.Constant(32.0)
						);
					}
					else {
						return Expression.Divide(
							Expression.Subtract(tempIn, Expression.Constant(32.0)),
							Expression.Constant(1.8)
						);
					}
				}
				return tempIn;
			}

			private Expression CreateConvertTempValue(Expression tempIn) {
				return CreateConvertTempValue(tempIn, dataTempUnit, desiredTempUnit);
			}

			private Expression CreateConvertPressValue(Expression pressIn) {
				if (dataPressUnit != desiredPressUnit) {
					return Expression.Multiply(
						pressIn,
						Expression.Constant(ConvertUnit(1.0, dataPressUnit, desiredPressUnit))
					);
				}
				return pressIn;
			}

			private Expression CreateConvertSpeedValue(Expression speedIn) {
				if (dataSpeedUnit != desiredSpeedUnit) {
					return Expression.Multiply(
						speedIn,
						Expression.Constant(ConvertUnit(1.0, dataSpeedUnit, desiredSpeedUnit))
					);
				}
				return speedIn;
			}

			private Expression CreateConvertOneExpression(Expression readingIn) {

				if (
					dataTempUnit == desiredTempUnit
					&& dataSpeedUnit == desiredSpeedUnit
					&& dataPressUnit == desiredPressUnit
				) {
					return Expression.New(
						typeof(SensorReadingValues).GetConstructor(new Type[] { typeof(ISensorReadingValues) }),
						readingIn
					);
				}

				Expression tempValue = CreateConvertTempValue(Expression.Property(readingIn, "Temperature"));
				Expression pressValue = CreateConvertPressValue(Expression.Property(readingIn, "Pressure"));
				Expression humValue = Expression.Property(readingIn, "Humidity");
				Expression speedValue = CreateConvertSpeedValue(Expression.Property(readingIn, "WindSpeed"));
				Expression dirValue = Expression.Property(readingIn, "WindDirection");

				Expression readingOut = Expression.New(
					typeof(SensorReadingValues).GetConstructor(Enumerable.Repeat(typeof(double), 5).ToArray()),
					tempValue,
					pressValue,
					humValue,
					speedValue,
					dirValue
				);
				return readingOut;
			}

			private Expression CreateConvertOneSummary(Expression summaryIn) {

				return Expression.New(
					typeof(ReadingsSummary).GetConstructor(
						Enumerable.Repeat(typeof(DateTime), 2)
						.Concat(Enumerable.Repeat(typeof(SensorReadingValues), 4))
						.Concat(new Type[] { typeof(int) })
						.Concat(Enumerable.Repeat(typeof(Dictionary<double, int>), 5))
						.ToArray()
					),
					Expression.Property(summaryIn, "BeginStamp"),
					Expression.Property(summaryIn, "EndStamp"),
					CreateConvertOneExpression(Expression.Property(summaryIn, "Min")),
					CreateConvertOneExpression(Expression.Property(summaryIn, "Max")),
					CreateConvertOneExpression(Expression.Property(summaryIn, "Mean")),
					CreateConvertOneExpression(Expression.Property(summaryIn, "Median")),
					Expression.Property(summaryIn, "Count"),
					Expression.Call(
						typeof(UnitUtility).GetMethod(
							"ConvertCounts",
							BindingFlags.Public | BindingFlags.Static,
							null,
							new Type[] { typeof(Dictionary<double, int>), typeof(TemperatureUnit), typeof(TemperatureUnit) },
							null
						),
						Expression.Call(summaryIn, typeof(ISensorReadingsSummary).GetMethod("GetTemperatureCounts")),
						Expression.Constant(dataTempUnit),
						Expression.Constant(desiredTempUnit)
					),
					Expression.Call(
						typeof(UnitUtility).GetMethod(
							"ConvertCounts",
							BindingFlags.Public | BindingFlags.Static,
							null,
							new Type[] { typeof(Dictionary<double, int>), typeof(PressureUnit), typeof(PressureUnit) },
							null
						),
						Expression.Call(summaryIn, typeof(ISensorReadingsSummary).GetMethod("GetPressureCounts")),
						Expression.Constant(dataPressUnit),
						Expression.Constant(desiredPressUnit)
					),
					Expression.Call(summaryIn, typeof(ISensorReadingsSummary).GetMethod("GetHumidityCounts")),
					Expression.Call(
						typeof(UnitUtility).GetMethod(
							"ConvertCounts",
							BindingFlags.Public | BindingFlags.Static,
							null,
							new Type[] { typeof(Dictionary<double, int>), typeof(SpeedUnit), typeof(SpeedUnit) },
							null
						),
						Expression.Call(summaryIn, typeof(ISensorReadingsSummary).GetMethod("GetWindSpeedCounts")),
						Expression.Constant(dataSpeedUnit),
						Expression.Constant(desiredSpeedUnit)
					),
					Expression.Call(summaryIn, typeof(ISensorReadingsSummary).GetMethod("GetWindDirectionCounts"))
				);

			}

		}
		

		private static List<ConversionOperation> ops = new List<ConversionOperation>();

		private static ConversionOperation GetConversionOps(
			TemperatureUnit dataTempUnit,
			TemperatureUnit desiredTempUnit,
			SpeedUnit dataSpeedUnit,
			SpeedUnit desiredSpeedUnit,
			PressureUnit dataPressUnit,
			PressureUnit desiredPressUnit
		) {
			foreach (ConversionOperation op in ops) {
				if (
					op.dataTempUnit == dataTempUnit
					&& op.desiredTempUnit == desiredTempUnit
					&& op.dataSpeedUnit == dataSpeedUnit
					&& op.desiredSpeedUnit == desiredSpeedUnit
					&& op.dataPressUnit == dataPressUnit
					&& op.desiredPressUnit == desiredPressUnit
				) {
					return op;
				}
			}
			ConversionOperation newOp = new ConversionOperation(
				dataTempUnit, desiredTempUnit,
				dataSpeedUnit, desiredSpeedUnit,
				dataPressUnit, desiredPressUnit
			);
			ops.Add(newOp);
			return newOp;
		}

		public static Func<ISensorReadingsSummary, ReadingsSummary> GetSummaryConvFunc(
			TemperatureUnit dataTempUnit,
			TemperatureUnit desiredTempUnit,
			SpeedUnit dataSpeedUnit,
			SpeedUnit desiredSpeedUnit,
			PressureUnit dataPressUnit,
			PressureUnit desiredPressUnit
		) {
			ConversionOperation op = GetConversionOps(
				dataTempUnit, desiredTempUnit,
				dataSpeedUnit, desiredSpeedUnit,
				dataPressUnit, desiredPressUnit
			);
			return op.ConvertSensorReadingsSummary;
		}

		public static Func<double, double> GetTempConvFunc(
			TemperatureUnit dataTempUnit,
			TemperatureUnit desiredTempUnit,
			SpeedUnit dataSpeedUnit,
			SpeedUnit desiredSpeedUnit,
			PressureUnit dataPressUnit,
			PressureUnit desiredPressUnit
		) {
			ConversionOperation op = GetConversionOps(
				dataTempUnit, desiredTempUnit,
				dataSpeedUnit, desiredSpeedUnit,
				dataPressUnit, desiredPressUnit
			);
			return op.ConvertTempValue;
		}

		public static IEnumerable<ReadingsSummary> ConvertUnits(
			IEnumerable<ISensorReadingsSummary> readings,
			TemperatureUnit dataTempUnit,
			TemperatureUnit desiredTempUnit,
			SpeedUnit dataSpeedUnit,
			SpeedUnit desiredSpeedUnit,
			PressureUnit dataPressUnit,
			PressureUnit desiredPressUnit
		) {
			ConversionOperation op = new ConversionOperation(
				dataTempUnit, desiredTempUnit,
				dataSpeedUnit, desiredSpeedUnit,
				dataPressUnit, desiredPressUnit
			);

			return readings.Select(op.ConvertSensorReadingsSummary);
		}

		public static SensorReadingValues ConvertUnits(
			ISensorReadingValues values,
			TemperatureUnit dataTempUnit,
			TemperatureUnit desiredTempUnit,
			SpeedUnit dataSpeedUnit,
			SpeedUnit desiredSpeedUnit,
			PressureUnit dataPressUnit,
			PressureUnit desiredPressUnit
		) {
			if (dataTempUnit == desiredTempUnit && dataPressUnit == desiredPressUnit && dataSpeedUnit == desiredSpeedUnit) {
				return new SensorReadingValues(values);
			}
			return new SensorReadingValues(
				dataTempUnit != desiredTempUnit ? UnitUtility.ConvertUnit(values.Temperature, dataTempUnit, desiredTempUnit) : values.Temperature,
				dataPressUnit != desiredPressUnit ? UnitUtility.ConvertUnit(values.Pressure, dataPressUnit, desiredPressUnit) : values.Pressure,
				values.Humidity,
				dataSpeedUnit != desiredSpeedUnit ? UnitUtility.ConvertUnit(values.WindSpeed, dataSpeedUnit, desiredSpeedUnit) : values.WindSpeed,
				values.WindDirection
			);
		}

		public static Dictionary<double, int> ConvertCounts(Dictionary<double, int> data, TemperatureUnit from, TemperatureUnit to) {
			if (from == to) {
				return data;
			}
			else {
				Func<double, double> op = GetTempConvFunc(from, to);
				return data.ToDictionary(kvp => op(kvp.Key), kvp => kvp.Value);
			}
		}

		public static Dictionary<double, int> ConvertCounts(Dictionary<double, int> data, PressureUnit from, PressureUnit to) {
			if (from == to) {
				return data;
			}
			else {
				Dictionary<double, int> result = new Dictionary<double, int>(data.Count);
				foreach (KeyValuePair<double, int> kvp in data) {
					result[UnitUtility.ConvertUnit(kvp.Key, from, to)] = kvp.Value;
				}
				return result;
			}
		}

		public static Dictionary<double, int> ConvertCounts(Dictionary<double, int> data, SpeedUnit from, SpeedUnit to) {
			if (from == to) {
				return data;
			}
			else {
				Dictionary<double, int> result = new Dictionary<double, int>(data.Count);
				foreach (KeyValuePair<double, int> kvp in data) {
					result[UnitUtility.ConvertUnit(kvp.Key, from, to)] = kvp.Value;
				}
				return result;
			}
		}
		*/


		public static DateTime StripToUnit(DateTime value, TimeUnit unit) {
			switch (unit) {
			case TimeUnit.Year: return new DateTime(value.Year, 1, 1);
			case TimeUnit.Month: return new DateTime(value.Year, value.Month, 1);
			case TimeUnit.Day: return new DateTime(value.Year, value.Month, value.Day);
			case TimeUnit.Hour: return new DateTime(value.Year, value.Month, value.Day, value.Hour, 0, 0);
			case TimeUnit.Minute: return new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, 0);
			case TimeUnit.Second: return new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second);
			}
			return value;
		}

		public static DateTime IncrementByUnit(DateTime value, TimeUnit unit) {
			switch (unit) {
			case TimeUnit.Year: return value.AddYears(1);
			case TimeUnit.Month: return value.AddMonths(1);
			case TimeUnit.Day: return value.AddDays(1.0);
			case TimeUnit.Hour: return value.AddHours(1.0);
			case TimeUnit.Minute: return value.AddMinutes(1.0);
			case TimeUnit.Second: return value.AddSeconds(1.0);
			}
			return value.AddTicks(1);
		}

		public static TimeSpan ChooseBestSummaryUnit(TimeSpan span) {
			if (span < TimeSpan.Zero) {
				return ChooseBestSummaryUnit(TimeSpan.Zero.Subtract(span));
			}
			if (span <= new TimeSpan(31, 0, 0, 0)) {
				return new TimeSpan(0,1,0);
			}
			if (span <= new TimeSpan(366, 0, 0, 0)) {
				return new TimeSpan(0,10,0);
			}
			return new TimeSpan(31,0,0,0);
		}

		public static TimeSpan TimeUnitToTimeSpan(TimeUnit unit) {
			switch (unit) {
			case TimeUnit.Second: return new TimeSpan(0,0,1);
			case TimeUnit.Minute: return new TimeSpan(0,1,0);
			case TimeUnit.Hour: return new TimeSpan(1, 0, 0);
			case TimeUnit.Day: return new TimeSpan(1,0,0,0);
			case TimeUnit.Month: return new TimeSpan(30,0,0,0);
			case TimeUnit.Year: return new TimeSpan(365,0,0,0);
			default: throw new ArgumentOutOfRangeException("unit");
			}
		}

		public static TimeSpan ChooseBestUpdateInterval(TimeUnit unit) {
			switch (unit) {
			case TimeUnit.Second:
				return new TimeSpan(0, 0, 1);
			case TimeUnit.Minute:
				return new TimeSpan(0, 0, 5);
			case TimeUnit.Hour:
				return new TimeSpan(0, 1, 0);
			case TimeUnit.Day:
				return new TimeSpan(0, 5, 0);
			case TimeUnit.Month:
				return new TimeSpan(1, 0, 0);
			case TimeUnit.Year:
				return new TimeSpan(1, 0, 0, 0);
			}
			return new TimeSpan(0, 0, 1);
		}


		public static readonly DateTime PosixTimeOrigin = new DateTime(1970, 1, 1, 0, 0, 0, 0);

		public static int ConvertToPosixTime(DateTime dateTime) {
			return (int)(dateTime.Subtract(PosixTimeOrigin).TotalSeconds);
		}

		public static DateTime ConvertFromPosixTime(int seconds) {
			return PosixTimeOrigin.AddSeconds(seconds);
		}
		
	}
}
