using System;

namespace Atmo.UI.DevEx.Controls {
    public partial class DateTimeRangePicker : DevExpress.XtraEditors.XtraUserControl {
        public DateTimeRangePicker() {
            InitializeComponent();
        }

        public DateTime From {
            get {
                return dateTimePickerFrom.DateTime;
            }
            set {
                dateTimePickerFrom.DateTime = value;
            }
        }

        public DateTime To {
            get {
                return dateTimePickerTo.DateTime;
            }
            set {
                dateTimePickerTo.DateTime = value;
            }
        }

        public DateTime Min {
            get {
                return From < To ? From : To;
            }
        }

        public DateTime Max {
            get {
                return From > To ? From : To;
            }
        }

    }
}
