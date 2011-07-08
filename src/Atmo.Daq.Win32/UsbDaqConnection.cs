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
using Atmo.Data;
using Atmo.Units;

namespace Atmo.Daq.Win32 {
	public class UsbDaqConnection : BaseDaqUsbConnection, IDaqConnection, IEnumerable<UsbDaqConnection.Sensor> {

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

		public class Sensor : ISensor {

			private const int DefaultMaxReadingsValue = 10;

			private readonly int _nid;
			private readonly UsbDaqConnection _connection;
			private readonly int _maxReadings;
			private readonly List<PackedReading> _latestReadings;
			private readonly object _stateMutex = new object();
			private bool _valid = false;

			internal Sensor(int nid, UsbDaqConnection parentConnection) {
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

			/// <inheritdoc/>
			public IReading GetCurrentReading() {
				lock (_stateMutex) {
					return _latestReadings.FirstOrDefault();
				}
			}

			public PackedReading Current {
				get { return _latestReadings.FirstOrDefault(); }
			}

			/// <inheritdoc/>
			public SpeedUnit SpeedUnit {
				get { return SpeedUnit.MetersPerSec; }
			}

			/// <inheritdoc/>
			public TemperatureUnit TemperatureUnit {
				get { return TemperatureUnit.Celsius; }
			}

			/// <inheritdoc/>
			public PressureUnit PressureUnit {
				get { return PressureUnit.Pascals; }
			}

			/// <inheritdoc/>
			public string Name {
				get { return _nid.ToString(); }
			}

			public bool IsBufferFull {
				get { return _latestReadings.Count == _maxReadings; }
			}

			public bool IsEntireBufferInvalid {
				get { return !_latestReadings.Any(reading => reading.IsValid); }
			}

		}

		private const string DefaultDaqDeviceIdValue = "Vid_04d8&Pid_fc4a";
		private const int DefaultMsPeriodValue = 333;

		public static string DefaultDaqDeviceId{ get { return DefaultDaqDeviceIdValue; }}

		private static byte[] GenerateEmptyPacketData() {
			//return Enumerable.Repeat((byte)0xff, 65).ToArray();
			var data = new byte[65];
			for (int i = 0; i < data.Length; i++) {
				data[i] = 0xff;
			}
			return data;
		}

		private static byte[] GenerateNetworkSizePacketData(byte currentId, byte desiredId, byte size) {
			var queryPacket = GenerateEmptyPacketData();
			queryPacket[0] = 0;
			queryPacket[1] = 0x82;
			queryPacket[2] = currentId;
			queryPacket[3] = desiredId;
			queryPacket[4] = size;
			return queryPacket;
		}

		private readonly Sensor[] _sensors;
		private int _networkSize;
		private Timer _queryTimer;
		private DateTime _lastClock = default(DateTime);
		private DateTime _lastDaqStatusQuery = default(DateTime);
		private bool _queryActive = true;
		private readonly TimeSpan _daqStatusQueryLifetime = new TimeSpan(0, 0, 15);
		private DaqStatusValues _daqStat = DaqStatusValues.Default;
		private bool? _usingDaqTemp = null;

		private readonly object _connLock = new object();

		public UsbDaqConnection() : this(DefaultDaqDeviceIdValue) { }

        public UsbDaqConnection(string deviceId)
			: base(deviceId) {
			_sensors = new []{
                new Sensor(0,this),
                new Sensor(1,this),
                new Sensor(2,this),
                new Sensor(3,this)
            };
        	PauseQuery();
			_queryTimer = new Timer(new TimerCallback(QueryThreadBody), null, Timeout.Infinite, DefaultMsPeriodValue);
			InitiateDeviceQuery();
		}

		public void PauseQuery() {
			_queryActive = false;
		}

		public void ResumeQuery() {
			_queryActive = true;
		}

		public bool IsQuerying {
			get { return _queryActive; }
		}

		/// <inheritdoc/>
		public ISensor GetSensor(int i) {
			return _sensors[i];
		}

		public Sensor this[int i] {
			get { return _sensors[i]; }
		}

		private bool InitiateDeviceQuery() {
			return _queryTimer.Change(1, DefaultMsPeriodValue);
		}

		private void TerminateDeviceQuery() {
			if (null != _queryTimer) {
				_queryTimer.Change(Timeout.Infinite, DefaultMsPeriodValue);
				_queryTimer.Dispose();
				_queryTimer = null;
			}
		}

		private void QueryThreadBody(object bs) {
			QueryThreadBody();
		}

