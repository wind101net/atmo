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
			this.tabControlSettings = new System.Windows.Forms.TabControl();
			this.tabPageGraph = new System.Windows.Forms.TabPage();
			this.tabPageData = new System.Windows.Forms.TabPage();
			this.tabPagePws = new System.Windows.Forms.TabPage();
			this.simpleButtonApply = new DevExpress.XtraEditors.SimpleButton();
			this.simpleButtonOk = new DevExpress.XtraEditors.SimpleButton();
			this.simpleButtonCancel = new DevExpress.XtraEditors.SimpleButton();
			this.tabControlSettings.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControlSettings
			// 
			this.tabControlSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tabControlSettings.Controls.Add(this.tabPageGraph);
			this.tabControlSettings.Controls.Add(this.tabPageData);
			this.tabControlSettings.Controls.Add(this.tabPagePws);
			this.tabControlSettings.Location = new System.Drawing.Point(12, 12);
			this.tabControlSettings.Name = "tabControlSettings";
			this.tabControlSettings.SelectedIndex = 0;
			this.tabControlSettings.Size = new System.Drawing.Size(398, 311);
			this.tabControlSettings.TabIndex = 0;
			// 
			// tabPageGraph
			// 
			this.tabPageGraph.Location = new System.Drawing.Point(4, 22);
			this.tabPageGraph.Name = "tabPageGraph";
			this.tabPageGraph.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageGraph.Size = new System.Drawing.Size(390, 285);
			this.tabPageGraph.TabIndex = 0;
			this.tabPageGraph.Text = "Graph";
			this.tabPageGraph.UseVisualStyleBackColor = true;
			// 
			// tabPageData
			// 
			this.tabPageData.Location = new System.Drawing.Point(4, 22);
			this.tabPageData.Name = "tabPageData";
			this.tabPageData.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageData.Size = new System.Drawing.Size(390, 285);
			this.tabPageData.TabIndex = 1;
			this.tabPageData.Text = "Data";
			this.tabPageData.UseVisualStyleBackColor = true;
			// 
			// tabPagePws
			// 
			this.tabPagePws.Location = new System.Drawing.Point(4, 22);
			this.tabPagePws.Name = "tabPagePws";
			this.tabPagePws.Size = new System.Drawing.Size(390, 285);
			this.tabPagePws.TabIndex = 2;
			this.tabPagePws.Text = "PWS";
			this.tabPagePws.UseVisualStyleBackColor = true;
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
			// SettingsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(422, 364);
			this.Controls.Add(this.simpleButtonCancel);
			this.Controls.Add(this.simpleButtonOk);
			this.Controls.Add(this.simpleButtonApply);
			this.Controls.Add(this.tabControlSettings);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(277, 247);
			this.Name = "SettingsForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Preferences";
			this.tabControlSettings.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControlSettings;
		private System.Windows.Forms.TabPage tabPageGraph;
		private System.Windows.Forms.TabPage tabPageData;
		private DevExpress.XtraEditors.SimpleButton simpleButtonApply;
		private DevExpress.XtraEditors.SimpleButton simpleButtonOk;
		private DevExpress.XtraEditors.SimpleButton simpleButtonCancel;
		private System.Windows.Forms.TabPage tabPagePws;
	}
}