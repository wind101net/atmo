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

using NUnit.Framework;
using Atmo.Device;

namespace Atmo.Test
{
	[TestFixture]
	public class WindDirectionCorrectionOffsetTest
	{

		[Test]
		public void WebSampleTest1() {
			Assert.AreEqual(
				77,
				CorrectionFactors.CalculateWindDirectionOffset(111, 5, 2)
			);
		}

		[Test]
		public void WebSampleTest2() {
			Assert.AreEqual(
				103,
				CorrectionFactors.CalculateWindDirectionOffset(111, 50, 60)
			);
		}

		[Test]
		public void WebSampleTest3() {
			Assert.AreEqual(
				58,
				CorrectionFactors.CalculateWindDirectionOffset(1, 358, 56)
			);
		}

		[Test]
		public void WebSampleTest4() {
			Assert.AreEqual(
				88,
				CorrectionFactors.CalculateWindDirectionOffset(67, 300, 254)
			);
		}

		[Test]
		public void WebSampleTest5() {
			Assert.AreEqual(
				57,
				CorrectionFactors.CalculateWindDirectionOffset(0, 358, 56)
			);
		}

		[Test]
		public void NoChange() {
			for (int i = 0; i <= 360; i++) {
				for (int b = 0; b <= 255; b++) {
					Assert.AreEqual(
						b,
						CorrectionFactors.CalculateWindDirectionOffset(i, i, checked((byte)b))
					);
				}
			}
		}

	}
}
