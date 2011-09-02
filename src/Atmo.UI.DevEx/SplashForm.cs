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
using System.Windows.Forms;

namespace Atmo.UI.DevEx {
	public partial class SplashForm : AboutForm {

		private readonly Color targetColor = Color.White;
		private readonly int increment = 16;
		private readonly double opacDec = 0.1;


		public SplashForm(MainForm spawnForm) {
			SpawnForm = spawnForm;
			Text = ProgramContext.ProgramFriendlyName;
			InitializeComponent();
			AboutForm_BackColorChanged(null, null);
			LoadBackground(ProgramContext.SplashBackground);
		}

		

		public MainForm SpawnForm { get; private set; }

		private void timerFadeIn_Tick(object sender, System.EventArgs e) {
			Color curColor = BackColor;
			if (curColor.R >= targetColor.R && curColor.G >= targetColor.G && curColor.B >= targetColor.B) {
				if (!SpawnForm.Visible) {
					SpawnForm.Show();
				}
				if (SpawnForm.Created) {
					timerFadeIn.Stop();
					timerFadeOut.Enabled = true;
				}
			}
			else {
				BackColor = Color.FromArgb(
					System.Math.Min(255, curColor.R + increment),
					System.Math.Min(255, curColor.G + increment),
					System.Math.Min(255, curColor.B + increment)
				);
			}
		}

		private void timerFadeOut_Tick(object sender, System.EventArgs e) {
			double opac = Opacity;
			opac = System.Math.Max(0, opac - opacDec);
			Opacity = opac;
			if (opac <= 0) {
				timerFadeOut.Enabled = false;
				Close();
			}
		}

		

		private void SplashForm_Shown(object sender, System.EventArgs e) {
			timerFadeIn.Enabled = true;
			timerFadeIn_Tick(null, null);
		}
	}
}
