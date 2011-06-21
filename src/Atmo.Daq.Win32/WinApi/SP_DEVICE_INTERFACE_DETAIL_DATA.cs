using System;
using System.Runtime.InteropServices;

namespace Atmo.Daq.Win32.WinApi {
	/// <summary>
	/// Device interface detail data.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public struct SP_DEVICE_INTERFACE_DETAIL_DATA {

		public const int BUFFER_SIZE = 256;

		public UInt32 cbSize;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = BUFFER_SIZE)]
		public string devicePath;
	}
}
