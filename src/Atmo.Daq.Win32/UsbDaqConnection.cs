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
using Atmo.Device;
using Atmo.Units;

namespace Atmo.Daq.Win32 {
	public class UsbDaqConnection : BaseDaqUsbConnection, IDaqConnection, IEnumerable<UsbDaqConnection.Sensor> {

		private struct DaqStatusValues : IEquatable<DaqStatusValues> {

			public static bool operator==(DaqStatusValues a, DaqStatusValues b) {
				return a.Equals(b);
			}

			public static bool operator !=(DaqStatusValues a, DaqStatusValues b) {
				return !a.Equals(b);
			}

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

			public bool Equals(DaqStatusValues other) {
				return Temperature == other.Temperature
					&& VoltageBattery == other.VoltageBattery
					&& VoltageUsb == other.VoltageUsb
				;
			}
		}

		private class QueryPause : IDisposable
		{
			public QueryPause(UsbDaqConnection conn) {
				if(null == conn)
					throw new ArgumentNullException("conn");

				Conn = conn;
				QueryWasActive = conn._queryActive;
				
				if(QueryWasActive)
					Conn.PauseQuery();
			}

			public readonly UsbDaqConnection Conn;
			public readonly bool QueryWasActive;

			public void Dispose() {
				if(QueryWasActive)
					Conn.ResumeQuery();
			}
		}

		private class ConnectionIsolator : IDisposable
		{
			public ConnectionIsolator(UsbDaqConnection conn)
				: this(conn, true) { }

			public ConnectionIsolator(UsbDaqConnection conn, bool cleanQueue) {
				if (null == conn)
					throw new ArgumentNullException("conn");

				Monitor.Enter(conn._connLock);
				if (cleanQueue)
					conn.UsbConn.ClearPacketQueue();
				
				Conn = conn;
			}

			public readonly UsbDaqConnection Conn;

			public void Dispose() {
				Monitor.Exit(Conn._connLock);
			}
		}

		public class Sensor : ISensor {

			private const int DefaultMaxReadingsValue = 10;

			private readonly int _nid;
			private readonly UsbDaqConnection _connection;
			private readonly int _maxReadings;
			private readonly List<PackedReading> _latestReadings; // should probably be a queue
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

			/// <inheritdoc/>
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

			/// <inheritdoc/>
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

		private static readonly TimeSpan HalfSecond = new TimeSpan(0, 0, 0, 0, 500);
		private static readonly TimeSpan QuarterSecond = new TimeSpan(0, 0, 0, 0, 250);
		private static readonly TimeSpan TenthSecond = new TimeSpan(0, 0, 0, 0, 100);
		private static readonly TimeSpan ThreeQuarterSecond = new TimeSpan(0, 0, 0, 0, 750);
		private static readonly TimeSpan OneSecond = new TimeSpan(0, 0, 0, 1);

		public static string DefaultDaqDeviceId{ get { return DefaultDaqDeviceIdValue; }}

		private static byte[] GenerateNetworkSizePacketData(byte currentId, byte desiredId, int size) {
			checked {
				return GenerateNetworkSizePacketData(currentId, desiredId, (byte) size);
			}
		}

		private static byte[] GenerateNetworkSizePacketData(byte currentId, byte desiredId, byte size) {
			var packet = GenerateEmptyPacketData();
			packet[1] = 0x82;
			packet[2] = currentId;
			packet[3] = desiredId;
			packet[4] = size;
			return packet;
		}

		private static byte[] GenereateQueryPacketData(int nid) {
			checked {
				return GenereateQueryPacketData((byte) nid);
			}
		}

		private static byte[] GenereateQueryPacketData(byte nid) {
			var packet = GenerateEmptyPacketData();
			packet[1] = 0x85;
			packet[2] = nid;
			return packet;
		}

		private static byte[] GenerateQueryDaqStatusPacketData()
		{
			var packet = GenerateEmptyPacketData();
			packet[1] = 0x37;
			return packet;
		}

		private static byte[] GenerateEnterAnemBootPacketData(int nid) {
			checked {
				return GenerateEnterAnemBootPacketData((byte) nid);
			}
		}

		private static byte[] GenerateEnterAnemBootPacketData(byte nid) {
			var packet = GenerateEmptyPacketData();
			packet[1] = 0x74;
			packet[2] = nid;
			return packet;
		}

