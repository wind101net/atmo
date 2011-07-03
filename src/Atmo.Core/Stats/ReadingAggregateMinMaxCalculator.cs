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

namespace Atmo.Stats {
	public class ReadingAggregateMinMaxCalculator : IAggregateCalculator<ReadingAggregate, ReadingRangeAggregate> {


		private TimeRange _time;
		private Range _temperature;
		private Range _humidity;
		private Range _pressure;
		private Range _windSpeed;
		private Range _windDirection;


		public ReadingAggregateMinMaxCalculator() {
			_time = new TimeRange(default(DateTime),default(DateTime));
			_temperature = new Range(Double.NaN);
			_humidity = new Range(Double.NaN);
			_pressure = new Range(Double.NaN);
			_windSpeed = new Range(Double.NaN);
			_windDirection = new Range(Double.NaN);
		}

		public void Proccess(ReadingAggregate input) {
			if(null == input || !input.IsValid) {
				return;
			}

			if (_time.Low == default(DateTime) && _time.High == default(DateTime)) {
				_time = new TimeRange(input.TimeRange);
			}
			else {
				_time.Merge(input.TimeRange);
			}

			if(input.IsTemperatureValid) {
				_temperature.Merge(input.Temperature);
			}
			if(input.IsHumidityValid) {
				_humidity.Merge(input.Humidity);
			}
			if(input.IsPressureValid) {
				_pressure.Merge(input.Pressure);
			}
			if(input.IsWindSpeedValid) {
				_windSpeed.Merge(input.WindSpeed);
			}
			if(input.IsWindDirectionValid) {
				_windDirection.Merge(input.WindDirection);
			}
		}

		public ReadingRangeAggregate Result {
			get {
				return new ReadingRangeAggregate(
					_time,
					_temperature,
					_humidity,
					_pressure,
					_windSpeed,
					_windDirection
				);
			}
		}
	}
}
