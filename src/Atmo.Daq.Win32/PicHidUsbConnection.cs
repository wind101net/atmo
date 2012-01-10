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
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using Atmo.Daq.Win32.WinApi;
using Microsoft.Win32.SafeHandles;

namespace Atmo.Daq.Win32 {
	
	/// <summary>
	/// The core PIC HID connection for communication with the device.
	/// </summary>
	public class PicHidUsbConnection : IDisposable {

		private static readonly Guid HidGuidValue = new Guid("4D1E55B2-F16F-11CF-88CB-001111000030");
		private static readonly TimeSpan DefaultTimeout = new TimeSpan(0, 0, 0, 0, 500);
		private const int DefaultPacketSize = 65;
		private static readonly TimeSpan MinReadTimeSpan = new TimeSpan(50 * TimeSpan.TicksPerMillisecond);

		public static Guid HidGuid {
			get { return HidGuidValue; }
		}

		private static int SafeRead(Stream readStream, byte[] packet) {
			try {
				if (null == readStream)
					return 0;

				var ar = readStream.BeginRead(packet, 0, packet.Length, null, null);
				int result = readStream.EndRead(ar);
				return result;
			}
			catch (IOException) {
				return 0;
			}
			catch (OperationCanceledException) {
				return 0;
			}
			catch (Exception ex) {
				return 0;
			}
		}

		[Obsolete]
		private static int SafeRead(Stream readStream, byte[] packet, TimeSpan waitSpan) {
			try {
				if (TimeSpan.Zero >= waitSpan)
					return readStream.Read(packet, 0, packet.Length);

				var tempPacket = new byte[packet.Length];

				var ar = readStream.BeginRead(tempPacket, 0, tempPacket.Length, null, null);

				var waitInterval = MinReadTimeSpan > waitSpan ? waitSpan : MinReadTimeSpan;
				DateTime start = DateTime.Now;
				do {
					Thread.Sleep(waitInterval);
					if (ar.IsCompleted) {
						break;
					}
				} while ((DateTime.Now - start) <= waitInterval);

				if (ar.IsCompleted) {
					var readLength = readStream.EndRead(ar);
					Array.Copy(tempPacket, packet, readLength);
					return readLength;
				}
				readStream.Close();
				readStream.Dispose();
				return -1;
			}
			catch (IOException) {
				return 0;
			}
			catch (OperationCanceledException) {
				return 0;
			}
			catch (Exception ex) {
				return 0;
			}
		}

		private readonly string _deviceId;
		private readonly string _deviceIdUpper;
		//private readonly Regex _deviceIdRegex;
		private SafeFileHandle _writeHandle;
		private SafeFileHandle _readHandle;
		private FileStream _writeStream;
		private FileStream _readStream;
		//private readonly object _readWriteLock = new object();
		private readonly object _readLock = new object();
		private readonly object _writeLock = new object();

		private int _packetSize = DefaultPacketSize;
		private string _devicePathCache;
		private DateTime _devicePathCacheLastUpdate = default(DateTime);
		private TimeSpan _devicePathCacheLifetime = new TimeSpan(0, 0, 0, 2);
		private object _packetQueueLock = new object();
		private Queue<byte[]> _packetQueue;
		private const int PacketQueueMax = 8;
		private Thread _packetReadThread;
		private bool _disposed = false;

		public PicHidUsbConnection(string deviceId) {
            _deviceId = deviceId;
			_deviceIdUpper = deviceId.ToUpperInvariant();
			/*_deviceIdRegex = new Regex(
				String.Format(".*{0}.*",_deviceId),
				RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled
			);*/
            _writeHandle = null;
            _readHandle = null;
            _writeStream = null;
            _readStream = null;
			_devicePathCache = null;
			_packetQueue = new Queue<byte[]>(PacketQueueMax);
		}

		public bool IsConnected {
			get {
				return null != _writeHandle
					&& null != _readHandle
					&& !String.IsNullOrEmpty(GetDevicePath())
				;
			}
		}

		public bool WritePacket(byte[] packet) {
			return WritePacket(packet, 0);
		}