		private static bool PacketHasOkIn3And4(byte[] packet) {
			return null != packet
				&& packet.Length > 4
				&& packet[3] == 0x4f && packet[4] == 0x4b // "OK"
			;
		}

		private static bool IsValidEnterAnemBootResponse(byte[] packet, byte nid) {
			return PacketHasOkIn3And4(packet)
				&& packet[1] == 0x74 // the message we execpted
				&& packet[2] == nid // same NID
			;
		}

		private static byte[] GenerateSetDaqTempPacket(bool useDaq, byte nid) {
			var packet = GenerateEmptyPacketData();
			packet[1] = 0x72;
			packet[3] = useDaq ? (byte)0 : (byte)1;
			packet[2] = nid;
			return packet;
		}

		private static bool IsValidSetDaqTempResponse(byte[] packet) {
			return PacketHasOkIn3And4(packet);
		}

		private static byte[] GenerateResetAnemPacket(byte nid) {
			var packet = GenerateEmptyPacketData();
			packet[1] = 0x76;
			packet[2] = nid;
			return packet;
		}

		private static bool IsValidResetAnemResponse(byte[] packet) {
			return PacketHasOkIn3And4(packet);
		}

		private static byte[] GenerateEnterDaqBootPacketData() {
			var packet = GenerateEmptyPacketData();
			packet[1] = 0x77;
			return packet;
		}

		private static byte[] GenerateReconnectMediaPacketData() {
			var packet = GenerateEmptyPacketData();
			packet[1] = 0x78;
			return packet;
		}

		private static byte[] GenerateGetCorrectionFactoryPacketData(byte nid) {
			var packet = GenerateEmptyPacketData();
			packet[1] = 0x71;
			packet[2] = nid;
			return packet;
		}

		private static byte[] GeneratePutCorrectionFactorsPacketData(byte nid, CorrectionFactors factors) {
			var packet = GenerateEmptyPacketData();
			packet[1] = 0x70;
			packet[2] = nid;
			CorrectionFactors.ToBytes(factors, packet, 3);
			return packet;
		}

