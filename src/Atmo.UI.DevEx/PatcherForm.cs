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
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using Atmo.Daq.Win32;
using Atmo.Device;
using DevExpress.XtraEditors;
using log4net;

namespace Atmo.UI.DevEx {
    public partial class PatcherForm : DevExpress.XtraEditors.XtraForm {

		private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private MemoryRegionDataCollection _patchData = null;
        private MemoryRegionDataCollection _anemPatchData = null;
        private readonly UsbDaqConnection _device = null;
        private int _anemNid = -1;

		protected PatcherForm() : this(null) { }

		public PatcherForm(UsbDaqConnection device) {
            _device = device;
            InitializeComponent();
        }

    	protected RadioGroup AnemSelectionRadioItems {
			get { return radioGroup1; }
    	}

        private void daqUpdateFileChooser_Click(object sender, EventArgs e) {

            var openFile = new OpenFileDialog();
            openFile.AutoUpgradeEnabled = true;
            openFile.DefaultExt = "hex";
            openFile.Filter = "Hex memory files (*.hex)|*.hex";
            openFile.SupportMultiDottedExtensions = true;
            openFile.Multiselect = false;
        	
			if (DialogResult.OK != openFile.ShowDialog()) {
        		return;
        	}

        	var fileInfo = new FileInfo(openFile.FileName);
        	try {
        		_patchData = HexParser.ParseToMemoryRegions(
        			new StreamReader(
        				File.OpenRead(fileInfo.FullName)
        				)
        			);
        		daqFileLabel.Text = String.Concat("Loaded: ", fileInfo.Name);
        		beginDaqUpdate.Enabled = true;
        	}
        	catch (Exception ex) {
        		daqFileLabel.Text = String.Concat("Load error: ", ex.ToString());
				Log.Error("DAQ firmware load error.",ex);
        		beginDaqUpdate.Enabled = false;
        	}
        }

        private void beginDaqUpdate_Click(object sender, EventArgs e) {
            beginDaqUpdate.Enabled = false;
            daqUpdateProgress.Enabled = true;
            daqUpgradeThread.RunWorkerAsync();
        }

        private void daqReportProgress(double percentage, string description) {
            daqUpgradeThread.ReportProgress((int)(percentage*100.0), description);
        }

        private void daqUpgradeThread_DoWork(object sender, DoWorkEventArgs e) {
            if (null == _patchData) {
                return;
            }

            bool programmed = false;
            bool connected = false;

            if (null != _device && _device.IsConnected)
            {
                _device.EnterBootMode();
            }


			using (var daqBootloader = new UsbDaqBootloaderConnection()) {
                int counter = 0;
                while (!(connected = daqBootloader.Connect()) && counter < 10)
                {
                    daqUpgradeThread.ReportProgress(0,"Waiting for DAQ...");
                    System.Threading.Thread.Sleep(1000);
                    counter++;
                }

                if (connected) {
                    programmed = daqBootloader.Program(_patchData,daqReportProgress);
                    if (programmed)
                    {
                        daqBootloader.Reboot();
                    }
                }
            }

            if (programmed) {
                daqUpgradeThread.ReportProgress(-1,"DAQ update successful");
            }
            else {
                if (connected) {
                    daqUpgradeThread.ReportProgress(-1,"DAQ update failed");
                }
                else {
                    daqUpgradeThread.ReportProgress(-1,"Device not found");
                }
            }
        }

        private void daqUpgradeThread_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            if (e.ProgressPercentage >= 0) {
                daqUpdateProgress.Position = e.ProgressPercentage;
            }
            else {
                MessageBox.Show(this, e.UserState as string);
            }
        }

