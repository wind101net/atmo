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
using Atmo.Data;
using Atmo.Units;
using DevExpress.XtraEditors;

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

			var unitGroupValues = (UnitGroupType[]) Enum.GetValues(typeof (UnitGroupType));
			comboBoxEditUnitGroup.Properties.Items.AddRange(unitGroupValues);

			SetValuesFromState();
		}

		public void SetValuesFromState() {
			SetGraphRangeValues();
			SetUserGraphFormValue();
			SetUnitGroupFormValue();
		}

		public void SetStateFromForm() {
			SetStateGraphRangeValues();
			SetStateUserGraphType();
			SetStateUnitGroup();
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

		public void SetUnitGroupFormValue() {
			try {
				comboBoxEditUnitGroup.SelectedItem = State.UnitGroup;
				comboBoxEditUnitGroup_SelectedIndexChanged(null, null);
			}catch {
				;
			}
		}

		public void SetStateUnitGroup() {
			try {
				State.UnitGroup = (UnitGroupType)comboBoxEditUnitGroup.SelectedItem;
			}catch {
				State.UnitGroup = default(UnitGroupType);
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

		private void comboBoxEditUnitGroup_SelectedIndexChanged(object sender, EventArgs e) {
			const string na = "N/A";
			string pressName;
			string speedName;
			string tempName;
			try {
				var selected = (UnitGroupType) comboBoxEditUnitGroup.SelectedItem;
				try {
					pressName = UnitUtility.GetPressureUnitForGroup(selected).ToString();
				}catch {
					pressName = null;
				}
				try {
					speedName = UnitUtility.GetSpeedUnitForGroup(selected).ToString();
				}catch {
					speedName = null;
				}
				try {
					tempName = UnitUtility.GetTemperatureUnitForGroup(selected).ToString();
				}catch {
					tempName = null;
				}
			}catch {
				pressName = speedName = tempName = null;
			}
			labelControlPressUnit.Text = pressName ?? na;
			labelControlSpeedUnit.Text = speedName ?? na;
			labelControlTempUnit.Text = tempName ?? na;
		}

	}
}
