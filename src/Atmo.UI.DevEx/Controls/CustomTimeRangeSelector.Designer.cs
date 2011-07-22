namespace Atmo.UI.DevEx.Controls {
	partial class CustomTimeRangeSelector {
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
			this.rangeSlider = new DevExpress.XtraEditors.TrackBarControl();
			this.rangeValue = new DevExpress.XtraEditors.LabelControl();
			((System.ComponentModel.ISupportInitialize)(this.rangeSlider)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.rangeSlider.Properties)).BeginInit();
			this.SuspendLayout();
			// 
			// rangeSlider
			// 
			this.rangeSlider.Dock = System.Windows.Forms.DockStyle.Left;
			this.rangeSlider.EditValue = null;
			this.rangeSlider.Location = new System.Drawing.Point(0, 0);
			this.rangeSlider.Name = "rangeSlider";
			this.rangeSlider.Properties.AutoSize = false;
			this.rangeSlider.Properties.Maximum = 7;
			this.rangeSlider.Size = new System.Drawing.Size(137, 25);
			this.rangeSlider.TabIndex = 1;
			this.rangeSlider.ValueChanged += new System.EventHandler(this.rangeSlider_ValueChanged);
			// 
			// rangeValue
			// 
			this.rangeValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.rangeValue.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rangeValue.Location = new System.Drawing.Point(137, 0);
			this.rangeValue.Name = "rangeValue";
			this.rangeValue.Size = new System.Drawing.Size(63, 25);
			this.rangeValue.TabIndex = 2;
			this.rangeValue.Text = "N/A";
			// 
			// CustomTimeRangeSelector
			// 
			this.Appearance.BackColor = System.Drawing.Color.Transparent;
			this.Appearance.Options.UseBackColor = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.rangeValue);
			this.Controls.Add(this.rangeSlider);
			this.Name = "CustomTimeRangeSelector";
			this.Size = new System.Drawing.Size(200, 25);
			((System.ComponentModel.ISupportInitialize)(this.rangeSlider.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.rangeSlider)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private DevExpress.XtraEditors.TrackBarControl rangeSlider;
		private DevExpress.XtraEditors.LabelControl rangeValue;
	}
}
