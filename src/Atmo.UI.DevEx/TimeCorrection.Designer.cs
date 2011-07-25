namespace Atmo.UI.DevEx {
    partial class TimeCorrection {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TimeCorrection));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
			this.dateTimeRangePickerData = new Atmo.UI.DevEx.Controls.DateTimeRangePicker();
			this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
			this.dateTimeRangePickerCorrect = new Atmo.UI.DevEx.Controls.DateTimeRangePicker();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.cancelButton = new DevExpress.XtraEditors.SimpleButton();
			this.adjustButton = new DevExpress.XtraEditors.SimpleButton();
			this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
			this.sensorNameSelector = new DevExpress.XtraEditors.ComboBoxEdit();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
			this.groupControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
			this.groupControl2.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
			this.groupControl3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.sensorNameSelector.Properties)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.groupControl3, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(406, 266);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.Controls.Add(this.groupControl1, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.groupControl2, 0, 1);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 53);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 2;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(400, 180);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// groupControl1
			// 
			this.groupControl1.Controls.Add(this.dateTimeRangePickerData);
			this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupControl1.Location = new System.Drawing.Point(3, 3);
			this.groupControl1.Name = "groupControl1";
			this.groupControl1.Size = new System.Drawing.Size(394, 84);
			this.groupControl1.TabIndex = 0;
			this.groupControl1.Text = "Current Recorded Range";
			// 
			// dateTimeRangePickerData
			// 
			this.dateTimeRangePickerData.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dateTimeRangePickerData.From = new System.DateTime(((long)(0)));
			this.dateTimeRangePickerData.Location = new System.Drawing.Point(2, 22);
			this.dateTimeRangePickerData.Name = "dateTimeRangePickerData";
			this.dateTimeRangePickerData.Size = new System.Drawing.Size(390, 60);
			this.dateTimeRangePickerData.TabIndex = 0;
			this.dateTimeRangePickerData.To = new System.DateTime(((long)(0)));
			// 
			// groupControl2
			// 
			this.groupControl2.Controls.Add(this.dateTimeRangePickerCorrect);
			this.groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupControl2.Location = new System.Drawing.Point(3, 93);
			this.groupControl2.Name = "groupControl2";
			this.groupControl2.Size = new System.Drawing.Size(394, 84);
			this.groupControl2.TabIndex = 1;
			this.groupControl2.Text = "Correct To";
			// 
			// dateTimeRangePickerCorrect
			// 
			this.dateTimeRangePickerCorrect.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dateTimeRangePickerCorrect.From = new System.DateTime(((long)(0)));
			this.dateTimeRangePickerCorrect.Location = new System.Drawing.Point(2, 22);
			this.dateTimeRangePickerCorrect.Name = "dateTimeRangePickerCorrect";
			this.dateTimeRangePickerCorrect.Size = new System.Drawing.Size(390, 60);
			this.dateTimeRangePickerCorrect.TabIndex = 0;
			this.dateTimeRangePickerCorrect.To = new System.DateTime(((long)(0)));
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.ColumnCount = 2;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel3.Controls.Add(this.cancelButton, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.adjustButton, 1, 0);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 239);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 1;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(400, 24);
			this.tableLayoutPanel3.TabIndex = 1;
			// 
			// cancelButton
			// 
			this.cancelButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cancelButton.Location = new System.Drawing.Point(3, 3);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(194, 18);
			this.cancelButton.TabIndex = 0;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// adjustButton
			// 
			this.adjustButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.adjustButton.Location = new System.Drawing.Point(203, 3);
			this.adjustButton.Name = "adjustButton";
			this.adjustButton.Size = new System.Drawing.Size(194, 18);
			this.adjustButton.TabIndex = 1;
			this.adjustButton.Text = "Adjust";
			this.adjustButton.Click += new System.EventHandler(this.adjustButton_Click);
			// 
			// groupControl3
			// 
			this.groupControl3.Controls.Add(this.sensorNameSelector);
			this.groupControl3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupControl3.Location = new System.Drawing.Point(3, 3);
			this.groupControl3.Name = "groupControl3";
			this.groupControl3.Size = new System.Drawing.Size(400, 44);
			this.groupControl3.TabIndex = 2;
			this.groupControl3.Text = "Sensor Database";
			// 
			// sensorNameSelector
			// 
			this.sensorNameSelector.Dock = System.Windows.Forms.DockStyle.Fill;
			this.sensorNameSelector.Location = new System.Drawing.Point(2, 22);
			this.sensorNameSelector.Name = "sensorNameSelector";
			this.sensorNameSelector.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.sensorNameSelector.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
			this.sensorNameSelector.Size = new System.Drawing.Size(396, 20);
			this.sensorNameSelector.TabIndex = 0;
			// 
			// TimeCorrection
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(406, 266);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(400, 260);
			this.Name = "TimeCorrection";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Time Correction";
			this.Load += new System.EventHandler(this.TimeCorrection_Load);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
			this.groupControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
			this.groupControl2.ResumeLayout(false);
			this.tableLayoutPanel3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
			this.groupControl3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.sensorNameSelector.Properties)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private Controls.DateTimeRangePicker dateTimeRangePickerData;
        private Controls.DateTimeRangePicker dateTimeRangePickerCorrect;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private DevExpress.XtraEditors.SimpleButton cancelButton;
        private DevExpress.XtraEditors.SimpleButton adjustButton;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraEditors.ComboBoxEdit sensorNameSelector;
    }
}