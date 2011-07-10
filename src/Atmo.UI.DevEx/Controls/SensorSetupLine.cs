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

namespace Atmo.UI.DevEx.Controls {
    public partial class SensorSetupLine : DevExpress.XtraEditors.XtraUserControl {

        private int _sensorId;
        private SensorStatus _status = SensorStatus.Unknown;

        public event Action<int> SensorRemove;
        public event Action<int> SensorSet;

        public SensorSetupLine() : this(0) { }

        public SensorSetupLine(int sensorId) {
            InitializeComponent();
            SensorId = sensorId;
            Status = SensorStatus.Unknown;
        }

        public int SensorId {
            get { return _sensorId; }
            set { _sensorId = value; UpdateSensorName(); }
        }

        public char SensorLetter {
            get { return (char)(((byte)'A') + _sensorId); }
        }

        private void UpdateSensorName() {
            nameLabel.Text = String.Concat("Sensor ", SensorLetter);
        }

        public SensorStatus Status {
            get {
                return _status;
            }
            set {
                _status = value;
                UpdateSensorButton();
            }
        }

        private void UpdateSensorButton() {
            if (_status == SensorStatus.Connected) {
                actionButton.Text = "Remove";
            }
            else if (_status == SensorStatus.Disconnected) {
                actionButton.Text = "Add";
            }
            else {
                actionButton.Text = "Unknown";
            }
        }

        private void actionButton_Click(object sender, EventArgs e) {
            if (_status == SensorStatus.Connected) {
                if (null != SensorRemove) {
                    SensorRemove.Invoke(_sensorId);
                }
            }
            else if (_status == SensorStatus.Disconnected) {
                if (null != SensorSet) {
                    SensorSet.Invoke(_sensorId);
                }
            }
        }

        public bool Enabled {
            get {
                return actionButton.Enabled;
            }
            set {
                if (actionButton.Enabled != value) {
                    actionButton.Enabled = value;
                }
            }
        }

    }
}
