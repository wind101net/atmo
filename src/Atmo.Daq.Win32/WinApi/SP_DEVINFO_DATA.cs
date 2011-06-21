using System;
using System.Runtime.InteropServices;

namespace Atmo.Daq.Win32.WinApi {
	[StructLayout(LayoutKind.Sequential)]
	public struct SP_DEVINFO_DATA {
		public UInt32 cbSize;
		public Guid classGuid;
		public UInt32 devInst;
		public IntPtr reserved;
	}
}
