namespace Atmo.UI.DevEx {
    partial class TimeSync {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TimeSync));
			this.syncButton = new DevExpress.XtraEditors.SimpleButton();
			this.chkAdjust = new DevExpress.XtraEditors.CheckEdit();
			((System.ComponentModel.ISupportInitialize)(this.chkAdjust.Properties)).BeginInit();
			this.SuspendLayout();
			// 
			// syncButton
			// 
			this.syncButton.Location = new System.Drawing.Point(14, 12);
			this.syncButton.Name = "syncButton";
			this.syncButton.Size = new System.Drawing.Size(165, 23);
			this.syncButton.TabIndex = 1;
			this.syncButton.Text = "Sync. Logger TIme";
			this.syncButton.Click += new System.EventHandler(this.syncButton_Click);
			// 
			// chkAdjust
			// 
			this.chkAdjust.Enabled = false;
			this.chkAdjust.Location = new System.Drawing.Point(14, 44);
			this.chkAdjust.Name = "chkAdjust";
			this.chkAdjust.Properties.Caption = "Adjust Data";
			this.chkAdjust.Size = new System.Drawing.Size(165, 18);
			this.chkAdjust.TabIndex = 2;
			this.chkAdjust.Visible = false;
			// 
			// TimeSync
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(191, 45);
			this.Controls.Add(this.chkAdjust);
			this.Controls.Add(this.syncButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "TimeSync";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Logger Time Sync";
			this.Load += new System.EventHandler(this.TimeSync_Load);
			((System.ComponentModel.ISupportInitialize)(this.chkAdjust.Properties)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton syncButton;
        private DevExpress.XtraEditors.CheckEdit chkAdjust;
    }
}