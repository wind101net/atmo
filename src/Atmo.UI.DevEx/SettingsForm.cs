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
			checkEditSyncDaqClock.Checked = State.AutoSyncClock;
		}

		public void SetStateFromForm() {
			SetStateGraphRangeValues();
			SetStateUserGraphType();
			SetStateUnits();
			SetStatePws();
			State.AutoSyncClock = checkEditSyncDaqClock.Checked;
		}

		public void SetPwsFormValues() {



            //rp - uprava pre weather underground
            textBoxWeatherName.Text = State.StationNameWeather;
            textBoxWeatherPassword.Text = State.StationPasswordWeather;
            checkEditWeather.Checked = State.WeatherEnabled;
            listBoxWeatherTime.Text = State.StationIntervalWeather.ToString();
            listBoxWeatherSensor.SetSelected(0, false);
            listBoxWeatherSensor.SetSelected(1, false);
            listBoxWeatherSensor.SetSelected(2, false);
            listBoxWeatherSensor.SetSelected(3, false);
            listBoxWeatherSensor.SetSelected(State.StationSensorIndexWeather, true);


            //rp
            checkEditWF.Checked = State.PwfEnabled;
            //checkEditWF_CheckedChanged(null, null);
            listBoxWFtime.Text = State.StationIntervalWF.ToString();
            textBoxWFname.Text = State.StationNameWF;
            textBoxWFpassword.Text = State.StationPasswordWF;
            listBoxWFsensor.SetSelected(0, false);
            listBoxWFsensor.SetSelected(1, false);
            listBoxWFsensor.SetSelected(2, false);
            listBoxWFsensor.SetSelected(3, false);
            listBoxWFsensor.SetSelected(State.StationSensorIndexWF, true);

            //rp
            checkEditAW.Checked = State.PawEnabled;
            //checkEditWF_CheckedChanged(null, null);
            textBoxAWName.Text = State.StationNameAw;
            textBoxAwPassword.Text = State.StationPasswordAw;
            listBoxAwTime.Text = State.StationIntervalAW.ToString();
            listBoxAwSensor.SetSelected(0, false);
            listBoxAwSensor.SetSelected(1, false);
            listBoxAwSensor.SetSelected(2, false);
            listBoxAwSensor.SetSelected(3, false);
            listBoxAwSensor.SetSelected(State.StationSensorIndexAw, true);
        }

		public void SetStatePws() {

            //rp - pre weather under...
            State.WeatherEnabled = checkEditWeather.Checked;
            State.StationNameWeather = textBoxWeatherName.Text;
            State.StationPasswordWeather = textBoxWeatherPassword.Text;
            State.StationIntervalWeather = Int16.Parse(listBoxWeatherTime.Text);
            int ako = 0;
            if (listBoxWeatherSensor.GetSelected(0) == true)
                ako = 0;
            if (listBoxWeatherSensor.GetSelected(1) == true)
                ako = 1;
            if (listBoxWeatherSensor.GetSelected(2) == true)
                ako = 2;
            if (listBoxWeatherSensor.GetSelected(3) == true)
                ako = 3;
            State.StationSensorIndexWeather = ako;

            //rp - pre AW
            State.PawEnabled = checkEditAW.Checked;
            State.StationNameAw = textBoxAWName.Text;
            State.StationPasswordAw = textBoxAwPassword.Text;
            State.StationIntervalAW = Int16.Parse(listBoxAwTime.Text);

            ako = 0;
            if (listBoxAwSensor.GetSelected(0) == true)
                ako = 0;
            if (listBoxAwSensor.GetSelected(1) == true)
                ako = 1;
            if (listBoxAwSensor.GetSelected(2) == true)
                ako = 2;
            if (listBoxAwSensor.GetSelected(3) == true)
                ako = 3;
            State.StationSensorIndexAw = ako;
            
            //rp windfinder
            State.PwfEnabled = checkEditWF.Checked;
            State.StationNameWF = textBoxWFname.Text;
            State.StationPasswordWF = textBoxWFpassword.Text;
            State.StationIntervalWF = Int16.Parse(listBoxWFtime.Text);
            ako = 0;
            if (listBoxWFsensor.GetSelected(0) == true)
                ako = 0;
            if (listBoxWFsensor.GetSelected(1) == true)
                ako = 1;
            if (listBoxWFsensor.GetSelected(2) == true)
                ako = 2;
            if (listBoxWFsensor.GetSelected(3) == true)
                ako = 3;
            State.StationSensorIndexWF = ako;



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
		}

		private void labelControl16_Click(object sender, EventArgs e) {

		}

		private void SettingsForm_Load(object sender, EventArgs e) {

		}

        private void checkEditWF_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBoxShowPasswordAwekas_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxShowPasswordAwekas.Checked == true)
            {
                textBoxAwPassword.PasswordChar = '\0';
            }
            else
            {
                textBoxAwPassword.PasswordChar = '*';
            }
        }

        private void checkBoxShowPasswordWind_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxShowPasswordWind.Checked == true)
            {
                textBoxWFpassword.PasswordChar = '\0';
            }
            else
            {
                textBoxWFpassword.PasswordChar = '*';
            }
        }

        private void checkBoxShowPasswordWeather_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxShowPasswordWeather.Checked == true)
                textBoxWeatherPassword.PasswordChar = '\0';
            else
                textBoxWeatherPassword.PasswordChar = '*';
        }

	}
}
