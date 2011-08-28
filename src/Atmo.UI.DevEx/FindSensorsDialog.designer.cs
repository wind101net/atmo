using Atmo.UI.DevEx.Controls;

namespace Atmo.UI.DevEx {
    partial class FindSensorsDialog {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindSensorsDialog));
			this.updateTimer = new System.Windows.Forms.Timer(this.components);
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.sensorSetupLine1 = new Atmo.UI.DevEx.Controls.SensorSetupLine();
			this.sensorSetupLine2 = new Atmo.UI.DevEx.Controls.SensorSetupLine();
			this.sensorSetupLine3 = new Atmo.UI.DevEx.Controls.SensorSetupLine();
			this.sensorSetupLine4 = new Atmo.UI.DevEx.Controls.SensorSetupLine();
			this.addRemWorker = new System.ComponentModel.BackgroundWorker();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// updateTimer
			// 
			this.updateTimer.Enabled = true;
			this.updateTimer.Interval = 500;
			this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.sensorSetupLine1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.sensorSetupLine2, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.sensorSetupLine3, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.sensorSetupLine4, 0, 3);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(260, 142);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// sensorSetupLine1
			// 
			this.sensorSetupLine1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.sensorSetupLine1.Location = new System.Drawing.Point(3, 3);
			this.sensorSetupLine1.Name = "sensorSetupLine1";
			this.sensorSetupLine1.SensorId = 0;
			this.sensorSetupLine1.Size = new System.Drawing.Size(254, 29);
			this.sensorSetupLine1.Status = Atmo.SensorStatus.Unknown;
			this.sensorSetupLine1.TabIndex = 0;
			this.sensorSetupLine1.SensorRemove += new System.Action<int>(this.sensorSetupLine_SensorRemove);
			this.sensorSetupLine1.SensorSet += new System.Action<int>(this.sensorSetupLine_SensorSet);
			// 
			// sensorSetupLine2
			// 
			this.sensorSetupLine2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.sensorSetupLine2.Location = new System.Drawing.Point(3, 38);
			this.sensorSetupLine2.Name = "sensorSetupLine2";
			this.sensorSetupLine2.SensorId = 0;
			this.sensorSetupLine2.Size = new System.Drawing.Size(254, 29);
			this.sensorSetupLine2.Status = Atmo.SensorStatus.Unknown;
			this.sensorSetupLine2.TabIndex = 1;
			this.sensorSetupLine2.SensorRemove += new System.Action<int>(this.sensorSetupLine_SensorRemove);
			this.sensorSetupLine2.SensorSet += new System.Action<int>(this.sensorSetupLine_SensorSet);
			// 
			// sensorSetupLine3
			// 
			this.sensorSetupLine3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.sensorSetupLine3.Location = new System.Drawing.Point(3, 73);
			this.sensorSetupLine3.Name = "sensorSetupLine3";
			this.sensorSetupLine3.SensorId = 0;
			this.sensorSetupLine3.Size = new System.Drawing.Size(254, 29);
			this.sensorSetupLine3.Status = Atmo.SensorStatus.Unknown;
			this.sensorSetupLine3.TabIndex = 2;
			this.sensorSetupLine3.SensorRemove += new System.Action<int>(this.sensorSetupLine_SensorRemove);
			this.sensorSetupLine3.SensorSet += new System.Action<int>(this.sensorSetupLine_SensorSet);
			// 
			// sensorSetupLine4
			// 
			this.sensorSetupLine4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.sensorSetupLine4.Location = new System.Drawing.Point(3, 108);
			this.sensorSetupLine4.Name = "sensorSetupLine4";
			this.sensorSetupLine4.SensorId = 0;
			this.sensorSetupLine4.Size = new System.Drawing.Size(254, 31);
			this.sensorSetupLine4.Status = Atmo.SensorStatus.Unknown;
			this.sensorSetupLine4.TabIndex = 3;
			this.sensorSetupLine4.SensorRemove += new System.Action<int>(this.sensorSetupLine_SensorRemove);
			this.sensorSetupLine4.SensorSet += new System.Action<int>(this.sensorSetupLine_SensorSet);
			// 
			// addRemWorker
			// 
			this.addRemWorker.WorkerReportsProgress = true;
			this.addRemWorker.WorkerSupportsCancellation = true;
			this.addRemWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.addRemWorker_DoWork);
			this.addRemWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.addRemWorker_ProgressChanged);
			this.addRemWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.addRemWorker_RunWorkerCompleted);
			// 
			// FindSensorsDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 166);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(300, 180);
			this.Name = "FindSensorsDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Find Sensors";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SensorSetupDialog_FormClosing);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer updateTimer;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private SensorSetupLine sensorSetupLine1;
        private SensorSetupLine sensorSetupLine2;
        private SensorSetupLine sensorSetupLine3;
        private SensorSetupLine sensorSetupLine4;
        private System.ComponentModel.BackgroundWorker addRemWorker;
    }
}