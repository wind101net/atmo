namespace Atmo.UI.WinForms.Controls {
	partial class HistoricSensorView {
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
			this.labelSensorName = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// labelSensorName
			// 
			this.labelSensorName.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelSensorName.Font = new System.Drawing.Font("Tahoma", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelSensorName.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.labelSensorName.Location = new System.Drawing.Point(0, 0);
			this.labelSensorName.Name = "labelSensorName";
			this.labelSensorName.Size = new System.Drawing.Size(187, 24);
			this.labelSensorName.TabIndex = 0;
			this.labelSensorName.Text = "Sensor";
			this.labelSensorName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.labelSensorName.Click += new System.EventHandler(this.labelSensorName_Click);
			// 
			// HistoricSensorView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.labelSensorName);
			this.Name = "HistoricSensorView";
			this.Size = new System.Drawing.Size(187, 24);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label labelSensorName;
	}
}
