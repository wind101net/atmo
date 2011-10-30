using System;
using System.Collections.Generic;
using System.Linq;
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

			if(span.TotalDays > 12)
				return span.ToString();

			var result = String.Join(" ", ToStringParts(span).ToArray());

			if(span < TimeSpan.Zero)
				result = "- " + result;

			return result;
		}

		private static IEnumerable<string> ToStringParts(TimeSpan span) {
			if (span.Days != 0)
				yield return AbsUnitNumber("day", span.Days);
			if (span.Hours != 0)
				yield return AbsUnitNumber("hour", span.Hours);
			if (span.Minutes != 0)
				yield return AbsUnitNumber("minue", span.Minutes);
			if (span.Seconds != 0)
				yield return AbsUnitNumber("second", span.Seconds);
			if (Math.Abs(span.TotalSeconds) < 1)
				yield return AbsUnitNumber("second", 0);
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
