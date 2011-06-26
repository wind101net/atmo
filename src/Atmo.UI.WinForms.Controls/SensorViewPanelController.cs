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
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using Atmo.Units;

namespace Atmo.UI.WinForms.Controls {
	public class SensorViewPanelController {

		

		public SensorViewPanelController(Control container) {
			if(null == container) {
				throw new ArgumentNullException("container");
			}
			Container = container;
			ConverterCache = ReadingValuesConverterCache<IReadingValues, ReadingValues>.Default;
			DockStyle = DockStyle.Top;
			SensorsFirst = false;
			ReverseSync = true;
		}

		public Control Container { get; private set; }
		public ReadingValuesConverterCache<IReadingValues, ReadingValues> ConverterCache { get; set; }
		public DockStyle DockStyle { get; set; }
		public bool SensorsFirst { get; set; }
		public bool ReverseSync { get; set; }

		public bool UpdateView(IEnumerable<ISensor> sensors) {
			return UpdateView(sensors.ToArray());
		}

		public bool UpdateView(ISensor[] sensors) {
			var existing = Container.Controls.OfType<SensorView>().ToArray();
			SensorView[] reused;
			SensorView[] added;

			// reuse what can be
			if(existing.Length > 0 && sensors.Length > 0) {
				reused = new SensorView[Math.Min(existing.Length, sensors.Length)];
				Array.Copy(existing, reused, reused.Length);
			}else {
				reused = new SensorView[0];
			}

			// dump what we can't
			if(reused.Length < existing.Length) {
				for(int i = reused.Length; i < existing.Length; i++) {
					Container.Controls.Remove(existing[i]);
				}
			}

			if(sensors.Length == 0) {
				return false; // nothing else to do
			}

			// add what we need
			if(sensors.Length > reused.Length) {
				added = new SensorView[sensors.Length-reused.Length];
				for(var i = 0; i < added.Length; i++) {
					added[i] = new SensorView() {
                        ConverterCache = ConverterCache
					};
				}
				if(SensorsFirst) {
					var otherControls = Container.Controls.OfType<Control>().Where(c => !(c is SensorView)).ToArray();
					Container.Controls.Clear();
					Container.Controls.AddRange(reused);
					Container.Controls.AddRange(added);
					Container.Controls.AddRange(otherControls);
				}else {
					Container.Controls.AddRange(added);
				}
			}else {
				added = new SensorView[0];
			}

			var sync = new SensorView[reused.Length + added.Length];
			if(reused.Length > 0) {
				Array.Copy(reused, sync, reused.Length);
			}
			if(added.Length > 0) {
				Array.Copy(added, 0, sync, reused.Length, added.Length);
			}

			Synchronize(sync, sensors);
			return true;
		}

		private void Synchronize(SensorView[] views, ISensor[] sensors) {
			if(views.Length < sensors.Length) {
				throw new ArgumentOutOfRangeException("views","not enough views for sensors");
			}
			for(int i = 0; i < sensors.Length; i++) {
				var view = ReverseSync ? views[views.Length-1-i] : views[i];
				view.ConverterCache = ConverterCache;
				view.Update(sensors[i]);
				view.Dock = DockStyle;
			}
		}

	}
}
