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

namespace Atmo.Test {
	public struct Vector2D : IEquatable<Vector2D> {

		public struct CardinalDirection {

			private static readonly CardinalDirection[] Directions = new []{
                new CardinalDirection("N",360),
                new CardinalDirection("NNW",337.5),
                new CardinalDirection("NW",315),
                new CardinalDirection("WNW",292.5),
                new CardinalDirection("W",270),
                new CardinalDirection("WSW",247.5),
                new CardinalDirection("SW",225),
                new CardinalDirection("SSW",202.5),
                new CardinalDirection("S",180),
                new CardinalDirection("SSE",157.5),
                new CardinalDirection("SE",135),
                new CardinalDirection("ESE",112.5),
                new CardinalDirection("E",90),
                new CardinalDirection("ENE",67.5),
                new CardinalDirection("NE",45),
                new CardinalDirection("NNE",22.5),
                new CardinalDirection("N",0)
            };

			public static string DegreesToBestCardinalName(double degrees) {
				var bestName = Directions[0].Name;
				var bestDist = Math.Abs(Directions[0].Value - degrees);
				for (int i = 1; i < Directions.Length; i++) {
					var dist = Math.Abs(Directions[i].Value - degrees);
					if (dist >= bestDist) {
						continue;
					}
					bestDist = dist;
					bestName = Directions[i].Name;
				}
				return bestName;
			}

			private readonly string _name;
			private readonly double _value;

			private CardinalDirection(string name, double value) {
				_name = name;
				_value = value;
			}

			public string Name {
				get { return _name; }
			}

			public double Value {
				get { return _value; }
			}

		}

		/// <summary>
		/// Implements the operator ==.
		/// </summary>
		/// <param name="a">A vector.</param>
		/// <param name="b">A vector.</param>
		/// <returns>True if both vectors have the same component values.</returns>
		public static bool operator ==(Vector2D a, Vector2D b) {
			return a.Equals(b);
		}

		/// <summary>
		/// Implements the operator !=.
		/// </summary>
		/// <param name="a">A vector.</param>
		/// <param name="b">A vector.</param>
		/// <returns>True if both vectors do not have the same component values.</returns>
		public static bool operator !=(Vector2D a, Vector2D b) {
			return !a.Equals(b);
		}

		public static double RadiansToDegrees(double value) {
			return PositivePeriod(180.0f * (value / Math.PI), 360.0f);
		}

		public static double PositivePeriod(double value, double period) {
			var periods = (int)(Math.Abs(value / period));
			return (
				(0 != periods)
				? (
					(value < 0)
					? (value / (double)periods) + period
					: value / (double)periods
				)
				: (
					(value < 0)
					? value + period
					: value
				)
			);
		}

		private readonly double _x;
		private readonly double _y;

		public Vector2D(Vector2D v) : this(v._x, v._y) { }

		public Vector2D(double x, double y) {
			_x = x;
			_y = y;
		}

		public double X { get { return _x; } }

		public double Y { get { return _y; } }

		public double GetMagnitude() {
			return Math.Sqrt(GetMagnitudeSquared());
		}

		public double GetMagnitudeSquared() {
			return (_x * _x) + (_y * _y);
		}

		public Vector2D GenerateNormal() {
			var magnitude = GetMagnitude();
			return (1.0 == magnitude || 0 == magnitude) ? (new Vector2D(this)) : new Vector2D(_x / magnitude, _y / magnitude);
		}

		[Obsolete]
		public double GetSouthRelativeAngularValue() {
			var normal = GenerateNormal();
			return Math.Atan2(-normal._x, normal._y) / Math.PI;
		}
		[Obsolete]
		public double GetNorthRelativeClockwiseAngularRadians() {
			var normal = GenerateNormal();
			return Math.Atan2(normal._x, normal._y);
		}
		[Obsolete]
		public double GetNorthRelativeClockwiseAngularDegrees() {
			return RadiansToDegrees(GetNorthRelativeClockwiseAngularRadians());
		}

		public bool Equals(Vector2D other) {
			return _x == other._x && _y == other._y;
		}

		public override bool Equals(object obj) {
			return (obj is Vector2D && Equals((Vector2D)obj));
		}

		public override int GetHashCode() {
			return _x.GetHashCode() ^ _y.GetHashCode();
		}

		public override string ToString() {
			return String.Concat('(', _x, ' ', _y, ')');
		}

	}
}
