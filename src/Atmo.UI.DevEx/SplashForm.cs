using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Atmo.UI.DevEx {
	public partial class SplashForm : Form {

		private readonly MainForm _spawnForm;

		public SplashForm(MainForm spawnForm) {
			_spawnForm = spawnForm;
			InitializeComponent();
		}
	}
}
