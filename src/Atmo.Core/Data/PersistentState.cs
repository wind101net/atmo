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
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Atmo.Units;
using System.Xml;
using log4net;

namespace Atmo.Data {

	/// <summary>
	/// The persistent state of an application that can be saved to an XML configuration file.
	/// </summary>
	[XmlRoot("PersistentState")]
	public class PersistentState {

		private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public enum UserCalculatedAttribute {
			DewPoint,
			AirDensity
		}

		public static XmlSerializer Serializer { get; private set; }

		static PersistentState() {
			Serializer = new XmlSerializer(typeof(PersistentState));
		}

		public static PersistentState ReadFile(string filePath) {
			if(!File.Exists(filePath)) {
				return null;
			}
			try {
				using (var fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
					return ReadStream(fileStream);
				}
			}
			catch(Exception ex) {
				Log.Error("PersistentState file could not be loaded from '" + filePath + "'.", ex);
			}
			return null;
		}

		public static PersistentState ReadStream(Stream stream) {
			PersistentState state;
			using (var reader = new StreamReader(stream, Encoding.UTF8)) {
				try {
					state = Serializer.Deserialize(reader) as PersistentState;
				} catch(Exception ex) {
					state = null;
					Log.Error("PersistentState deserialize failed.", ex);
				}
			}

			if(null != state)
				state.IsDirty = false;

			return state;
		}

		public static bool SaveFile(string filePath, PersistentState state) {
			using(var fileStream = File.Open(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None)) {
				return SaveStream(fileStream, state);
			}
		}

		public static bool SaveStream(Stream stream, PersistentState state) {
			try {
				using (var writer = new XmlTextWriter(stream, Encoding.UTF8)) {
					Serializer.Serialize(writer, state);
				}
				return true;
			}
			catch (Exception ex) {
				Log.Error("PersistentState serialize failed.", ex);
				return false;
			}
		}

		/// <summary>
		/// Constructs a new persistent state store with default values.
		/// </summary>
		public PersistentState() {
			IsDirty = true;
			MinRangeSizes = new ReadingValues(10, 5, 0.20, 0, 0);
			MinRangeSizeDewPoint = 10;
			MinRangeSizeAirDensity = 0.05;
			HeightAboveSeaLevel = Double.NaN;
			UserGraphAttribute = default(UserCalculatedAttribute);
			PressureUnit = default(PressureUnit);
			TemperatureUnit = default(TemperatureUnit);
			SpeedUnit = default(SpeedUnit);
			SelectedDatabases = null;
			StationNames = null;
			AutoSyncClock = false;
		}

		[XmlIgnore]
		public bool IsDirty { get; set; }

		[XmlElement]
		public ReadingValues MinRangeSizes { get; set; }
		
		[XmlElement]
		public double MinRangeSizeDewPoint { get; set; }
		
		[XmlElement]
		public double MinRangeSizeAirDensity { get; set; }
		
		[XmlElement]
		public UserCalculatedAttribute UserGraphAttribute { get; set; }

		[XmlElement]
		public double HeightAboveSeaLevel { get; set; }

		[XmlElement]
		public PressureUnit PressureUnit { get; set; }

		[XmlElement]
		public TemperatureUnit TemperatureUnit { get; set; }

		[XmlElement]
		public SpeedUnit SpeedUnit { get; set; }

		[XmlIgnore]
		public TimeSpan LiveTimeScale { get; set; }

		[XmlElement]
		public bool AutoSyncClock { get; set; }

		[XmlElement]
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public long LiveTimeScaleTicks {
			get { return LiveTimeScale.Ticks; }
			set { LiveTimeScale = new TimeSpan(value); }
		}

		[XmlIgnore]
		public TimeSpan HistoricalTimeScale { get; set; }

		[XmlElement]
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public long HistoricalTimeScaleTicks {
			get { return HistoricalTimeScale.Ticks; }
			set { HistoricalTimeScale = new TimeSpan(value); }
		}

		/// <summary>
		/// Backing field for SelectedDatabases to ensure non-null collections.
		/// </summary>
		private List<string> _selectedDatabases;

		[XmlArray]
		[XmlArrayItem(ElementName = "Database")]
		public List<string> SelectedDatabases {
			get { return _selectedDatabases; }
			set { _selectedDatabases = value ?? new List<string>(); }
		}

		[XmlElement]
		public string LastDaqFileLoadPath { get; set; }

		/// <summary>
		/// Backing field for StationNames to ensure non-null collections.
		/// </summary>
		private List<string> _stationNames;

		[XmlArray]
		[XmlArrayItem(ElementName = "StationNames")]
		public List<string> StationNames {
			get { return _stationNames; }
			set { _stationNames = value ?? new List<string>(); }
		}

		[XmlElement]
		public string StationPassword { get; set; }

        [XmlElement]
        public string StationNameWF { get; set; }

        [XmlElement]
        public string StationPasswordWF { get; set; }

        [XmlElement]
        public string StationNameAw  { get; set; }

        [XmlElement]
        public string StationPasswordAw { get; set; }

        [XmlElement]
        public int StationIntervalWF { get; set; }

        [XmlElement]
        public int StationIntervalAW { get; set; }

        [XmlElement]
        public bool PwsEnabled { get; set; }

        //rp
        [XmlElement]
        public bool WeatherEnabled { get; set; }
        

        [XmlElement]
        public bool PwfEnabled { get; set; }

        [XmlElement]
        public bool PawEnabled { get; set; }

        [XmlElement]
        public int StationSensorIndexWeather { get; set; }

        [XmlElement]
        public int StationSensorIndexWF { get; set; }

        [XmlElement]
        public int StationSensorIndexAw { get; set; }
                
	}
}
