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
using System.Drawing;
using System.Windows.Forms;

namespace Atmo.UI.DevEx.Controls {
	public partial class SensorView : UserControl {

		private bool _selected;

		public SensorView() {
			InitializeComponent();
			foreach (Control c in tableLayoutPanel.Controls) {
				c.MouseClick += tableLayoutPanel_MouseClick;
			}
			ResetValues();
			SetBackgroundColor();
		}

		public bool IsSelected {
			get { return _selected; }
			set { _selected = value; SetBackgroundColor(); }
		}

		private void SetBackgroundColor() {
			SetBackgroundColor(IsSelected);
		}

		private void SetBackgroundColor(bool selected) {
			BackColor = (selected) ? SystemColors.Highlight : Color.Transparent;
			ForeColor = (selected) ? SystemColors.ControlText : SystemColors.InactiveCaptionText;
			//this.sensorNameLabel.ForeColor = this.ForeColor;
			//this.tableLayoutPanel1.ForeColor = this.ForeColor;
			//foreach (Control c in this.tableLayoutPanel1.Controls.OfType<Control>()) {
			//	c.ForeColor = this.ForeColor;
			//}
		}

		public void ResetValues() {
			SetValues(null,null);
		}

		public void SetValues(ISensor sensor, IReadingValues reading) {
			if (null != sensor) {
				int n;
				sensorNameLabel.Text = (Int32.TryParse(sensor.Name, out n) && 0 <= n && n < 26)
					? ((char) ((byte) ('A') + (byte) n)).ToString()
					: sensor.Name;
			}
			else {
				sensorNameLabel.Text = "Sensor";
			}

			const string na = "N/A";
			if (null != reading && reading.IsValid) {
				throw new NotImplementedException();
			}
			else {
				tempValue.Text = na;
				pressureValue.Text = na;
				humidityValue.Text = na;
				windSpeedValue.Text = na;
				windDirValue.Text = na;
			}
		}

		private void tableLayoutPanel_MouseClick(object sender, MouseEventArgs e) {
			SensorView_MouseClick(sender, e);
		}

		private void SensorView_MouseClick(object sender, MouseEventArgs e) {
			IsSelected = !IsSelected;
		}
	}
}
