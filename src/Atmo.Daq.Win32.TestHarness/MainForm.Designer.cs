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
			this.panelJunk = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.panelSensors = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownNetSize)).BeginInit();
			this.panelJunk.SuspendLayout();
			this.panelSensors.SuspendLayout();
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
			// panelJunk
			// 
			this.panelJunk.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.panelJunk.Controls.Add(this.label1);
			this.panelJunk.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelJunk.Location = new System.Drawing.Point(0, 0);
			this.panelJunk.Name = "panelJunk";
			this.panelJunk.Size = new System.Drawing.Size(324, 382);
			this.panelJunk.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(324, 382);
			this.label1.TabIndex = 0;
			this.label1.Text = "This control intentionally left blank";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panelSensors
			// 
			this.panelSensors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panelSensors.AutoScroll = true;
			this.panelSensors.AutoScrollMinSize = new System.Drawing.Size(200, 0);
			this.panelSensors.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelSensors.Controls.Add(this.panelJunk);
			this.panelSensors.Location = new System.Drawing.Point(628, 12);
			this.panelSensors.Name = "panelSensors";
			this.panelSensors.Size = new System.Drawing.Size(326, 384);
			this.panelSensors.TabIndex = 12;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(966, 408);
			this.Controls.Add(this.panelSensors);
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
			this.panelJunk.ResumeLayout(false);
			this.panelSensors.ResumeLayout(false);
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
		private System.Windows.Forms.Panel panelJunk;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel panelSensors;
	}
}