        private void daqUpgradeThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            beginDaqUpdate.Enabled = null != _patchData;
            daqUpdateProgress.Enabled = false;
        }

        private void anemUpdateFileChooser_Click(object sender, EventArgs e) {
            var openFile = new OpenFileDialog();
            openFile.AutoUpgradeEnabled = true;
            openFile.DefaultExt = "hex";
            openFile.Filter = "Hex memory files (*.hex)|*.hex";
            openFile.SupportMultiDottedExtensions = true;
            openFile.Multiselect = false;
            if (DialogResult.OK == openFile.ShowDialog()) {
                FileInfo fileInfo = new FileInfo(openFile.FileName);
                try {
                    _anemPatchData = HexParser.ParseToMemoryRegions(
                        new StreamReader(
                            File.OpenRead(fileInfo.FullName)
                        )
                    );
                    anemFileLabel.Text = String.Concat("Loaded: ", fileInfo.Name);
                }
                catch (Exception ex) {
                    _anemPatchData = null;
					Log.Error("Anem firmware load error.", ex);
                    anemFileLabel.Text = String.Concat("Load error: ", ex.ToString());
                }
            }
            RefreshAnemUpdateEnabled();
        }

        private void RefreshAnemUpdateEnabled() {
            if (null != _anemPatchData && radioGroup1.SelectedIndex >= 0) {
                beginAnemUpdate.Enabled = true;
            }
            else {
                beginAnemUpdate.Enabled = false;
            }
        }

        private void beginAnemUpdate_Click(object sender, EventArgs e) {
            beginAnemUpdate.Enabled = false;
            anemUpdateProgress.Enabled = true;
            anemUpgradeThread.RunWorkerAsync();
        }

    	protected UsbDaqConnection Device {
			get { return _device; }
    	}

		protected virtual bool ProgramAnem(int nid, MemoryRegionDataCollection memoryRegionDataBlocks, Action<double, string> progressUpdated) {
			return Device.ProgramAnem(nid, memoryRegionDataBlocks, progressUpdated);
		}

        private void anemUpgradeThread_DoWork(object sender, DoWorkEventArgs e) {
            if (null == _anemPatchData) {
                return;
            }

            bool programmed = false;
			bool connected = _device.Connect();

            if (connected) {
            	programmed = ProgramAnem(_anemNid, _anemPatchData, anemReportProgress);
            }

            if (programmed) {
                anemUpgradeThread.ReportProgress(-1, "Anemometer update successful");
            }
            else {
                if (connected) {
                    anemUpgradeThread.ReportProgress(-1, "Anemometer update failed");
                }
                else {
                    anemUpgradeThread.ReportProgress(-1, "Device not found");
                }
            }
        }

        private void anemUpgradeThread_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            if (e.ProgressPercentage >= 0) {
                anemUpdateProgress.Position = e.ProgressPercentage;
            }
            else {
                MessageBox.Show(this, e.UserState as string);
            }
        }

        private void anemUpgradeThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            beginAnemUpdate.Enabled = null != _anemPatchData;
            anemUpdateProgress.Enabled = false;
        }

        private void anemReportProgress(double percentage, string description) {
            anemUpgradeThread.ReportProgress((int)(percentage * 100.0), description);
        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e) {
            //_anemNid = this.radioGroup1.SelectedIndex;
            _anemNid = Convert.ToInt32(
                radioGroup1.Properties.Items[
                    radioGroup1.SelectedIndex
                ].Value
            );
            RefreshAnemUpdateEnabled();
            textEditFactors.Text = String.Empty;
            sendCorrection.Enabled = false;
        }

        private void getCorrection_Click(object sender, EventArgs e)
        {
            if (_anemNid >= 0)
            {
                var factors = _device.GetCorrectionFactors(_anemNid);
                textEditFactors.Text = null == factors ? String.Empty : factors.ToString();
                sendCorrection.Enabled = false;
            }
            else
            {
                MessageBox.Show("Select a sensor.", "Select a sensor.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }

        }

        private void textEditFactors_EditValueChanged(object sender, EventArgs e)
        {
            sendCorrection.Enabled = true;
        }

        private void sendCorrection_Click(object sender, EventArgs e)
        {

            if (_anemNid < 0)
            {
                MessageBox.Show("Select a sensor.", "Select a sensor.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            string error = String.Empty;
            try
            {
                CorrectionFactors factors = CorrectionFactors.FromString(textEditFactors.Text);
                if (!_device.PutCorrectionFactors(_anemNid, factors))
                {
                    error = "Write Failed.";
                }
            }
            catch (FormatException)
            {
                error = "Invalid Format.";
            }
            catch (IndexOutOfRangeException)
            {
                error = "Not enough factors.";
            }

            if (!String.IsNullOrEmpty(error))
            {
                MessageBox.Show(error, "Error");
            }
            else
            {
                MessageBox.Show("Factors saved.", "Success");
            }

        }

		private void simpleButtonDirOffset_Click(object sender, EventArgs e) {
			if (_anemNid < 0) {
				MessageBox.Show("Select a sensor.", "Select a sensor.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return;
			}

			var factorsText = textEditFactors.Text;
			if (String.IsNullOrEmpty(factorsText)) {
				MessageBox.Show("You must first get the correction factors.", "Invalid value.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return;
			}

			CorrectionFactors factors = CorrectionFactors.FromString(factorsText);
			var currentOffset = factors.windDirectionOffset;

			var sensor = _device.GetSensor(_anemNid);
			if(null == sensor) {
				MessageBox.Show("Invalid sensor.", "Invalid sensor.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return;
			}

			var reading = sensor.GetCurrentReading();
			var isWindDirValid = reading.IsWindDirectionValid;
			if (!isWindDirValid) {
				MessageBox.Show("Invalid wind direction value.", "Invalid value.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return;
			}

			var currentWindDirection = reading.WindDirection;
			var desiredWindDirection = (double)spinEditDesiredDirection.Value;

			var newOffset = CorrectionFactors.CalculateWindDirectionOffset(currentWindDirection, desiredWindDirection, currentOffset);

			factors.windDirectionOffset = newOffset;
			textEditFactors.Text = factors.ToString();
			MessageBox.Show("Applying the correction factor change was successful.", "Success.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

		}

    }
}
