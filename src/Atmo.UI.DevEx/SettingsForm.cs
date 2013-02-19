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
using System.Net.Sockets;
using System.Net;

namespace Atmo.UI.DevEx {
	public partial class SettingsForm : XtraForm {

		public PersistentState State { get; private set; }

        public double time_correction;


        public SettingsForm(PersistentState state, InternetStreamingStatistics _internetStreamingStatistics)
        {
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


            //rp
            //MessageBox.Show("statistic___ sended: "+_internetStreamingStatistics.m_StreamingServers[0].GetSendetPacekets() + " received" + _internetStreamingStatistics.m_StreamingServers[0].GetReadedPacekets() );
            label_statistic_incomming_packets.Text = ""+_internetStreamingStatistics.m_StreamingServers[0].GetReadedPacekets();
            label_statistic_sendet_packets.Text = "" + _internetStreamingStatistics.m_StreamingServers[0].GetSendetPacekets();


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

            //rp
            time_label.Text = time_correction.ToString();

            //rp - uprava pre weather underground
            textBoxWeatherName.Text = State.StationNameWeather;
            textBoxWeatherPassword.Text = State.StationPasswordWeather;
            checkEditWeather.Checked = State.WeatherEnabled;
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


        public DateTime GetNetworkTime(string ntpServer)
        {

            try
            {

                //default Windows time server
                //const string ntpServer = "time.windows.com";
                //const string ntpServer = "pool.ntp.org";


                // NTP message size - 16 bytes of the digest (RFC 2030)
                var ntpData = new byte[48];

                //Setting the Leap Indicator, Version Number and Mode values
                ntpData[0] = 0x1B; //LI = 0 (no warning), VN = 3 (IPv4 only), Mode = 3 (Client Mode)

                var addresses = Dns.GetHostEntry(ntpServer).AddressList;

                //The UDP port number assigned to NTP is 123
                var ipEndPoint = new IPEndPoint(addresses[0], 123);
                //NTP uses UDP
                var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                //rp
                socket.ReceiveTimeout = 1000;
                socket.SendTimeout = 1000;


                socket.Connect(ipEndPoint);



                socket.Send(ntpData);
                socket.Receive(ntpData);
                socket.Close();

                //Offset to get to the "Transmit Timestamp" field (time at which the reply 
                //departed the server for the client, in 64-bit timestamp format."
                const byte serverReplyTime = 40;

                //Get the seconds part
                ulong intPart = BitConverter.ToUInt32(ntpData, serverReplyTime);

                //Get the seconds fraction
                ulong fractPart = BitConverter.ToUInt32(ntpData, serverReplyTime + 4);

                //Convert From big-endian to little-endian
                intPart = SwapEndianness(intPart);
                fractPart = SwapEndianness(fractPart);

                var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);

                //**UTC** time
                var networkDateTime = (new DateTime(1900, 1, 1)).AddMilliseconds((long)milliseconds);

                return networkDateTime;
            }
            catch (Exception ex)
            {
                //  MessageBox.Show("Error with time synchronization !!!");

                return new DateTime(1905, 2, 2, 2, 2, 2);
            }
        }

        // stackoverflow.com/a/3294698/162671
        static uint SwapEndianness(ulong x)
        {
            return (uint)(((x & 0x000000ff) << 24) +
                           ((x & 0x0000ff00) << 8) +
                           ((x & 0x00ff0000) >> 8) +
                           ((x & 0xff000000) >> 24));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ntpServer1 = "pool.ntp.org";
            string ntpServer2 = "time.windows.com";

            bool ok = false;

            DateTime dtNet = GetNetworkTime(ntpServer1);
            DateTime dtNow = DateTime.UtcNow;

            if (dtNet.Year == 1905)
            {
                ok = false;
                //                MessageBox.Show("Time synchronization error !");
            }
            else
                ok = true;


            if (ok == false )
            {
                dtNet = GetNetworkTime(ntpServer2);
                dtNow = DateTime.UtcNow;
            }

            if (ok == false && dtNet.Year == 1905)
            {
            }


            if (ok == false && dtNet.Year != 1905)
            {
                ok = true;
            }

            
            if (ok == true)
            {
                time_correction = (dtNet - dtNow).TotalMilliseconds;
                time_label.Text = "Time Correction = " + time_correction + " [ms]";
                MessageBox.Show("Time correction resolved !");
            }
            else
                MessageBox.Show("Time correction not resolved !");


        }

        private void xtraTabPageStatistic_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }




	}

}