		private static bool IsValidPutFactorsResponse(byte[] packet) {
			return PacketHasOkIn3And4(packet);
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
        	_queryTimer = new Timer(QueryThreadBodyTrigger, null, Timeout.Infinite, DefaultMsPeriodValue);
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

		private void InitiateDeviceQuery() {
			ResumeQuery();
			_queryTimer.Change(1, DefaultMsPeriodValue);
		}

		private void TerminateDeviceQuery() {
			if (null != _queryTimer) {
				PauseQuery();
				_queryTimer.Change(Timeout.Infinite, DefaultMsPeriodValue);
				_queryTimer.Dispose();
				_queryTimer = null;
			}
		}

		private void QueryThreadBodyTrigger(object bs) {
			QueryThreadBodyTrigger();
		}

		private object _queryBodylock = new object();

		private void QueryThreadBodyTrigger() {
			if(Monitor.TryEnter(_queryBodylock)) {
				try {
					QueryThreadBody();
				}
				finally {
					Monitor.Exit(_queryBodylock);
				}
			}
		}

		private void Swap(byte[] data, int a, int b) {
			var t = data[a];
			data[a] = data[b];
			data[b] = t;
		}

		private bool WritePacket(byte[] packet) {
			return null != UsbConn && UsbConn.WritePacket(packet);
		}

		private bool ConnectIfRequired() {
			if (IsConnected)
				return true;
			return Connect();
		}

		private QueryPause NewQueryPause() {
			return new QueryPause(this);
		}

		private ConnectionIsolator IsolateConnection() {
			return new ConnectionIsolator(this);
		}

		private void QueryThreadBody() {
			if (!_queryActive)
				return;
			
			int networkSize = 4;
			var values = new PackedReadingValues[networkSize];
			bool? usingDaqTemp = null;
			int highestValid = -1;
			var now = DateTime.Now;
			var daqSafeTime = UnitUtility.StripToSecond(now);

			for (int i = 0; i < values.Length; i++) {
				values[i] = QueryValues(i);
			}

			for (int i = 0; i < values.Length; i++) {
				if (!values[i].IsValid)
					continue;

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
			_lastClock = QueryAdjustedClock(0);
			_daqStat = _lastDaqStatusQuery >= now ? QueryStatus() : DaqStatusValues.Default;

			for (int i = 0; i < values.Length; i++) {
				_sensors[i].HandleObservation(values[i], networkSize, daqSafeTime);
			}

			_networkSize = networkSize;
			if (!_usingDaqTempUntil.HasValue || _usingDaqTempUntil.Value <= DateTime.Now) {
				_usingDaqTemp = usingDaqTemp;
				_usingDaqTempUntil = null;
			}
		}

		public PackedReadingValues QueryValues(int nid) {
			if(nid < 0 || nid > 3)
				throw new ArgumentOutOfRangeException("nid");

			using (IsolateConnection()) {
				System.Diagnostics.Debug.Write(String.Format("Query {0}", nid));
				ConnectIfRequired();
				var queryPacket = GenereateQueryPacketData(nid);
				if (WritePacket(queryPacket)) {
					for (int i = 0; i < 3; i++) {
						queryPacket = UsbConn.ReadPacket(TenthSecond);
						if (null != queryPacket) {
							var result = PackedReadingValues.FromDeviceBytes(queryPacket, 1);
							System.Diagnostics.Debug.WriteLine(String.Format(", {0}: {1}", nid, result));
							return result;
						}
					}
				}
			}
			System.Diagnostics.Debug.WriteLine(String.Format(": FAIL!", nid));
			return PackedReadingValues.Invalid;
		}

		private DaqStatusValues QueryStatus() {
			var status = DaqStatusValues.Default;
			using (IsolateConnection()) {
				System.Diagnostics.Debug.Write("Status");
				ConnectIfRequired();
				var queryPacket = GenerateQueryDaqStatusPacketData();
				if (null != UsbConn && UsbConn.WritePacket(queryPacket)) {
					queryPacket = UsbConn.ReadPacket(QuarterSecond);
					if (null != queryPacket) {
						var volUsbRaw = (ushort)(queryPacket[2] | (queryPacket[3] << 8));
						var volBatRaw = (ushort)(queryPacket[4] | (queryPacket[5] << 8));
						var tmpDaqRaw = (ushort)(queryPacket[7] | (queryPacket[6] << 8));

						var temp = (tmpDaqRaw / 10.0) - 40.0;
						var volUsb = (volUsbRaw / 1024.0) * 6.4;
						var volBat = (volBatRaw / 1024.0) * 6.4;

						status = new DaqStatusValues(volBat, volUsb, temp);
						System.Diagnostics.Debug.WriteLine(": " + status);
					}
				}
				if (status == DaqStatusValues.Default) {
					System.Diagnostics.Debug.WriteLine(": FAIL!");
				}
			}
			return status;
		}

		public void SetNetworkSize(int size) {
			using (NewQueryPause()) {
				using (IsolateConnection()) {
					System.Diagnostics.Debug.Write("SetSize");
					ConnectIfRequired();
					var queryPacket = GenerateNetworkSizePacketData(0xff, 0xff, size);
					if (UsbConn.WritePacket(queryPacket)) {
						_networkSize = size;
					}
					System.Diagnostics.Debug.WriteLine(": OK");
				}
			}
		}

		private bool RepeatWrite(byte[] packet, int n, int sleepMs = 50) {
			if (n <= 0)
				return false;

			var ok = WritePacket(packet);
			for (int i = 1; i < n; i++) {
				Thread.Sleep(sleepMs);
				ok |= WritePacket(packet);
			}
			return ok;
		}

		private static T Clamp<T>(T value, T min, T max) where T:IComparable<T> {
			if (value.CompareTo(min) < 0)
				return min;
			if (max.CompareTo(value) < 0)
				return max;
			return value;
		}

		public bool ChangeSensorId(int currentId, int desiredId, int numTries = 3) {
			using (NewQueryPause()) {
				const int maxSize = 4;
				int calculatedSize = _networkSize;
				if (desiredId >= 0 && desiredId < maxSize && desiredId >= _networkSize) {
					calculatedSize = desiredId + 1;
				}

				var packet = GenerateNetworkSizePacketData(
					currentId >= 0 ? (byte) currentId : (byte) 0xff,
					desiredId >= 0 ? (byte) desiredId : (byte) 0xff,
					calculatedSize
				);
				using (IsolateConnection()) {
					System.Diagnostics.Debug.Write("ChangeSensor");
					ConnectIfRequired();
					var ok = RepeatWrite(packet, numTries);
					System.Diagnostics.Debug.WriteLine(": OK");
					return ok;
				}
			}
		}

		public bool SetClock(DateTime time) {
			var start = DateTime.Now;
			using (NewQueryPause()) {
				Thread.Sleep(750);
				using (IsolateConnection()) {
					System.Diagnostics.Debug.Write("SetClock");
					// determine the best time to send the set clock command
					var optimalTime = start;
					var firstDaqClock = RawQueryClock();
					const int checkSeg = 4;
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
						if (DateTime.Now.AddSeconds(1.0/checkSeg) >= nextBestTime) {
							break;
						}
						Thread.Sleep(1000/checkSeg);
					}
					// set it
					var elapsed = DateTime.Now - start;

					var ok = SetClockRaw(time.Add(elapsed).AddSeconds(1));
					System.Diagnostics.Debug.WriteLine(": OK");
					return ok;
				}
			}
		}

		private bool SetClockRaw(DateTime time) {
			var start = DateTime.Now;
			var pingTime = new TimeSpan(AveragePing().Ticks);

			ConnectIfRequired();
			
			var queryPacket = GenerateEmptyPacketData();
			queryPacket[1] = 0x86;

			time.Add(pingTime + DateTime.Now.Subtract(start));

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

			return WritePacket(queryPacket);
		}

		public TimeSpan Ping() {
			using (IsolateConnection()) {
				System.Diagnostics.Debug.Write("Ping");
				var ping = AveragePing();
				System.Diagnostics.Debug.WriteLine(": OK");
				return ping;
			}
		}
		
		private TimeSpan AveragePing(int n = 2) {
			var sum = RawPing();
			for (int i = 1; i < n; i++) {
				sum += RawPing();
			}
			return new TimeSpan(sum.Ticks / n);
		}

		private TimeSpan RawPing() {
			ConnectIfRequired();
			
			var start = DateTime.Now;
			RawQueryClock();
			return DateTime.Now.Subtract(start);
		}

		private DateTime RawQueryClock() {
			ConnectIfRequired();
			
			var queryPacket = GenerateEmptyPacketData();
			queryPacket[1] = 0x87;
			Thread.Sleep(25);
			if (WritePacket(queryPacket)) {
				Thread.Sleep(25);
				queryPacket = UsbConn.ReadPacket(QuarterSecond);
				if (null != queryPacket) {
					Swap(queryPacket, 4, 5);
					DateTime dt;
					return DaqDataFileInfo.TryConvert7ByteDateTime(queryPacket, 2, out dt)
						? dt
						: (
							DaqDataFileInfo.TryConvertFrom7ByteDateTimeForce(queryPacket, 2, out dt)
								? dt
								: default(DateTime)
						)
					;
				}
			}
			return default(DateTime);
		}

		private DateTime QueryAdjustedClock(int pings = 2) {
			using (IsolateConnection()) {
				System.Diagnostics.Debug.Write("QueryAdjustedClock");
				var halfPingTime = pings > 0 ? new TimeSpan(AveragePing(pings).Ticks / 2) : TimeSpan.Zero;
				var rawClock = RawQueryClock();
				var clock = default(DateTime).Equals(rawClock)
					? rawClock
					: rawClock.Add(halfPingTime);

				System.Diagnostics.Debug.WriteLine(": " + clock);
				return clock;
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

		IEnumerator<Sensor> IEnumerable<Sensor>.GetEnumerator() {
			return _sensors.AsEnumerable().GetEnumerator();
		}

		public DateTime QueryClock() {
			return _lastClock;
		}

		bool IDaqConnection.SetSensorId(int currentId, int desiredId) {
			return ChangeSensorId(currentId, desiredId);
		}

		void IDaqConnection.Pause() {
			PauseQuery();
		}

		void IDaqConnection.Resume() {
			ResumeQuery();
		}

		public double VoltageUsb {
			get {
				_lastDaqStatusQuery = DateTime.Now.Add(_daqStatusQueryLifetime);
				return _daqStat.VoltageUsb;
			}
		}

		public double VoltageBattery {
			get {
				_lastDaqStatusQuery = DateTime.Now.Add(_daqStatusQueryLifetime);
				return _daqStat.VoltageBattery;
			}
		}

		public double Temperature {
			get {
				_lastDaqStatusQuery = DateTime.Now.Add(_daqStatusQueryLifetime);
				return _daqStat.Temperature;
			}
		}

		public TemperatureUnit TemperatureUnit {
			get { return TemperatureUnit.Celsius; }
		}

		public bool UsingDaqTemp {
			get { return _usingDaqTemp.HasValue && _usingDaqTemp.Value; }
		}

		private DateTime? _usingDaqTempUntil = null;

		public void UseDaqTemp(bool useDaqTemp) {
			SetDaqTempSelected(useDaqTemp);
			_usingDaqTemp = useDaqTemp;
			_usingDaqTempUntil = DateTime.Now.Add(new TimeSpan(0, 0, 2));
		}

		private bool AnemEnterBootMode(int nid) {
			return AnemEnterBootMode(checked((byte) nid));
		}

		private bool AnemEnterBootMode(byte nid) {
			var packet = GenerateEnterAnemBootPacketData(nid);
			if (WritePacket(packet)) {
				packet = UsbConn.ReadPacket(QuarterSecond);
				return IsValidEnterAnemBootResponse(packet, nid);
			}
			return false;
		}

		private bool AnemEnterProgramModeVersion2(int nid) {
			return AnemEnterProgramModeVersion2(checked((byte)nid));
		}

		private bool AnemEnterProgramModeVersion2(byte nid) {
			var packet = GenerateEmptyPacketData();
			packet[1] = 0xa0;
			packet[2] = nid;
			if(WritePacket(packet)) {
				packet = UsbConn.ReadPacket(HalfSecond);
				return null != packet && packet[0] == 0xa0 && packet[1] == 0x01;
			}
			return false;
		}

		private bool AnemReset(int nid) {
			return AnemReset(checked((byte) nid));
		}

		private bool AnemReset(byte nid) {
			const int numTries = 5;
			var packet = GenerateResetAnemPacket(nid);
			for (int i = 0; i < numTries; i++ ) {
				if(WritePacket(packet)) {
					var res = UsbConn.ReadPacket(QuarterSecond);
					if (IsValidResetAnemResponse(res)) {
						Thread.Sleep(QuarterSecond);
						return true;
					}
				}
			}
			return false;
		}

		private bool SetDaqTempSelected(bool useDaq) {
			using (NewQueryPause()) {
				var anyOk = false;
				using (IsolateConnection()) {
					ConnectIfRequired();

					for (int nid = 0; nid < _networkSize; nid++) {
						var ok = AnemEnterBootMode(nid);
						try {
							var packet = GenerateSetDaqTempPacket(useDaq, (byte) nid);
							if (WritePacket(packet)) {
								var res = UsbConn.ReadPacket();
								ok &= IsValidSetDaqTempResponse(res);
							}
						}
						finally {
							AnemReset(nid);
						}
						anyOk |= ok;
					}
				}
				return anyOk;
			}
		}

		public bool EnterBootMode() {
			using (NewQueryPause()) {
				using (IsolateConnection()) {
					ConnectIfRequired();
					var queryPacket = GenerateEnterDaqBootPacketData();
					var ok = WritePacket(queryPacket);
					return ok;
				}
			}
		}

		public bool ReconnectMedia() {
			using (NewQueryPause()) {
				using (IsolateConnection()) {
					ConnectIfRequired();
					var queryPacket = GenerateReconnectMediaPacketData();
					var ok = WritePacket(queryPacket);
					return ok;
				}
			}
		}

		public CorrectionFactors GetCorrectionFactors(int nid) {
			using (NewQueryPause()) {
				CorrectionFactors factors = null;
				using (IsolateConnection()) {
					ConnectIfRequired();

					var ok = AnemEnterBootMode(nid);
					Thread.Sleep(HalfSecond);

					try {
						if (ok) {
							var packet = GenerateGetCorrectionFactoryPacketData((byte)nid);
							if (WritePacket(packet)) {
								packet = UsbConn.ReadPacket(OneSecond);
								if (null != packet) {
									factors = CorrectionFactors.ToCorrectionFactors(packet, 3);
								}
							}
						}
					}
					finally {
						Thread.Sleep(HalfSecond);
						AnemReset(nid);
					}
				}
				return factors;
			}
		}

		public bool PutCorrectionFactors(int nid, CorrectionFactors factors) {
			using (NewQueryPause()) {
				bool ok;
				using (IsolateConnection()) {
					ConnectIfRequired();

					ok = AnemEnterBootMode(nid);
					try {
						Thread.Sleep(HalfSecond);
						var packet = GeneratePutCorrectionFactorsPacketData((byte)nid, factors);
						if (WritePacket(packet)) {
							packet = UsbConn.ReadPacket(OneSecond);
							ok &= IsValidPutFactorsResponse(packet);
						}
					}
					finally {
						Thread.Sleep(HalfSecond);
						AnemReset(nid);
					}
				}
				return ok;
			}
		}

		private static void NullAction(double d, string s) {
			;
		}

		public bool ProgramAnem(int nid, MemoryRegionDataCollection memoryRegionDataBlocks, Action<double, string> progressUpdated) {
			if (nid < 0 || nid > 255)
				throw new ArgumentOutOfRangeException("nid");
			if (null == progressUpdated)
				progressUpdated = NullAction; // so we can avoid null checks all over the place

			using (NewQueryPause()) {
				Thread.Sleep(ThreeQuarterSecond);
				bool result = true;
				try {
					if (!UsbConn.IsConnected)
						return false;

					progressUpdated(0, "Entering boot mode.");

					using (IsolateConnection()) {

						var anemBootResult = AnemEnterBootMode(nid);

						if (!anemBootResult) {
							progressUpdated(0, "Bad boot response.");
							if (0xff != nid) {
								result = false;
							}
						}
						Thread.Sleep(HalfSecond);

						progressUpdated(0, "Writing.");
						int totalBytes = memoryRegionDataBlocks.Sum(b => (int)b.Size);
						int bytesSent = 0;
						int lastNoticeByte = 0;
						const int noticeInterval = 128;
						int dotCount = 1;
						if (result) {
							bool firstBlock = true;
							foreach (var block in memoryRegionDataBlocks) {
								byte[] data = block.Data.ToArray();
								int checksumFails = 0;
								const int maxChecksumFails = 64;
								const int stride = 32;
								for (int i = 0; i < data.Length; i += stride) {
									int bytesToWrite = Math.Min(32, data.Length - i);
									int address = i + (int)block.Address;
									var packet = GenerateEmptyPacketData();
									packet[1] = 0x75;
									packet[2] = checked((byte)nid);
									Array.Copy(BitConverter.GetBytes(address).Reverse().ToArray(), 0, packet, 3, 4);
									packet[7] = (byte)bytesToWrite;
									byte checkSum = data[i];
									int endIndex = i + bytesToWrite;
									for (int csi = i + 1; csi < endIndex; csi++) {
										checkSum = unchecked((byte)(checkSum + data[csi]));
									}
									packet[8] = checkSum;
									Array.Copy(data, i, packet, 9, bytesToWrite);
									
									UsbConn.ClearPacketQueue();
									if (!WritePacket(packet)) {
										progressUpdated(0, "Write failure!");
										result = false;
										break; //return false;
									}

									packet = UsbConn.ReadPacket(ThreeQuarterSecond);
									if (null == packet || packet[3] != checkSum) {
										checksumFails++;
										if (checksumFails <= (firstBlock ? 3 : maxChecksumFails)) {
											i-=stride;
											continue;
										}
										progressUpdated(0, "Checksum failure.");
										result = false;
										break;
									}
									checksumFails = 0;

									bytesSent += bytesToWrite;
									if ((bytesSent - lastNoticeByte) > noticeInterval) {
										lastNoticeByte = bytesSent;
										dotCount++;
										if (dotCount > 3) {
											dotCount = 1;
										}
										progressUpdated(
											bytesSent / (double)(totalBytes),
											String.Concat("Writing", new String(Enumerable.Repeat('.', dotCount).ToArray()))
										);
									}
								}
								firstBlock = false;
							}
						}
					}
				}
				catch (Exception ex) {
					progressUpdated(0, ex.ToString());
					result = false;
				}
				finally {
					var resetOk = AnemReset(nid);
					if (!resetOk) {
						progressUpdated(0, "Anem reboot failure.");
					}
					result &= resetOk;
				}
				return result;
			}
		}

		public bool ProgramAnemVersion2(int nid, MemoryRegionDataCollection memoryRegionDataBlocks, Action<double, string> progressUpdated) {
			if(nid < 0 || nid > 255)
				throw new ArgumentOutOfRangeException("nid");
			if(null == progressUpdated)
				progressUpdated = NullAction; // so we can avoid null checks all over the place

			using(NewQueryPause()) {
				Thread.Sleep(ThreeQuarterSecond);
				bool result = true;
				try {
					if(!UsbConn.IsConnected)
						return false;

					progressUpdated(0, "Entering boot mode.");

					using(IsolateConnection()) {

						var anemEnterProgramMode = AnemEnterProgramModeVersion2(nid);

						if(!anemEnterProgramMode) {
							progressUpdated(0, "Bad boot response.");
							if(0xf != nid) {
								result = false; // NOTE: if it was corrupted, try it anyhow
							}
						}
						Thread.Sleep(HalfSecond);

						progressUpdated(0, "Writing.");
						int totalBytes = memoryRegionDataBlocks.Sum(b => (int)b.Size);
						int bytesSent = 0;
						int lastNoticeByte = 0;
						const int noticeInterval = 128;
						int dotCount = 1;
						if(result) {
							bool firstBlock = true;
							foreach(
								var block in memoryRegionDataBlocks
									.Where(x => x.Address < 0x3fff && x.LastAddress > 0x800)
									.OrderBy(x => x.Address)
							) {
								byte[] data = block.Data.ToArray();
								int checksumFails = 0;
								const int maxChecksumFails = 64;
								const int stride = 64;

								long startIndex = 0;
								if(block.Address < 0x800)
									startIndex = 0x800 - block.Address;
								
								long indexLimit = data.Length;
								if(block.LastAddress > 0x3fff) {
									indexLimit = indexLimit - (block.LastAddress - 0x3fff);
								}

								for(long i = startIndex; i < indexLimit; i += stride) {
									var bytesToWrite = Math.Min(64, (int)((long)(data.Length) - i));
									var address = i + block.Address;
									var packet = GenerateEmptyPacketData(68); // NOTE: because of the first weird byte, indices are 1 based, not zero based
									packet[1] = 0xa1;
									packet[2] = unchecked((byte)(address >> 8));
									packet[3] = unchecked((byte)address);
									Array.Copy(data, i, packet, 4, bytesToWrite);
									byte checkSum = 0;
									for(int csi = 4; csi < 68; csi++ ) {
										checkSum = unchecked((byte)(checkSum + packet[csi]));
									}
									packet[68] = checkSum;

									UsbConn.ClearPacketQueue();
									if(!WritePacket(packet)) {
										progressUpdated(0, "Write failure!");
										result = false;
										break; //return false;
									}

									packet = UsbConn.ReadPacket(ThreeQuarterSecond);
									if(null == packet || (packet[2] != 0x00)) {
										checksumFails++;
										if(checksumFails <= (firstBlock ? 3 : maxChecksumFails)) {
											i -= stride;
											continue;
										}
										progressUpdated(0, "Checksum failure.");
										result = false;
										break;
									}
									checksumFails = 0;

									bytesSent += bytesToWrite;
									if((bytesSent - lastNoticeByte) > noticeInterval) {
										lastNoticeByte = bytesSent;
										dotCount++;
										if(dotCount > 3) {
											dotCount = 1;
										}
										progressUpdated(
											bytesSent / (double)(totalBytes),
											String.Concat("Writing", new String(Enumerable.Repeat('.', dotCount).ToArray()))
										);
									}
								}
								firstBlock = false;
							}
						}
					}
				}
				catch(Exception ex) {
					progressUpdated(0, ex.ToString());
					result = false;
				}
				finally {
					var resetOk = AnemReset(nid);
					if(!resetOk) {
						progressUpdated(0, "Anem reboot failure.");
					}
					result &= resetOk;
				}
				return result;
			}
		}

	}
}
