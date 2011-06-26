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
using System.Windows.Forms;
using Atmo.UI.WinForms.Controls;
using Atmo.Units;

namespace Atmo.Daq.Win32.TestHarness {
	public partial class MainForm : Form {

		private UsbDaqConnection _connection;
		private readonly SensorViewPanelController _sensorViewPanelControler;

		public MainForm() {
			InitializeComponent();
			
			_sensorViewPanelControler = new SensorViewPanelController(panelSensors);

			_connection = new UsbDaqConnection();
			timerProperties_Tick(this, null);
		}

		private void HandleConnectionResult(bool? result) {
			if (result.HasValue) {
				if (buttonConnect.Enabled == result.Value) {
					buttonConnect.Enabled = !result.Value;
				}
				if (buttonDisconnect.Enabled != result.Value) {
					buttonDisconnect.Enabled = result.Value;
				}
				var labelText = result.Value ? "connected!" : "disconnected";
				if(labelText != labelConnect.Text) {
					labelConnect.Text = labelText;
				}
			}else {
				buttonConnect.Enabled = false;
				buttonDisconnect.Enabled = true;
				labelConnect.Text = "...";
			}
		}

		private void timerProperties_Tick(object sender, EventArgs e) {
			HandleConnectionResult(_connection.IsConnected);
		}

		private void buttonConnect_Click(object sender, EventArgs e) {
			HandleConnectionResult(null);
			var result = _connection.Connect();
			HandleConnectionResult(result);
		}

		private void buttonDisconnect_Click(object sender, EventArgs e) {
			HandleConnectionResult(null);
			_connection.Dispose();
			_connection = new UsbDaqConnection();
			HandleConnectionResult(_connection.IsConnected);
		}

		private void HandleQueryResult(bool? result) {
			if (result.HasValue) {
				if (buttonStartQuery.Enabled == result.Value) {
					buttonStartQuery.Enabled = !result.Value;
				}
				if (buttonStopQuery.Enabled != result.Value) {
					buttonStopQuery.Enabled = result.Value;
				}
				var labelText = result.Value ? "connected!" : "disconnected";
				if (labelText != labelIsQuery.Text) {
					labelIsQuery.Text = labelText;
				}
			}
			else {
				buttonStartQuery.Enabled = false;
				buttonStopQuery.Enabled = true;
				labelIsQuery.Text = "...";
			}
		}

		private void buttonStartQuery_Click(object sender, EventArgs e) {
			HandleQueryResult(null);
			_connection.ResumeQuery();

			var result = _connection.IsQuerying;

			HandleQueryResult(result);
			if (result) {
				timerQuery.Start();
			}
		}

		private void buttonStopQuery_Click(object sender, EventArgs e) {
			HandleQueryResult(null);
			_connection.PauseQuery();
			HandleQueryResult(_connection.IsQuerying);
			timerQuery.Stop();
		}

		private void timerQuery_Tick(object sender, EventArgs e) {
			if (!_connection.IsQuerying) {
				return;
			}

			_sensorViewPanelControler.UpdateView(Enumerable.Range(0, 4).Select(i => _connection.GetSensor(i)));
			
		}

		private void buttonSetNetSize_Click(object sender, EventArgs e) {
			_connection.SetNetworkSize((int)numericUpDownNetSize.Value);
		}

		private void buttonPing_Click(object sender, EventArgs e) {
			var pingTime = _connection.Ping();
			labelPing.Text = pingTime.ToString();
		}

	}
}
