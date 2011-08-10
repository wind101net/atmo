namespace Atmo.UI.DevEx {
    partial class ExportForm {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportForm));
			this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
			this.comboChooseSensor = new DevExpress.XtraEditors.ComboBoxEdit();
			this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
			this.dateTimeRangePicker = new Atmo.UI.DevEx.Controls.DateTimeRangePicker();
			this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
			this.btnExecute = new DevExpress.XtraEditors.SimpleButton();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
			this.groupControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.comboChooseSensor.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
			this.groupControl2.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupControl1
			// 
			this.groupControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupControl1.Controls.Add(this.comboChooseSensor);
			this.groupControl1.Location = new System.Drawing.Point(12, 12);
			this.groupControl1.Name = "groupControl1";
			this.groupControl1.Size = new System.Drawing.Size(291, 45);
			this.groupControl1.TabIndex = 0;
			this.groupControl1.Text = "1. Select Sensor Database";
			// 
			// comboChooseSensor
			// 
			this.comboChooseSensor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.comboChooseSensor.Location = new System.Drawing.Point(2, 22);
			this.comboChooseSensor.Name = "comboChooseSensor";
			this.comboChooseSensor.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
			this.comboChooseSensor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.comboChooseSensor.Size = new System.Drawing.Size(287, 20);
			this.comboChooseSensor.TabIndex = 0;
			// 
			// groupControl2
			// 
			this.groupControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupControl2.Controls.Add(this.dateTimeRangePicker);
			this.groupControl2.Location = new System.Drawing.Point(12, 63);
			this.groupControl2.Name = "groupControl2";
			this.groupControl2.Size = new System.Drawing.Size(291, 88);
			this.groupControl2.TabIndex = 1;
			this.groupControl2.Text = "2. Select Date/Time Range";
			// 
			// dateTimeRangePicker
			// 
			this.dateTimeRangePicker.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dateTimeRangePicker.From = new System.DateTime(((long)(0)));
			this.dateTimeRangePicker.Location = new System.Drawing.Point(2, 22);
			this.dateTimeRangePicker.Name = "dateTimeRangePicker";
			this.dateTimeRangePicker.Size = new System.Drawing.Size(287, 64);
			this.dateTimeRangePicker.TabIndex = 0;
			this.dateTimeRangePicker.To = new System.DateTime(((long)(0)));
			// 
			// btnCancel
			// 
			this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnCancel.Location = new System.Drawing.Point(5, 5);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(137, 25);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnExecute
			// 
			this.btnExecute.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnExecute.Location = new System.Drawing.Point(148, 5);
			this.btnExecute.Name = "btnExecute";
			this.btnExecute.Size = new System.Drawing.Size(138, 25);
			this.btnExecute.TabIndex = 3;
			this.btnExecute.Text = "Export";
			this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Controls.Add(this.btnExecute, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.btnCancel, 0, 0);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 157);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(2);
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(291, 35);
			this.tableLayoutPanel1.TabIndex = 4;
			// 
			// ExportForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(315, 204);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.groupControl2);
			this.Controls.Add(this.groupControl1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximumSize = new System.Drawing.Size(1024, 242);
			this.MinimumSize = new System.Drawing.Size(16, 242);
			this.Name = "ExportForm";
			this.Text = "ExportForm";
			this.Load += new System.EventHandler(this.ExportForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
			this.groupControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.comboChooseSensor.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
			this.groupControl2.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnExecute;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraEditors.ComboBoxEdit comboChooseSensor;
        private Controls.DateTimeRangePicker dateTimeRangePicker;
    }
}