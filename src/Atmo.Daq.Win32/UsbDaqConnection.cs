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
using System.Threading;

namespace Atmo.Daq.Win32 {
	public class UsbDaqConnection : BaseDaqUsbConnection {

		private struct DaqStatusValues {

			public static readonly DaqStatusValues Default = new DaqStatusValues(Double.NaN, Double.NaN, Double.NaN);

			public readonly double VoltageBattery;
			public readonly double VoltageUsb;
			public readonly double Temperature;

			public DaqStatusValues(
				double voltageBattery,
				double voltageUsb,
				double temperature
			) {
				Temperature = temperature;
				VoltageBattery = voltageBattery;
				VoltageUsb = voltageUsb;
			}

		}

		[Obsolete("rename")]
		public class AnemWrapper : ISensor {

			private const int DefaultMaxReadingsValue = 10;

			private readonly int _nid;
			private readonly UsbDaqConnection _connection;
			private readonly int _maxReadings;
			private readonly List<PackedReading> _latestReadings;
			private readonly object _stateMutex = new object();
			private bool _valid = false;

			internal AnemWrapper(int nid, UsbDaqConnection parentConnection) {
				_nid = nid;
				_connection = parentConnection;
				if(null == _connection) {
					throw new ArgumentNullException("parentConnection");
				}
				_maxReadings = DefaultMaxReadingsValue;
				_latestReadings = new List<PackedReading>(_maxReadings);
			}

			public bool IsValid {
				get { return _valid; }
			}

			public void HandleObservation(PackedReadingValues values, int networkSize, DateTime daqRecordedTime) {
				lock (_stateMutex) {
					if (values.IsValid) {
						var recordStamp = daqRecordedTime.Subtract(new TimeSpan(0, 0, networkSize));
						if (_latestReadings.Count == 0) {
							_latestReadings.Add(new PackedReading(recordStamp, values));
						}
						else {
							var latest = _latestReadings.FirstOrDefault();
							if (latest.TimeStamp != recordStamp) {
								_latestReadings.Insert(0, new PackedReading(recordStamp, values));
							}
							while (_latestReadings.Count > _maxReadings) {
								_latestReadings.RemoveAt(_maxReadings - 1);
							}
						}
						_valid = true;
					}
					else {
						_valid = false;
					}
				}
			}

			public IReading GetCurrentReading() {
				lock (_stateMutex) {
					return _latestReadings.FirstOrDefault();
				}
			}

			public PackedReading Current {
				get { return _latestReadings.FirstOrDefault(); }
			}

			public string Name {
				get { return _nid.ToString(); }
			}

		}

		private const string DefaultDaqDeviceIdValue = "Vid_04d8&Pid_fc4a";
		private const int DefaultMsPeriodValue = 333;

		public static string DefaultDaqDeviceId{ get { return DefaultDaqDeviceIdValue; }}

		private AnemWrapper[] _wrappers;
		private int _networkSize;
		private Timer _queryTimer;
		private DateTime _lastClock = default(DateTime);
		private readonly object _connLock = new object();
		private bool _queryActive = true;
		private DateTime _lastDaqStatusQuery = default(DateTime);
		private readonly TimeSpan _daqStatusQueryLifetime = new TimeSpan(0, 0, 15);
		private DaqStatusValues _daqStat = DaqStatusValues.Default;
		private bool? _usingDaqTemp = null;


		public UsbDaqConnection() : this(DefaultDaqDeviceIdValue) { }

        public UsbDaqConnection(string deviceId)
			: base(deviceId) {
			_wrappers = new []{
                new AnemWrapper(0,this),
                new AnemWrapper(1,this),
                new AnemWrapper(2,this),
                new AnemWrapper(3,this)
            };
        	Pause();
			_queryTimer = new Timer(new TimerCallback(QueryThreadBody), null, Timeout.Infinite, DefaultMsPeriodValue);
			InitiateDeviceQuery();
		}

		[Obsolete("rename")]
		public void Pause() {
			_queryActive = false;
		}

		[Obsolete("rename")]
		public void Resume() {
			_queryActive = true;
		}

		public bool IsQuerying {
			get { return _queryActive; }
		}

		public ISensor GetSensor(int i) {
			return _wrappers[i];
		}

		public AnemWrapper this[int i] {
			get { return _wrappers[i]; }
		}

		private bool InitiateDeviceQuery() {
			return _queryTimer.Change(1, DefaultMsPeriodValue);
		}

		private bool TerminateDeviceQuery() {
			if (null != _queryTimer) {
				_queryTimer.Change(Timeout.Infinite, DefaultMsPeriodValue);
				_queryTimer.Dispose();
				_queryTimer = null;
				return true;
			}
			return false;
		}

		private void QueryThreadBody(object bs) {
			QueryThreadBody();
		}

		private void QueryThreadBody() {
			var values = new PackedReadingValues[4];
			_networkSize = 4; // just assume 4 for now //SetNetworkSize(4);
			if (_queryActive) {
				var now = DateTime.Now;
				var daqTime = now;
				daqTime = daqTime.Date.Add(new TimeSpan(daqTime.Hour, daqTime.Minute, daqTime.Second));
				for (int i = 0; i < values.Length; i++) {
					values[i] = QueryValues(i);
				}
				int highestValid = -1;
				_usingDaqTemp = null;
				for (int i = 0; i < values.Length; i++) {
					_wrappers[i].HandleObservation(values[i], _networkSize, daqTime);
					if (values[i].IsValid) {

						if (highestValid < i) {
							highestValid = i;
						}
					}
				}
				if (highestValid < 0) {
					highestValid = 3;
				}

			}

		}

		private PackedReadingValues QueryValues(int nid) {
			lock (_connLock) {
				if (!IsConnected) {
					Connect();
				}
				var queryPacket = Enumerable.Repeat((byte)0xff, 65).ToArray();
				queryPacket[0] = 0;
				queryPacket[1] = 0x85;
				queryPacket[2] = (byte)nid;
				if (null != UsbConn && UsbConn.WritePacket(queryPacket)) {
					queryPacket = UsbConn.ReadPacket();
					if (null != queryPacket) {
						return PackedReadingValues.FromDeviceBytes(queryPacket, 1);
					}
				}
				return PackedReadingValues.Invalid;
			}
		}

		protected override void Dispose(bool disposing) {
			if(disposing) {
				;
			}
			TerminateDeviceQuery();

			base.Dispose(disposing);
		}

	}
}
