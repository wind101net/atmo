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
using DevExpress.XtraEditors;

namespace Atmo.UI.DevEx.Controls {
	public partial class HistoricalTimeSelectHeader : XtraUserControl {
		public HistoricalTimeSelectHeader() {
			InitializeComponent();
			checkEdit_CheckedChanged(null, null);
		}

		public event Action OnTimeRangeChanged;

		public string HeaderText {
			get { return groupControlHistoricHeader.Text; }
			set { groupControlHistoricHeader.Text = value; }
		}

		public DateEdit DateEdit {
			get { return dateEdit; }
		}

		public TimeEdit TimeEdit {
			get { return timeEdit; }
		}

		public CustomTimeRangeSelector TimeRange {
			get { return customTimeRangeSelector; }
		}

		public CheckEdit CheckEdit {
			get { return checkEdit; }
		}

		private void checkEdit_CheckedChanged(object sender, EventArgs e) {
			if (checkEdit.Checked) {
				var cumulativeTimeSpan = TimeRange.SelectedSpan;
				var drillStartDate = DateTime.Now.Subtract(cumulativeTimeSpan);
				DateEdit.DateTime = drillStartDate.Date;
				DateEdit.Enabled = false;
				TimeEdit.Time = drillStartDate;
				TimeEdit.Enabled = false;
			}
			else {
				DateEdit.Enabled = true;
				TimeEdit.Enabled = (TimeRange.SelectedSpan <= new TimeSpan(1, 0, 0, 0));
			}
			if(null != OnTimeRangeChanged) {
				OnTimeRangeChanged();
			}
		}

		private void customTimeRangeSelector_RangeSlider_ValueChanged(object sender, EventArgs e) {
			checkEdit_CheckedChanged(null, null);
			if (null != OnTimeRangeChanged) {
				OnTimeRangeChanged();
			}
		}

		private void dateEdit_EditValueChanged(object sender, EventArgs e) {
			if (null != OnTimeRangeChanged) {
				OnTimeRangeChanged();
			}
		}

		private void timeEdit_EditValueChanged(object sender, EventArgs e) {
			if (null != OnTimeRangeChanged) {
				OnTimeRangeChanged();
			}
		}

		private void customTimeRangeSelector_Load(object sender, EventArgs e) {
			if (null != OnTimeRangeChanged) {
				OnTimeRangeChanged();
			}
		}


	}
}
