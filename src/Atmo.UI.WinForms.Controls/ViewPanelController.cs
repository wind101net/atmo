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
using System.Linq;
using System.Windows.Forms;

namespace Atmo.UI.WinForms.Controls {
	public abstract class ViewPanelController<TView,TModel>
		where TView : Control
	{

		public Control Container { get; private set; }
		public DockStyle DockStyle { get; set; }
		public bool SensorsFirst { get; set; }
		public bool ReverseSync { get; set; }
		public bool DefaultSelected { get; set; }

		protected ViewPanelController(Control container) {
			if (null == container) {
				throw new ArgumentNullException("container");
			}
			Container = container;
			DockStyle = DockStyle.Top;
			SensorsFirst = false;
			ReverseSync = true;
		}

		public bool UpdateView(IEnumerable<TModel> sensors) {
			return UpdateView(sensors.ToArray());
		}

		protected abstract TView CreateNewView();

		protected abstract void UpdateView(TView view, TModel model);

		public bool UpdateView(TModel[] sensors) {
			var existing = Container.Controls.OfType<TView>().ToArray();
			TView[] reused;
			TView[] added;

			// reuse what can be
			if (existing.Length > 0 && sensors.Length > 0) {
				reused = new TView[Math.Min(existing.Length, sensors.Length)];
				Array.Copy(existing, reused, reused.Length);
			}
			else {
				reused = new TView[0];
			}

			// dump what we can't
			if (reused.Length < existing.Length) {
				for (int i = reused.Length; i < existing.Length; i++) {
					Container.Controls.Remove(existing[i]);
				}
			}

			if (sensors.Length == 0) {
				return false; // nothing else to do
			}

			// add what we need
			if (sensors.Length > reused.Length) {
				added = new TView[sensors.Length - reused.Length];
				for (var i = 0; i < added.Length; i++) {
					added[i] = CreateNewView();
				}
				if (SensorsFirst) {
					var otherControls = Container.Controls.OfType<Control>().Where(c => !(c is SensorView)).ToArray();
					Container.Controls.Clear();
					Container.Controls.AddRange(reused);
					Container.Controls.AddRange(added);
					Container.Controls.AddRange(otherControls);
				}
				else {
					Container.Controls.AddRange(added);
				}
			}
			else {
				added = new TView[0];
			}

			var sync = new TView[reused.Length + added.Length];
			if (reused.Length > 0) {
				Array.Copy(reused, sync, reused.Length);
			}
			if (added.Length > 0) {
				Array.Copy(added, 0, sync, reused.Length, added.Length);
			}

			Synchronize(sync, sensors);
			return true;
		}

		public IEnumerable<TView> Views {
			get { return Container.Controls.OfType<TView>(); }
		}

		private void Synchronize(TView[] views, TModel[] models) {
			if (views.Length < models.Length) {
				throw new ArgumentOutOfRangeException("views", "not enough views for models");
			}
			for (int i = 0; i < models.Length; i++) {
				var view = ReverseSync ? views[views.Length - 1 - i] : views[i];
				UpdateView(view, models[i]);
			}
		}


	}
}
