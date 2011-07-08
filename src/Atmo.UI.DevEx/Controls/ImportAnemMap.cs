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
    public partial class ImportAnemMap : DevExpress.XtraEditors.XtraUserControl {

        private static string anemIdLabelText = "Import from Anem. {0} into";

        private bool _enabled;
        private string _anemId;
        

        public ImportAnemMap() {
            InitializeComponent();
            this.Enabled = true;
        }

        public bool Enabled {
            get {
                return _enabled;
            }
            set {
                _enabled = value;
                this.checkEditLoad.Enabled = _enabled;
                this.comboBoxEditImportName.Enabled = _enabled;
                this.dateEdit.Enabled = _enabled;
                this.timeEdit.Enabled = _enabled;
            }
        }

        public DateTime StartStamp {
            get {
                return dateEdit.DateTime.Date.Add(timeEdit.Time.TimeOfDay);
            }
            set {
                dateEdit.DateTime = value.Date;
                timeEdit.Time = value;
            }
        }

        public string AnemId {
            get {
                return _anemId;
            }
            set {
                _anemId = value;
                this.labelAnemId.Text = String.Format(anemIdLabelText, String.IsNullOrEmpty(_anemId) ? "N/A" : _anemId);
            }
        }

        public string DatabaseSensorId {
            get {
                return this.comboBoxEditImportName.Text;
            }
            set {
                this.comboBoxEditImportName.Text = value;
            }
        }

        public bool Checked {
            get {
                return this.checkEditLoad.Checked;
            }
            set {
                this.checkEditLoad.Checked = value;
            }
        }

        public void SetNameSuggestions(string[] names) {

            this.comboBoxEditImportName.Properties.Items.Clear();
            this.comboBoxEditImportName.Properties.Items.AddRange(names);

        }

        public bool IsValid {
            get {
                return this.Enabled && this.Checked && !String.IsNullOrEmpty(AnemId) && !String.IsNullOrEmpty(DatabaseSensorId);
            }
        }

        private void comboBoxEditImportName_SelectedIndexChanged(object sender, EventArgs e) {

        }

    }
}
