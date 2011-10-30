namespace Atmo.UI.DevEx {
	partial class RenameForm {
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
			this.textEditName = new DevExpress.XtraEditors.TextEdit();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.simpleButtonCancel = new DevExpress.XtraEditors.SimpleButton();
			this.simpleButtonOk = new DevExpress.XtraEditors.SimpleButton();
			((System.ComponentModel.ISupportInitialize)(this.textEditName.Properties)).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// textEditName
			// 
			this.textEditName.Dock = System.Windows.Forms.DockStyle.Top;
			this.textEditName.Location = new System.Drawing.Point(0, 0);
			this.textEditName.Name = "textEditName";
			this.textEditName.Size = new System.Drawing.Size(361, 20);
			this.textEditName.TabIndex = 0;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Controls.Add(this.simpleButtonCancel, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.simpleButtonOk, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 20);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(361, 29);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// simpleButtonCancel
			// 
			this.simpleButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.simpleButtonCancel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.simpleButtonCancel.Location = new System.Drawing.Point(3, 3);
			this.simpleButtonCancel.Name = "simpleButtonCancel";
			this.simpleButtonCancel.Size = new System.Drawing.Size(174, 23);
			this.simpleButtonCancel.TabIndex = 0;
			this.simpleButtonCancel.Text = "Cancel";
			this.simpleButtonCancel.Click += new System.EventHandler(this.simpleButtonCancel_Click);
			// 
			// simpleButtonOk
			// 
			this.simpleButtonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.simpleButtonOk.Dock = System.Windows.Forms.DockStyle.Fill;
			this.simpleButtonOk.Location = new System.Drawing.Point(183, 3);
			this.simpleButtonOk.Name = "simpleButtonOk";
			this.simpleButtonOk.Size = new System.Drawing.Size(175, 23);
			this.simpleButtonOk.TabIndex = 1;
			this.simpleButtonOk.Text = "OK";
			this.simpleButtonOk.Click += new System.EventHandler(this.simpleButtonOk_Click);
			// 
			// RenameForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.simpleButtonCancel;
			this.ClientSize = new System.Drawing.Size(361, 49);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.textEditName);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "RenameForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Rename";
			((System.ComponentModel.ISupportInitialize)(this.textEditName.Properties)).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraEditors.TextEdit textEditName;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private DevExpress.XtraEditors.SimpleButton simpleButtonCancel;
		private DevExpress.XtraEditors.SimpleButton simpleButtonOk;

	}
}