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

namespace Atmo.Daq.Win32 {

	public abstract class BaseDaqUsbConnection : IDisposable {

		/// <summary>
		/// Generates 65 bytes which have been cleared using <see cref="ClearPacket"/>. 
		/// </summary>
		/// <returns>A new blank packet.</returns>
		protected static byte[] GenerateEmptyPacketData() {
			return GenerateEmptyPacketData(64);
		}

		/// <summary>
		/// Generates bytes which have been cleared using <see cref="ClearPacket"/>. 
		/// </summary>
		/// <returns>A new blank packet.</returns>
		protected static byte[] GenerateEmptyPacketData(int dataSize) {
			//return Enumerable.Repeat((byte)0xff, 65).ToArray();
			var data = new byte[dataSize+1];
			ClearPacket(data);
			return data;
		}

		/// <summary>
		/// Clears an array formatted as a new data packet.
		/// </summary>
		/// <param name="data">The data to clear.</param>
		/// <remarks>
		/// The first byte will have the value 0 while the remaining bytes will have the value 255.
		/// The 0 value appears to be required while the other values are set to 255 to supposedly
		/// reduce the power required to transmit the packet.
		/// </remarks>
		protected static void ClearPacket(byte[] data) {
			if (null == data || data.Length <= 0)
				return;
			
			data[0] = 0;
			for (int i = 1; i < data.Length; i++)
				data[i] = 0xff;
		}

		/// <summary>
		/// Constructs a new connection for the given device ID.
		/// </summary>
		/// <param name="deviceId">The device ID to connect to.</param>
		protected BaseDaqUsbConnection(string deviceId)
			: this(new PicHidUsbConnection(deviceId)) { }

		/// <summary>
		/// Constructs a new connection wrapping the given connection object.
		/// </summary>
		/// <param name="usbConn">The USB connection to use.</param>
		protected BaseDaqUsbConnection(PicHidUsbConnection usbConn) {
			UsbConn = usbConn;
			if(null == UsbConn) {
				throw new ArgumentNullException("usbConn");
			}
		}

		/// <summary>
		/// The core USB connection object.
		/// </summary>
		public PicHidUsbConnection UsbConn { get; private set; }

		/// <summary>
		/// A quick connection test.
		/// </summary>
		public bool IsConnected {
			get { return null != UsbConn && UsbConn.IsConnected; }
		}

		/// <summary>
		/// Forces a connection if possible.
		/// </summary>
		/// <returns></returns>
		public bool Connect() {
			return null != UsbConn && UsbConn.Connect();
		}

		/// <summary>
		/// Closes and disposes the connection.
		/// </summary>
		public void Dispose() {
            GC.SuppressFinalize(this);
			Dispose(true);
		}

		/// <summary>
		/// Closes and disposes the connection.
		/// </summary>
		protected virtual void Dispose(bool disposing) {
			if(disposing) {
				;
			}
			DisposeComm();
		}

        ~BaseDaqUsbConnection() {
            Dispose(false);
        }

		/// <summary>
		/// Closes and disposes the connection.
		/// </summary>
        protected virtual void DisposeComm() {
			if (null != UsbConn) {
				UsbConn.Dispose();
            }
        }

	}

}
