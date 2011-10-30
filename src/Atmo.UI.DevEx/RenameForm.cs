using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace Atmo.UI.DevEx {
	public partial class RenameForm : XtraForm {

		public RenameForm() {
			InitializeComponent();
		}

		public string Value {
			get { return textEditName.Text; }
			set { textEditName.Text = value; }
		}

		private void simpleButtonOk_Click(object sender, EventArgs e) {
			DialogResult = DialogResult.OK;
		}

		private void simpleButtonCancel_Click(object sender, EventArgs e) {
			DialogResult = DialogResult.Cancel;
		}
	}
}