namespace Atmo.UI.DevEx.Controls {
	partial class HistoricalGraphBreakdown {
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
			this.components = new System.ComponentModel.Container();
			this.groupControl = new DevExpress.XtraEditors.GroupControl();
			this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
			this.comboBoxEditSelProp = new DevExpress.XtraEditors.ComboBoxEdit();
			this.tableLayout = new System.Windows.Forms.TableLayoutPanel();
			this.bindingSourceReadingSummary = new System.Windows.Forms.BindingSource(this.components);
			((System.ComponentModel.ISupportInitialize)(this.groupControl)).BeginInit();
			this.groupControl.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.comboBoxEditSelProp.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.bindingSourceReadingSummary)).BeginInit();
			this.SuspendLayout();
			// 
			// groupControl
			// 
			this.groupControl.Controls.Add(this.labelControl1);
			this.groupControl.Controls.Add(this.comboBoxEditSelProp);
			this.groupControl.Controls.Add(this.tableLayout);
			this.groupControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupControl.Location = new System.Drawing.Point(0, 0);
			this.groupControl.Name = "groupControl";
			this.groupControl.Size = new System.Drawing.Size(639, 508);
			this.groupControl.TabIndex = 1;
			this.groupControl.Text = "Historical Breakdown";
			// 
			// labelControl1
			// 
			this.labelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelControl1.Appearance.Options.UseTextOptions = true;
			this.labelControl1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.labelControl1.Location = new System.Drawing.Point(471, 4);
			this.labelControl1.Name = "labelControl1";
			this.labelControl1.Size = new System.Drawing.Size(26, 13);
			this.labelControl1.TabIndex = 2;
			this.labelControl1.Text = "Field:";
			// 
			// comboBoxEditSelProp
			// 
			this.comboBoxEditSelProp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboBoxEditSelProp.EditValue = "Wind Speed";
			this.comboBoxEditSelProp.Location = new System.Drawing.Point(503, 1);
			this.comboBoxEditSelProp.Name = "comboBoxEditSelProp";
			this.comboBoxEditSelProp.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.comboBoxEditSelProp.Properties.Items.AddRange(new object[] {
            "Wind Speed",
            "Wind Direction",
            "Temperature",
            "Humidity",
            "Pressure"});
			this.comboBoxEditSelProp.Size = new System.Drawing.Size(133, 20);
			this.comboBoxEditSelProp.TabIndex = 1;
			this.comboBoxEditSelProp.SelectedIndexChanged += new System.EventHandler(this.comboBoxEditSelProp_SelectedIndexChanged);
			// 
			// tableLayout
			// 
			this.tableLayout.ColumnCount = 4;
			this.tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayout.Location = new System.Drawing.Point(2, 22);
			this.tableLayout.Name = "tableLayout";
			this.tableLayout.RowCount = 3;
			this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayout.Size = new System.Drawing.Size(635, 484);
			this.tableLayout.TabIndex = 0;
			// 
			// bindingSourceReadingSummary
			// 
			this.bindingSourceReadingSummary.DataSource = typeof(Atmo.Stats.ReadingsSummary);
			// 
			// HistoricalGraphBreakdown
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupControl);
			this.Name = "HistoricalGraphBreakdown";
			this.Size = new System.Drawing.Size(639, 508);
			((System.ComponentModel.ISupportInitialize)(this.groupControl)).EndInit();
			this.groupControl.ResumeLayout(false);
			this.groupControl.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.comboBoxEditSelProp.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.bindingSourceReadingSummary)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraEditors.GroupControl groupControl;
		private System.Windows.Forms.TableLayoutPanel tableLayout;
		private System.Windows.Forms.BindingSource bindingSourceReadingSummary;
		private DevExpress.XtraEditors.LabelControl labelControl1;
		private DevExpress.XtraEditors.ComboBoxEdit comboBoxEditSelProp;
	}
}
