using System;
using System.Runtime.InteropServices;

namespace Atmo.Daq.Win32.WinApi {
	[StructLayout(LayoutKind.Sequential)]
	public struct SP_DEVICE_INTERFACE_DATA {
		public uint cbSize;
		public Guid interfaceClassGuid;
		public uint flags;
		private IntPtr reserved;
	}
}
