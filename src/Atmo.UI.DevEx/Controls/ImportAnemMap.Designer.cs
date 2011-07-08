namespace Atmo.UI.DevEx.Controls {
    partial class ImportAnemMap {
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
            this.labelAnemId = new DevExpress.XtraEditors.LabelControl();
            this.comboBoxEditImportName = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.dateEdit = new DevExpress.XtraEditors.DateEdit();
            this.timeEdit = new DevExpress.XtraEditors.TimeEdit();
            this.checkEditLoad = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEditImportName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditLoad.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelAnemId
            // 
            this.labelAnemId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.labelAnemId.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelAnemId.Location = new System.Drawing.Point(27, 0);
            this.labelAnemId.Name = "labelAnemId";
            this.labelAnemId.Size = new System.Drawing.Size(144, 26);
            this.labelAnemId.TabIndex = 0;
            this.labelAnemId.Text = "Import from Anem. #N/A into sensor";
            // 
            // comboBoxEditImportName
            // 
            this.comboBoxEditImportName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxEditImportName.Location = new System.Drawing.Point(177, 3);
            this.comboBoxEditImportName.Name = "comboBoxEditImportName";
            this.comboBoxEditImportName.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.comboBoxEditImportName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEditImportName.Size = new System.Drawing.Size(187, 20);
            this.comboBoxEditImportName.TabIndex = 2;
            this.comboBoxEditImportName.SelectedIndexChanged += new System.EventHandler(this.comboBoxEditImportName_SelectedIndexChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl1.Location = new System.Drawing.Point(370, 0);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(32, 26);
            this.labelControl1.TabIndex = 3;
            this.labelControl1.Text = "after";
            // 
            // dateEdit
            // 
            this.dateEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dateEdit.EditValue = null;
            this.dateEdit.Location = new System.Drawing.Point(408, 3);
            this.dateEdit.Name = "dateEdit";
            this.dateEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEdit.Size = new System.Drawing.Size(85, 20);
            this.dateEdit.TabIndex = 4;
            // 
            // timeEdit
            // 
            this.timeEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.timeEdit.EditValue = new System.DateTime(2010, 2, 15, 0, 0, 0, 0);
            this.timeEdit.Location = new System.Drawing.Point(499, 3);
            this.timeEdit.Name = "timeEdit";
            this.timeEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.timeEdit.Size = new System.Drawing.Size(100, 20);
            this.timeEdit.TabIndex = 5;
            // 
            // checkEditLoad
            // 
            this.checkEditLoad.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.checkEditLoad.EditValue = true;
            this.checkEditLoad.Location = new System.Drawing.Point(4, 4);
            this.checkEditLoad.Name = "checkEditLoad";
            this.checkEditLoad.Properties.Caption = "";
            this.checkEditLoad.Size = new System.Drawing.Size(17, 18);
            this.checkEditLoad.TabIndex = 6;
            // 
            // ImportAnemMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkEditLoad);
            this.Controls.Add(this.timeEdit);
            this.Controls.Add(this.dateEdit);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.comboBoxEditImportName);
            this.Controls.Add(this.labelAnemId);
            this.Name = "ImportAnemMap";
            this.Size = new System.Drawing.Size(602, 26);
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEditImportName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditLoad.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelAnemId;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEditImportName;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.DateEdit dateEdit;
        private DevExpress.XtraEditors.TimeEdit timeEdit;
        private DevExpress.XtraEditors.CheckEdit checkEditLoad;
    }
}
