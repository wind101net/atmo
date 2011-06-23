// ================================================================================
//
// Atmo 2
// Copyright (C) 2011  BARANI DESIGN
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
// 
// Contact: Jan Barani mailto:jan@baranidesign.com
//
// ================================================================================

using System;
using System.Drawing;
using System.Windows.Forms;
using Atmo.Test;
using Atmo.Units;

namespace Atmo.UI.DevEx.Controls {
	public partial class SensorView : UserControl {

		private bool _selected;
		private ReadingValuesConverter<IReadingValues,ReadingValues> _converter;

		public SensorView() {
			_converter = null;
			_selected = false;

			InitializeComponent();
			foreach (Control c in tableLayoutPanel.Controls) {
				c.MouseClick += tableLayoutPanel_MouseClick;
			}
			ResetValues();
			SetBackgroundColor();
		}

		public ReadingValuesConverter<IReadingValues,ReadingValues> Converter {
			get { return _converter; }
			set { _converter = value; }
		}

		public bool IsSelected {
			get { return _selected; }
			set { _selected = value; SetBackgroundColor(); }
		}

		private void SetBackgroundColor() {
			SetBackgroundColor(IsSelected);
		}

		private void SetBackgroundColor(bool selected) {
			BackColor = (selected) ? SystemColors.Highlight : Color.Transparent;
			ForeColor = (selected) ? SystemColors.ControlText : SystemColors.InactiveCaptionText;
			//this.sensorNameLabel.ForeColor = this.ForeColor;
			//this.tableLayoutPanel1.ForeColor = this.ForeColor;
			//foreach (Control c in this.tableLayoutPanel1.Controls.OfType<Control>()) {
			//	c.ForeColor = this.ForeColor;
			//}
		}

		public void ResetValues() {
			SetValues(null,null);
		}

		public TemperatureUnit TemperatureUnit {
			get {
				if(null != _converter && null != _converter.TemperatureConverter) {
					return _converter.TemperatureConverter.To;
				}
				return TemperatureUnit.Celsius;
			}
		}

		public void SetValues(ISensor sensor, IReadingValues reading) {
			if (null != sensor) {
				int n;
				sensorNameLabel.Text = (Int32.TryParse(sensor.Name, out n) && 0 <= n && n < 26)
					? ((char) ((byte) ('A') + (byte) n)).ToString()
					: sensor.Name;
			}
			else {
				sensorNameLabel.Text = "Sensor";
			}

			const string na = "N/A";
			if (null != reading && reading.IsValid) {
				var data = null == Converter
				    ? reading
				    : Converter.Convert(reading)
				;
				
				string tempText;
				if(data.IsTemperatureValid) {
					tempText = data.Temperature.ToString("F1");
					if (null != _converter && null != _converter.TemperatureConverter) {
						tempText += ' ' + UnitUtility.GetFriendlyName(_converter.TemperatureConverter.To);
					}
				}else {
					tempText = na;
				}

				string presText;
				if(data.IsPressureValid) {
					if(null != _converter && null != _converter.PressureConverter) {
						var presUnit = _converter.PressureConverter.To;
						presText = Math.Round(data.Pressure,presUnit == PressureUnit.Millibar ? 1 : 2).ToString()
							+ ' '
							+ UnitUtility.GetFriendlyName(presUnit)
						;
					}else {
						presText = Math.Round(data.Pressure, 2).ToString();
					}
				}else {
					presText = na;
				}

				string speedText;
				if(data.IsWindSpeedValid) {
					speedText = data.WindSpeed.ToString("F2");
					if (null != _converter && null != _converter.SpeedConverter) {
						speedText += ' ' + UnitUtility.GetFriendlyName(_converter.SpeedConverter.To);
					}
				}else {
					speedText = na;
				}

				string dirText;
				if(data.IsWindDirectionValid) {
					dirText = Math.Round(data.WindDirection).ToString()
						+ "\xb0, "
						+ Vector2D.CardinalDirection.DegreesToBestCardinalName(reading.WindDirection)
					;

				}else {
					dirText = na;
				}

				tempValue.Text = tempText;
				pressureValue.Text = presText;
				humidityValue.Text = data.IsHumidityValid
					? (Math.Round(data.Humidity * 100.0, 1).ToString() + '%')
					: na
				;
				windSpeedValue.Text = speedText;
				windDirValue.Text = dirText;

			}
			else {
				tempValue.Text = na;
				pressureValue.Text = na;
				humidityValue.Text = na;
				windSpeedValue.Text = na;
				windDirValue.Text = na;
			}
		}

		private void tableLayoutPanel_MouseClick(object sender, MouseEventArgs e) {
			SensorView_MouseClick(sender, e);
		}

		private void SensorView_MouseClick(object sender, MouseEventArgs e) {
			IsSelected = !IsSelected;
		}
	}
}
