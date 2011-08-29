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

		private const string DefaultBootloaderDeviceIdValue = "Vid_04d8&Pid_003c";

		public static string DefaultBootloaderDeviceId {
			get { return DefaultBootloaderDeviceIdValue; }
		}

		protected BaseDaqUsbConnection(string deviceId)
			: this(new PicHidUsbConnection(deviceId)) { }

		protected BaseDaqUsbConnection(PicHidUsbConnection usbConn) {
			UsbConn = usbConn;
			if(null == UsbConn) {
				throw new ArgumentNullException("usbConn");
			}
		}

		public PicHidUsbConnection UsbConn { get; private set; }

		public bool IsConnected {
			get { return null != UsbConn && UsbConn.IsConnected; }
		}

		public bool Connect() {
			return null != UsbConn && UsbConn.Connect();
		}

		public void Dispose() {
            GC.SuppressFinalize(this);
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing) {
			if(disposing) {
				;
			}
			DisposeComm();
		}

        ~BaseDaqUsbConnection() {
            Dispose(false);
        }

        protected virtual void DisposeComm() {
			if (null != UsbConn) {
				UsbConn.Dispose();
				//UsbConn = null;
            }
        }

	}

}
