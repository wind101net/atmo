using System;


namespace Atmo.UI.DevEx.Controls {
    public partial class DateTimePicker : DevExpress.XtraEditors.XtraUserControl {
        public DateTimePicker() {
            InitializeComponent();
        }

        public DateTime DateTime {
            get {
                return dateEdit.DateTime.Date.Add(timeEdit.Time.TimeOfDay);
            }
            set {
                dateEdit.DateTime = value.Date;
                timeEdit.Time = value;
            }
        }
    }
}
