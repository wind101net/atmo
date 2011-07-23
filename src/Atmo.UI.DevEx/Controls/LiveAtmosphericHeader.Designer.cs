namespace Atmo.UI.DevEx.Controls {
	partial class LiveAtmosphericHeader {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LiveAtmosphericHeader));
			this.groupControl = new DevExpress.XtraEditors.GroupControl();
			this.customTimeRangeSelector1 = new Atmo.UI.DevEx.Controls.CustomTimeRangeSelector();
			((System.ComponentModel.ISupportInitialize)(this.groupControl)).BeginInit();
			this.groupControl.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.customTimeRangeSelector1.RangeSlider.Properties)).BeginInit();
			this.SuspendLayout();
			// 
			// groupControl
			// 
			this.groupControl.AppearanceCaption.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
			this.groupControl.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 12F);
			this.groupControl.AppearanceCaption.Options.UseFont = true;
			this.groupControl.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
			this.groupControl.Controls.Add(this.customTimeRangeSelector1);
			this.groupControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupControl.Location = new System.Drawing.Point(0, 0);
			this.groupControl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.groupControl.Name = "groupControl";
			this.groupControl.Size = new System.Drawing.Size(279, 28);
			this.groupControl.TabIndex = 0;
			this.groupControl.Text = "Live Data";
			// 
			// customTimeRangeSelector1
			// 
			this.customTimeRangeSelector1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.customTimeRangeSelector1.Appearance.BackColor = System.Drawing.Color.Transparent;
			this.customTimeRangeSelector1.Appearance.Options.UseBackColor = true;
			this.customTimeRangeSelector1.Location = new System.Drawing.Point(75, 6);
			this.customTimeRangeSelector1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.customTimeRangeSelector1.Name = "customTimeRangeSelector1";
			// 
			// 
			// 
			this.customTimeRangeSelector1.RangeSlider.Dock = System.Windows.Forms.DockStyle.Left;
			this.customTimeRangeSelector1.RangeSlider.Location = new System.Drawing.Point(0, 0);
			this.customTimeRangeSelector1.RangeSlider.Name = "rangeSlider";
			this.customTimeRangeSelector1.RangeSlider.Properties.AutoSize = false;
			this.customTimeRangeSelector1.RangeSlider.Properties.Maximum = 4;
			this.customTimeRangeSelector1.RangeSlider.Size = new System.Drawing.Size(137, 17);
			this.customTimeRangeSelector1.RangeSlider.TabIndex = 1;
			this.customTimeRangeSelector1.SelectedIndex = 0;
			this.customTimeRangeSelector1.Size = new System.Drawing.Size(200, 17);
			this.customTimeRangeSelector1.TabIndex = 38;
			this.customTimeRangeSelector1.TimeSpans = ((System.Collections.Generic.List<System.TimeSpan>)(resources.GetObject("customTimeRangeSelector1.TimeSpans")));
			// 
			// LiveAtmosphericHeader
			// 
			this.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
			this.Appearance.Options.UseFont = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupControl);
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.MaximumSize = new System.Drawing.Size(0, 28);
			this.MinimumSize = new System.Drawing.Size(279, 28);
			this.Name = "LiveAtmosphericHeader";
			this.Size = new System.Drawing.Size(279, 28);
			((System.ComponentModel.ISupportInitialize)(this.groupControl)).EndInit();
			this.groupControl.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.customTimeRangeSelector1.RangeSlider.Properties)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraEditors.GroupControl groupControl;
		private CustomTimeRangeSelector customTimeRangeSelector1;
	}
}
