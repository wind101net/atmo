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
using System.Collections.Generic;
using System.Linq;
using Atmo.Data;
using Atmo.Stats;
using Atmo.Units;
using DevExpress.XtraCharts;

namespace Atmo.UI.DevEx.Controls {
	public partial class WindResourceGraph : DevExpress.XtraEditors.XtraUserControl {
		public WindResourceGraph() {
			InitializeComponent();
		}

		public ReadingValuesConverterCache<ReadingValues> ConverterCacheReadingValues { get; set; }

		public TemperatureUnit TemperatureUnit { get; set; }

		public SpeedUnit SpeedUnit { get; set; }

		public PressureUnit PressureUnit { get; set; }

		public PersistentState State { get; set; }

		public void SetDataSource<T>(IEnumerable<T> items) where T : IReadingsSummary {
			SetDataSource(items is List<T> ? items as List<T> : new List<T>(items));
		}

		public void SetDataSource<T>(List<T> items) where T : IReadingsSummary {
			List<ReadingsSummary> readings;
			if (typeof(T) == typeof(ReadingsSummary)) {
				readings = items as List<ReadingsSummary>;
			}
			else {
				readings = new List<ReadingsSummary>(items.Count);
				for (int i = 0; i < items.Count; i++) {
					readings.Add(new ReadingsSummary(items[i]));
				}
			}
			SetDataSource(readings);
		}

		public void SetDataSource(List<ReadingsSummary> items) {
			// todo: can this copy be eliminated?
			

			var windCalc = new WindDataSummaryCalculator<IReadingsSummary>();
			
			foreach (var item in items) {
				windCalc.Process(item);
			}

			var speedFrequencyData = windCalc.WindSpeedFrequencyData;
			chartControlWindDir.DataSource = PercentageOfMax(windCalc.WindDirectionEnergyData);
			chartControlWindSpeedFreq.DataSource = speedFrequencyData;
		}

		private static List<WindDirectionEnergy> PercentageOfMax(List<WindDirectionEnergy> records) {
			var result = new List<WindDirectionEnergy>(records.Count);
			double maxPower;
			double maxFrequency;
			if(records.Count == 0) {
				return result;
			}
			maxPower = records[0].Power;
			maxFrequency = records[0].Frequency;
			for(int i = 1; i < records.Count; i++) {
				var record = records[i];
				if(record.Power > maxPower) {
					maxPower = record.Power;
				}
				if(record.Frequency > maxFrequency) {
					maxFrequency = record.Frequency;
				}
			}

			for (int i = 0; i < records.Count; i++) {
				var record = records[i];
				result.Add(new WindDirectionEnergy(
					record.Direction,
					record.Frequency / maxFrequency,
					record.Power / maxPower
				));
			}
			return result;
		}

	}
}
