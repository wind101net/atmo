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

		public static DateTime StripToSecond(DateTime value) {
			return new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second);
		}

		public static TimeSpan ExtractHms(DateTime value) {
			return new TimeSpan(value.Hour,value.Minute, value.Second);
		}

		public static DateTime StripToUnit(DateTime value, TimeUnit unit) {
			switch (unit) {
			case TimeUnit.Year: return new DateTime(value.Year, 1, 1);
			case TimeUnit.Month: return new DateTime(value.Year, value.Month, 1);
			case TimeUnit.Day: return new DateTime(value.Year, value.Month, value.Day);
			case TimeUnit.Hour: return new DateTime(value.Year, value.Month, value.Day, value.Hour, 0, 0);
			case TimeUnit.TenMinutes: {
				return new DateTime(value.Year, value.Month, value.Day, value.Hour, (value.Minute / 10) * 10, 0);
			}
			case TimeUnit.Minute: return new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, 0);
			case TimeUnit.Second: return StripToSecond(value);
			}
			return value;
		}

		public static DateTime IncrementByUnit(DateTime value, TimeUnit unit) {
			switch (unit) {
			case TimeUnit.Year: return value.AddYears(1);
			case TimeUnit.Month: return value.AddMonths(1);
			case TimeUnit.Day: return value.AddDays(1);
			case TimeUnit.Hour: return value.AddHours(1);
			case TimeUnit.TenMinutes: return value.AddMinutes(10);
			case TimeUnit.Minute: return value.AddMinutes(1);
			case TimeUnit.Second: return value.AddSeconds(1);
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
			case TimeUnit.TenMinutes: return new TimeSpan(0, 10, 0);
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
			case TimeUnit.TenMinutes:
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
