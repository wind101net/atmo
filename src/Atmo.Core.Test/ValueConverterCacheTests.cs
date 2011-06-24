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

using Atmo.Units;
using NUnit.Framework;

namespace Atmo.Test {

	[TestFixture]
	public class ValueConverterCacheTests {

		[Test]
		public void TemperatureConverterTest() {
			var converterCache = new ValueConverterCache<TemperatureUnit>((k) => new TemperatureConverter(k.From, k.To));
			Assert.AreEqual(32, converterCache.Get(TemperatureUnit.Celsius, TemperatureUnit.Fahrenheit).Convert(0));
			Assert.AreEqual(33.8, converterCache.Get(TemperatureUnit.Celsius, TemperatureUnit.Fahrenheit).Convert(1));
			Assert.AreEqual(89.6, converterCache.Get(TemperatureUnit.Celsius, TemperatureUnit.Fahrenheit).Convert(32));
			Assert.AreEqual(212, converterCache.Get(TemperatureUnit.Celsius, TemperatureUnit.Fahrenheit).Convert(100));

			Assert.AreEqual(-17.7777778, converterCache.Get(TemperatureUnit.Fahrenheit, TemperatureUnit.Celsius).Convert(0), 0.00000005);
			Assert.AreEqual(0, converterCache.Get(TemperatureUnit.Fahrenheit, TemperatureUnit.Celsius).Convert(32), 0.0000000001);
			Assert.AreEqual(1, converterCache.Get(TemperatureUnit.Fahrenheit, TemperatureUnit.Celsius).Convert(33.8), 0.0000000001);
			Assert.AreEqual(32, converterCache.Get(TemperatureUnit.Fahrenheit, TemperatureUnit.Celsius).Convert(89.6), 0.0000000001);
			Assert.AreEqual(100, converterCache.Get(TemperatureUnit.Fahrenheit, TemperatureUnit.Celsius).Convert(212), 0.0000000001);
		}

	}

}
