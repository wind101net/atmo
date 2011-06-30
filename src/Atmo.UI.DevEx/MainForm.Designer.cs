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
			this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
			this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
			this.barButtonItem11 = new DevExpress.XtraBars.BarButtonItem();
			this.barSubItem3 = new DevExpress.XtraBars.BarSubItem();
			this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
			this.barButtonItem6 = new DevExpress.XtraBars.BarButtonItem();
			this.barButtonItem4 = new DevExpress.XtraBars.BarButtonItem();
			this.barButtonItem5 = new DevExpress.XtraBars.BarButtonItem();
			this.barSubItem4 = new DevExpress.XtraBars.BarSubItem();
			this.barButtonItem7 = new DevExpress.XtraBars.BarButtonItem();
			this.barButtonItem8 = new DevExpress.XtraBars.BarButtonItem();
			this.barButtonItem9 = new DevExpress.XtraBars.BarButtonItem();
			this.barButtonItem10 = new DevExpress.XtraBars.BarButtonItem();
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
            this.barButtonItem1,
            this.barButtonItem2,
            this.barButtonItem3,
            this.barButtonItem4,
            this.barButtonItem5,
            this.barButtonItem6,
            this.barButtonItem7,
            this.barButtonItem8,
            this.barButtonItem9,
            this.barButtonItem10,
            this.barButtonItem11});
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
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem2),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem11, true)});
			this.barSubItem1.Name = "barSubItem1";
			// 
			// barButtonItem2
			// 
			this.barButtonItem2.Caption = "Import";
			this.barButtonItem2.Id = 7;
			this.barButtonItem2.Name = "barButtonItem2";
			// 
			// barButtonItem1
			// 
			this.barButtonItem1.Caption = "Export";
			this.barButtonItem1.Id = 6;
			this.barButtonItem1.Name = "barButtonItem1";
			// 
			// barButtonItem11
			// 
			this.barButtonItem11.Caption = "Exit";
			this.barButtonItem11.Id = 16;
			this.barButtonItem11.Name = "barButtonItem11";
			// 
			// barSubItem3
			// 
			this.barSubItem3.Caption = "Tools";
			this.barSubItem3.Id = 2;
			this.barSubItem3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem3),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem6, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem4),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem5, true)});
			this.barSubItem3.Name = "barSubItem3";
			// 
			// barButtonItem3
			// 
			this.barButtonItem3.Caption = "Preferences";
			this.barButtonItem3.Id = 8;
			this.barButtonItem3.Name = "barButtonItem3";
			// 
			// barButtonItem6
			// 
			this.barButtonItem6.Caption = "Sensor Setup";
			this.barButtonItem6.Id = 11;
			this.barButtonItem6.Name = "barButtonItem6";
			// 
			// barButtonItem4
			// 
			this.barButtonItem4.Caption = "Firmware Update";
			this.barButtonItem4.Id = 9;
			this.barButtonItem4.Name = "barButtonItem4";
			// 
			// barButtonItem5
			// 
			this.barButtonItem5.Caption = "Database Time Correction";
			this.barButtonItem5.Id = 10;
			this.barButtonItem5.Name = "barButtonItem5";
			// 
			// barSubItem4
			// 
			this.barSubItem4.Caption = "Help";
			this.barSubItem4.Id = 3;
			this.barSubItem4.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem7),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem8),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem9),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem10, true)});
			this.barSubItem4.Name = "barSubItem4";
			// 
			// barButtonItem7
			// 
			this.barButtonItem7.Caption = "Documentation";
			this.barButtonItem7.Id = 12;
			this.barButtonItem7.Name = "barButtonItem7";
			// 
			// barButtonItem8
			// 
			this.barButtonItem8.Caption = "Check for Updates";
			this.barButtonItem8.Id = 13;
			this.barButtonItem8.Name = "barButtonItem8";
			// 
			// barButtonItem9
			// 
			this.barButtonItem9.Caption = "Contact Support";
			this.barButtonItem9.Id = 14;
			this.barButtonItem9.Name = "barButtonItem9";
			// 
			// barButtonItem10
			// 
			this.barButtonItem10.Caption = "About";
			this.barButtonItem10.Id = 15;
			this.barButtonItem10.Name = "barButtonItem10";
			// 
			// barDockControlTop
			// 
			this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
			this.barDockControlTop.Size = new System.Drawing.Size(941, 24);
			// 
			// barDockControlBottom
			// 
			this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.barDockControlBottom.Location = new System.Drawing.Point(0, 779);
			this.barDockControlBottom.Size = new System.Drawing.Size(941, 0);
			// 
			// barDockControlLeft
			// 
			this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
			this.barDockControlLeft.Location = new System.Drawing.Point(0, 24);
			this.barDockControlLeft.Size = new System.Drawing.Size(0, 755);
			// 
			// barDockControlRight
			// 
			this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
			this.barDockControlRight.Location = new System.Drawing.Point(941, 24);
			this.barDockControlRight.Size = new System.Drawing.Size(0, 755);
			// 
			// barSubItem5
			// 
			this.barSubItem5.Caption = "Data";
			this.barSubItem5.Id = 5;
			this.barSubItem5.Name = "barSubItem5";
			// 
			// liveAtmosphericGraph
			// 
			this.liveAtmosphericGraph.Location = new System.Drawing.Point(234, 31);
			this.liveAtmosphericGraph.Name = "liveAtmosphericGraph";
			this.liveAtmosphericGraph.Size = new System.Drawing.Size(599, 478);
			this.liveAtmosphericGraph.TabIndex = 4;
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
			this.panelSensors.Location = new System.Drawing.Point(12, 30);
			this.panelSensors.Name = "panelSensors";
			this.panelSensors.Size = new System.Drawing.Size(205, 737);
			this.panelSensors.TabIndex = 13;
			// 
			// panelJunk
			// 
			this.panelJunk.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.panelJunk.Controls.Add(this.label1);
			this.panelJunk.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelJunk.Location = new System.Drawing.Point(0, 0);
			this.panelJunk.Name = "panelJunk";
			this.panelJunk.Size = new System.Drawing.Size(203, 735);
			this.panelJunk.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(203, 735);
			this.label1.TabIndex = 0;
			this.label1.Text = "This control intentionally left blank";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(941, 779);
			this.Controls.Add(this.panelSensors);
			this.Controls.Add(this.liveAtmosphericGraph);
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
		private DevExpress.XtraBars.BarButtonItem barButtonItem2;
		private DevExpress.XtraBars.BarButtonItem barButtonItem1;
		private DevExpress.XtraBars.BarButtonItem barButtonItem11;
		private DevExpress.XtraBars.BarButtonItem barButtonItem3;
		private DevExpress.XtraBars.BarButtonItem barButtonItem6;
		private DevExpress.XtraBars.BarButtonItem barButtonItem4;
		private DevExpress.XtraBars.BarButtonItem barButtonItem5;
		private DevExpress.XtraBars.BarButtonItem barButtonItem7;
		private DevExpress.XtraBars.BarButtonItem barButtonItem8;
		private DevExpress.XtraBars.BarButtonItem barButtonItem9;
		private DevExpress.XtraBars.BarButtonItem barButtonItem10;
		private DevExpress.XtraBars.BarSubItem barSubItem5;
		private Controls.LiveAtmosphericGraph liveAtmosphericGraph;
		private System.Windows.Forms.Timer timerTesting;
		private System.Windows.Forms.Panel panelSensors;
		private System.Windows.Forms.Panel panelJunk;
		private System.Windows.Forms.Label label1;

	}
}