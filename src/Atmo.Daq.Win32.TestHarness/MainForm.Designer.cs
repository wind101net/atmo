namespace Atmo.Daq.Win32.TestHarness {
	partial class MainForm {
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
			this.buttonConnect = new System.Windows.Forms.Button();
			this.labelConnect = new System.Windows.Forms.Label();
			this.timerProperties = new System.Windows.Forms.Timer(this.components);
			this.buttonDisconnect = new System.Windows.Forms.Button();
			this.buttonStartQuery = new System.Windows.Forms.Button();
			this.buttonStopQuery = new System.Windows.Forms.Button();
			this.labelIsQuery = new System.Windows.Forms.Label();
			this.timerQuery = new System.Windows.Forms.Timer(this.components);
			this.label6 = new System.Windows.Forms.Label();
			this.numericUpDownNetSize = new System.Windows.Forms.NumericUpDown();
			this.buttonSetNetSize = new System.Windows.Forms.Button();
			this.buttonPing = new System.Windows.Forms.Button();
			this.labelPing = new System.Windows.Forms.Label();
			this.sensorViewA = new Atmo.UI.DevEx.Controls.SensorView();
			this.sensorViewB = new Atmo.UI.DevEx.Controls.SensorView();
			this.sensorViewC = new Atmo.UI.DevEx.Controls.SensorView();
			this.sensorViewD = new Atmo.UI.DevEx.Controls.SensorView();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownNetSize)).BeginInit();
			this.SuspendLayout();
			// 
			// buttonConnect
			// 
			this.buttonConnect.Location = new System.Drawing.Point(12, 28);
			this.buttonConnect.Name = "buttonConnect";
			this.buttonConnect.Size = new System.Drawing.Size(68, 23);
			this.buttonConnect.TabIndex = 0;
			this.buttonConnect.Text = "Connect";
			this.buttonConnect.UseVisualStyleBackColor = true;
			this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
			// 
			// labelConnect
			// 
			this.labelConnect.AutoSize = true;
			this.labelConnect.Location = new System.Drawing.Point(163, 33);
			this.labelConnect.Name = "labelConnect";
			this.labelConnect.Size = new System.Drawing.Size(16, 13);
			this.labelConnect.TabIndex = 1;
			this.labelConnect.Text = "...";
			// 
			// timerProperties
			// 
			this.timerProperties.Enabled = true;
			this.timerProperties.Interval = 2000;
			this.timerProperties.Tick += new System.EventHandler(this.timerProperties_Tick);
			// 
			// buttonDisconnect
			// 
			this.buttonDisconnect.Location = new System.Drawing.Point(86, 28);
			this.buttonDisconnect.Name = "buttonDisconnect";
			this.buttonDisconnect.Size = new System.Drawing.Size(71, 23);
			this.buttonDisconnect.TabIndex = 2;
			this.buttonDisconnect.Text = "Disconnect";
			this.buttonDisconnect.UseVisualStyleBackColor = true;
			this.buttonDisconnect.Click += new System.EventHandler(this.buttonDisconnect_Click);
			// 
			// buttonStartQuery
			// 
			this.buttonStartQuery.Location = new System.Drawing.Point(12, 57);
			this.buttonStartQuery.Name = "buttonStartQuery";
			this.buttonStartQuery.Size = new System.Drawing.Size(68, 23);
			this.buttonStartQuery.TabIndex = 3;
			this.buttonStartQuery.Text = "Start Query";
			this.buttonStartQuery.UseVisualStyleBackColor = true;
			this.buttonStartQuery.Click += new System.EventHandler(this.buttonStartQuery_Click);
			// 
			// buttonStopQuery
			// 
			this.buttonStopQuery.Location = new System.Drawing.Point(86, 57);
			this.buttonStopQuery.Name = "buttonStopQuery";
			this.buttonStopQuery.Size = new System.Drawing.Size(71, 23);
			this.buttonStopQuery.TabIndex = 4;
			this.buttonStopQuery.Text = "Stop Query";
			this.buttonStopQuery.UseVisualStyleBackColor = true;
			this.buttonStopQuery.Click += new System.EventHandler(this.buttonStopQuery_Click);
			// 
			// labelIsQuery
			// 
			this.labelIsQuery.AutoSize = true;
			this.labelIsQuery.Location = new System.Drawing.Point(163, 62);
			this.labelIsQuery.Name = "labelIsQuery";
			this.labelIsQuery.Size = new System.Drawing.Size(16, 13);
			this.labelIsQuery.TabIndex = 5;
			this.labelIsQuery.Text = "...";
			// 
			// timerQuery
			// 
			this.timerQuery.Tick += new System.EventHandler(this.timerQuery_Tick);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(275, 32);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(50, 13);
			this.label6.TabIndex = 7;
			this.label6.Text = "Net Size:";
			// 
			// numericUpDownNetSize
			// 
			this.numericUpDownNetSize.Location = new System.Drawing.Point(328, 30);
			this.numericUpDownNetSize.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
			this.numericUpDownNetSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numericUpDownNetSize.Name = "numericUpDownNetSize";
			this.numericUpDownNetSize.Size = new System.Drawing.Size(33, 20);
			this.numericUpDownNetSize.TabIndex = 8;
			this.numericUpDownNetSize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// buttonSetNetSize
			// 
			this.buttonSetNetSize.Location = new System.Drawing.Point(367, 28);
			this.buttonSetNetSize.Name = "buttonSetNetSize";
			this.buttonSetNetSize.Size = new System.Drawing.Size(75, 23);
			this.buttonSetNetSize.TabIndex = 9;
			this.buttonSetNetSize.Text = "Set";
			this.buttonSetNetSize.UseVisualStyleBackColor = true;
			this.buttonSetNetSize.Click += new System.EventHandler(this.buttonSetNetSize_Click);
			// 
			// buttonPing
			// 
			this.buttonPing.Location = new System.Drawing.Point(278, 62);
			this.buttonPing.Name = "buttonPing";
			this.buttonPing.Size = new System.Drawing.Size(75, 23);
			this.buttonPing.TabIndex = 10;
			this.buttonPing.Text = "Ping";
			this.buttonPing.UseVisualStyleBackColor = true;
			this.buttonPing.Click += new System.EventHandler(this.buttonPing_Click);
			// 
			// labelPing
			// 
			this.labelPing.AutoSize = true;
			this.labelPing.Location = new System.Drawing.Point(360, 66);
			this.labelPing.Name = "labelPing";
			this.labelPing.Size = new System.Drawing.Size(0, 13);
			this.labelPing.TabIndex = 11;
			// 
			// sensorViewA
			// 
			this.sensorViewA.BackColor = System.Drawing.Color.Transparent;
			this.sensorViewA.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
			this.sensorViewA.IsSelected = false;
			this.sensorViewA.Location = new System.Drawing.Point(22, 109);
			this.sensorViewA.Name = "sensorViewA";
			this.sensorViewA.PressureUnit = Atmo.Units.PressureUnit.KiloPascals;
			this.sensorViewA.Size = new System.Drawing.Size(246, 124);
			this.sensorViewA.SpeedUnit = Atmo.Units.SpeedUnit.MilesPerHour;
			this.sensorViewA.TabIndex = 12;
			this.sensorViewA.TemperatureUnit = Atmo.Units.TemperatureUnit.Fahrenheit;
			// 
			// sensorViewB
			// 
			this.sensorViewB.BackColor = System.Drawing.Color.Transparent;
			this.sensorViewB.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
			this.sensorViewB.IsSelected = false;
			this.sensorViewB.Location = new System.Drawing.Point(274, 109);
			this.sensorViewB.Name = "sensorViewB";
			this.sensorViewB.PressureUnit = Atmo.Units.PressureUnit.KiloPascals;
			this.sensorViewB.Size = new System.Drawing.Size(246, 124);
			this.sensorViewB.SpeedUnit = Atmo.Units.SpeedUnit.MilesPerHour;
			this.sensorViewB.TabIndex = 13;
			this.sensorViewB.TemperatureUnit = Atmo.Units.TemperatureUnit.Fahrenheit;
			// 
			// sensorViewC
			// 
			this.sensorViewC.BackColor = System.Drawing.Color.Transparent;
			this.sensorViewC.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
			this.sensorViewC.IsSelected = false;
			this.sensorViewC.Location = new System.Drawing.Point(22, 239);
			this.sensorViewC.Name = "sensorViewC";
			this.sensorViewC.PressureUnit = Atmo.Units.PressureUnit.KiloPascals;
			this.sensorViewC.Size = new System.Drawing.Size(246, 124);
			this.sensorViewC.SpeedUnit = Atmo.Units.SpeedUnit.MilesPerHour;
			this.sensorViewC.TabIndex = 14;
			this.sensorViewC.TemperatureUnit = Atmo.Units.TemperatureUnit.Fahrenheit;
			// 
			// sensorViewD
			// 
			this.sensorViewD.BackColor = System.Drawing.Color.Transparent;
			this.sensorViewD.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
			this.sensorViewD.IsSelected = false;
			this.sensorViewD.Location = new System.Drawing.Point(274, 239);
			this.sensorViewD.Name = "sensorViewD";
			this.sensorViewD.PressureUnit = Atmo.Units.PressureUnit.KiloPascals;
			this.sensorViewD.Size = new System.Drawing.Size(246, 124);
			this.sensorViewD.SpeedUnit = Atmo.Units.SpeedUnit.MilesPerHour;
			this.sensorViewD.TabIndex = 15;
			this.sensorViewD.TemperatureUnit = Atmo.Units.TemperatureUnit.Fahrenheit;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(655, 484);
			this.Controls.Add(this.sensorViewD);
			this.Controls.Add(this.sensorViewC);
			this.Controls.Add(this.sensorViewB);
			this.Controls.Add(this.sensorViewA);
			this.Controls.Add(this.labelPing);
			this.Controls.Add(this.buttonPing);
			this.Controls.Add(this.buttonSetNetSize);
			this.Controls.Add(this.numericUpDownNetSize);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.labelIsQuery);
			this.Controls.Add(this.buttonStopQuery);
			this.Controls.Add(this.buttonStartQuery);
			this.Controls.Add(this.buttonDisconnect);
			this.Controls.Add(this.labelConnect);
			this.Controls.Add(this.buttonConnect);
			this.Name = "MainForm";
			this.Text = "Connection Test for DAQ Win32 USB HID";
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownNetSize)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonConnect;
		private System.Windows.Forms.Label labelConnect;
		private System.Windows.Forms.Timer timerProperties;
		private System.Windows.Forms.Button buttonDisconnect;
		private System.Windows.Forms.Button buttonStartQuery;
		private System.Windows.Forms.Button buttonStopQuery;
		private System.Windows.Forms.Label labelIsQuery;
		private System.Windows.Forms.Timer timerQuery;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.NumericUpDown numericUpDownNetSize;
		private System.Windows.Forms.Button buttonSetNetSize;
		private System.Windows.Forms.Button buttonPing;
		private System.Windows.Forms.Label labelPing;
		private UI.DevEx.Controls.SensorView sensorViewA;
		private UI.DevEx.Controls.SensorView sensorViewB;
		private UI.DevEx.Controls.SensorView sensorViewC;
		private UI.DevEx.Controls.SensorView sensorViewD;
	}
}