		public bool WritePacket(byte[] packet, int offset) {
			return WritePacket(packet, offset, TimeSpan.Zero);
		}

		public bool WritePacket(byte[] packet, int offset, TimeSpan timeout) {
			if ((offset + _packetSize) > packet.Length) {
				throw new ArgumentException(String.Concat("packet must have ", _packetSize, " bytes from offset"), "packet");
			}
			if (TimeSpan.Zero != timeout)
				throw new NotSupportedException("Write timeout is not supported.");
			
			var result = false;
			lock (_writeLock) {
				if (null != _writeHandle && null != _writeStream) {
					try {
						_writeStream.Write(packet, offset, _packetSize);
						result = true;
					}
					catch (IOException ioEx) {
						result = false;
					}
					catch (OperationCanceledException ocEx) {
						result = false;
					}
				}
			}
			return result;
		}

		public byte[] ReadPacket() {
			return ReadPacket(new TimeSpan(0, 0, 0, 0, 50));
		}

		public byte[] ReadPacket(TimeSpan timeout) {
			DateTime start = DateTime.Now;
			var lastPacket = DequeueLastPacket();
			if (null != lastPacket)
				return lastPacket;
			do {
				Thread.Sleep(50);
				lastPacket = DequeueLastPacket();
				if (null != lastPacket)
					return lastPacket;
			} while(DateTime.Now - start <= timeout);
			return null;
		}

		private void PacketReadThreadBody() {
			while (true) {
				int bytesRead = 0;
				byte[] packet;
				lock (_readLock) {
					if (null == _readStream) {
						Thread.Sleep(50);
						continue;
					}

					packet = new byte[_packetSize];
					//System.Diagnostics.Debug.WriteLine("Waiting for packet..");
					bytesRead = SafeRead(_readStream, packet);
					//System.Diagnostics.Debug.WriteLine("Packet recieved!");
					if (bytesRead < 0 || null == _readStream || !_readStream.CanRead) {
						lock (_writeLock) {
							DisposeHandlesCore();
							CreateHandlesCore();
						}
					}
				}
				if (bytesRead <= 0)
					continue;

				lock (_packetQueueLock) {
					_packetQueue.Enqueue(packet);
					while (_packetQueue.Count > PacketQueueMax) {
						_packetQueue.Dequeue();
					}
				}
			}
		}

		public void ClearPacketQueue() {
			lock (_packetQueueLock) {
				_packetQueue.Clear();
			}
		}

		public byte[] DequeueLastPacket() {
			lock(_packetQueueLock) {
				if(_packetQueue.Count > 0) {
					return _packetQueue.Dequeue();
				}
			}
			return null;
		}

		public bool Connect() {
			var handlesOk = CreateHandles();

			if (null == _packetReadThread) {
				_packetReadThread = new Thread(PacketReadThreadBody){
					IsBackground = true
				};
				_packetReadThread.Start();
			}
			
			return handlesOk;
		}

		private string GetDevicePath() {
			var now = DateTime.Now;
			if (
				null == _devicePathCache
				|| now < _devicePathCacheLastUpdate
				|| now.Subtract(_devicePathCacheLastUpdate) > _devicePathCacheLifetime
			) {
				_devicePathCache = FindDevicePath();
			}
			return _devicePathCache;
		}