		private void QueryThreadBody() {
			if (!_queryActive) {
				return;
			}

			var values = new PackedReadingValues[1];
			bool? usingDaqTemp = null;
			int networkSize = 4;
			int highestValid = -1;
			var now = DateTime.Now;
			var daqSafeTime = now;
			daqSafeTime = daqSafeTime.Date.Add(new TimeSpan(daqSafeTime.Hour, daqSafeTime.Minute, daqSafeTime.Second));

			for (int i = 0; i < values.Length; i++) {
				values[i] = QueryValues(i);
			}

			for (int i = 0; i < values.Length; i++) {
					
				if (!values[i].IsValid) {
					continue;
				}

				if (PackedValuesFlags.AnemTemperatureSource != (values[i].RawFlags & PackedValuesFlags.AnemTemperatureSource)) {
					usingDaqTemp = true;
				}
				else if (!usingDaqTemp.HasValue) {
					usingDaqTemp = false;
				}

				highestValid = Math.Max(highestValid, i);
			}

			if (highestValid < 0) {
				highestValid = 3;
			}

			networkSize = highestValid + 1;

			for (int i = 0; i < values.Length; i++) {
				_sensors[i].HandleObservation(values[i], networkSize, daqSafeTime);
			}

			_networkSize = networkSize;
			_usingDaqTemp = usingDaqTemp;
		}

