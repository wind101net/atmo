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

		[Obsolete]
		private static int SafeRead(Stream readStream, byte[] packet, int waitMs) {
			return SafeRead(readStream, packet, new TimeSpan(TimeSpan.TicksPerMillisecond*waitMs));
		}

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
		private readonly Regex _deviceIdRegex;
		private SafeFileHandle _writeHandle;
		private SafeFileHandle _readHandle;
		private FileStream _writeStream;
		private FileStream _readStream;
		private readonly object _readWriteLock = new object();
		//private Thread _ioThread;
		private int _packetSize = DefaultPacketSize;
		private string _devicePathCache;
		private DateTime _devicePathCacheLastUpdate = default(DateTime);
		private TimeSpan _devicePathCacheLifetime = new TimeSpan(0, 0, 0, 2);

		public PicHidUsbConnection(string deviceId) {
            _deviceId = deviceId;
			_deviceIdRegex = new Regex(
				String.Format(".*{0}.*",_deviceId),
				RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled
			);
            _writeHandle = null;
            _readHandle = null;
            _writeStream = null;
            _readStream = null;
			_devicePathCache = null;
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
			if (TimeSpan.Zero == timeout) {
				var result = false;
				lock (_readWriteLock) {
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
			throw new NotSupportedException("Write timeout is not supported.");
		}

		public byte[] ReadPacket() {
			return ReadPacket(DefaultTimeout);
		}

		public byte[] ReadPacket(TimeSpan timeout) {
			var packet = new byte[_packetSize];
			Array.Clear(packet, 0, packet.Length);
			var result = false;
			int bytesRecieved = 0;
			if (null != _readHandle && null != _readStream) {
				try {
					lock (_readWriteLock) {
						bytesRecieved = SafeRead(_readStream, packet, timeout);
						if(bytesRecieved < 0 || !_readStream.CanRead) {
							DisposeHandlesCore();
							CreateHandlesCore();
						}
					}
					result = bytesRecieved > 0;
				}
				catch (IOException ioEx) {
					result = false;
				}
				catch (OperationCanceledException ocEx) {
					result = false;
				}
			}
			if (result) {
				if (bytesRecieved == _packetSize)
					return packet;
				
				if (bytesRecieved < _packetSize) {
					var shrunkPacket = new byte[bytesRecieved];
					Array.Copy(packet, shrunkPacket, bytesRecieved);
					return shrunkPacket;
				}
				throw new InvalidDataException(String.Format("Read {0} bytes but packet size is {1} .", bytesRecieved, _packetSize));
			}
			return null;
		}

		public bool Connect() {
			return CreateHandles();
		}

		private string GetDevicePath() {
			if (
				Math.Abs((_devicePathCacheLastUpdate - DateTime.Now).Ticks) > _devicePathCacheLifetime.Ticks
				|| null == _devicePathCache
			) {
				_devicePathCache = FindDevicePath();
			}
			return _devicePathCache;
		}

		private string FindDevicePath() {
			var hidGuid = HidGuid;
			var deviceInfoList = SetupApi.SetupDiGetClassDevs(ref hidGuid, null, IntPtr.Zero, DIGCF.DeviceInterface | DIGCF.Present);
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
						if (!SetupApi.SetupDiGetDeviceRegistryProperty(deviceInfoList, ref deviceInfoData, SPDRP.HardwareId,out propRegDataType, propertyBuffer, bufferSize, out requiredSize))
							continue;
							
						var deviceId = Marshal.PtrToStringAuto(propertyBuffer, (int) requiredSize);
						if (!_deviceIdRegex.IsMatch(deviceId))
							continue;
						
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
						if (IntPtr.Zero != propertyBuffer)
							Marshal.FreeHGlobal(propertyBuffer);
					}
					
				}
			}
			finally {
				if (IntPtr.Zero != deviceInfoList) {
					SetupApi.SetupDiDestroyDeviceInfoList(deviceInfoList);
				}
			}
			return null;
		}

		[Obsolete]
		private string FindDevicePath_old() {

			if (String.IsNullOrEmpty(_deviceId)) {
				return null;
			}

			string devicePath = null;
			var deviceInfoList = IntPtr.Zero;

			try {
				// get a device list
				var hidGuid = HidGuid;
				deviceInfoList = SetupApi.SetupDiGetClassDevs(
					ref hidGuid,
					null,
					IntPtr.Zero,
					DIGCF.DeviceInterface | DIGCF.Present
				);
				// search through all the devices
				for (uint i = 0; ; i++) {
					var deviceInterfaceData = new SP_DEVICE_INTERFACE_DATA();
					deviceInterfaceData.cbSize = (uint)Marshal.SizeOf(deviceInterfaceData);
					if (SetupApi.SetupDiEnumDeviceInterfaces(
						deviceInfoList,
						IntPtr.Zero,
						ref hidGuid,
						i,
						ref deviceInterfaceData
					)) {
						var deviceInfoData = new SP_DEVINFO_DATA();
						deviceInfoData.cbSize = (uint)Marshal.SizeOf(deviceInfoData);
						if (SetupApi.SetupDiEnumDeviceInfo(
							deviceInfoList,
							i,
							ref deviceInfoData
						)) {
							uint propRegDataType;
							//byte[] propertyBuffer = new byte[1024];
							const uint bufferSize = 1048;
							var propertyBuffer = Marshal.AllocHGlobal((int)bufferSize);
							try {
								uint requiredSize;
								if (SetupApi.SetupDiGetDeviceRegistryProperty(
									deviceInfoList,
									ref deviceInfoData,
									SPDRP.HardwareId,
									out propRegDataType,
									propertyBuffer,
									bufferSize,
									out requiredSize
								)) {
									//var deviceId = System.Text.Encoding.Unicode.GetString(propertyBuffer, 0, (int)requiredSize);
									var deviceId = Marshal.PtrToStringAuto(propertyBuffer, (int) requiredSize);
									if (_deviceIdRegex.IsMatch(deviceId)) {
										var deviceInterfaceDetailData = new SP_DEVICE_INTERFACE_DETAIL_DATA();
										deviceInterfaceDetailData.cbSize = (IntPtr.Size == 8)
										                                   	? 8
										                                   	: (uint) (4 + Marshal.SystemDefaultCharSize); // fix for 64bit systems
										if (SetupApi.SetupDiGetDeviceInterfaceDetail(
											deviceInfoList,
											ref deviceInterfaceData,
											ref deviceInterfaceDetailData,
											SP_DEVICE_INTERFACE_DETAIL_DATA.BUFFER_SIZE,
											out requiredSize,
											ref deviceInfoData
											)) {
											devicePath = deviceInterfaceDetailData.devicePath;
											break;
										}
										else {
											; // some kind of terrible failure?
										}
									}
								}
							}
							finally {
								if (IntPtr.Zero != propertyBuffer)
									Marshal.FreeHGlobal(propertyBuffer);
							}
						}
					}
					else {
						break; // end of the list
					}
				}
			}
			finally {
				if (IntPtr.Zero != deviceInfoList) {
					SetupApi.SetupDiDestroyDeviceInfoList(deviceInfoList);
					deviceInfoList = IntPtr.Zero;
				}
			}
			return devicePath;
		}

		private bool CreateHandles() {
			DisposeHandles();
			lock (_readWriteLock) {
				return CreateHandlesCore();
			}
		}

		private bool CreateHandlesCore() {
			string devicePath = GetDevicePath();
			if (String.IsNullOrEmpty(devicePath))
				return false;
			
			_writeHandle = Kernel32.CreateFile(
				devicePath, EFileAccess.GenericWrite,
				EFileShare.Read | EFileShare.Write, IntPtr.Zero,
				ECreationDisposition.OpenExisting, EFileAttributes.None, IntPtr.Zero);
			_writeStream = new FileStream(_writeHandle, FileAccess.Write, 65, false);
			_readHandle = Kernel32.CreateFile(
				devicePath, EFileAccess.GenericRead,
				EFileShare.Read | EFileShare.Write, IntPtr.Zero,
				ECreationDisposition.OpenExisting, EFileAttributes.None, IntPtr.Zero);
			_readStream = new FileStream(_readHandle, FileAccess.Read, 65, false);

			return true;
		}

		public void Dispose() {
            GC.SuppressFinalize(this);
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing) {
			if(disposing) {
				;
			}
			DisposeHandles();
		}

        ~PicHidUsbConnection() {
			Dispose(false);
        }

        private void DisposeHandles() {
			lock (_readWriteLock) {
				DisposeHandlesCore();
			}
        }

		private void DisposeHandlesCore() {
			if (null != _writeStream) {
				_writeStream.Close();
				_writeStream = null;
			}
			if (null != _readStream) {
				_readStream.Close();
				_readStream = null;
			}
			if (null != _writeHandle && !_writeHandle.IsInvalid && !_writeHandle.IsClosed) {
				_writeHandle.Close();
			}
			_writeHandle = null;
			if (null != _readHandle && !_readHandle.IsInvalid && !_readHandle.IsClosed) {
				_readHandle.Close();
			}
			_readHandle = null;
		}


	}

}