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
	public class WindDirectionEnergy {

		public readonly double Direction;
		public double Energy;
		public double Frequency;

		public WindDirectionEnergy(double direction) : this(direction, 0, 0) { }

		public WindDirectionEnergy(double direction, double frequency, double energy) {
			Direction = direction;
			Frequency = frequency;
			Energy = energy;
		}

		public double DirectionProperty {
			get { return Direction; }
		}

		public double EnergyProperty {
			get { return Energy; }
			//set { _energy = value; }
		}

		public double FrequencyProperty {
			get { return Frequency; }
			//set { _frequency = value; }
		}

	}
}
