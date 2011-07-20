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

namespace Atmo.Stats {
	public class WindSpeedFrequency {

		public readonly double Speed;
		public int Frequency;
		public double Weibull;

		public WindSpeedFrequency(double speed) {
			Speed = speed;
			Frequency = 0;
			Weibull = 0;
		}

		public WindSpeedFrequency(double speed, int frequency) {
			Speed = speed;
			Frequency = frequency;
			Weibull = 0;
		}

		public WindSpeedFrequency(double speed, int frequency, double weibull) {
			Speed = speed;
			Frequency = frequency;
			Weibull = weibull;
		}

		public double SpeedPropertty {
			get { return Speed; }
		}

		public int FrequencyProperty {
			get { return Frequency; }
			//set { _frequency = value; }
		}

		public double WeibullProperty {
			get { return Weibull; }
			//set { _weibull = value; }
		}

		public double Energy {
			get {
				return Speed * Speed * Speed * (double)Frequency;
			}
		}

	}
}
