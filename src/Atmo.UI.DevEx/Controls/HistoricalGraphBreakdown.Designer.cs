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
			this.tableLayout = new System.Windows.Forms.TableLayoutPanel();
			this.bindingSourceReadingSummary = new System.Windows.Forms.BindingSource(this.components);
			((System.ComponentModel.ISupportInitialize)(this.groupControl)).BeginInit();
			this.groupControl.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.bindingSourceReadingSummary)).BeginInit();
			this.SuspendLayout();
			// 
			// groupControl
			// 
			this.groupControl.Controls.Add(this.tableLayout);
			this.groupControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupControl.Location = new System.Drawing.Point(0, 0);
			this.groupControl.Name = "groupControl";
			this.groupControl.Size = new System.Drawing.Size(639, 508);
			this.groupControl.TabIndex = 1;
			this.groupControl.Text = "Historical Breakdown";
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
			((System.ComponentModel.ISupportInitialize)(this.bindingSourceReadingSummary)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraEditors.GroupControl groupControl;
		private System.Windows.Forms.TableLayoutPanel tableLayout;
		private System.Windows.Forms.BindingSource bindingSourceReadingSummary;
	}
}
