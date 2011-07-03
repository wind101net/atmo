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
	public class ReadingAggregateMeanCalculator : IAggregateCalculator<ReadingAggregate,ReadingAggregate> {

		private const double DegToRadFactor = Math.PI / 180.0;
		private const double RadToDegFactor = 180.0 / Math.PI;

		double _temperatureValue = 0;
		double _pressureValue = 0;
		double _humidityValue = 0;
		double _speedValue = 0;
		double _dirSinSum = 0;
		double _dirCosSum = 0;

		int _temperatureCount = 0;
		int _pressureCount = 0;
		int _humidityCount = 0;
		int _speedCount = 0;
		int _dirCount = 0;

		int _count = 0;

		DateTime _minStart = DateTime.MaxValue;
		DateTime _maxEnd = DateTime.MinValue;

		public void Proccess(ReadingAggregate input) {
			if (!input.IsValid) {
				return;
			}

			var localCount = input.Count;

			if (input.TimeRange.Low < _minStart) {
				_minStart = input.TimeRange.Low;
			}
			if (input.TimeRange.High > _maxEnd) {
				_maxEnd = input.TimeRange.High;
			}

			if (input.IsTemperatureValid) {
				_temperatureValue += (input.Temperature * localCount);
				_temperatureCount += localCount;
			}
			if (input.IsPressureValid) {
				_pressureValue += (input.Pressure * localCount);
				_pressureCount += localCount;
			}
			if (input.IsHumidityValid) {
				_humidityValue += (input.Humidity * localCount);
				_humidityCount += localCount;
			}
			if (input.IsWindSpeedValid) {
				_speedValue += (input.WindSpeed * localCount);
				_speedCount += localCount;
			}
			if (input.IsWindDirectionValid) {
				var dir = input.WindDirection * DegToRadFactor;
				_dirSinSum += (Math.Sin(dir) * localCount);
				_dirCosSum += (Math.Cos(dir) * localCount);
				_dirCount += localCount;
			}
			_count += localCount;
		}

		public ReadingAggregate Result {
			get {
				if(0 == _count) {
					return ReadingAggregate.CreateInvalid();
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

				return new ReadingAggregate(
					_minStart, _maxEnd,
					new ReadingValues(temperatureValue, pressureValue, humidityValue, directionValue, speedValue),
					_count
				);
			}
		}
	}
}
