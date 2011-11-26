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
using System.Threading;
using System.Windows.Forms;
using Atmo.UI.DevEx.Controls;

namespace Atmo.UI.DevEx {
    public partial class FindSensorsDialog : DevExpress.XtraEditors.XtraForm {

        private SensorSetupLine[] _sensorLines;
        private IDaqConnection _device;


        private enum PendingOperation {
            None,
            Add,
            Rem
        }

        private PendingOperation _pendingOperation = PendingOperation.None;
        private int _pendingSensorIndex;

		public FindSensorsDialog(IDaqConnection device) {
            InitializeComponent();
            _sensorLines = new[]{
                sensorSetupLine1,
                sensorSetupLine2,
                sensorSetupLine3,
                sensorSetupLine4
            };
            for (int i = 0; i < _sensorLines.Length; i++) {
                _sensorLines[i].SensorId = i;
            }
            _device = device;
            _device.SetNetworkSize(_sensorLines.Length);
        }

        private void updateTimer_Tick(object sender, EventArgs e) {
            if (_device.IsConnected) {
                bool hasAnAddLine = false;
                for (int i = 0; i < _sensorLines.Length; i++) {
                    SensorSetupLine setupLine = _sensorLines[i];
                    ISensor sensor = _device.GetSensor(i);
                    IReading reading = sensor.IsValid ? sensor.GetCurrentReading() : null;
                    if (null != reading && reading.IsValid) {
                        setupLine.Status = SensorStatus.Connected;
                        setupLine.Enabled = true;
                    }
                    else {
                        setupLine.Status = SensorStatus.Disconnected;
                        setupLine.Enabled = !hasAnAddLine;
                        hasAnAddLine = true;
                    }
                }
            }
            else {
                for (int i = 0; i < _sensorLines.Length; i++) {
                    _sensorLines[i].Status = SensorStatus.Unknown;
                    _sensorLines[i].Enabled = false;
                }
            }
        }

        private void sensorSetupLine_SensorRemove(int obj) {
            if (_pendingOperation == PendingOperation.None) {
                if (DialogResult.Yes == MessageBox.Show("Are you sure you wish to remove this sensor?", "Confirm", MessageBoxButtons.YesNoCancel)) {
                    _device.SetSensorId(obj, -1);
                }
                _pendingOperation = PendingOperation.Rem;
                _pendingSensorIndex = obj;
                addRemWorker.RunWorkerAsync();
            }

        }

        private void sensorSetupLine_SensorSet(int obj) {
            if (_pendingOperation == PendingOperation.None) {
                if (DialogResult.OK == MessageBox.Show("Make sure only one unassigned sensor is connected.", "Verify", MessageBoxButtons.OKCancel)) {
                    _device.SetSensorId(-1, obj);
                }
                _pendingOperation = PendingOperation.Add;
                _pendingSensorIndex = obj;
                addRemWorker.RunWorkerAsync();
            }
        }

        private void SensorSetupDialog_FormClosing(object sender, FormClosingEventArgs e) {
            if (null != _device && _device.IsConnected) {
                int highestIndex = -1;
                for (int i = 0; i < _sensorLines.Length; i++) {
                    SensorSetupLine setupLine = _sensorLines[i];
                    ISensor sensor = _device.GetSensor(i);
                    IReading reading = sensor.IsValid ? sensor.GetCurrentReading() : null;
                    if (null != reading && reading.IsValid && i > highestIndex) {
                        highestIndex = i;
                    }
                }
                if (highestIndex < 0) {
                    highestIndex = _sensorLines.Length - 1;
                }
                _device.SetNetworkSize(highestIndex + 1);
            }
        }

        private void addRemWorker_DoWork(object sender, DoWorkEventArgs e) {
            TimeSpan waitTime = new TimeSpan(0, 0, 10);
            DateTime now = DateTime.Now;
            DateTime endTime = now.Add(waitTime);
            while ((now = DateTime.Now) <= endTime) {
                int percent = (int)((endTime.Subtract(now).Ticks * 100) / waitTime.Ticks);
                addRemWorker.ReportProgress(percent);
                ISensor sensor = _device.GetSensor(_pendingSensorIndex);
                if (_pendingOperation == PendingOperation.Add) {
                    if (null != sensor && sensor.IsValid) {
                        break;
                    }
                }
                else if (_pendingOperation == PendingOperation.Rem) {
                    if (null == sensor || !sensor.IsValid) {
                        break;
                    }
                }
                Thread.Sleep(100);
            }

        }

        private void addRemWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
			ISensor sensor = _device.GetSensor(_pendingSensorIndex);

        	MessageBoxIcon resultIcon;
        	string resultTitle;
        	string resultMessage = "Operation";
        	bool sensorIsDesiredToBeFound = true;

			if (_pendingOperation == PendingOperation.Add) {
				resultMessage = "Sensor assignment";
			}
			else if (_pendingOperation == PendingOperation.Rem) {
				resultMessage = "Sensor removal";
				sensorIsDesiredToBeFound = false;
			}

			Thread.Sleep(500); // wait for the sensor to be active

        	var sensorFound = null != sensor && sensor.IsValid;

			if (sensorFound == sensorIsDesiredToBeFound) {
				resultTitle = "Success";
				resultMessage += " successful";
				resultIcon = MessageBoxIcon.Information;
			}
			else {
				resultTitle = "Failure";
				resultMessage += " failed";
				resultIcon = MessageBoxIcon.Error;
			}

			_pendingOperation = PendingOperation.None;
			MessageBox.Show(this, resultTitle, resultMessage, MessageBoxButtons.OK, resultIcon);
        }

        private void addRemWorker_ProgressChanged(object sender, ProgressChangedEventArgs e) {

        }

    }
}