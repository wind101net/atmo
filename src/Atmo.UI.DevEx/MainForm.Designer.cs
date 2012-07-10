namespace Atmo.UI.DevEx {
	partial class MainForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
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
			this.barSubItemDebug = new DevExpress.XtraBars.BarSubItem();
			this.barButtonItemAdd24Hours = new DevExpress.XtraBars.BarButtonItem();
			this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
			this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
			this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
			this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
			this.barSubItem5 = new DevExpress.XtraBars.BarSubItem();
			this.barButtonItemTimeSync = new DevExpress.XtraBars.BarButtonItem();
			this.liveAtmosphericGraph = new Atmo.UI.DevEx.Controls.LiveAtmosphericGraph();
			this.timerLive = new System.Windows.Forms.Timer(this.components);
			this.panelSensors = new System.Windows.Forms.Panel();
			this.groupControlDbList = new DevExpress.XtraEditors.GroupControl();
			this.groupControlSensors = new DevExpress.XtraEditors.GroupControl();
			this.simpleButtonFindSensors = new DevExpress.XtraEditors.SimpleButton();
			this.defaultLookAndFeel = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
			this.simpleButtonDownloadData = new DevExpress.XtraEditors.SimpleButton();
			this.windResourceGraph = new Atmo.UI.DevEx.Controls.WindResourceGraph();
			this.historicalGraphBreakdown = new Atmo.UI.DevEx.Controls.HistoricalGraphBreakdown();
			this.historicalTimeSelectHeader = new Atmo.UI.DevEx.Controls.HistoricalTimeSelectHeader();
			this.liveAtmosphericHeader = new Atmo.UI.DevEx.Controls.LiveAtmosphericHeader();
			this.mainScrollableControl = new DevExpress.XtraEditors.XtraScrollableControl();
			this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
			this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
			this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
			this.labelVolUsb = new DevExpress.XtraEditors.LabelControl();
			this.labelTmpDaq = new DevExpress.XtraEditors.LabelControl();
			this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
			this.labelLocalTime = new DevExpress.XtraEditors.LabelControl();
			this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
			this.labelVolBat = new DevExpress.XtraEditors.LabelControl();
			this.labelDaqTime = new DevExpress.XtraEditors.LabelControl();
			this.timerQueryTime = new System.Windows.Forms.Timer(this.components);
			this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
			this.labelControlPwsStatus = new DevExpress.XtraEditors.LabelControl();
			this.simpleButtonPwsAction = new DevExpress.XtraEditors.SimpleButton();
			this.timerRapidFire = new System.Windows.Forms.Timer(this.components);
			this.simpleButtonTimeSync = new DevExpress.XtraEditors.SimpleButton();
			this.simpleButtonTempSource = new DevExpress.XtraEditors.SimpleButton();
			this.panelLeftSide = new System.Windows.Forms.Panel();
			this.backgroundWorkerLiveGraph = new System.ComponentModel.BackgroundWorker();
			this.barButtonItemFirmwareUpdateV2 = new DevExpress.XtraBars.BarButtonItem();
			((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
			this.panelSensors.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.groupControlDbList)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.groupControlSensors)).BeginInit();
			this.mainScrollableControl.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
			this.groupControl1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
			this.groupControl2.SuspendLayout();
			this.panelLeftSide.SuspendLayout();
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
            this.barButtonItemExit,
            this.barButtonItemTimeSync,
            this.barSubItemDebug,
            this.barButtonItemAdd24Hours,
            this.barButtonItemFirmwareUpdateV2});
			this.barManager.MainMenu = this.bar2;
			this.barManager.MaxItemId = 21;
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
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem4),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItemDebug)});
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
			this.barButtonItemImport.Caption = "Get Logger Data...";
			this.barButtonItemImport.Id = 7;
			this.barButtonItemImport.Name = "barButtonItemImport";
			this.barButtonItemImport.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemImport_ItemClick);
			// 
			// barButtonItemExport
			// 
			this.barButtonItemExport.Caption = "Export...";
			this.barButtonItemExport.Id = 6;
			this.barButtonItemExport.Name = "barButtonItemExport";
			this.barButtonItemExport.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemExport_ItemClick);
			// 
			// barButtonItemExit
			// 
			this.barButtonItemExit.Caption = "Exit";
			this.barButtonItemExit.Id = 16;
			this.barButtonItemExit.Name = "barButtonItemExit";
			this.barButtonItemExit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemExit_ItemClick);
			// 
			// barSubItem3
			// 
			this.barSubItem3.Caption = "Tools";
			this.barSubItem3.Id = 2;
			this.barSubItem3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemPrefs),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemSensorSetup, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemFirmwareUpdate),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemFirmwareUpdateV2),
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
			this.barButtonItemFirmwareUpdate.Caption = "Firmware Update V1...";
			this.barButtonItemFirmwareUpdate.Id = 9;
			this.barButtonItemFirmwareUpdate.Name = "barButtonItemFirmwareUpdate";
			this.barButtonItemFirmwareUpdate.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemFirmwareUpdate_ItemClick);
			// 
			// barButtonItemTimeCorrection
			// 
			this.barButtonItemTimeCorrection.Caption = "Database Time Correction...";
			this.barButtonItemTimeCorrection.Id = 10;
			this.barButtonItemTimeCorrection.Name = "barButtonItemTimeCorrection";
			this.barButtonItemTimeCorrection.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemTimeCorrection_ItemClick);
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
			this.barButtonItemAbout.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemAbout_ItemClick);
			// 
			// barSubItemDebug
			// 
			this.barSubItemDebug.Caption = "Debug";
			this.barSubItemDebug.Id = 18;
			this.barSubItemDebug.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemAdd24Hours)});
			this.barSubItemDebug.Name = "barSubItemDebug";
			this.barSubItemDebug.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
			// 
			// barButtonItemAdd24Hours
			// 
			this.barButtonItemAdd24Hours.Caption = "Add 24 hours of live data";
			this.barButtonItemAdd24Hours.Id = 19;
			this.barButtonItemAdd24Hours.Name = "barButtonItemAdd24Hours";
			this.barButtonItemAdd24Hours.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemAdd24Hours_ItemClick);
			// 
			// barDockControlTop
			// 
			this.barDockControlTop.CausesValidation = false;
			this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
			this.barDockControlTop.Size = new System.Drawing.Size(813, 24);
			// 
			// barDockControlBottom
			// 
			this.barDockControlBottom.CausesValidation = false;
			this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.barDockControlBottom.Location = new System.Drawing.Point(0, 862);
			this.barDockControlBottom.Size = new System.Drawing.Size(813, 0);
			// 
			// barDockControlLeft
			// 
			this.barDockControlLeft.CausesValidation = false;
			this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
			this.barDockControlLeft.Location = new System.Drawing.Point(0, 24);
			this.barDockControlLeft.Size = new System.Drawing.Size(0, 838);
			// 
			// barDockControlRight
			// 
			this.barDockControlRight.CausesValidation = false;
			this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
			this.barDockControlRight.Location = new System.Drawing.Point(813, 24);
			this.barDockControlRight.Size = new System.Drawing.Size(0, 838);
			// 
			// barSubItem5
			// 
			this.barSubItem5.Caption = "Data";
			this.barSubItem5.Id = 5;
			this.barSubItem5.Name = "barSubItem5";
			// 
			// barButtonItemTimeSync
			// 
			this.barButtonItemTimeSync.Caption = "Logger Time Sync...";
			this.barButtonItemTimeSync.Id = 17;
			this.barButtonItemTimeSync.Name = "barButtonItemTimeSync";
			this.barButtonItemTimeSync.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemTimeSync_ItemClick);
			// 
			// liveAtmosphericGraph
			// 
			this.liveAtmosphericGraph.ConverterCacheReadingValues = null;
			this.liveAtmosphericGraph.Dock = System.Windows.Forms.DockStyle.Top;
			this.liveAtmosphericGraph.HeightAboveSeaLevel = 0D;
			this.liveAtmosphericGraph.Location = new System.Drawing.Point(0, 28);
			this.liveAtmosphericGraph.Name = "liveAtmosphericGraph";
			this.liveAtmosphericGraph.PressureUnit = Atmo.Units.PressureUnit.Pascals;
			this.liveAtmosphericGraph.Size = new System.Drawing.Size(586, 506);
			this.liveAtmosphericGraph.SpeedUnit = Atmo.Units.SpeedUnit.MetersPerSec;
			this.liveAtmosphericGraph.State = null;
			this.liveAtmosphericGraph.TabIndex = 4;
			this.liveAtmosphericGraph.TemperatureUnit = Atmo.Units.TemperatureUnit.Celsius;
			// 
			// timerLive
			// 
			this.timerLive.Enabled = true;
			this.timerLive.Interval = 1000;
			this.timerLive.Tick += new System.EventHandler(this.timerLive_Tick);
			// 
			// panelSensors
			// 
			this.panelSensors.AutoScroll = true;
			this.panelSensors.AutoScrollMinSize = new System.Drawing.Size(180, 0);
			this.panelSensors.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelSensors.Controls.Add(this.groupControlDbList);
			this.panelSensors.Controls.Add(this.groupControlSensors);
			this.panelSensors.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelSensors.Location = new System.Drawing.Point(0, 269);
			this.panelSensors.Name = "panelSensors";
			this.panelSensors.Size = new System.Drawing.Size(204, 483);
			this.panelSensors.TabIndex = 13;
			// 
			// groupControlDbList
			// 
			this.groupControlDbList.AutoSize = true;
			this.groupControlDbList.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.groupControlDbList.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupControlDbList.Location = new System.Drawing.Point(0, 24);
			this.groupControlDbList.Name = "groupControlDbList";
			this.groupControlDbList.Size = new System.Drawing.Size(202, 24);
			this.groupControlDbList.TabIndex = 2;
			this.groupControlDbList.Text = "Available Sensor Data Databases";
			// 
			// groupControlSensors
			// 
			this.groupControlSensors.AutoSize = true;
			this.groupControlSensors.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.groupControlSensors.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupControlSensors.Location = new System.Drawing.Point(0, 0);
			this.groupControlSensors.Name = "groupControlSensors";
			this.groupControlSensors.Size = new System.Drawing.Size(202, 24);
			this.groupControlSensors.TabIndex = 1;
			this.groupControlSensors.Text = "Available Sensors";
			// 
			// simpleButtonFindSensors
			// 
			this.simpleButtonFindSensors.Dock = System.Windows.Forms.DockStyle.Top;
			this.simpleButtonFindSensors.Image = global::Atmo.UI.DevEx.Properties.Resources.Search_Sensor;
			this.simpleButtonFindSensors.Location = new System.Drawing.Point(0, 164);
			this.simpleButtonFindSensors.Name = "simpleButtonFindSensors";
			this.simpleButtonFindSensors.Size = new System.Drawing.Size(204, 35);
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
			this.simpleButtonDownloadData.Dock = System.Windows.Forms.DockStyle.Top;
			this.simpleButtonDownloadData.Image = global::Atmo.UI.DevEx.Properties.Resources.Download_Data;
			this.simpleButtonDownloadData.Location = new System.Drawing.Point(0, 129);
			this.simpleButtonDownloadData.Name = "simpleButtonDownloadData";
			this.simpleButtonDownloadData.Size = new System.Drawing.Size(204, 35);
			this.simpleButtonDownloadData.TabIndex = 23;
			this.simpleButtonDownloadData.Text = "Get Logger Data";
			this.simpleButtonDownloadData.Click += new System.EventHandler(this.simpleButtonDownloadData_Click);
			// 
			// windResourceGraph
			// 
			this.windResourceGraph.ConverterCacheReadingValues = null;
			this.windResourceGraph.Dock = System.Windows.Forms.DockStyle.Top;
			this.windResourceGraph.Location = new System.Drawing.Point(0, 1123);
			this.windResourceGraph.Name = "windResourceGraph";
			this.windResourceGraph.PressureUnit = Atmo.Units.PressureUnit.Pascals;
			this.windResourceGraph.Size = new System.Drawing.Size(586, 778);
			this.windResourceGraph.SpeedUnit = Atmo.Units.SpeedUnit.MetersPerSec;
			this.windResourceGraph.State = null;
			this.windResourceGraph.TabIndex = 28;
			this.windResourceGraph.TemperatureUnit = Atmo.Units.TemperatureUnit.Celsius;
			// 
			// historicalGraphBreakdown
			// 
			this.historicalGraphBreakdown.ConverterCacheReadingValues = null;
			this.historicalGraphBreakdown.CumulativeTimeSpan = System.TimeSpan.Parse("00:00:00");
			this.historicalGraphBreakdown.Dock = System.Windows.Forms.DockStyle.Top;
			this.historicalGraphBreakdown.DrillStartDate = new System.DateTime(((long)(0)));
			this.historicalGraphBreakdown.Location = new System.Drawing.Point(0, 562);
			this.historicalGraphBreakdown.Name = "historicalGraphBreakdown";
			this.historicalGraphBreakdown.PressureUnit = Atmo.Units.PressureUnit.Pascals;
			this.historicalGraphBreakdown.Size = new System.Drawing.Size(586, 561);
			this.historicalGraphBreakdown.SpeedUnit = Atmo.Units.SpeedUnit.MetersPerSec;
			this.historicalGraphBreakdown.State = null;
			this.historicalGraphBreakdown.StepBack = false;
			this.historicalGraphBreakdown.TabIndex = 33;
			this.historicalGraphBreakdown.TemperatureUnit = Atmo.Units.TemperatureUnit.Celsius;
			// 
			// historicalTimeSelectHeader
			// 
			this.historicalTimeSelectHeader.Dock = System.Windows.Forms.DockStyle.Top;
			this.historicalTimeSelectHeader.HeaderText = "Historic Data";
			this.historicalTimeSelectHeader.Location = new System.Drawing.Point(0, 534);
			this.historicalTimeSelectHeader.MaximumSize = new System.Drawing.Size(0, 28);
			this.historicalTimeSelectHeader.MinimumSize = new System.Drawing.Size(581, 28);
			this.historicalTimeSelectHeader.Name = "historicalTimeSelectHeader";
			this.historicalTimeSelectHeader.Size = new System.Drawing.Size(586, 28);
			this.historicalTimeSelectHeader.TabIndex = 43;
			// 
			// liveAtmosphericHeader
			// 
			this.liveAtmosphericHeader.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
			this.liveAtmosphericHeader.Appearance.Options.UseFont = true;
			this.liveAtmosphericHeader.Dock = System.Windows.Forms.DockStyle.Top;
			this.liveAtmosphericHeader.Location = new System.Drawing.Point(0, 0);
			this.liveAtmosphericHeader.Margin = new System.Windows.Forms.Padding(4);
			this.liveAtmosphericHeader.MaximumSize = new System.Drawing.Size(0, 28);
			this.liveAtmosphericHeader.MinimumSize = new System.Drawing.Size(279, 28);
			this.liveAtmosphericHeader.Name = "liveAtmosphericHeader";
			this.liveAtmosphericHeader.Size = new System.Drawing.Size(586, 28);
			this.liveAtmosphericHeader.TabIndex = 48;
			// 
			// mainScrollableControl
			// 
			this.mainScrollableControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.mainScrollableControl.Controls.Add(this.windResourceGraph);
			this.mainScrollableControl.Controls.Add(this.historicalGraphBreakdown);
			this.mainScrollableControl.Controls.Add(this.historicalTimeSelectHeader);
			this.mainScrollableControl.Controls.Add(this.liveAtmosphericGraph);
			this.mainScrollableControl.Controls.Add(this.liveAtmosphericHeader);
			this.mainScrollableControl.FireScrollEventOnMouseWheel = true;
			this.mainScrollableControl.Location = new System.Drawing.Point(210, 23);
			this.mainScrollableControl.Name = "mainScrollableControl";
			this.mainScrollableControl.Size = new System.Drawing.Size(603, 840);
			this.mainScrollableControl.TabIndex = 49;
			// 
			// groupControl1
			// 
			this.groupControl1.Controls.Add(this.tableLayoutPanel1);
			this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupControl1.Location = new System.Drawing.Point(0, 0);
			this.groupControl1.Name = "groupControl1";
			this.groupControl1.Size = new System.Drawing.Size(204, 129);
			this.groupControl1.TabIndex = 54;
			this.groupControl1.Text = "Logger Status";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
			this.tableLayoutPanel1.Controls.Add(this.labelControl2, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.labelControl1, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.labelControl3, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.labelVolUsb, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.labelTmpDaq, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.labelControl5, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.labelLocalTime, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.labelControl4, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.labelVolBat, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.labelDaqTime, 1, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 22);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 5;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0008F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0008F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0008F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 19.9988F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 19.9988F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(200, 105);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// labelControl2
			// 
			this.labelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.labelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelControl2.Location = new System.Drawing.Point(3, 86);
			this.labelControl2.Name = "labelControl2";
			this.labelControl2.Size = new System.Drawing.Size(84, 16);
			this.labelControl2.TabIndex = 1;
			this.labelControl2.Text = "USB Voltage:";
			// 
			// labelControl1
			// 
			this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.labelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelControl1.Location = new System.Drawing.Point(3, 66);
			this.labelControl1.Name = "labelControl1";
			this.labelControl1.Size = new System.Drawing.Size(84, 14);
			this.labelControl1.TabIndex = 0;
			this.labelControl1.Text = "Battery Voltage:";
			// 
			// labelControl3
			// 
			this.labelControl3.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.labelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelControl3.Location = new System.Drawing.Point(3, 45);
			this.labelControl3.Name = "labelControl3";
			this.labelControl3.Size = new System.Drawing.Size(84, 15);
			this.labelControl3.TabIndex = 2;
			this.labelControl3.Text = "Logger Temp.:";
			// 
			// labelVolUsb
			// 
			this.labelVolUsb.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.labelVolUsb.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelVolUsb.Location = new System.Drawing.Point(93, 86);
			this.labelVolUsb.Name = "labelVolUsb";
			this.labelVolUsb.Size = new System.Drawing.Size(104, 16);
			this.labelVolUsb.TabIndex = 4;
			this.labelVolUsb.Text = "N/A";
			// 
			// labelTmpDaq
			// 
			this.labelTmpDaq.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.labelTmpDaq.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelTmpDaq.Location = new System.Drawing.Point(93, 45);
			this.labelTmpDaq.Name = "labelTmpDaq";
			this.labelTmpDaq.Size = new System.Drawing.Size(104, 15);
			this.labelTmpDaq.TabIndex = 3;
			this.labelTmpDaq.Text = "N/A";
			// 
			// labelControl5
			// 
			this.labelControl5.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.labelControl5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelControl5.Location = new System.Drawing.Point(3, 3);
			this.labelControl5.Name = "labelControl5";
			this.labelControl5.Size = new System.Drawing.Size(84, 15);
			this.labelControl5.TabIndex = 7;
			this.labelControl5.Text = "Computer Time:";
			// 
			// labelLocalTime
			// 
			this.labelLocalTime.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.labelLocalTime.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelLocalTime.Location = new System.Drawing.Point(93, 3);
			this.labelLocalTime.Name = "labelLocalTime";
			this.labelLocalTime.Size = new System.Drawing.Size(104, 15);
			this.labelLocalTime.TabIndex = 9;
			this.labelLocalTime.Text = "N/A";
			// 
			// labelControl4
			// 
			this.labelControl4.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.labelControl4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelControl4.Location = new System.Drawing.Point(3, 24);
			this.labelControl4.Name = "labelControl4";
			this.labelControl4.Size = new System.Drawing.Size(84, 15);
			this.labelControl4.TabIndex = 6;
			this.labelControl4.Text = "Logger Time:";
			// 
			// labelVolBat
			// 
			this.labelVolBat.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.labelVolBat.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelVolBat.Location = new System.Drawing.Point(93, 66);
			this.labelVolBat.Name = "labelVolBat";
			this.labelVolBat.Size = new System.Drawing.Size(104, 14);
			this.labelVolBat.TabIndex = 5;
			this.labelVolBat.Text = "N/A";
			// 
			// labelDaqTime
			// 
			this.labelDaqTime.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.labelDaqTime.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelDaqTime.Location = new System.Drawing.Point(93, 24);
			this.labelDaqTime.Name = "labelDaqTime";
			this.labelDaqTime.Size = new System.Drawing.Size(104, 15);
			this.labelDaqTime.TabIndex = 8;
			this.labelDaqTime.Text = "N/A";
			// 
			// timerQueryTime
			// 
			this.timerQueryTime.Enabled = true;
			this.timerQueryTime.Interval = 250;
			this.timerQueryTime.Tick += new System.EventHandler(this.timerQueryTime_Tick);
			// 
			// groupControl2
			// 
			this.groupControl2.Controls.Add(this.labelControlPwsStatus);
			this.groupControl2.Controls.Add(this.simpleButtonPwsAction);
			this.groupControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.groupControl2.Location = new System.Drawing.Point(0, 752);
			this.groupControl2.Name = "groupControl2";
			this.groupControl2.Size = new System.Drawing.Size(204, 88);
			this.groupControl2.TabIndex = 59;
			this.groupControl2.Text = "Web Streaming Status";
			// 
			// labelControlPwsStatus
			// 
			this.labelControlPwsStatus.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
			this.labelControlPwsStatus.AutoEllipsis = true;
			this.labelControlPwsStatus.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.labelControlPwsStatus.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelControlPwsStatus.Location = new System.Drawing.Point(2, 22);
			this.labelControlPwsStatus.Name = "labelControlPwsStatus";
			this.labelControlPwsStatus.Size = new System.Drawing.Size(200, 29);
			this.labelControlPwsStatus.TabIndex = 0;
			this.labelControlPwsStatus.Text = "N/A";
			// 
			// simpleButtonPwsAction
			// 
			this.simpleButtonPwsAction.Appearance.ForeColor = System.Drawing.Color.Red;
			this.simpleButtonPwsAction.Appearance.Options.UseForeColor = true;
			this.simpleButtonPwsAction.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.simpleButtonPwsAction.Image = global::Atmo.UI.DevEx.Properties.Resources.Stream_Off;
			this.simpleButtonPwsAction.Location = new System.Drawing.Point(2, 51);
			this.simpleButtonPwsAction.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
			this.simpleButtonPwsAction.Name = "simpleButtonPwsAction";
			this.simpleButtonPwsAction.Size = new System.Drawing.Size(200, 35);
			this.simpleButtonPwsAction.TabIndex = 1;
			this.simpleButtonPwsAction.Text = "N/A";
			this.simpleButtonPwsAction.Click += new System.EventHandler(this.simpleButtonPwsAction_Click);
			// 
			// timerRapidFire
			// 
			this.timerRapidFire.Interval = 5000;
			this.timerRapidFire.Tick += new System.EventHandler(this.timerRapidFire_Tick);
			// 
			// simpleButtonTimeSync
			// 
			this.simpleButtonTimeSync.Dock = System.Windows.Forms.DockStyle.Top;
			this.simpleButtonTimeSync.Image = global::Atmo.UI.DevEx.Properties.Resources.Sync_Time;
			this.simpleButtonTimeSync.Location = new System.Drawing.Point(0, 234);
			this.simpleButtonTimeSync.Name = "simpleButtonTimeSync";
			this.simpleButtonTimeSync.Size = new System.Drawing.Size(204, 35);
			this.simpleButtonTimeSync.TabIndex = 64;
			this.simpleButtonTimeSync.Text = "Logger Time Sync...";
			this.simpleButtonTimeSync.Click += new System.EventHandler(this.simpleButtonTimeSync_Click);
			// 
			// simpleButtonTempSource
			// 
			this.simpleButtonTempSource.Appearance.Options.UseTextOptions = true;
			this.simpleButtonTempSource.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
			this.simpleButtonTempSource.Dock = System.Windows.Forms.DockStyle.Top;
			this.simpleButtonTempSource.Image = global::Atmo.UI.DevEx.Properties.Resources.Temp_Sensor_01;
			this.simpleButtonTempSource.Location = new System.Drawing.Point(0, 199);
			this.simpleButtonTempSource.Name = "simpleButtonTempSource";
			this.simpleButtonTempSource.Size = new System.Drawing.Size(204, 35);
			this.simpleButtonTempSource.TabIndex = 69;
			this.simpleButtonTempSource.Text = "Temperature Source";
			this.simpleButtonTempSource.Click += new System.EventHandler(this.simpleButtonTempSource_Click);
			// 
			// panelLeftSide
			// 
			this.panelLeftSide.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.panelLeftSide.Controls.Add(this.panelSensors);
			this.panelLeftSide.Controls.Add(this.simpleButtonTimeSync);
			this.panelLeftSide.Controls.Add(this.simpleButtonTempSource);
			this.panelLeftSide.Controls.Add(this.simpleButtonFindSensors);
			this.panelLeftSide.Controls.Add(this.simpleButtonDownloadData);
			this.panelLeftSide.Controls.Add(this.groupControl1);
			this.panelLeftSide.Controls.Add(this.groupControl2);
			this.panelLeftSide.Location = new System.Drawing.Point(0, 23);
			this.panelLeftSide.Name = "panelLeftSide";
			this.panelLeftSide.Size = new System.Drawing.Size(204, 840);
			this.panelLeftSide.TabIndex = 74;
			// 
			// backgroundWorkerLiveGraph
			// 
			this.backgroundWorkerLiveGraph.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerLiveGraph_DoWork);
			this.backgroundWorkerLiveGraph.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerLiveGraph_RunWorkerCompleted);
			// 
			// barButtonItemFirmwareUpdateV2
			// 
			this.barButtonItemFirmwareUpdateV2.Caption = "Firmware Update V2...";
			this.barButtonItemFirmwareUpdateV2.Id = 20;
			this.barButtonItemFirmwareUpdateV2.Name = "barButtonItemFirmwareUpdateV2";
			this.barButtonItemFirmwareUpdateV2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemFirmwareUpdateV2_ItemClick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(813, 862);
			this.Controls.Add(this.panelLeftSide);
			this.Controls.Add(this.mainScrollableControl);
			this.Controls.Add(this.barDockControlLeft);
			this.Controls.Add(this.barDockControlRight);
			this.Controls.Add(this.barDockControlBottom);
			this.Controls.Add(this.barDockControlTop);
			this.DoubleBuffered = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(800, 600);
			this.Name = "MainForm";
			this.Text = "Atmo 2";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
			((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
			this.panelSensors.ResumeLayout(false);
			this.panelSensors.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.groupControlDbList)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.groupControlSensors)).EndInit();
			this.mainScrollableControl.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
			this.groupControl1.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
			this.groupControl2.ResumeLayout(false);
			this.panelLeftSide.ResumeLayout(false);
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
		private System.Windows.Forms.Timer timerLive;
		private System.Windows.Forms.Panel panelSensors;
		private DevExpress.XtraEditors.SimpleButton simpleButtonFindSensors;
		private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel;
		private DevExpress.XtraEditors.SimpleButton simpleButtonDownloadData;
		private Controls.WindResourceGraph windResourceGraph;
		private Controls.HistoricalGraphBreakdown historicalGraphBreakdown;
		private Controls.HistoricalTimeSelectHeader historicalTimeSelectHeader;
		private DevExpress.XtraEditors.XtraScrollableControl mainScrollableControl;
		private Controls.LiveAtmosphericHeader liveAtmosphericHeader;
		private DevExpress.XtraEditors.GroupControl groupControlDbList;
		private DevExpress.XtraEditors.GroupControl groupControlSensors;
		private DevExpress.XtraBars.BarButtonItem barButtonItemTimeSync;
		private DevExpress.XtraEditors.GroupControl groupControl1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private DevExpress.XtraEditors.LabelControl labelControl1;
		private DevExpress.XtraEditors.LabelControl labelControl2;
		private DevExpress.XtraEditors.LabelControl labelControl3;
		private DevExpress.XtraEditors.LabelControl labelTmpDaq;
		private DevExpress.XtraEditors.LabelControl labelVolUsb;
		private DevExpress.XtraEditors.LabelControl labelVolBat;
		private DevExpress.XtraEditors.LabelControl labelControl4;
		private DevExpress.XtraEditors.LabelControl labelControl5;
		private DevExpress.XtraEditors.LabelControl labelDaqTime;
		private DevExpress.XtraEditors.LabelControl labelLocalTime;
		private System.Windows.Forms.Timer timerQueryTime;
		private DevExpress.XtraEditors.GroupControl groupControl2;
		private DevExpress.XtraEditors.SimpleButton simpleButtonPwsAction;
		private DevExpress.XtraEditors.LabelControl labelControlPwsStatus;
		private System.Windows.Forms.Timer timerRapidFire;
		private DevExpress.XtraEditors.SimpleButton simpleButtonTimeSync;
		private DevExpress.XtraEditors.SimpleButton simpleButtonTempSource;
		private System.Windows.Forms.Panel panelLeftSide;
		private System.ComponentModel.BackgroundWorker backgroundWorkerLiveGraph;
		private DevExpress.XtraBars.BarSubItem barSubItemDebug;
		private DevExpress.XtraBars.BarButtonItem barButtonItemAdd24Hours;
		private DevExpress.XtraBars.BarButtonItem barButtonItemFirmwareUpdateV2;

	}
}