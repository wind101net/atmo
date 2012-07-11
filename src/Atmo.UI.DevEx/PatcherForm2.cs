using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Atmo.Daq.Win32;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using Atmo.Device;

namespace Atmo.UI.DevEx
{
	public partial class PatcherForm2 : PatcherForm
	{
		protected PatcherForm2() : this(null) { }

		public PatcherForm2(UsbDaqConnection device) : base(device) {
			InitializeComponent();
			AnemSelectionRadioItems.Properties.Items.Clear();
			AnemSelectionRadioItems.Properties.Items.AddRange(new [] {
				new RadioGroupItem(1, "A"),
				new RadioGroupItem(2, "B"),
				new RadioGroupItem(3, "C"),
				new RadioGroupItem(4, "D"),
				new RadioGroupItem(0, "Unassigned"),
				new RadioGroupItem(0xf, "Corrupted"), 
			});

		}

		protected override bool ProgramAnem(int nid, MemoryRegionDataCollection memoryRegionDataBlocks, Action<double, string> progressUpdated) {
			return Device.ProgramAnemVersion2(nid, memoryRegionDataBlocks, progressUpdated);
		}


	}
}