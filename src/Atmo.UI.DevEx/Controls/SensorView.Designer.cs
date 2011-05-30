namespace Atmo.UI.DevEx.Controls {
	partial class SensorView {
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
			this.sensorNameLabel = new DevExpress.XtraEditors.LabelControl();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
			this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
			this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
			this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
			this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
			this.windSpeedValue = new DevExpress.XtraEditors.LabelControl();
			this.windDirValue = new DevExpress.XtraEditors.LabelControl();
			this.tempValue = new DevExpress.XtraEditors.LabelControl();
			this.humidityValue = new DevExpress.XtraEditors.LabelControl();
			this.pressureValue = new DevExpress.XtraEditors.LabelControl();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// sensorNameLabel
			// 
			this.sensorNameLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 12.75F, System.Drawing.FontStyle.Bold);
			this.sensorNameLabel.Appearance.Options.UseFont = true;
			this.sensorNameLabel.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.sensorNameLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.sensorNameLabel.Location = new System.Drawing.Point(0, 0);
			this.sensorNameLabel.Name = "sensorNameLabel";
			this.sensorNameLabel.Size = new System.Drawing.Size(246, 21);
			this.sensorNameLabel.TabIndex = 0;
			this.sensorNameLabel.Text = "Sensor";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.labelControl5, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.labelControl1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.labelControl2, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.labelControl3, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.labelControl4, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.windSpeedValue, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.windDirValue, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.tempValue, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.humidityValue, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.pressureValue, 1, 4);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 21);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 5;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(246, 103);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// labelControl5
			// 
			this.labelControl5.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.labelControl5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelControl5.Location = new System.Drawing.Point(2, 82);
			this.labelControl5.Margin = new System.Windows.Forms.Padding(2);
			this.labelControl5.Name = "labelControl5";
			this.labelControl5.Size = new System.Drawing.Size(69, 19);
			this.labelControl5.TabIndex = 5;
			this.labelControl5.Text = "Pressure: ";
			// 
			// labelControl1
			// 
			this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.labelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelControl1.Location = new System.Drawing.Point(2, 2);
			this.labelControl1.Margin = new System.Windows.Forms.Padding(2);
			this.labelControl1.Name = "labelControl1";
			this.labelControl1.Size = new System.Drawing.Size(69, 16);
			this.labelControl1.TabIndex = 1;
			this.labelControl1.Text = "Wind Speed: ";
			// 
			// labelControl2
			// 
			this.labelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.labelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelControl2.Location = new System.Drawing.Point(2, 22);
			this.labelControl2.Margin = new System.Windows.Forms.Padding(2);
			this.labelControl2.Name = "labelControl2";
			this.labelControl2.Size = new System.Drawing.Size(69, 16);
			this.labelControl2.TabIndex = 2;
			this.labelControl2.Text = "Direction:  ";
			// 
			// labelControl3
			// 
			this.labelControl3.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.labelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelControl3.Location = new System.Drawing.Point(2, 42);
			this.labelControl3.Margin = new System.Windows.Forms.Padding(2);
			this.labelControl3.Name = "labelControl3";
			this.labelControl3.Size = new System.Drawing.Size(69, 16);
			this.labelControl3.TabIndex = 3;
			this.labelControl3.Text = "Temperature: ";
			// 
			// labelControl4
			// 
			this.labelControl4.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.labelControl4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelControl4.Location = new System.Drawing.Point(2, 62);
			this.labelControl4.Margin = new System.Windows.Forms.Padding(2);
			this.labelControl4.Name = "labelControl4";
			this.labelControl4.Size = new System.Drawing.Size(69, 16);
			this.labelControl4.TabIndex = 4;
			this.labelControl4.Text = "Humidity: ";
			// 
			// windSpeedValue
			// 
			this.windSpeedValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.windSpeedValue.Dock = System.Windows.Forms.DockStyle.Fill;
			this.windSpeedValue.Location = new System.Drawing.Point(75, 2);
			this.windSpeedValue.Margin = new System.Windows.Forms.Padding(2);
			this.windSpeedValue.Name = "windSpeedValue";
			this.windSpeedValue.Size = new System.Drawing.Size(169, 16);
			this.windSpeedValue.TabIndex = 6;
			this.windSpeedValue.Text = "N/A";
			// 
			// windDirValue
			// 
			this.windDirValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.windDirValue.Dock = System.Windows.Forms.DockStyle.Fill;
			this.windDirValue.Location = new System.Drawing.Point(75, 22);
			this.windDirValue.Margin = new System.Windows.Forms.Padding(2);
			this.windDirValue.Name = "windDirValue";
			this.windDirValue.Size = new System.Drawing.Size(169, 16);
			this.windDirValue.TabIndex = 7;
			this.windDirValue.Text = "N/A";
			// 
			// tempValue
			// 
			this.tempValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.tempValue.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tempValue.Location = new System.Drawing.Point(75, 42);
			this.tempValue.Margin = new System.Windows.Forms.Padding(2);
			this.tempValue.Name = "tempValue";
			this.tempValue.Size = new System.Drawing.Size(169, 16);
			this.tempValue.TabIndex = 8;
			this.tempValue.Text = "N/A";
			// 
			// humidityValue
			// 
			this.humidityValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.humidityValue.Dock = System.Windows.Forms.DockStyle.Fill;
			this.humidityValue.Location = new System.Drawing.Point(75, 62);
			this.humidityValue.Margin = new System.Windows.Forms.Padding(2);
			this.humidityValue.Name = "humidityValue";
			this.humidityValue.Size = new System.Drawing.Size(169, 16);
			this.humidityValue.TabIndex = 9;
			this.humidityValue.Text = "N/A";
			// 
			// pressureValue
			// 
			this.pressureValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.pressureValue.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pressureValue.Location = new System.Drawing.Point(75, 82);
			this.pressureValue.Margin = new System.Windows.Forms.Padding(2);
			this.pressureValue.Name = "pressureValue";
			this.pressureValue.Size = new System.Drawing.Size(169, 19);
			this.pressureValue.TabIndex = 10;
			this.pressureValue.Text = "N/A";
			// 
			// SensorView
			// 
			this.Appearance.BackColor = System.Drawing.SystemColors.Window;
			this.Appearance.Options.UseBackColor = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.sensorNameLabel);
			this.Name = "SensorView";
			this.Size = new System.Drawing.Size(246, 124);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraEditors.LabelControl sensorNameLabel;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private DevExpress.XtraEditors.LabelControl labelControl5;
		private DevExpress.XtraEditors.LabelControl labelControl1;
		private DevExpress.XtraEditors.LabelControl labelControl2;
		private DevExpress.XtraEditors.LabelControl labelControl3;
		private DevExpress.XtraEditors.LabelControl labelControl4;
		private DevExpress.XtraEditors.LabelControl windSpeedValue;
		private DevExpress.XtraEditors.LabelControl windDirValue;
		private DevExpress.XtraEditors.LabelControl tempValue;
		private DevExpress.XtraEditors.LabelControl humidityValue;
		private DevExpress.XtraEditors.LabelControl pressureValue;
	}
}
