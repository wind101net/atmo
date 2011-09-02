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
using System.Drawing;
using DevExpress.XtraEditors;

namespace Atmo.UI.DevEx {
	public partial class AboutForm : XtraForm {

		public AboutForm() {
			InitializeComponent();
			LoadBackground(ProgramContext.SplashBackground);
			AboutForm_BackColorChanged(null, null);
			labelControlVersion.Text = String.Format(
				"Version: {0}",
				typeof(AboutForm).Assembly.GetName().Version
			);
		}


		protected void LoadBackground(string filePath) {
			try {
				var i = Image.FromFile(filePath);
				BackgroundImage = i;
				ClientSize = new Size(i.Width, i.Height);
			}
			catch (Exception ex) {
				;
			}
		}

		protected void AboutForm_BackColorChanged(object sender, EventArgs e) {
			labelControlVersion.BackColor = BackColor;
		}
	}
}