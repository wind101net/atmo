using System;
using System.Collections.Generic;
using DevExpress.XtraEditors;


namespace Atmo.UI.DevEx.Controls {
	public partial class TimeSpanSecondsEdit : XtraUserControl {
		public TimeSpanSecondsEdit() {
			InitializeComponent();
			spinEditSecondsDelta.Value = 0;
			spinEditSecondsDelta_EditValueChanged(null, null);
		}

		public TimeSpan Value {
			get { return new TimeSpan(TimeSpan.TicksPerSecond * (int)spinEditSecondsDelta.Value); }
			set { spinEditSecondsDelta.Value = (int) value.TotalSeconds; }
		}

		private void spinEditSecondsDelta_EditValueChanged(object sender, EventArgs e) {
			labelControlTimeSpanTest.Text = ToFriendlyString(Value);
		}

		private static string ToFriendlyString(TimeSpan span) {

			if(span.TotalDays >= 1)
				return span.ToString();
			
			var parts = new List<string>();

			if (span.Hours != 0)
				parts.Add(AbsUnitNumber("hour", span.Hours));
			if(span.Minutes != 0)
				parts.Add(AbsUnitNumber("minue", span.Minutes));
			if(span.Seconds != 0)
				parts.Add(AbsUnitNumber("second", span.Seconds));

			var result = String.Join(" ", parts.ToArray());

			if (String.IsNullOrEmpty(result))
				result = AbsUnitNumber("second",0);
			else if(span < TimeSpan.Zero)
				result = "- " + result;

			return result;
		}

		private static string AbsUnitNumber(string unit, int n) {
			return String.Format("{0} {2}{1}", Math.Abs(n), Plural(n), unit);
		}

		private static string Plural(int n) {
			return Math.Abs(n) == 1 ? String.Empty : "s";
		}

		private void spinEditSecondsDelta_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e) {
			labelControlTimeSpanTest.Text = ToFriendlyString(Value);
		}
	}
}
