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
using Atmo.Data;
using Atmo.Units;
using DevExpress.XtraEditors;
using System.Collections.Generic;

namespace Atmo.UI.DevEx {
	public partial class SettingsForm : XtraForm {

		public PersistentState State { get; private set; }

		public SettingsForm(PersistentState state) {
			if(null == state) {
				throw new ArgumentNullException("state");
			}
			State = state;
			InitializeComponent();

			var userGraphValues = (PersistentState.UserCalculatedAttribute[])Enum.GetValues(typeof (PersistentState.UserCalculatedAttribute));
			comboBoxEditUserGraph.Properties.Items.AddRange(userGraphValues);

			comboBoxEditTemp.Properties.Items.AddRange(Enum.GetValues(typeof(TemperatureUnit)));
			comboBoxEditPress.Properties.Items.AddRange(Enum.GetValues(typeof(PressureUnit)));
			comboBoxEditSpeed.Properties.Items.AddRange(Enum.GetValues(typeof(SpeedUnit)));

			SetValuesFromState();
		}

		public void SetValuesFromState() {
			SetGraphRangeValues();
			SetUserGraphFormValue();
			SetUnitsFormValue();
			SetPwsFormValues();
		}

		public void SetStateFromForm() {
			SetStateGraphRangeValues();
			SetStateUserGraphType();
			SetStateUnits();
			SetStatePws();
		}

		public void SetPwsFormValues() {
			var stationTextFields = new[] {
				textEditStationA,
				textEditStationB,
				textEditStationC,
				textEditStationD
			};

			for (int i = 0; i < stationTextFields.Length; i++ ) {
				stationTextFields[i].Text = i < State.StationNames.Count
					? (State.StationNames[i] ?? String.Empty)
					: String.Empty;
			}
			textEditPwsPass.Text = State.StationPassword;
			checkButtonPwsEnabled.Checked = State.PwsEnabled;
		}

		public void SetStatePws() {
			var stationTextFields = new[] {
				textEditStationA,
				textEditStationB,
				textEditStationC,
				textEditStationD
			};
			State.StationNames = stationTextFields.Select(tf => tf.Text ?? String.Empty).ToList();
			State.StationPassword = textEditPwsPass.Text;
			State.PwsEnabled = checkButtonPwsEnabled.Checked;
		}

		public void SetGraphRangeValues() {
			spinEditTemperature.Value = (decimal) State.MinRangeSizes.Temperature;
			spinEditHumidity.Value = (decimal) (State.MinRangeSizes.Humidity * 100);
			spinEditPressure.Value = (decimal) State.MinRangeSizes.Pressure;
			spinEditSpeed.Value = (decimal) State.MinRangeSizes.WindSpeed;
			spinEditAirDensity.Value = (decimal) State.MinRangeSizeAirDensity;
			spinEditDewPoint.Value = (decimal) State.MinRangeSizeDewPoint;
		}

		public void SetStateGraphRangeValues() {
			State.MinRangeSizes.Temperature = (double)spinEditTemperature.Value;
			State.MinRangeSizes.Humidity = ((double) spinEditHumidity.Value) / 100.0;
			State.MinRangeSizes.Pressure = (double) spinEditPressure.Value;
			State.MinRangeSizes.WindSpeed = (double) spinEditSpeed.Value;
			State.MinRangeSizeAirDensity = (double) spinEditAirDensity.Value;
			State.MinRangeSizeDewPoint = (double) spinEditDewPoint.Value;
		}

		public void SetUserGraphFormValue() {
			try {
				comboBoxEditUserGraph.SelectedItem = State.UserGraphAttribute;
			}catch {
				;
			}
		}

		public void SetStateUserGraphType() {
			try {
				State.UserGraphAttribute =  (PersistentState.UserCalculatedAttribute)comboBoxEditUserGraph.SelectedItem;
			}catch {
				State.UserGraphAttribute = default(PersistentState.UserCalculatedAttribute);
			}
		}

		public void SetUnitsFormValue() {
			try {
				comboBoxEditTemp.SelectedItem = State.TemperatureUnit;
				comboBoxEditPress.SelectedItem = State.PressureUnit;
				comboBoxEditSpeed.SelectedItem = State.SpeedUnit;
			}catch {
				;
			}
		}

		public void SetStateUnits() {
			try {
				State.TemperatureUnit = (TemperatureUnit)comboBoxEditTemp.SelectedItem;
				State.PressureUnit = (PressureUnit) comboBoxEditPress.SelectedItem;
				State.SpeedUnit = (SpeedUnit) comboBoxEditSpeed.SelectedItem;
			}catch {
				State.TemperatureUnit = default(TemperatureUnit);
				State.PressureUnit = default(PressureUnit);
				State.SpeedUnit = default(SpeedUnit);
			}
		}

		private void simpleButtonCancel_Click(object sender, EventArgs e) {
			Close();
		}

		private void simpleButtonOk_Click(object sender, EventArgs e) {
			SetStateFromForm();
			Close();
		}

		private void simpleButtonApply_Click(object sender, EventArgs e) {
			SetStateFromForm();
		}

		private void checkButtonPwsEnabled_CheckedChanged(object sender, EventArgs e) {
			var message = checkButtonPwsEnabled.Checked
				? "PWS Enabled (click to disable then click apply)"
				: "PWS Disabled (click to enable then click apply)"
			;
			checkButtonPwsEnabled.Text = message;
		}

		private void labelControl16_Click(object sender, EventArgs e) {
			try {
				System.Diagnostics.Process.Start("http://www.wunderground.com/wxstation/signup.html");
			}
			catch (Exception exceptionMonster) {
				; // eat it... just alert the user
				MessageBox.Show("Navigate to http://www.wunderground.com/wxstation/signup.html", "Location", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

	}
}
