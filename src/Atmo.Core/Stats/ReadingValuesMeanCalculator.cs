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
using Atmo.Units;

namespace Atmo.Stats {
	public class ReadingValuesMeanCalculator<T> : IAggregateCalculator<T,ReadingValues>
		where T:IReadingValues
	{

		private const double DegToRadFactor = Math.PI / 180.0;
		private const double RadToDegFactor = 180.0 / Math.PI;

		private double _temperatureValue = 0;
		private double _pressureValue = 0;
		private double _humidityValue = 0;
		private double _speedValue = 0;
		private double _dirSinSum = 0;
		private double _dirCosSum = 0;

		private int _temperatureCount = 0;
		private int _pressureCount = 0;
		private int _humidityCount = 0;
		private int _speedCount = 0;
		private int _dirCount = 0;

		private int _count = 0;

		public int ProcessedCount { get { return _count; } }

		public void Proccess(T input) {
			if (!input.IsValid) {
				return;
			}
			if (input.IsTemperatureValid) {
				_temperatureValue += input.Temperature;
				_temperatureCount++;
			}
			if (input.IsPressureValid) {
				_pressureValue += input.Pressure;
				_pressureCount++;
			}
			if (input.IsHumidityValid) {
				_humidityValue += input.Humidity;
				_humidityCount++;
			}
			if (input.IsWindSpeedValid) {
				_speedValue += input.WindSpeed;
				_speedCount++;
			}
			if (input.IsWindDirectionValid) {
				var dir = input.WindDirection * DegToRadFactor;
				_dirSinSum += Math.Sin(dir);
				_dirCosSum += Math.Cos(dir);
				_dirCount++;
			}
			_count++;
		}

		public ReadingValues Result {
			get {
				if(_count == 0) {
					return ReadingValues.CreateInvalid();
				}

				var temperatureValue = (_temperatureCount > 0 && !Double.IsNaN(_temperatureValue))
					? _temperatureValue / _temperatureCount
					: Double.NaN;
				var pressureValue = (_pressureCount > 0 && !Double.IsNaN(_pressureValue))
					? _pressureValue / _pressureCount
					: Double.NaN;
				var humidityValue = (_humidityCount > 0 && !Double.IsNaN(_humidityValue))
					? _humidityValue / _humidityCount
					: Double.NaN;
				var speedValue = (_speedCount > 0 && !Double.IsNaN(_speedValue))
					? _speedValue / _speedCount
					: Double.NaN;
				var directionValue = (_dirCount > 0 && !Double.IsNaN(_dirSinSum) && !Double.IsNaN(_dirCosSum))
					? UnitUtility.WrapDegree(Math.Atan2(_dirSinSum / _dirCount, _dirCosSum / _dirCount) * RadToDegFactor)
					: Double.NaN;
				return new ReadingValues(temperatureValue, pressureValue, humidityValue, directionValue, speedValue);
			}
		}
	}
}
