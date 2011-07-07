namespace Atmo.UI.DevEx {
	partial class SettingsForm {
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
			this.simpleButtonApply = new DevExpress.XtraEditors.SimpleButton();
			this.simpleButtonOk = new DevExpress.XtraEditors.SimpleButton();
			this.simpleButtonCancel = new DevExpress.XtraEditors.SimpleButton();
			this.xtraTabControlSettings = new DevExpress.XtraTab.XtraTabControl();
			this.xtraTabPageGraph = new DevExpress.XtraTab.XtraTabPage();
			this.groupControlUserGraph = new DevExpress.XtraEditors.GroupControl();
			this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
			this.comboBoxEditUserGraph = new DevExpress.XtraEditors.ComboBoxEdit();
			this.groupControlGraphMinRanges = new DevExpress.XtraEditors.GroupControl();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.spinEditAirDensity = new DevExpress.XtraEditors.SpinEdit();
			this.spinEditDewPoint = new DevExpress.XtraEditors.SpinEdit();
			this.spinEditSpeed = new DevExpress.XtraEditors.SpinEdit();
			this.spinEditPressure = new DevExpress.XtraEditors.SpinEdit();
			this.spinEditHumidity = new DevExpress.XtraEditors.SpinEdit();
			this.labelControl11 = new DevExpress.XtraEditors.LabelControl();
			this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
			this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
			this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
			this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
			this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
			this.spinEditTemperature = new DevExpress.XtraEditors.SpinEdit();
			this.xtraTabPageData = new DevExpress.XtraTab.XtraTabPage();
			this.xtraTabPagePws = new DevExpress.XtraTab.XtraTabPage();
			((System.ComponentModel.ISupportInitialize)(this.xtraTabControlSettings)).BeginInit();
			this.xtraTabControlSettings.SuspendLayout();
			this.xtraTabPageGraph.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.groupControlUserGraph)).BeginInit();
			this.groupControlUserGraph.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.comboBoxEditUserGraph.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.groupControlGraphMinRanges)).BeginInit();
			this.groupControlGraphMinRanges.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.spinEditAirDensity.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.spinEditDewPoint.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.spinEditSpeed.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.spinEditPressure.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.spinEditHumidity.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.spinEditTemperature.Properties)).BeginInit();
			this.SuspendLayout();
			// 
			// simpleButtonApply
			// 
			this.simpleButtonApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.simpleButtonApply.Location = new System.Drawing.Point(365, 312);
			this.simpleButtonApply.Name = "simpleButtonApply";
			this.simpleButtonApply.Size = new System.Drawing.Size(75, 23);
			this.simpleButtonApply.TabIndex = 1;
			this.simpleButtonApply.Text = "Apply";
			this.simpleButtonApply.Click += new System.EventHandler(this.simpleButtonApply_Click);
			// 
			// simpleButtonOk
			// 
			this.simpleButtonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.simpleButtonOk.Location = new System.Drawing.Point(203, 312);
			this.simpleButtonOk.Name = "simpleButtonOk";
			this.simpleButtonOk.Size = new System.Drawing.Size(75, 23);
			this.simpleButtonOk.TabIndex = 2;
			this.simpleButtonOk.Text = "OK";
			this.simpleButtonOk.Click += new System.EventHandler(this.simpleButtonOk_Click);
			// 
			// simpleButtonCancel
			// 
			this.simpleButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.simpleButtonCancel.Location = new System.Drawing.Point(284, 312);
			this.simpleButtonCancel.Name = "simpleButtonCancel";
			this.simpleButtonCancel.Size = new System.Drawing.Size(75, 23);
			this.simpleButtonCancel.TabIndex = 3;
			this.simpleButtonCancel.Text = "Cancel";
			this.simpleButtonCancel.Click += new System.EventHandler(this.simpleButtonCancel_Click);
			// 
			// xtraTabControlSettings
			// 
			this.xtraTabControlSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.xtraTabControlSettings.Location = new System.Drawing.Point(12, 12);
			this.xtraTabControlSettings.Name = "xtraTabControlSettings";
			this.xtraTabControlSettings.SelectedTabPage = this.xtraTabPageGraph;
			this.xtraTabControlSettings.Size = new System.Drawing.Size(428, 294);
			this.xtraTabControlSettings.TabIndex = 4;
			this.xtraTabControlSettings.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPageGraph,
            this.xtraTabPageData,
            this.xtraTabPagePws});
			// 
			// xtraTabPageGraph
			// 
			this.xtraTabPageGraph.AutoScroll = true;
			this.xtraTabPageGraph.Controls.Add(this.groupControlUserGraph);
			this.xtraTabPageGraph.Controls.Add(this.groupControlGraphMinRanges);
			this.xtraTabPageGraph.Name = "xtraTabPageGraph";
			this.xtraTabPageGraph.Size = new System.Drawing.Size(426, 271);
			this.xtraTabPageGraph.Text = "Graph";
			// 
			// groupControlUserGraph
			// 
			this.groupControlUserGraph.Controls.Add(this.labelControl2);
			this.groupControlUserGraph.Controls.Add(this.comboBoxEditUserGraph);
			this.groupControlUserGraph.Location = new System.Drawing.Point(3, 3);
			this.groupControlUserGraph.Name = "groupControlUserGraph";
			this.groupControlUserGraph.Size = new System.Drawing.Size(415, 50);
			this.groupControlUserGraph.TabIndex = 1;
			this.groupControlUserGraph.Text = "User Graph";
			// 
			// labelControl2
			// 
			this.labelControl2.Location = new System.Drawing.Point(5, 28);
			this.labelControl2.Name = "labelControl2";
			this.labelControl2.Size = new System.Drawing.Size(54, 13);
			this.labelControl2.TabIndex = 1;
			this.labelControl2.Text = "User Graph";
			// 
			// comboBoxEditUserGraph
			// 
			this.comboBoxEditUserGraph.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.comboBoxEditUserGraph.Location = new System.Drawing.Point(130, 25);
			this.comboBoxEditUserGraph.Name = "comboBoxEditUserGraph";
			this.comboBoxEditUserGraph.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.comboBoxEditUserGraph.Size = new System.Drawing.Size(280, 20);
			this.comboBoxEditUserGraph.TabIndex = 0;
			// 
			// groupControlGraphMinRanges
			// 
			this.groupControlGraphMinRanges.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupControlGraphMinRanges.Controls.Add(this.tableLayoutPanel1);
			this.groupControlGraphMinRanges.Location = new System.Drawing.Point(3, 54);
			this.groupControlGraphMinRanges.Name = "groupControlGraphMinRanges";
			this.groupControlGraphMinRanges.Size = new System.Drawing.Size(415, 177);
			this.groupControlGraphMinRanges.TabIndex = 0;
			this.groupControlGraphMinRanges.Text = "Live Plot Minimum Ranges";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.spinEditAirDensity, 1, 5);
			this.tableLayoutPanel1.Controls.Add(this.spinEditDewPoint, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.spinEditSpeed, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.spinEditPressure, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.spinEditHumidity, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.labelControl11, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this.labelControl9, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.labelControl7, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.labelControl5, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.labelControl3, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.labelControl1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.spinEditTemperature, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 22);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 6;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(411, 153);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// spinEditAirDensity
			// 
			this.spinEditAirDensity.Dock = System.Windows.Forms.DockStyle.Fill;
			this.spinEditAirDensity.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.spinEditAirDensity.Location = new System.Drawing.Point(128, 128);
			this.spinEditAirDensity.Name = "spinEditAirDensity";
			this.spinEditAirDensity.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
			this.spinEditAirDensity.Properties.MaxValue = new decimal(new int[] {
            99999,
            0,
            0,
            0});
			this.spinEditAirDensity.Size = new System.Drawing.Size(280, 20);
			this.spinEditAirDensity.TabIndex = 20;
			// 
			// spinEditDewPoint
			// 
			this.spinEditDewPoint.Dock = System.Windows.Forms.DockStyle.Fill;
			this.spinEditDewPoint.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.spinEditDewPoint.Location = new System.Drawing.Point(128, 103);
			this.spinEditDewPoint.Name = "spinEditDewPoint";
			this.spinEditDewPoint.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
			this.spinEditDewPoint.Properties.MaxValue = new decimal(new int[] {
            99999,
            0,
            0,
            0});
			this.spinEditDewPoint.Size = new System.Drawing.Size(280, 20);
			this.spinEditDewPoint.TabIndex = 19;
			// 
			// spinEditSpeed
			// 
			this.spinEditSpeed.Dock = System.Windows.Forms.DockStyle.Fill;
			this.spinEditSpeed.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.spinEditSpeed.Location = new System.Drawing.Point(128, 78);
			this.spinEditSpeed.Name = "spinEditSpeed";
			this.spinEditSpeed.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
			this.spinEditSpeed.Properties.MaxValue = new decimal(new int[] {
            99999,
            0,
            0,
            0});
			this.spinEditSpeed.Size = new System.Drawing.Size(280, 20);
			this.spinEditSpeed.TabIndex = 18;
			// 
			// spinEditPressure
			// 
			this.spinEditPressure.Dock = System.Windows.Forms.DockStyle.Fill;
			this.spinEditPressure.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.spinEditPressure.Location = new System.Drawing.Point(128, 53);
			this.spinEditPressure.Name = "spinEditPressure";
			this.spinEditPressure.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
			this.spinEditPressure.Properties.MaxValue = new decimal(new int[] {
            99999,
            0,
            0,
            0});
			this.spinEditPressure.Size = new System.Drawing.Size(280, 20);
			this.spinEditPressure.TabIndex = 17;
			// 
			// spinEditHumidity
			// 
			this.spinEditHumidity.Dock = System.Windows.Forms.DockStyle.Fill;
			this.spinEditHumidity.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.spinEditHumidity.Location = new System.Drawing.Point(128, 28);
			this.spinEditHumidity.Name = "spinEditHumidity";
			this.spinEditHumidity.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
			this.spinEditHumidity.Properties.MaxValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.spinEditHumidity.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.spinEditHumidity.Size = new System.Drawing.Size(280, 20);
			this.spinEditHumidity.TabIndex = 16;
			// 
			// labelControl11
			// 
			this.labelControl11.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.labelControl11.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelControl11.Location = new System.Drawing.Point(3, 128);
			this.labelControl11.Name = "labelControl11";
			this.labelControl11.Size = new System.Drawing.Size(119, 22);
			this.labelControl11.TabIndex = 14;
			this.labelControl11.Text = "Air Density";
			// 
			// labelControl9
			// 
			this.labelControl9.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.labelControl9.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelControl9.Location = new System.Drawing.Point(3, 103);
			this.labelControl9.Name = "labelControl9";
			this.labelControl9.Size = new System.Drawing.Size(119, 19);
			this.labelControl9.TabIndex = 8;
			this.labelControl9.Text = "Dew Point";
			// 
			// labelControl7
			// 
			this.labelControl7.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.labelControl7.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelControl7.Location = new System.Drawing.Point(3, 78);
			this.labelControl7.Name = "labelControl7";
			this.labelControl7.Size = new System.Drawing.Size(119, 19);
			this.labelControl7.TabIndex = 6;
			this.labelControl7.Text = "Wind Speed";
			// 
			// labelControl5
			// 
			this.labelControl5.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.labelControl5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelControl5.Location = new System.Drawing.Point(3, 53);
			this.labelControl5.Name = "labelControl5";
			this.labelControl5.Size = new System.Drawing.Size(119, 19);
			this.labelControl5.TabIndex = 4;
			this.labelControl5.Text = "Pressure";
			// 
			// labelControl3
			// 
			this.labelControl3.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.labelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelControl3.Location = new System.Drawing.Point(3, 28);
			this.labelControl3.Name = "labelControl3";
			this.labelControl3.Size = new System.Drawing.Size(119, 19);
			this.labelControl3.TabIndex = 2;
			this.labelControl3.Text = "Humidity";
			// 
			// labelControl1
			// 
			this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.labelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelControl1.Location = new System.Drawing.Point(3, 3);
			this.labelControl1.Name = "labelControl1";
			this.labelControl1.Size = new System.Drawing.Size(119, 19);
			this.labelControl1.TabIndex = 0;
			this.labelControl1.Text = "Temperature";
			// 
			// spinEditTemperature
			// 
			this.spinEditTemperature.Dock = System.Windows.Forms.DockStyle.Fill;
			this.spinEditTemperature.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.spinEditTemperature.Location = new System.Drawing.Point(128, 3);
			this.spinEditTemperature.Name = "spinEditTemperature";
			this.spinEditTemperature.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
			this.spinEditTemperature.Properties.MaxValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.spinEditTemperature.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.spinEditTemperature.Size = new System.Drawing.Size(280, 20);
			this.spinEditTemperature.TabIndex = 15;
			// 
			// xtraTabPageData
			// 
			this.xtraTabPageData.Name = "xtraTabPageData";
			this.xtraTabPageData.Size = new System.Drawing.Size(426, 271);
			this.xtraTabPageData.Text = "Data";
			// 
			// xtraTabPagePws
			// 
			this.xtraTabPagePws.Name = "xtraTabPagePws";
			this.xtraTabPagePws.Size = new System.Drawing.Size(426, 271);
			this.xtraTabPagePws.Text = "PWS";
			// 
			// SettingsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(452, 347);
			this.Controls.Add(this.xtraTabControlSettings);
			this.Controls.Add(this.simpleButtonCancel);
			this.Controls.Add(this.simpleButtonOk);
			this.Controls.Add(this.simpleButtonApply);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(468, 385);
			this.Name = "SettingsForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Preferences";
			((System.ComponentModel.ISupportInitialize)(this.xtraTabControlSettings)).EndInit();
			this.xtraTabControlSettings.ResumeLayout(false);
			this.xtraTabPageGraph.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.groupControlUserGraph)).EndInit();
			this.groupControlUserGraph.ResumeLayout(false);
			this.groupControlUserGraph.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.comboBoxEditUserGraph.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.groupControlGraphMinRanges)).EndInit();
			this.groupControlGraphMinRanges.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.spinEditAirDensity.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.spinEditDewPoint.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.spinEditSpeed.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.spinEditPressure.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.spinEditHumidity.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.spinEditTemperature.Properties)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraEditors.SimpleButton simpleButtonApply;
		private DevExpress.XtraEditors.SimpleButton simpleButtonOk;
		private DevExpress.XtraEditors.SimpleButton simpleButtonCancel;
		private DevExpress.XtraTab.XtraTabControl xtraTabControlSettings;
		private DevExpress.XtraTab.XtraTabPage xtraTabPageGraph;
		private DevExpress.XtraTab.XtraTabPage xtraTabPageData;
		private DevExpress.XtraTab.XtraTabPage xtraTabPagePws;
		private DevExpress.XtraEditors.GroupControl groupControlGraphMinRanges;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private DevExpress.XtraEditors.LabelControl labelControl11;
		private DevExpress.XtraEditors.LabelControl labelControl9;
		private DevExpress.XtraEditors.LabelControl labelControl7;
		private DevExpress.XtraEditors.LabelControl labelControl5;
		private DevExpress.XtraEditors.LabelControl labelControl3;
		private DevExpress.XtraEditors.LabelControl labelControl1;
		private DevExpress.XtraEditors.SpinEdit spinEditAirDensity;
		private DevExpress.XtraEditors.SpinEdit spinEditDewPoint;
		private DevExpress.XtraEditors.SpinEdit spinEditSpeed;
		private DevExpress.XtraEditors.SpinEdit spinEditPressure;
		private DevExpress.XtraEditors.SpinEdit spinEditHumidity;
		private DevExpress.XtraEditors.SpinEdit spinEditTemperature;
		private DevExpress.XtraEditors.GroupControl groupControlUserGraph;
		private DevExpress.XtraEditors.LabelControl labelControl2;
		private DevExpress.XtraEditors.ComboBoxEdit comboBoxEditUserGraph;
	}
}