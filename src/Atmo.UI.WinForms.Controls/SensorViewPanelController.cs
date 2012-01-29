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

using System.Windows.Forms;
using Atmo.Units;

namespace Atmo.UI.WinForms.Controls {
	public class SensorViewPanelController : ViewPanelController<SensorView, ISensor> {

		public SensorViewPanelController(Control container) : base(container) {
			ConverterCache = ReadingValuesConverterCache<IReadingValues, ReadingValues>.Default;
		}

		public ReadingValuesConverterCache<IReadingValues, ReadingValues> ConverterCache { get; set; }

		protected override SensorView CreateNewView() {
			return new SensorView() {
				ConverterCache = ConverterCache,
				IsSelected = DefaultSelected
			};
		}

		protected override void UpdateView(SensorView view, ISensor model) {
			view.ConverterCache = ConverterCache;
			if (view.Dock != DockStyle) {
				view.Dock = DockStyle;
			}
			view.Update(model);
		}

		protected override void Synchronize(SensorView[] views, ISensor[] models, Data.PersistentState state) {
			foreach (var view in views) {
				view.TemperatureUnit = state.TemperatureUnit;
				view.PressureUnit = state.PressureUnit;
				view.SpeedUnit = state.SpeedUnit;
			}
			base.Synchronize(views, models, state);
		}

	}
}
