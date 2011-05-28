using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace Atmo.UI.DevEx.Controls {
	public partial class SensorView : DevExpress.XtraEditors.XtraUserControl {

		private bool _selected;

		public SensorView() {
			InitializeComponent();
		}

		public bool IsSelected {
			get { return _selected; }
			set { _selected = value; SetBackgroundColor(); }
		}

		private void SetBackgroundColor() {
			BackColor = (IsSelected) ? SystemColors.Highlight : Color.Transparent;
			ForeColor = (IsSelected) ? SystemColors.ControlText : SystemColors.InactiveCaptionText;
			//this.sensorNameLabel.ForeColor = this.ForeColor;
			//this.tableLayoutPanel1.ForeColor = this.ForeColor;
			//foreach (Control c in this.tableLayoutPanel1.Controls.OfType<Control>()) {
			//	c.ForeColor = this.ForeColor;
			//}
		}
	}
}
