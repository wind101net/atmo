using System;
using System.Windows.Forms;

namespace Atmo.Daq.Win32.TestHarness {
	public partial class MainForm : Form {

		private UsbDaqConnection _connection;

		public MainForm() {
			InitializeComponent();
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
			labelWindSpeed1.Text = _connection[0].Current.WindSpeed.ToString();
			labelWindDir1.Text = _connection[0].Current.WindDirection.ToString();
		}

		private void buttonSetNetSize_Click(object sender, EventArgs e) {
			_connection.SetNetworkSize((int)numericUpDownNetSize.Value);
		}

		private void button1_Click(object sender, EventArgs e) {
			_connection.Unknown2();
		}

	}
}
