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
			this.xtraTabPageData = new DevExpress.XtraTab.XtraTabPage();
			this.xtraTabPagePws = new DevExpress.XtraTab.XtraTabPage();
			((System.ComponentModel.ISupportInitialize)(this.xtraTabControlSettings)).BeginInit();
			this.xtraTabControlSettings.SuspendLayout();
			this.SuspendLayout();
			// 
			// simpleButtonApply
			// 
			this.simpleButtonApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.simpleButtonApply.Location = new System.Drawing.Point(335, 329);
			this.simpleButtonApply.Name = "simpleButtonApply";
			this.simpleButtonApply.Size = new System.Drawing.Size(75, 23);
			this.simpleButtonApply.TabIndex = 1;
			this.simpleButtonApply.Text = "Apply";
			// 
			// simpleButtonOk
			// 
			this.simpleButtonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.simpleButtonOk.Location = new System.Drawing.Point(173, 329);
			this.simpleButtonOk.Name = "simpleButtonOk";
			this.simpleButtonOk.Size = new System.Drawing.Size(75, 23);
			this.simpleButtonOk.TabIndex = 2;
			this.simpleButtonOk.Text = "OK";
			// 
			// simpleButtonCancel
			// 
			this.simpleButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.simpleButtonCancel.Location = new System.Drawing.Point(254, 329);
			this.simpleButtonCancel.Name = "simpleButtonCancel";
			this.simpleButtonCancel.Size = new System.Drawing.Size(75, 23);
			this.simpleButtonCancel.TabIndex = 3;
			this.simpleButtonCancel.Text = "Cancel";
			this.simpleButtonCancel.Click += new System.EventHandler(this.simpleButtonCancel_Click);
			// 
			// xtraTabControlSettings
			// 
			this.xtraTabControlSettings.Location = new System.Drawing.Point(12, 12);
			this.xtraTabControlSettings.Name = "xtraTabControlSettings";
			this.xtraTabControlSettings.SelectedTabPage = this.xtraTabPageGraph;
			this.xtraTabControlSettings.Size = new System.Drawing.Size(398, 311);
			this.xtraTabControlSettings.TabIndex = 4;
			this.xtraTabControlSettings.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPageGraph,
            this.xtraTabPageData,
            this.xtraTabPagePws});
			// 
			// xtraTabPageGraph
			// 
			this.xtraTabPageGraph.Name = "xtraTabPageGraph";
			this.xtraTabPageGraph.Size = new System.Drawing.Size(396, 288);
			this.xtraTabPageGraph.Text = "Graph";
			// 
			// xtraTabPageData
			// 
			this.xtraTabPageData.Name = "xtraTabPageData";
			this.xtraTabPageData.Size = new System.Drawing.Size(396, 288);
			this.xtraTabPageData.Text = "Data";
			// 
			// xtraTabPagePws
			// 
			this.xtraTabPagePws.Name = "xtraTabPagePws";
			this.xtraTabPagePws.Size = new System.Drawing.Size(396, 288);
			this.xtraTabPagePws.Text = "PWS";
			// 
			// SettingsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(422, 364);
			this.Controls.Add(this.xtraTabControlSettings);
			this.Controls.Add(this.simpleButtonCancel);
			this.Controls.Add(this.simpleButtonOk);
			this.Controls.Add(this.simpleButtonApply);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(277, 247);
			this.Name = "SettingsForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Preferences";
			((System.ComponentModel.ISupportInitialize)(this.xtraTabControlSettings)).EndInit();
			this.xtraTabControlSettings.ResumeLayout(false);
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
	}
}