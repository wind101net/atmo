namespace Atmo.UI.DevEx.Controls {
	partial class HistoricalTimeSelectHeader {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HistoricalTimeSelectHeader));
			this.groupControlHistoricHeader = new DevExpress.XtraEditors.GroupControl();
			this.customTimeRangeSelector = new Atmo.UI.DevEx.Controls.CustomTimeRangeSelector();
			this.checkEdit = new DevExpress.XtraEditors.CheckEdit();
			this.dateEdit = new DevExpress.XtraEditors.DateEdit();
			this.timeEdit = new DevExpress.XtraEditors.TimeEdit();
			((System.ComponentModel.ISupportInitialize)(this.groupControlHistoricHeader)).BeginInit();
			this.groupControlHistoricHeader.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.customTimeRangeSelector.RangeSlider.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.checkEdit.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dateEdit.Properties.VistaTimeProperties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dateEdit.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.timeEdit.Properties)).BeginInit();
			this.SuspendLayout();
			// 
			// groupControlHistoricHeader
			// 
			this.groupControlHistoricHeader.Appearance.BackColor = System.Drawing.Color.Maroon;
			this.groupControlHistoricHeader.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
			this.groupControlHistoricHeader.Appearance.Options.UseBackColor = true;
			this.groupControlHistoricHeader.Appearance.Options.UseBorderColor = true;
			this.groupControlHistoricHeader.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 12F);
			this.groupControlHistoricHeader.AppearanceCaption.Options.UseFont = true;
			this.groupControlHistoricHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
			this.groupControlHistoricHeader.Controls.Add(this.customTimeRangeSelector);
			this.groupControlHistoricHeader.Controls.Add(this.checkEdit);
			this.groupControlHistoricHeader.Controls.Add(this.dateEdit);
			this.groupControlHistoricHeader.Controls.Add(this.timeEdit);
			this.groupControlHistoricHeader.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupControlHistoricHeader.Location = new System.Drawing.Point(0, 0);
			this.groupControlHistoricHeader.Name = "groupControlHistoricHeader";
			this.groupControlHistoricHeader.Size = new System.Drawing.Size(581, 28);
			this.groupControlHistoricHeader.TabIndex = 0;
			this.groupControlHistoricHeader.Text = "Historic Data";
			// 
			// customTimeRangeSelector
			// 
			this.customTimeRangeSelector.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.customTimeRangeSelector.Appearance.BackColor = System.Drawing.Color.Transparent;
			this.customTimeRangeSelector.Appearance.Options.UseBackColor = true;
			this.customTimeRangeSelector.Location = new System.Drawing.Point(317, 6);
			this.customTimeRangeSelector.Name = "customTimeRangeSelector";
			// 
			// 
			// 
			this.customTimeRangeSelector.RangeSlider.Dock = System.Windows.Forms.DockStyle.Left;
			this.customTimeRangeSelector.RangeSlider.EditValue = 3;
			this.customTimeRangeSelector.RangeSlider.Location = new System.Drawing.Point(0, 0);
			this.customTimeRangeSelector.RangeSlider.Name = "rangeSlider";
			this.customTimeRangeSelector.RangeSlider.Properties.AutoSize = false;
			this.customTimeRangeSelector.RangeSlider.Properties.Maximum = 4;
			this.customTimeRangeSelector.RangeSlider.Size = new System.Drawing.Size(137, 17);
			this.customTimeRangeSelector.RangeSlider.TabIndex = 1;
			this.customTimeRangeSelector.RangeSlider.Value = 3;
			this.customTimeRangeSelector.RangeSlider.ValueChanged += new System.EventHandler(this.customTimeRangeSelector_RangeSlider_ValueChanged);
			this.customTimeRangeSelector.SelectedIndex = 3;
			this.customTimeRangeSelector.Size = new System.Drawing.Size(200, 17);
			this.customTimeRangeSelector.TabIndex = 39;
			this.customTimeRangeSelector.TimeSpans = ((System.Collections.Generic.List<System.TimeSpan>)(resources.GetObject("customTimeRangeSelector.TimeSpans")));
			this.customTimeRangeSelector.Load += new System.EventHandler(this.customTimeRangeSelector_Load);
			// 
			// checkEdit
			// 
			this.checkEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkEdit.EditValue = true;
			this.checkEdit.Location = new System.Drawing.Point(523, 5);
			this.checkEdit.Name = "checkEdit";
			this.checkEdit.Properties.Caption = "Latest";
			this.checkEdit.Size = new System.Drawing.Size(53, 18);
			this.checkEdit.TabIndex = 44;
			this.checkEdit.CheckedChanged += new System.EventHandler(this.checkEdit_CheckedChanged);
			// 
			// dateEdit
			// 
			this.dateEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.dateEdit.EditValue = null;
			this.dateEdit.Location = new System.Drawing.Point(107, 5);
			this.dateEdit.Name = "dateEdit";
			this.dateEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.dateEdit.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
			this.dateEdit.Size = new System.Drawing.Size(98, 20);
			this.dateEdit.TabIndex = 50;
			this.dateEdit.EditValueChanged += new System.EventHandler(this.dateEdit_EditValueChanged);
			// 
			// timeEdit
			// 
			this.timeEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.timeEdit.EditValue = new System.DateTime(2010, 1, 11, 0, 0, 0, 0);
			this.timeEdit.Location = new System.Drawing.Point(211, 5);
			this.timeEdit.Name = "timeEdit";
			this.timeEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
			this.timeEdit.Size = new System.Drawing.Size(100, 20);
			this.timeEdit.TabIndex = 49;
			this.timeEdit.EditValueChanged += new System.EventHandler(this.timeEdit_EditValueChanged);
			// 
			// HistoricalTimeSelectHeader
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupControlHistoricHeader);
			this.MaximumSize = new System.Drawing.Size(0, 28);
			this.MinimumSize = new System.Drawing.Size(581, 28);
			this.Name = "HistoricalTimeSelectHeader";
			this.Size = new System.Drawing.Size(581, 28);
			((System.ComponentModel.ISupportInitialize)(this.groupControlHistoricHeader)).EndInit();
			this.groupControlHistoricHeader.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.customTimeRangeSelector.RangeSlider.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.checkEdit.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dateEdit.Properties.VistaTimeProperties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dateEdit.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.timeEdit.Properties)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraEditors.GroupControl groupControlHistoricHeader;
		private CustomTimeRangeSelector customTimeRangeSelector;
		private DevExpress.XtraEditors.CheckEdit checkEdit;
		private DevExpress.XtraEditors.DateEdit dateEdit;
		private DevExpress.XtraEditors.TimeEdit timeEdit;
	}
}
