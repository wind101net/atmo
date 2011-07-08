using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atmo.Units;

namespace Atmo {
	public class SensorInfo : ISensorInfo {

		public SensorInfo(
			string name,
			SpeedUnit speedUnit,
			TemperatureUnit tempUnit,
			PressureUnit pressureUnit
		) {
			Name = name;
			SpeedUnit = speedUnit;
			TemperatureUnit = tempUnit;
			PressureUnit = pressureUnit;
		}

		public string Name { get; set; }

		public SpeedUnit SpeedUnit { get; set; }

		public TemperatureUnit TemperatureUnit { get; set; }

		public PressureUnit PressureUnit { get; set; }
	}
}
