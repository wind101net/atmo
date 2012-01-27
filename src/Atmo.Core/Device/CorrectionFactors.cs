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
using System.Linq;
using System.Text;

namespace Atmo.Device {

	/// <summary>
	/// Correction factors that can be sent to an anemometer.
	/// </summary>
	public class CorrectionFactors {

		private const double decimalFactor = 100000.0;

		private static double Period(double value, double cap) {
			if(value < 0) {
				while (value < 0)
					value += cap;
			}
			else if(value >= cap) {
				while(value >= cap)
					value -= cap;
			}
			return value;
		}

		public static byte CalculateWindDirectionOffset(double currentDirectionReading, double desiredDirectionReading, byte currentDirectionOffset) {
			var difference = currentDirectionReading - desiredDirectionReading;
			var offsetCorrection = 256 * (difference / 360.0);
			return unchecked((byte)(Period(currentDirectionOffset + offsetCorrection,256)));
		}

		private static double ToDouble(byte[] bytes, int offset) {
			byte[] tmp = new byte[4];
			Array.Copy(bytes, offset, tmp, 0, tmp.Length);
			int iVal = BitConverter.ToInt32(tmp.Reverse().ToArray(), 0);
			return ((double)(iVal) / decimalFactor);
		}

		private static void ToBytes(double value, byte[] bytes, int offset) {
			int iVal = (int)(value * decimalFactor);
			byte[] tmp = BitConverter.GetBytes(iVal).Reverse().ToArray();
			Array.Copy(tmp, 0, bytes, offset, tmp.Length);
		}

		public static CorrectionFactors ToCorrectionFactors(byte[] bytes, int offset) {
			var factors = new CorrectionFactors();
			factors.windDirectionOffset = bytes[offset];
			factors.tempOffset = ToDouble(bytes, offset + 1);
			factors.pressureFactorF = ToDouble(bytes, offset + 5);
			factors.pressureFactorG = ToDouble(bytes, offset + 9);
			factors.pressureFactorH = ToDouble(bytes, offset + 13);
			factors.speedFactorX = ToDouble(bytes, offset + 17);
			factors.speedFactorY = ToDouble(bytes, offset + 21);
			factors.humidityOffset = ToDouble(bytes, offset + 25);
			factors.speedCalibrationA = ToDouble(bytes, offset + 29);
			factors.speedCalibrationB = ToDouble(bytes, offset + 33);
			factors.speedCalibrationC = ToDouble(bytes, offset + 37);
			factors.speedCalibrationD = ToDouble(bytes, offset + 41);
			factors.speedCalibrationOffset = ToDouble(bytes, offset + 45);
			return factors;
		}

		public static void ToBytes(CorrectionFactors factors, byte[] bytes, int offset) {
			bytes[offset] = factors.windDirectionOffset;
			ToBytes(factors.tempOffset, bytes, offset + 1);
			ToBytes(factors.pressureFactorF, bytes, offset + 5);
			ToBytes(factors.pressureFactorG, bytes, offset + 9);
			ToBytes(factors.pressureFactorH, bytes, offset + 13);
			ToBytes(factors.speedFactorX, bytes, offset + 17);
			ToBytes(factors.speedFactorY, bytes, offset + 21);
			ToBytes(factors.humidityOffset, bytes, offset + 25);
			ToBytes(factors.speedCalibrationA, bytes, offset + 29);
			ToBytes(factors.speedCalibrationB, bytes, offset + 33);
			ToBytes(factors.speedCalibrationC, bytes, offset + 37);
			ToBytes(factors.speedCalibrationD, bytes, offset + 41);
			ToBytes(factors.speedCalibrationOffset, bytes, offset + 45);
		}

		private static readonly char[] DefaultDelim = ";,".ToCharArray();
		private static readonly char DefaultSeperatorChar = DefaultDelim[0];
		private static readonly string DefaultSeperator = DefaultSeperatorChar.ToString();

		public static CorrectionFactors FromString(string value) {
			return FromString(value, null);
		}

		public static CorrectionFactors FromString(string value, char[] delim) {
			string[] split = value.Split(delim ?? DefaultDelim).Select(s => s.Trim()).ToArray();
			var factors = new CorrectionFactors();
			factors.windDirectionOffset = Byte.Parse(split[0]);
			factors.tempOffset = Double.Parse(split[1]);
			factors.pressureFactorF = Double.Parse(split[2]);
			factors.pressureFactorG = Double.Parse(split[3]);
			factors.pressureFactorH = Double.Parse(split[4]);
			factors.speedFactorX = Double.Parse(split[5]);
			factors.speedFactorY = Double.Parse(split[6]);
			factors.humidityOffset = Double.Parse(split[7]);
			factors.speedCalibrationA = Double.Parse(split[8]);
			factors.speedCalibrationB = Double.Parse(split[9]);
			factors.speedCalibrationC = Double.Parse(split[10]);
			factors.speedCalibrationD = Double.Parse(split[11]);
			factors.speedCalibrationOffset = Double.Parse(split[12]);
			return factors;
		}

		public byte windDirectionOffset;
		public double tempOffset;
		public double pressureFactorF;
		public double pressureFactorG;
		public double pressureFactorH;
		public double speedFactorX;
		public double speedFactorY;
		public double humidityOffset;
		public double speedCalibrationA;
		public double speedCalibrationB;
		public double speedCalibrationC;
		public double speedCalibrationD;
		public double speedCalibrationOffset;

		public override string ToString() {
			return ToString(DefaultSeperator);
		}

		public string ToString(string seperator) {
			var builder = new StringBuilder();
			builder.Append(windDirectionOffset); builder.Append(seperator);
			builder.Append(tempOffset); builder.Append(seperator);
			builder.Append(pressureFactorF); builder.Append(seperator);
			builder.Append(pressureFactorG); builder.Append(seperator);
			builder.Append(pressureFactorH); builder.Append(seperator);
			builder.Append(speedFactorX); builder.Append(seperator);
			builder.Append(speedFactorY); builder.Append(seperator);
			builder.Append(humidityOffset); builder.Append(seperator);
			builder.Append(speedCalibrationA); builder.Append(seperator);
			builder.Append(speedCalibrationB); builder.Append(seperator);
			builder.Append(speedCalibrationC); builder.Append(seperator);
			builder.Append(speedCalibrationD); builder.Append(seperator);
			builder.Append(speedCalibrationOffset);
			return builder.ToString();
		}


	}
}
