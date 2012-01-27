namespace Atmo.UI.DevEx {
    partial class PatcherForm {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PatcherForm));
			this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
			this.daqFileLabel = new DevExpress.XtraEditors.LabelControl();
			this.daqUpdateProgress = new DevExpress.XtraEditors.ProgressBarControl();
			this.beginDaqUpdate = new DevExpress.XtraEditors.SimpleButton();
			this.daqUpdateFileChooser = new DevExpress.XtraEditors.SimpleButton();
			this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
			this.spinEditDesiredDirection = new DevExpress.XtraEditors.SpinEdit();
			this.simpleButtonDirOffset = new DevExpress.XtraEditors.SimpleButton();
			this.getCorrection = new DevExpress.XtraEditors.SimpleButton();
			this.textEditFactors = new DevExpress.XtraEditors.TextEdit();
			this.sendCorrection = new DevExpress.XtraEditors.SimpleButton();
			this.radioGroup1 = new DevExpress.XtraEditors.RadioGroup();
			this.anemUpdateProgress = new DevExpress.XtraEditors.ProgressBarControl();
			this.beginAnemUpdate = new DevExpress.XtraEditors.SimpleButton();
			this.anemFileLabel = new DevExpress.XtraEditors.LabelControl();
			this.anemUpdateFileChooser = new DevExpress.XtraEditors.SimpleButton();
			this.daqUpgradeThread = new System.ComponentModel.BackgroundWorker();
			this.anemUpgradeThread = new System.ComponentModel.BackgroundWorker();
			((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
			this.groupControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.daqUpdateProgress.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
			this.groupControl2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.spinEditDesiredDirection.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.textEditFactors.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.anemUpdateProgress.Properties)).BeginInit();
			this.SuspendLayout();
			// 
			// groupControl1
			// 
			this.groupControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupControl1.Controls.Add(this.daqFileLabel);
			this.groupControl1.Controls.Add(this.daqUpdateProgress);
			this.groupControl1.Controls.Add(this.beginDaqUpdate);
			this.groupControl1.Controls.Add(this.daqUpdateFileChooser);
			this.groupControl1.Location = new System.Drawing.Point(12, 12);
			this.groupControl1.Name = "groupControl1";
			this.groupControl1.Size = new System.Drawing.Size(383, 84);
			this.groupControl1.TabIndex = 0;
			this.groupControl1.Text = "Update Logger/DAQ";
			// 
			// daqFileLabel
			// 
			this.daqFileLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.daqFileLabel.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.daqFileLabel.Location = new System.Drawing.Point(167, 25);
			this.daqFileLabel.Name = "daqFileLabel";
			this.daqFileLabel.Size = new System.Drawing.Size(211, 23);
			this.daqFileLabel.TabIndex = 3;
			this.daqFileLabel.Text = "Choose a file";
			// 
			// daqUpdateProgress
			// 
			this.daqUpdateProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.daqUpdateProgress.EditValue = "0";
			this.daqUpdateProgress.Location = new System.Drawing.Point(167, 54);
			this.daqUpdateProgress.Name = "daqUpdateProgress";
			this.daqUpdateProgress.Size = new System.Drawing.Size(211, 23);
			this.daqUpdateProgress.TabIndex = 2;
			// 
			// beginDaqUpdate
			// 
			this.beginDaqUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.beginDaqUpdate.Enabled = false;
			this.beginDaqUpdate.Location = new System.Drawing.Point(5, 54);
			this.beginDaqUpdate.Name = "beginDaqUpdate";
			this.beginDaqUpdate.Size = new System.Drawing.Size(156, 23);
			this.beginDaqUpdate.TabIndex = 1;
			this.beginDaqUpdate.Text = "Update Logger/DAQ";
			this.beginDaqUpdate.Click += new System.EventHandler(this.beginDaqUpdate_Click);
			// 
			// daqUpdateFileChooser
			// 
			this.daqUpdateFileChooser.Location = new System.Drawing.Point(5, 25);
			this.daqUpdateFileChooser.Name = "daqUpdateFileChooser";
			this.daqUpdateFileChooser.Size = new System.Drawing.Size(156, 23);
			this.daqUpdateFileChooser.TabIndex = 0;
			this.daqUpdateFileChooser.Text = "Select File...";
			this.daqUpdateFileChooser.Click += new System.EventHandler(this.daqUpdateFileChooser_Click);
			// 
			// groupControl2
			// 
			this.groupControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupControl2.Controls.Add(this.spinEditDesiredDirection);
			this.groupControl2.Controls.Add(this.simpleButtonDirOffset);
			this.groupControl2.Controls.Add(this.getCorrection);
			this.groupControl2.Controls.Add(this.textEditFactors);
			this.groupControl2.Controls.Add(this.sendCorrection);
			this.groupControl2.Controls.Add(this.radioGroup1);
			this.groupControl2.Controls.Add(this.anemUpdateProgress);
			this.groupControl2.Controls.Add(this.beginAnemUpdate);
			this.groupControl2.Controls.Add(this.anemFileLabel);
			this.groupControl2.Controls.Add(this.anemUpdateFileChooser);
			this.groupControl2.Location = new System.Drawing.Point(12, 102);
			this.groupControl2.Name = "groupControl2";
			this.groupControl2.Size = new System.Drawing.Size(383, 216);
			this.groupControl2.TabIndex = 1;
			this.groupControl2.Text = "Update Anemometers";
			// 
			// spinEditDesiredDirection
			// 
			this.spinEditDesiredDirection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.spinEditDesiredDirection.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.spinEditDesiredDirection.Location = new System.Drawing.Point(141, 189);
			this.spinEditDesiredDirection.Name = "spinEditDesiredDirection";
			this.spinEditDesiredDirection.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
			this.spinEditDesiredDirection.Properties.IsFloatValue = false;
			this.spinEditDesiredDirection.Properties.Mask.EditMask = "N00";
			this.spinEditDesiredDirection.Properties.MaxValue = new decimal(new int[] {
            360,
            0,
            0,
            0});
			this.spinEditDesiredDirection.Size = new System.Drawing.Size(52, 20);
			this.spinEditDesiredDirection.TabIndex = 12;
			// 
			// simpleButtonDirOffset
			// 
			this.simpleButtonDirOffset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.simpleButtonDirOffset.Location = new System.Drawing.Point(5, 188);
			this.simpleButtonDirOffset.Name = "simpleButtonDirOffset";
			this.simpleButtonDirOffset.Size = new System.Drawing.Size(130, 23);
			this.simpleButtonDirOffset.TabIndex = 11;
			this.simpleButtonDirOffset.Text = "Change Direction Offset";
			this.simpleButtonDirOffset.Click += new System.EventHandler(this.simpleButtonDirOffset_Click);
			// 
			// getCorrection
			// 
			this.getCorrection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.getCorrection.Location = new System.Drawing.Point(222, 188);
			this.getCorrection.Name = "getCorrection";
			this.getCorrection.Size = new System.Drawing.Size(45, 23);
			this.getCorrection.TabIndex = 10;
			this.getCorrection.Text = "Get";
			this.getCorrection.Click += new System.EventHandler(this.getCorrection_Click);
			// 
			// textEditFactors
			// 
			this.textEditFactors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textEditFactors.Location = new System.Drawing.Point(5, 162);
			this.textEditFactors.Name = "textEditFactors";
			this.textEditFactors.Size = new System.Drawing.Size(373, 20);
			this.textEditFactors.TabIndex = 9;
			this.textEditFactors.EditValueChanged += new System.EventHandler(this.textEditFactors_EditValueChanged);
			// 
			// sendCorrection
			// 
			this.sendCorrection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.sendCorrection.Enabled = false;
			this.sendCorrection.Location = new System.Drawing.Point(273, 188);
			this.sendCorrection.Name = "sendCorrection";
			this.sendCorrection.Size = new System.Drawing.Size(105, 23);
			this.sendCorrection.TabIndex = 8;
			this.sendCorrection.Text = "Apply Correction";
			this.sendCorrection.Click += new System.EventHandler(this.sendCorrection_Click);
			// 
			// radioGroup1
			// 
			this.radioGroup1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.radioGroup1.Location = new System.Drawing.Point(5, 25);
			this.radioGroup1.Name = "radioGroup1";
			this.radioGroup1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(0, "Sensor A"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "Sensor B"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(2, "Sensor C"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(3, "Sensor D"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(255, "Unassigned")});
			this.radioGroup1.Size = new System.Drawing.Size(373, 37);
			this.radioGroup1.TabIndex = 7;
			this.radioGroup1.SelectedIndexChanged += new System.EventHandler(this.radioGroup1_SelectedIndexChanged);
			// 
			// anemUpdateProgress
			// 
			this.anemUpdateProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.anemUpdateProgress.Location = new System.Drawing.Point(167, 99);
			this.anemUpdateProgress.Name = "anemUpdateProgress";
			this.anemUpdateProgress.Size = new System.Drawing.Size(211, 23);
			this.anemUpdateProgress.TabIndex = 6;
			// 
			// beginAnemUpdate
			// 
			this.beginAnemUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.beginAnemUpdate.Enabled = false;
			this.beginAnemUpdate.Location = new System.Drawing.Point(5, 99);
			this.beginAnemUpdate.Name = "beginAnemUpdate";
			this.beginAnemUpdate.Size = new System.Drawing.Size(156, 23);
			this.beginAnemUpdate.TabIndex = 5;
			this.beginAnemUpdate.Text = "Update Anemometer(s)";
			this.beginAnemUpdate.Click += new System.EventHandler(this.beginAnemUpdate_Click);
			// 
			// anemFileLabel
			// 
			this.anemFileLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.anemFileLabel.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.anemFileLabel.Location = new System.Drawing.Point(167, 70);
			this.anemFileLabel.Name = "anemFileLabel";
			this.anemFileLabel.Size = new System.Drawing.Size(211, 23);
			this.anemFileLabel.TabIndex = 5;
			this.anemFileLabel.Text = "Choose a file";
			// 
			// anemUpdateFileChooser
			// 
			this.anemUpdateFileChooser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.anemUpdateFileChooser.Location = new System.Drawing.Point(5, 70);
			this.anemUpdateFileChooser.Name = "anemUpdateFileChooser";
			this.anemUpdateFileChooser.Size = new System.Drawing.Size(156, 23);
			this.anemUpdateFileChooser.TabIndex = 4;
			this.anemUpdateFileChooser.Text = "Select File...";
			this.anemUpdateFileChooser.Click += new System.EventHandler(this.anemUpdateFileChooser_Click);
			// 
			// daqUpgradeThread
			// 
			this.daqUpgradeThread.WorkerReportsProgress = true;
			this.daqUpgradeThread.DoWork += new System.ComponentModel.DoWorkEventHandler(this.daqUpgradeThread_DoWork);
			this.daqUpgradeThread.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.daqUpgradeThread_ProgressChanged);
			this.daqUpgradeThread.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.daqUpgradeThread_RunWorkerCompleted);
			// 
			// anemUpgradeThread
			// 
			this.anemUpgradeThread.WorkerReportsProgress = true;
			this.anemUpgradeThread.DoWork += new System.ComponentModel.DoWorkEventHandler(this.anemUpgradeThread_DoWork);
			this.anemUpgradeThread.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.anemUpgradeThread_ProgressChanged);
			this.anemUpgradeThread.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.anemUpgradeThread_RunWorkerCompleted);
			// 
			// PatcherForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(407, 331);
			this.Controls.Add(this.groupControl2);
			this.Controls.Add(this.groupControl1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(1024, 500);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(397, 279);
			this.Name = "PatcherForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Upgrade Firmware";
			((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
			this.groupControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.daqUpdateProgress.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
			this.groupControl2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.spinEditDesiredDirection.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.textEditFactors.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.anemUpdateProgress.Properties)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.LabelControl daqFileLabel;
        private DevExpress.XtraEditors.ProgressBarControl daqUpdateProgress;
        private DevExpress.XtraEditors.SimpleButton beginDaqUpdate;
        private DevExpress.XtraEditors.SimpleButton daqUpdateFileChooser;
        private DevExpress.XtraEditors.LabelControl anemFileLabel;
        private DevExpress.XtraEditors.SimpleButton anemUpdateFileChooser;
        private DevExpress.XtraEditors.ProgressBarControl anemUpdateProgress;
        private DevExpress.XtraEditors.SimpleButton beginAnemUpdate;
        private System.ComponentModel.BackgroundWorker daqUpgradeThread;
        private System.ComponentModel.BackgroundWorker anemUpgradeThread;
        private DevExpress.XtraEditors.RadioGroup radioGroup1;
        private DevExpress.XtraEditors.TextEdit textEditFactors;
        private DevExpress.XtraEditors.SimpleButton sendCorrection;
        private DevExpress.XtraEditors.SimpleButton getCorrection;
		private DevExpress.XtraEditors.SimpleButton simpleButtonDirOffset;
		private DevExpress.XtraEditors.SpinEdit spinEditDesiredDirection;
    }
}