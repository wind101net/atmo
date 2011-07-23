using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace Atmo.UI.DevEx.Controls {
	public partial class LiveAtmosphericHeader : DevExpress.XtraEditors.XtraUserControl {
		public LiveAtmosphericHeader() {
			InitializeComponent();
		}

		public CustomTimeRangeSelector TimeRange {
			get { return customTimeRangeSelector1; }
		}
	}
}
