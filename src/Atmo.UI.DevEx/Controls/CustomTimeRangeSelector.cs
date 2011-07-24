using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraEditors;
using Atmo.Units;

namespace Atmo.UI.DevEx.Controls {
	public partial class CustomTimeRangeSelector : XtraUserControl {

		private static readonly List<TimeSpan> DefaultTimeSpanValues;

		static CustomTimeRangeSelector() {
			DefaultTimeSpanValues = new List<TimeSpan>() {
				new TimeSpan(0, 0, 30),
				new TimeSpan(0, 1, 0),
				new TimeSpan(1, 0, 0),
				new TimeSpan(3, 0, 0),
				new TimeSpan(1, 0, 0, 0),
				new TimeSpan(12, 0, 0, 0),
				new TimeSpan(84, 0, 0, 0),
				new TimeSpan(365, 0, 0, 0),
			};
			DefaultTimeSpanValues.Sort();
		}

		public static IEnumerable<TimeSpan> DefaultTimeSpans { get { return DefaultTimeSpanValues.AsEnumerable(); } }

		private readonly List<TimeSpan> _timeSpans;

		public CustomTimeRangeSelector() {
			_timeSpans = new List<TimeSpan>();
			InitializeComponent();
			TimeSpans = new List<TimeSpan>(DefaultTimeSpans);
			rangeSlider_ValueChanged(null, null);
		}

		public List<TimeSpan> TimeSpans {
			get { return _timeSpans; }
			set {
				_timeSpans.Clear();
				_timeSpans.AddRange(value);
				_timeSpans.Sort();
				rangeSlider.Properties.Minimum = 0;
				rangeSlider.Properties.Maximum = _timeSpans.Count - 1;
				rangeSlider_ValueChanged(null, null);
			}
		}

		public int SelectedIndex {
			get { return rangeSlider.Value; }
			set {
				rangeSlider.Value = value;
				rangeSlider_ValueChanged(null, null);
			}
		}

		public TimeSpan SelectedSpan {
			get {
				var selectedIndex = SelectedIndex;
				var spans = TimeSpans;

				if (selectedIndex < 0)
					selectedIndex = 0;
				else if (selectedIndex >= spans.Count) {
					selectedIndex = spans.Count - 1;
				}

				return spans[selectedIndex];
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public TrackBarControl RangeSlider {
			get {
				return rangeSlider;
			}
		}

		public virtual string TimeLabel {
			get { return UnitUtility.TimeSpanLabelString(SelectedSpan); }
		}

		public void AddChangeHandler(EventHandler handler) {
			rangeSlider.ValueChanged += handler;
		}

		private void rangeSlider_ValueChanged(object sender, EventArgs e) {
			rangeValue.Text = TimeLabel;
		}

	}
}