		private string FindDevicePath() {
			var hidGuid = HidGuid;
			var deviceInfoList = SetupApi.SetupDiGetClassDevs(ref hidGuid, null, IntPtr.Zero, DIGCF.DeviceInterface | DIGCF.Present);
			if (IntPtr.Zero == deviceInfoList)
				return null;

			try {
				for (uint i = 0; ; i++) {
					var deviceInterfaceData = new SP_DEVICE_INTERFACE_DATA();
					deviceInterfaceData.cbSize = (uint) Marshal.SizeOf(deviceInterfaceData);
					if (!SetupApi.SetupDiEnumDeviceInterfaces(deviceInfoList, IntPtr.Zero, ref hidGuid, i, ref deviceInterfaceData))
						break; // end of the list

					var deviceInfoData = new SP_DEVINFO_DATA();
					deviceInfoData.cbSize = (uint)Marshal.SizeOf(deviceInfoData);
					if (!SetupApi.SetupDiEnumDeviceInfo(deviceInfoList, i, ref deviceInfoData))
						continue;

					const uint bufferSize = 1048;
					var propertyBuffer = Marshal.AllocHGlobal((int)bufferSize);
					try {

						uint requiredSize;
						uint propRegDataType;
						if (!SetupApi.SetupDiGetDeviceRegistryProperty(
							deviceInfoList, ref deviceInfoData, SPDRP.HardwareId, out propRegDataType,
							propertyBuffer, bufferSize, out requiredSize)
						) {
							continue;
						}

						var deviceId = Marshal.PtrToStringAuto(propertyBuffer, (int) requiredSize);
						if (String.IsNullOrEmpty(deviceId)
							|| !deviceId.ToUpperInvariant().Contains(_deviceIdUpper)
						) {
							continue;
						}

						var deviceInterfaceDetailData = new SP_DEVICE_INTERFACE_DETAIL_DATA {
							cbSize = IntPtr.Size == 8 ? 8 : (uint)(4 + Marshal.SystemDefaultCharSize)
						};
						var interfaceDetailOk = SetupApi.SetupDiGetDeviceInterfaceDetail(
							deviceInfoList, ref deviceInterfaceData, ref deviceInterfaceDetailData,
							SP_DEVICE_INTERFACE_DETAIL_DATA.BUFFER_SIZE, out requiredSize, ref deviceInfoData);

						if (interfaceDetailOk)
							return deviceInterfaceDetailData.devicePath;
					}
					finally {
						Marshal.FreeHGlobal(propertyBuffer);
					}
					
				}
			}
			finally {
				SetupApi.SetupDiDestroyDeviceInfoList(deviceInfoList);
			}
			return null;
		}

		private bool CreateHandles() {
			if (_disposed)
				return false;

			lock (_readLock) {
				lock (_writeLock) {
					DisposeHandlesCore();
					return CreateHandlesCore();
				}
			}
		}

		private static SafeFileHandle OpenExistingFile(string devicePath, EFileAccess fileAccess) {
			return Kernel32.CreateFile(
				devicePath, fileAccess,
				EFileShare.Read | EFileShare.Write, IntPtr.Zero,
				ECreationDisposition.OpenExisting, EFileAttributes.None, IntPtr.Zero);
		}

		private bool CreateHandlesCore() {
			if (_disposed)
				return false;

			var devicePath = GetDevicePath();
			if (String.IsNullOrEmpty(devicePath))
				return false;

			_writeHandle = OpenExistingFile(devicePath, EFileAccess.GenericWrite);
			_writeStream = new FileStream(_writeHandle, FileAccess.Write, 65, false);
			_readHandle = OpenExistingFile(devicePath, EFileAccess.GenericRead);
			_readStream = new FileStream(_readHandle, FileAccess.Read, 65, false);

			return true;
		}

		public void Dispose() {
            GC.SuppressFinalize(this);
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing) {
			_disposed = true;
			if(disposing) {
				;
			}
			DisposeHandlesCore();
			if(null != _packetReadThread) {
				_packetReadThread.Abort();
			}
			
		}

        ~PicHidUsbConnection() {
			Dispose(false);
        }

        private void DisposeHandles() {
			lock (_readLock) {
				lock (_writeLock) {
					DisposeHandlesCore();
				}
			}
        }

		private void DisposeHandlesCore() {
			if (null != _writeStream) {
				_writeStream.Close();
				_writeStream.Dispose();
				_writeStream = null;
			}
			if (null != _readStream) {
				_readStream.Close();
				_readStream.Dispose();
				_readStream = null;
			}
			if (null != _writeHandle && !_writeHandle.IsInvalid && !_writeHandle.IsClosed) {
				_writeHandle.Close();
				_writeHandle.Dispose();
			}
			_writeHandle = null;
			if (null != _readHandle && !_readHandle.IsInvalid && !_readHandle.IsClosed) {
				_readHandle.Close();
				_readHandle.Dispose();
			}
			_readHandle = null;
		}


	}

}