namespace Atmo.UI.DevEx.Controls {
	partial class HistoricalGraphBreakdown {
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.groupControl = new DevExpress.XtraEditors.GroupControl();
			((System.ComponentModel.ISupportInitialize)(this.groupControl)).BeginInit();
			this.groupControl.SuspendLayout();
			this.SuspendLayout();
			// 
			// flowLayoutPanel
			// 
			this.flowLayoutPanel.AutoScroll = true;
			this.flowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel.Location = new System.Drawing.Point(2, 22);
			this.flowLayoutPanel.Name = "flowLayoutPanel";
			this.flowLayoutPanel.Size = new System.Drawing.Size(635, 484);
			this.flowLayoutPanel.TabIndex = 0;
			// 
			// groupControl
			// 
			this.groupControl.Controls.Add(this.flowLayoutPanel);
			this.groupControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupControl.Location = new System.Drawing.Point(0, 0);
			this.groupControl.Name = "groupControl";
			this.groupControl.Size = new System.Drawing.Size(639, 508);
			this.groupControl.TabIndex = 1;
			this.groupControl.Text = "Historical Breakdown";
			// 
			// HistoricalGraphBreakdown
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupControl);
			this.Name = "HistoricalGraphBreakdown";
			this.Size = new System.Drawing.Size(639, 508);
			((System.ComponentModel.ISupportInitialize)(this.groupControl)).EndInit();
			this.groupControl.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
		private DevExpress.XtraEditors.GroupControl groupControl;
	}
}