		public PackedReadingValues QueryValues(int nid) {
			
			if(nid < 0 || nid > 3) {
				throw new ArgumentOutOfRangeException("nid");
			}

			lock (_connLock) {
				if (!IsConnected) {
					Connect();
				}
				var queryPacket = GenerateEmptyPacketData();
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

		private DaqStatusValues QueryStatus() {
			var status = DaqStatusValues.Default;
			lock (_connLock) {
				if (!IsConnected) {
					Connect();
				}
				var queryPacket = GenerateEmptyPacketData();
				queryPacket[0] = 0;
				queryPacket[1] = 0x37;
				if (null != UsbConn && UsbConn.WritePacket(queryPacket)) {
					queryPacket = UsbConn.ReadPacket();
					if (null != queryPacket) {
						var volUsbRaw = (ushort)(queryPacket[2] | (queryPacket[3] << 8));
						var volBatRaw = (ushort)(queryPacket[4] | (queryPacket[5] << 8));
						var tmpDaqRaw = (ushort)(queryPacket[7] | (queryPacket[6] << 8));

						var temp = ((double)(tmpDaqRaw) / 10.0) - 40.0;
						var volUsb = ((double)(volUsbRaw) / 1024.0) * 6.4;
						var volBat = ((double)(volBatRaw) / 1024.0) * 6.4;

						status = new DaqStatusValues(volBat, volUsb, temp);
					}
				}
			}
			return status;
		}

		public void SetNetworkSize(int size) {
			lock (_connLock) {
				if (!IsConnected) {
					Connect();
				}
				var queryPacket = GenerateNetworkSizePacketData(0xff,0xff,(byte)size);
				if (UsbConn.WritePacket(queryPacket)) {
					_networkSize = size;
				}
			}
		}

		public bool ChangeSensorId(int currentId, int desiredId, int numTries = 3) {
			var queryPacket = GenerateNetworkSizePacketData(
				currentId >= 0 ? (byte) currentId : (byte) 0xff,
				desiredId >= 0 ? (byte) desiredId : (byte) 0xff,
				(byte) _networkSize
			);
			var ok = false;
			lock (_connLock) {
				if (!IsConnected) {
					Connect();
				}
				for (int i = 0; i < numTries; i++) {

					ok |= UsbConn.WritePacket(queryPacket);

					if (i < (numTries - 1)) {
						Thread.Sleep(50);
					}
				}
			}
			return ok;
		}

		public bool SetClock(DateTime time) {
			lock (_connLock) {
				var start = DateTime.Now;
				// determine the best time to send the set clock command
				var optimalTime = start;
				var firstDaqClock = RawQueryClock();
				const int checkSeg = 10;
				for (int i = 0; i < checkSeg; i++) {
					var currentDaqClock = RawQueryClock();
					if (currentDaqClock != firstDaqClock) {
						break;
					}
					optimalTime = DateTime.Now;
					Thread.Sleep(1000/checkSeg);
				}
				// wait for it
				var nextBestTime = optimalTime.AddSeconds(1);
				for (int i = 0; i < checkSeg; i++) {
					if (DateTime.Now.AddSeconds(1.0 / (double)checkSeg) >= nextBestTime) {
						break;
					}
					Thread.Sleep(1000 / checkSeg);
				}
				// set it
				var elapsed = DateTime.Now - start;
				return SetClockRaw(time.Add(elapsed).AddSeconds(1));
			}
		}

		private bool SetClockRaw(DateTime time) {
			var start = DateTime.Now;
			var pingTime = new TimeSpan(this.AveragePing(2).Ticks / 2);

			if (!IsConnected) {
				Connect();
			}
			byte[] queryPacket = GenerateEmptyPacketData();
			queryPacket[0] = 0;
			queryPacket[1] = 0x86;

			var calcTime = DateTime.Now.Subtract(start);

			time.Add(pingTime + calcTime);

			queryPacket[2] = (byte)(time.Year % 100);
			queryPacket[2] = (byte)((queryPacket[2] % 10) + ((queryPacket[2] / 10) * 16));
			queryPacket[3] = (byte)time.Month;
			queryPacket[3] = (byte)((queryPacket[3] % 10) + ((queryPacket[3] / 10) * 16));
			queryPacket[4] = (byte)time.Day;
			queryPacket[4] = (byte)((queryPacket[4] % 10) + ((queryPacket[4] / 10) * 16));
			queryPacket[5] = (byte)time.Hour;
			queryPacket[5] = (byte)((queryPacket[5] % 10) + ((queryPacket[5] / 10) * 16));
			queryPacket[6] = (byte)time.Minute;
			queryPacket[6] = (byte)((queryPacket[6] % 10) + ((queryPacket[6] / 10) * 16));
			queryPacket[7] = (byte)time.Second;
			queryPacket[7] = (byte)((queryPacket[7] % 10) + ((queryPacket[7] / 10) * 16));

			if (UsbConn.WritePacket(queryPacket)) {
				//return null != this.UsbComm.ReadPacket();
				return true;
			}
			return false;
		}

		public TimeSpan Ping() {
			lock (_connLock) {
				return AveragePing(2);
			}
		}

		private TimeSpan AveragePing(int n) {
			var sum = RawPing();
			for (int i = 1; i < n; i++) {
				sum += RawPing();
			}
			return new TimeSpan(sum.Ticks / n);
		}

		private TimeSpan RawPing() {
			if (!IsConnected) {
				Connect();
			}
			var start = DateTime.Now;
			RawQueryClock();
			return DateTime.Now.Subtract(start);
		}

		private DateTime RawQueryClock() {
			if (!IsConnected) {
				Connect();
			}
			var queryPacket = GenerateEmptyPacketData();
			queryPacket[0] = 0;
			queryPacket[1] = 0x87;
			if (null != UsbConn && UsbConn.WritePacket(queryPacket)) {
				queryPacket = UsbConn.ReadPacket();
				if (null != queryPacket) {
					{
						var tmp = queryPacket[4];
						queryPacket[4] = queryPacket[5];
						queryPacket[5] = tmp;
					}
					DateTime dt;
					return DaqDataFileInfo.TryConvert7ByteDateTime(queryPacket, 2, out dt)
						? dt
						: default(DateTime)
					;
				}
			}
			return default(DateTime);
		}

		private DateTime QueryAdjustedClock() {
			lock (_connLock) {
				var pingTime = new TimeSpan(this.AveragePing(2).Ticks / 2);
				var rawClock = RawQueryClock();
				return default(DateTime).Equals(rawClock) ? rawClock : rawClock.Add(pingTime);
			}
		}

		protected override void Dispose(bool disposing) {
			if(disposing) {
				;
			}
			TerminateDeviceQuery();

			base.Dispose(disposing);
		}

		

		IEnumerator<ISensor> IEnumerable<ISensor>.GetEnumerator() {
			return _sensors.Cast<ISensor>().GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return _sensors.AsEnumerable().GetEnumerator();
		}

		IEnumerator<UsbDaqConnection.Sensor> IEnumerable<UsbDaqConnection.Sensor>.GetEnumerator() {
			return _sensors.AsEnumerable().GetEnumerator();
		}


		public DateTime QueryClock() {
			throw new NotImplementedException();
		}

		public bool SetSensorId(int currentId, int desiredId) {
			throw new NotImplementedException();
		}

		public void Pause() {
			throw new NotImplementedException();
		}

		public void Resume() {
			throw new NotImplementedException();
		}

		public double VoltageUsb {
			get { throw new NotImplementedException(); }
		}

		public double VoltageBattery {
			get { throw new NotImplementedException(); }
		}

		public double Temperature {
			get { throw new NotImplementedException(); }
		}

		public TemperatureUnit TemperatureUnit {
			get { throw new NotImplementedException(); }
		}

		public bool UsingDaqTemp {
			get { throw new NotImplementedException(); }
		}

		public void UseDaqTemp(bool useDaqTemp) {
			throw new NotImplementedException();
		}

		public bool ReconnectMedia() {
			throw new NotImplementedException();
		}
	}
}
