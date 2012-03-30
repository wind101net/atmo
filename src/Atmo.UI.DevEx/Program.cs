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
using System.Configuration;
using System.Windows.Forms;
using System.IO;
using Atmo.Data;
using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace Atmo.UI.DevEx {

	public class ProgramContext : ApplicationContext {

		private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		private const string PersistentFileName = "config.xml";

		public static string AssemblyDirectory {
			get { return Path.GetDirectoryName(typeof (ProgramContext).Assembly.Location); }
		}

		public static string PersistentStateFileName {
			get { return Path.Combine(AssemblyDirectory, PersistentFileName); }
		}

		public static string GetAppConfigSetting(string keyName) {
			return ConfigurationManager.AppSettings.Get(keyName);
		}

		public static string GetAppConfigSetting(string keyName, string defaultValue) {
			var result = GetAppConfigSetting(keyName);
			return String.IsNullOrEmpty(result) ? defaultValue : result;
		}

		public static string ProgramFriendlyName {
			get { return GetAppConfigSetting("programName", "Atmo 2"); }
		}

		public static string SplashBackground {
			get { return GetAppConfigSetting("splashBackground", "atmoSplash.png"); }
		}

		private const string BaraniDesignSupportLinkValue = "www.baranidesign.com/support";

		public static string DocumentationLink {
			get { return GetAppConfigSetting("documentationLink", BaraniDesignSupportLinkValue); }
		}

		public static string UpdateLink {
			get { return GetAppConfigSetting("updateLink", BaraniDesignSupportLinkValue); }
		}

		public static string ContactLink {
			get { return GetAppConfigSetting("contactLink", BaraniDesignSupportLinkValue); }
		}

		public static bool IsDemoMode {
			get {
				bool b;
				return bool.TryParse(GetAppConfigSetting("demoMode"), out b) && b;
			}
		}

		private readonly SplashForm _splashForm;
		private readonly MainForm _mainForm;
		private PersistentState _state;

		public ProgramContext() {
			_mainForm = new MainForm(this);
			_splashForm = new SplashForm(_mainForm);
			_splashForm.Closing += _splashForm_Closing;
		}

		void _splashForm_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			MainForm = _mainForm;
			if(!MainForm.Visible) {
				MainForm.Show();
			}
		}

		public void Start() {
			_splashForm.Show(_mainForm);
		}

		protected override void OnMainFormClosed(object sender, EventArgs e) {
			SaveState();
			base.OnMainFormClosed(sender, e);
		}

		public PersistentState PersistentState {
			get {
				return _state
					?? (_state = PersistentState.ReadFile(PersistentStateFileName) ?? new PersistentState());
			}
		}

		public void SaveState() {
			try {
				PersistentState.SaveFile(PersistentStateFileName, PersistentState);
			}
			catch(Exception ex) {
				Log.ErrorFormat("Unable to save state to {0}:\nState:{1}\nEx:{2}", PersistentFileName, PersistentState, ex);
			}
		}

		protected override void Dispose(bool disposing) {
			MainForm = _mainForm;
			_mainForm.Dispose();
			base.Dispose(disposing);
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
			using (var context = new ProgramContext()) {
				context.Start();
				Application.Run(context);
			}
		}

	}
}
