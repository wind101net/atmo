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
using System.Windows.Forms;
using System.IO;
using Atmo.Data;

namespace Atmo.UI.DevEx {

	public class ProgramContext : ApplicationContext {

		private const string PersistentFileName = "config.xml";

		public static string AssemblyDirectory {
			get { return Path.GetDirectoryName(typeof (ProgramContext).Assembly.Location); }
		}

		public static string PersistentStateFileName {
			get { return Path.Combine(AssemblyDirectory, PersistentFileName); }
		}

		private readonly SplashForm _splashForm;
		private readonly MainForm _mainForm;
		private PersistentState _state;

		public ProgramContext() {
			_mainForm = new MainForm(this);
			_mainForm.Closed += OnMainFormClosed;
			_splashForm = new SplashForm(_mainForm);
			_splashForm.Show(_mainForm);

			// TODO: replace with fade in
			_mainForm.Show();
			_splashForm.Close();
		}

		protected override void OnMainFormClosed(object sender, EventArgs e) {
			SaveState();
			base.OnMainFormClosed(sender, e);
		}

		public PersistentState PersistentState {
			get {
				return _state ?? (
					_state = (
						PersistentState.ReadFile(PersistentStateFileName)
						?? new PersistentState()
					)
				);
			}
		}

		public void SaveState() {
			try {
				PersistentState.SaveFile(PersistentStateFileName, PersistentState);
			}catch(Exception e) {
				; // should maybe get a logger in here
			}
		}

	}

	static class Program {

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {

			var appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
			if (!String.IsNullOrEmpty(appPath)) {
				Directory.SetCurrentDirectory(appPath);
			}

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			var context = new ProgramContext();
			Application.Run(context);
		}

	}
}
