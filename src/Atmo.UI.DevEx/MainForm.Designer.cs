namespace Atmo.UI.DevEx {
	partial class MainForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.barManager = new DevExpress.XtraBars.BarManager(this.components);
			this.bar2 = new DevExpress.XtraBars.Bar();
			this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
			this.barButtonItemImport = new DevExpress.XtraBars.BarButtonItem();
			this.barButtonItemExport = new DevExpress.XtraBars.BarButtonItem();
			this.barButtonItemExit = new DevExpress.XtraBars.BarButtonItem();
			this.barSubItem3 = new DevExpress.XtraBars.BarSubItem();
			this.barButtonItemPrefs = new DevExpress.XtraBars.BarButtonItem();
			this.barButtonItemSensorSetup = new DevExpress.XtraBars.BarButtonItem();
			this.barButtonItemFirmwareUpdate = new DevExpress.XtraBars.BarButtonItem();
			this.barButtonItemTimeCorrection = new DevExpress.XtraBars.BarButtonItem();
			this.barSubItem4 = new DevExpress.XtraBars.BarSubItem();
			this.barButtonItemDoc = new DevExpress.XtraBars.BarButtonItem();
			this.barButtonItemUpdates = new DevExpress.XtraBars.BarButtonItem();
			this.barButtonItemSupport = new DevExpress.XtraBars.BarButtonItem();
			this.barButtonItemAbout = new DevExpress.XtraBars.BarButtonItem();
			this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
			this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
			this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
			this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
			this.barSubItem5 = new DevExpress.XtraBars.BarSubItem();
			this.liveAtmosphericGraph = new Atmo.UI.DevEx.Controls.LiveAtmosphericGraph();
			this.timerTesting = new System.Windows.Forms.Timer(this.components);
			this.panelSensors = new System.Windows.Forms.Panel();
			this.panelJunk = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.simpleButtonFindSensors = new DevExpress.XtraEditors.SimpleButton();
			this.defaultLookAndFeel = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
			this.simpleButtonDownloadData = new DevExpress.XtraEditors.SimpleButton();
			((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
			this.panelSensors.SuspendLayout();
			this.panelJunk.SuspendLayout();
			this.SuspendLayout();
			// 
			// barManager
			// 
			this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar2});
			this.barManager.DockControls.Add(this.barDockControlTop);
			this.barManager.DockControls.Add(this.barDockControlBottom);
			this.barManager.DockControls.Add(this.barDockControlLeft);
			this.barManager.DockControls.Add(this.barDockControlRight);
			this.barManager.Form = this;
			this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barSubItem1,
            this.barSubItem3,
            this.barSubItem4,
            this.barSubItem5,
            this.barButtonItemExport,
            this.barButtonItemImport,
            this.barButtonItemPrefs,
            this.barButtonItemFirmwareUpdate,
            this.barButtonItemTimeCorrection,
            this.barButtonItemSensorSetup,
            this.barButtonItemDoc,
            this.barButtonItemUpdates,
            this.barButtonItemSupport,
            this.barButtonItemAbout,
            this.barButtonItemExit});
			this.barManager.MainMenu = this.bar2;
			this.barManager.MaxItemId = 17;
			// 
			// bar2
			// 
			this.bar2.BarName = "Main menu";
			this.bar2.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Top;
			this.bar2.DockCol = 0;
			this.bar2.DockRow = 0;
			this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
			this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem3),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem4)});
			this.bar2.OptionsBar.AllowQuickCustomization = false;
			this.bar2.OptionsBar.DisableClose = true;
			this.bar2.OptionsBar.DisableCustomization = true;
			this.bar2.OptionsBar.DrawDragBorder = false;
			this.bar2.OptionsBar.UseWholeRow = true;
			this.bar2.Text = "Main menu";
			// 
			// barSubItem1
			// 
			this.barSubItem1.Caption = "File";
			this.barSubItem1.Id = 0;
			this.barSubItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemImport),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemExport),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemExit, true)});
			this.barSubItem1.Name = "barSubItem1";
			// 
			// barButtonItemImport
			// 
			this.barButtonItemImport.Caption = "Download Sensor Data...";
			this.barButtonItemImport.Id = 7;
			this.barButtonItemImport.Name = "barButtonItemImport";
			this.barButtonItemImport.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemImport_ItemClick);
			// 
			// barButtonItemExport
			// 
			this.barButtonItemExport.Caption = "Export...";
			this.barButtonItemExport.Id = 6;
			this.barButtonItemExport.Name = "barButtonItemExport";
			// 
			// barButtonItemExit
			// 
			this.barButtonItemExit.Caption = "Exit";
			this.barButtonItemExit.Id = 16;
			this.barButtonItemExit.Name = "barButtonItemExit";
			// 
			// barSubItem3
			// 
			this.barSubItem3.Caption = "Tools";
			this.barSubItem3.Id = 2;
			this.barSubItem3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemPrefs),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemSensorSetup, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemFirmwareUpdate),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemTimeCorrection, true)});
			this.barSubItem3.Name = "barSubItem3";
			// 
			// barButtonItemPrefs
			// 
			this.barButtonItemPrefs.Caption = "Preferences...";
			this.barButtonItemPrefs.Id = 8;
			this.barButtonItemPrefs.Name = "barButtonItemPrefs";
			this.barButtonItemPrefs.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemPrefs_ItemClick);
			// 
			// barButtonItemSensorSetup
			// 
			this.barButtonItemSensorSetup.Caption = "Find Sensors...";
			this.barButtonItemSensorSetup.Id = 11;
			this.barButtonItemSensorSetup.Name = "barButtonItemSensorSetup";
			this.barButtonItemSensorSetup.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemSensorSetup_ItemClick);
			// 
			// barButtonItemFirmwareUpdate
			// 
			this.barButtonItemFirmwareUpdate.Caption = "Firmware Update...";
			this.barButtonItemFirmwareUpdate.Id = 9;
			this.barButtonItemFirmwareUpdate.Name = "barButtonItemFirmwareUpdate";
			// 
			// barButtonItemTimeCorrection
			// 
			this.barButtonItemTimeCorrection.Caption = "Database Time Correction...";
			this.barButtonItemTimeCorrection.Id = 10;
			this.barButtonItemTimeCorrection.Name = "barButtonItemTimeCorrection";
			// 
			// barSubItem4
			// 
			this.barSubItem4.Caption = "Help";
			this.barSubItem4.Id = 3;
			this.barSubItem4.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemDoc),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemUpdates),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemSupport),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemAbout, true)});
			this.barSubItem4.Name = "barSubItem4";
			// 
			// barButtonItemDoc
			// 
			this.barButtonItemDoc.Caption = "Documentation";
			this.barButtonItemDoc.Id = 12;
			this.barButtonItemDoc.Name = "barButtonItemDoc";
			// 
			// barButtonItemUpdates
			// 
			this.barButtonItemUpdates.Caption = "Check for Updates";
			this.barButtonItemUpdates.Id = 13;
			this.barButtonItemUpdates.Name = "barButtonItemUpdates";
			// 
			// barButtonItemSupport
			// 
			this.barButtonItemSupport.Caption = "Contact Support";
			this.barButtonItemSupport.Id = 14;
			this.barButtonItemSupport.Name = "barButtonItemSupport";
			// 
			// barButtonItemAbout
			// 
			this.barButtonItemAbout.Caption = "About...";
			this.barButtonItemAbout.Id = 15;
			this.barButtonItemAbout.Name = "barButtonItemAbout";
			// 
			// barDockControlTop
			// 
			this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
			this.barDockControlTop.Size = new System.Drawing.Size(994, 24);
			// 
			// barDockControlBottom
			// 
			this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.barDockControlBottom.Location = new System.Drawing.Point(0, 1287);
			this.barDockControlBottom.Size = new System.Drawing.Size(994, 0);
			// 
			// barDockControlLeft
			// 
			this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
			this.barDockControlLeft.Location = new System.Drawing.Point(0, 24);
			this.barDockControlLeft.Size = new System.Drawing.Size(0, 1263);
			// 
			// barDockControlRight
			// 
			this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
			this.barDockControlRight.Location = new System.Drawing.Point(994, 24);
			this.barDockControlRight.Size = new System.Drawing.Size(0, 1263);
			// 
			// barSubItem5
			// 
			this.barSubItem5.Caption = "Data";
			this.barSubItem5.Id = 5;
			this.barSubItem5.Name = "barSubItem5";
			// 
			// liveAtmosphericGraph
			// 
			this.liveAtmosphericGraph.ConverterCacheReadingValues = null;
			this.liveAtmosphericGraph.HeightAboveSeaLevel = 0D;
			this.liveAtmosphericGraph.Location = new System.Drawing.Point(248, 144);
			this.liveAtmosphericGraph.Name = "liveAtmosphericGraph";
			this.liveAtmosphericGraph.PressureUnit = Atmo.Units.PressureUnit.Pascals;
			this.liveAtmosphericGraph.Size = new System.Drawing.Size(677, 410);
			this.liveAtmosphericGraph.SpeedUnit = Atmo.Units.SpeedUnit.MetersPerSec;
			this.liveAtmosphericGraph.State = null;
			this.liveAtmosphericGraph.TabIndex = 4;
			this.liveAtmosphericGraph.TemperatureUnit = Atmo.Units.TemperatureUnit.Celsius;
			// 
			// timerTesting
			// 
			this.timerTesting.Enabled = true;
			this.timerTesting.Interval = 1000;
			this.timerTesting.Tick += new System.EventHandler(this.timerTesting_Tick);
			// 
			// panelSensors
			// 
			this.panelSensors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.panelSensors.AutoScroll = true;
			this.panelSensors.AutoScrollMinSize = new System.Drawing.Size(180, 0);
			this.panelSensors.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelSensors.Controls.Add(this.panelJunk);
			this.panelSensors.Location = new System.Drawing.Point(0, 88);
			this.panelSensors.Name = "panelSensors";
			this.panelSensors.Size = new System.Drawing.Size(205, 1187);
			this.panelSensors.TabIndex = 13;
			// 
			// panelJunk
			// 
			this.panelJunk.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.panelJunk.Controls.Add(this.label1);
			this.panelJunk.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelJunk.Location = new System.Drawing.Point(0, 0);
			this.panelJunk.Name = "panelJunk";
			this.panelJunk.Size = new System.Drawing.Size(203, 1185);
			this.panelJunk.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(203, 1185);
			this.label1.TabIndex = 0;
			this.label1.Text = "This control intentionally left blank";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// simpleButtonFindSensors
			// 
			this.simpleButtonFindSensors.Location = new System.Drawing.Point(1, 30);
			this.simpleButtonFindSensors.Name = "simpleButtonFindSensors";
			this.simpleButtonFindSensors.Size = new System.Drawing.Size(205, 23);
			this.simpleButtonFindSensors.TabIndex = 18;
			this.simpleButtonFindSensors.Text = "Find Sensors...";
			this.simpleButtonFindSensors.Click += new System.EventHandler(this.simpleButtonFindSensors_Click);
			// 
			// defaultLookAndFeel
			// 
			this.defaultLookAndFeel.LookAndFeel.SkinName = "Money Twins";
			// 
			// simpleButtonDownloadData
			// 
			this.simpleButtonDownloadData.Location = new System.Drawing.Point(1, 59);
			this.simpleButtonDownloadData.Name = "simpleButtonDownloadData";
			this.simpleButtonDownloadData.Size = new System.Drawing.Size(205, 23);
			this.simpleButtonDownloadData.TabIndex = 23;
			this.simpleButtonDownloadData.Text = "Download Data...";
			this.simpleButtonDownloadData.Click += new System.EventHandler(this.simpleButtonDownloadData_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(994, 1287);
			this.Controls.Add(this.liveAtmosphericGraph);
			this.Controls.Add(this.simpleButtonDownloadData);
			this.Controls.Add(this.simpleButtonFindSensors);
			this.Controls.Add(this.panelSensors);
			this.Controls.Add(this.barDockControlLeft);
			this.Controls.Add(this.barDockControlRight);
			this.Controls.Add(this.barDockControlBottom);
			this.Controls.Add(this.barDockControlTop);
			this.Name = "MainForm";
			this.Text = "MainForm";
			((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
			this.panelSensors.ResumeLayout(false);
			this.panelJunk.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraBars.BarManager barManager;
		private DevExpress.XtraBars.Bar bar2;
		private DevExpress.XtraBars.BarSubItem barSubItem1;
		private DevExpress.XtraBars.BarSubItem barSubItem3;
		private DevExpress.XtraBars.BarSubItem barSubItem4;
		private DevExpress.XtraBars.BarDockControl barDockControlTop;
		private DevExpress.XtraBars.BarDockControl barDockControlBottom;
		private DevExpress.XtraBars.BarDockControl barDockControlLeft;
		private DevExpress.XtraBars.BarDockControl barDockControlRight;
		private DevExpress.XtraBars.BarButtonItem barButtonItemImport;
		private DevExpress.XtraBars.BarButtonItem barButtonItemExport;
		private DevExpress.XtraBars.BarButtonItem barButtonItemExit;
		private DevExpress.XtraBars.BarButtonItem barButtonItemPrefs;
		private DevExpress.XtraBars.BarButtonItem barButtonItemSensorSetup;
		private DevExpress.XtraBars.BarButtonItem barButtonItemFirmwareUpdate;
		private DevExpress.XtraBars.BarButtonItem barButtonItemTimeCorrection;
		private DevExpress.XtraBars.BarButtonItem barButtonItemDoc;
		private DevExpress.XtraBars.BarButtonItem barButtonItemUpdates;
		private DevExpress.XtraBars.BarButtonItem barButtonItemSupport;
		private DevExpress.XtraBars.BarButtonItem barButtonItemAbout;
		private DevExpress.XtraBars.BarSubItem barSubItem5;
		private Controls.LiveAtmosphericGraph liveAtmosphericGraph;
		private System.Windows.Forms.Timer timerTesting;
		private System.Windows.Forms.Panel panelSensors;
		private System.Windows.Forms.Panel panelJunk;
		private System.Windows.Forms.Label label1;
		private DevExpress.XtraEditors.SimpleButton simpleButtonFindSensors;
		private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel;
		private DevExpress.XtraEditors.SimpleButton simpleButtonDownloadData;

	}
}