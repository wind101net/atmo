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

namespace Atmo.Stats
{
    public class ReadingValuesRange
    {

        public readonly Range Temperature;
		public readonly Range Humidity;
		public readonly Range Pressure;
		public readonly Range WindSpeed;
		public readonly Range WindDirection;
		
        [Obsolete("May need to remove this to increase data density")]
        private ReadingValues _min;
        [Obsolete("May need to remove this to increase data density")]
		private ReadingValues _max;

        public ReadingValuesRange(ReadingValuesRange ranges)
        {
            Temperature = new Range(ranges.Temperature);
            Humidity = new Range(ranges.Humidity);
            Pressure = new Range(ranges.Pressure);
            WindSpeed = new Range(ranges.WindSpeed);
            WindDirection = new Range(ranges.WindDirection);
            _min = _max = null;
        }

        public ReadingValuesRange(
			Range temperature,
			Range humidity,
			Range pressure,
			Range windSpeed,
			Range windDirection
		) {
			Temperature = new Range(temperature);
			Humidity = new Range(humidity);
			Pressure = new Range(pressure);
			WindSpeed = new Range(windSpeed);
			WindDirection = new Range(windDirection);
			_min = _max = null;
		}

		public ReadingValues Min { get {
			return _min ?? (_min = new ReadingValues(
				Temperature.Low,
				Pressure.Low,
				Humidity.Low,
				WindDirection.Low,
				WindSpeed.Low
			));
		} }

		public ReadingValues Max { get {
			return _max ?? (_max = new ReadingValues(
				Temperature.High,
				Pressure.High,
				Humidity.High,
				WindDirection.High,
				WindSpeed.High
			));
		} }

    }
}
