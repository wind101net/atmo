namespace Atmo.UI.DevEx.Controls {
	partial class TimeSpanSecondsEdit {
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
			this.spinEditSecondsDelta = new DevExpress.XtraEditors.SpinEdit();
			this.labelControlTimeSpanTest = new DevExpress.XtraEditors.LabelControl();
			((System.ComponentModel.ISupportInitialize)(this.spinEditSecondsDelta.Properties)).BeginInit();
			this.SuspendLayout();
			// 
			// spinEditSecondsDelta
			// 
			this.spinEditSecondsDelta.Dock = System.Windows.Forms.DockStyle.Left;
			this.spinEditSecondsDelta.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.spinEditSecondsDelta.Location = new System.Drawing.Point(0, 0);
			this.spinEditSecondsDelta.Name = "spinEditSecondsDelta";
			this.spinEditSecondsDelta.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
			this.spinEditSecondsDelta.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
			this.spinEditSecondsDelta.Properties.IsFloatValue = false;
			this.spinEditSecondsDelta.Properties.Mask.EditMask = "N00";
			this.spinEditSecondsDelta.Properties.MaxValue = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
			this.spinEditSecondsDelta.Properties.MinValue = new decimal(new int[] {
            9999999,
            0,
            0,
            -2147483648});
			this.spinEditSecondsDelta.Size = new System.Drawing.Size(112, 20);
			this.spinEditSecondsDelta.TabIndex = 0;
			this.spinEditSecondsDelta.EditValueChanged += new System.EventHandler(this.spinEditSecondsDelta_EditValueChanged);
			this.spinEditSecondsDelta.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.spinEditSecondsDelta_EditValueChanging);
			// 
			// labelControlTimeSpanTest
			// 
			this.labelControlTimeSpanTest.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.labelControlTimeSpanTest.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelControlTimeSpanTest.Location = new System.Drawing.Point(112, 0);
			this.labelControlTimeSpanTest.Name = "labelControlTimeSpanTest";
			this.labelControlTimeSpanTest.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.labelControlTimeSpanTest.Size = new System.Drawing.Size(140, 20);
			this.labelControlTimeSpanTest.TabIndex = 1;
			this.labelControlTimeSpanTest.Text = "0";
			// 
			// TimeSpanSecondsEdit
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.labelControlTimeSpanTest);
			this.Controls.Add(this.spinEditSecondsDelta);
			this.Name = "TimeSpanSecondsEdit";
			this.Size = new System.Drawing.Size(252, 20);
			((System.ComponentModel.ISupportInitialize)(this.spinEditSecondsDelta.Properties)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraEditors.SpinEdit spinEditSecondsDelta;
		private DevExpress.XtraEditors.LabelControl labelControlTimeSpanTest;
	}
}
