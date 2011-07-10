namespace Atmo.UI.DevEx {
    partial class ImportDataForm {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportDataForm));
			this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
			this.listBoxAnemFiles = new DevExpress.XtraEditors.ListBoxControl();
			this.textEditFolderPath = new DevExpress.XtraEditors.TextEdit();
			this.buttonSelectDataFolder = new DevExpress.XtraEditors.SimpleButton();
			this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
			this.buttonCancel = new DevExpress.XtraEditors.SimpleButton();
			this.buttonImport = new DevExpress.XtraEditors.SimpleButton();
			this.progressBarControl1 = new DevExpress.XtraEditors.ProgressBarControl();
			this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
			this.syncChk = new DevExpress.XtraEditors.CheckEdit();
			this.daqCheckTimer = new System.Windows.Forms.Timer(this.components);
			this.groupControl4 = new DevExpress.XtraEditors.GroupControl();
			this.chkOverwrite = new DevExpress.XtraEditors.CheckEdit();
			this.importAnemMap3 = new Atmo.UI.DevEx.Controls.ImportAnemMap();
			this.importAnemMap2 = new Atmo.UI.DevEx.Controls.ImportAnemMap();
			this.importAnemMap1 = new Atmo.UI.DevEx.Controls.ImportAnemMap();
			this.importAnemMap0 = new Atmo.UI.DevEx.Controls.ImportAnemMap();
			((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
			this.groupControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.listBoxAnemFiles)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.textEditFolderPath.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
			this.groupControl2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.progressBarControl1.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
			this.groupControl3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.syncChk.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.groupControl4)).BeginInit();
			this.groupControl4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.chkOverwrite.Properties)).BeginInit();
			this.SuspendLayout();
			// 
			// groupControl1
			// 
			this.groupControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupControl1.Controls.Add(this.listBoxAnemFiles);
			this.groupControl1.Controls.Add(this.textEditFolderPath);
			this.groupControl1.Controls.Add(this.buttonSelectDataFolder);
			this.groupControl1.Location = new System.Drawing.Point(12, 12);
			this.groupControl1.Name = "groupControl1";
			this.groupControl1.Size = new System.Drawing.Size(563, 137);
			this.groupControl1.TabIndex = 0;
			this.groupControl1.Text = "1. Data Source";
			// 
			// listBoxAnemFiles
			// 
			this.listBoxAnemFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.listBoxAnemFiles.Location = new System.Drawing.Point(5, 51);
			this.listBoxAnemFiles.Name = "listBoxAnemFiles";
			this.listBoxAnemFiles.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.listBoxAnemFiles.Size = new System.Drawing.Size(553, 81);
			this.listBoxAnemFiles.TabIndex = 3;
			// 
			// textEditFolderPath
			// 
			this.textEditFolderPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.textEditFolderPath.Location = new System.Drawing.Point(120, 25);
			this.textEditFolderPath.Name = "textEditFolderPath";
			this.textEditFolderPath.Properties.ReadOnly = true;
			this.textEditFolderPath.Size = new System.Drawing.Size(438, 20);
			this.textEditFolderPath.TabIndex = 2;
			// 
			// buttonSelectDataFolder
			// 
			this.buttonSelectDataFolder.Location = new System.Drawing.Point(5, 25);
			this.buttonSelectDataFolder.Name = "buttonSelectDataFolder";
			this.buttonSelectDataFolder.Size = new System.Drawing.Size(109, 20);
			this.buttonSelectDataFolder.TabIndex = 1;
			this.buttonSelectDataFolder.Text = "Select Data Folder";
			this.buttonSelectDataFolder.Click += new System.EventHandler(this.buttonSelectDataFolder_Click);
			// 
			// groupControl2
			// 
			this.groupControl2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupControl2.Controls.Add(this.importAnemMap3);
			this.groupControl2.Controls.Add(this.importAnemMap2);
			this.groupControl2.Controls.Add(this.importAnemMap1);
			this.groupControl2.Controls.Add(this.importAnemMap0);
			this.groupControl2.Location = new System.Drawing.Point(12, 155);
			this.groupControl2.Name = "groupControl2";
			this.groupControl2.Size = new System.Drawing.Size(563, 153);
			this.groupControl2.TabIndex = 1;
			this.groupControl2.Text = "2. Select Anemometers For Import";
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonCancel.Location = new System.Drawing.Point(12, 442);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// buttonImport
			// 
			this.buttonImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonImport.Enabled = false;
			this.buttonImport.Location = new System.Drawing.Point(93, 442);
			this.buttonImport.Name = "buttonImport";
			this.buttonImport.Size = new System.Drawing.Size(75, 23);
			this.buttonImport.TabIndex = 3;
			this.buttonImport.Text = "Import";
			this.buttonImport.Click += new System.EventHandler(this.buttonImport_Click);
			// 
			// progressBarControl1
			// 
			this.progressBarControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.progressBarControl1.Enabled = false;
			this.progressBarControl1.Location = new System.Drawing.Point(174, 442);
			this.progressBarControl1.Name = "progressBarControl1";
			this.progressBarControl1.Size = new System.Drawing.Size(401, 23);
			this.progressBarControl1.TabIndex = 4;
			// 
			// groupControl3
			// 
			this.groupControl3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupControl3.Controls.Add(this.syncChk);
			this.groupControl3.Location = new System.Drawing.Point(12, 314);
			this.groupControl3.Name = "groupControl3";
			this.groupControl3.Size = new System.Drawing.Size(563, 58);
			this.groupControl3.TabIndex = 5;
			this.groupControl3.Text = "3. Synchronization";
			// 
			// syncChk
			// 
			this.syncChk.Enabled = false;
			this.syncChk.Location = new System.Drawing.Point(10, 30);
			this.syncChk.Name = "syncChk";
			this.syncChk.Properties.Caption = "Synchronize time and adjust data after import.";
			this.syncChk.Size = new System.Drawing.Size(545, 18);
			this.syncChk.TabIndex = 0;
			// 
			// daqCheckTimer
			// 
			this.daqCheckTimer.Enabled = true;
			this.daqCheckTimer.Interval = 333;
			this.daqCheckTimer.Tick += new System.EventHandler(this.daqCheckTimer_Tick);
			// 
			// groupControl4
			// 
			this.groupControl4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupControl4.Controls.Add(this.chkOverwrite);
			this.groupControl4.Location = new System.Drawing.Point(12, 378);
			this.groupControl4.Name = "groupControl4";
			this.groupControl4.Size = new System.Drawing.Size(563, 58);
			this.groupControl4.TabIndex = 6;
			this.groupControl4.Text = "4. Options";
			// 
			// chkOverwrite
			// 
			this.chkOverwrite.Location = new System.Drawing.Point(10, 31);
			this.chkOverwrite.Name = "chkOverwrite";
			this.chkOverwrite.Properties.Caption = "Overwrite";
			this.chkOverwrite.Size = new System.Drawing.Size(75, 18);
			this.chkOverwrite.TabIndex = 0;
			// 
			// importAnemMap3
			// 
			this.importAnemMap3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.importAnemMap3.AnemId = null;
			this.importAnemMap3.Checked = true;
			this.importAnemMap3.DatabaseSensorId = "";
			this.importAnemMap3.Enabled = false;
			this.importAnemMap3.Location = new System.Drawing.Point(5, 121);
			this.importAnemMap3.Name = "importAnemMap3";
			this.importAnemMap3.Size = new System.Drawing.Size(553, 26);
			this.importAnemMap3.StartStamp = new System.DateTime(((long)(0)));
			this.importAnemMap3.TabIndex = 3;
			// 
			// importAnemMap2
			// 
			this.importAnemMap2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.importAnemMap2.AnemId = null;
			this.importAnemMap2.Checked = true;
			this.importAnemMap2.DatabaseSensorId = "";
			this.importAnemMap2.Enabled = false;
			this.importAnemMap2.Location = new System.Drawing.Point(5, 89);
			this.importAnemMap2.Name = "importAnemMap2";
			this.importAnemMap2.Size = new System.Drawing.Size(553, 26);
			this.importAnemMap2.StartStamp = new System.DateTime(((long)(0)));
			this.importAnemMap2.TabIndex = 2;
			// 
			// importAnemMap1
			// 
			this.importAnemMap1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.importAnemMap1.AnemId = null;
			this.importAnemMap1.Checked = true;
			this.importAnemMap1.DatabaseSensorId = "";
			this.importAnemMap1.Enabled = false;
			this.importAnemMap1.Location = new System.Drawing.Point(5, 57);
			this.importAnemMap1.Name = "importAnemMap1";
			this.importAnemMap1.Size = new System.Drawing.Size(553, 26);
			this.importAnemMap1.StartStamp = new System.DateTime(((long)(0)));
			this.importAnemMap1.TabIndex = 1;
			// 
			// importAnemMap0
			// 
			this.importAnemMap0.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.importAnemMap0.AnemId = null;
			this.importAnemMap0.Checked = true;
			this.importAnemMap0.DatabaseSensorId = "";
			this.importAnemMap0.Enabled = false;
			this.importAnemMap0.Location = new System.Drawing.Point(5, 25);
			this.importAnemMap0.Name = "importAnemMap0";
			this.importAnemMap0.Size = new System.Drawing.Size(553, 26);
			this.importAnemMap0.StartStamp = new System.DateTime(((long)(0)));
			this.importAnemMap0.TabIndex = 0;
			// 
			// ImportDataForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(587, 477);
			this.Controls.Add(this.groupControl4);
			this.Controls.Add(this.groupControl3);
			this.Controls.Add(this.progressBarControl1);
			this.Controls.Add(this.buttonImport);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.groupControl2);
			this.Controls.Add(this.groupControl1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(600, 450);
			this.Name = "ImportDataForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Download Sensor Data";
			this.Load += new System.EventHandler(this.ImportDataForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
			this.groupControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.listBoxAnemFiles)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.textEditFolderPath.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
			this.groupControl2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.progressBarControl1.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
			this.groupControl3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.syncChk.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.groupControl4)).EndInit();
			this.groupControl4.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.chkOverwrite.Properties)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.TextEdit textEditFolderPath;
        private DevExpress.XtraEditors.SimpleButton buttonSelectDataFolder;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private Atmo.UI.DevEx.Controls.ImportAnemMap importAnemMap3;
        private Atmo.UI.DevEx.Controls.ImportAnemMap importAnemMap2;
        private Atmo.UI.DevEx.Controls.ImportAnemMap importAnemMap1;
        private Atmo.UI.DevEx.Controls.ImportAnemMap importAnemMap0;
        private DevExpress.XtraEditors.SimpleButton buttonCancel;
        private DevExpress.XtraEditors.SimpleButton buttonImport;
        private DevExpress.XtraEditors.ProgressBarControl progressBarControl1;
        private DevExpress.XtraEditors.ListBoxControl listBoxAnemFiles;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraEditors.CheckEdit syncChk;
        private System.Windows.Forms.Timer daqCheckTimer;
        private DevExpress.XtraEditors.GroupControl groupControl4;
        private DevExpress.XtraEditors.CheckEdit chkOverwrite;
    }
}