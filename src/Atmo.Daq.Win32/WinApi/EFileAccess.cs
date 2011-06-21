using System;

namespace Atmo.Daq.Win32.WinApi {
	[Flags]
	public enum EFileAccess : uint {
		/// <summary>
		/// Generic Read
		/// </summary>
		GenericRead = 0x80000000,
		/// <summary>
		/// Generic Write
		/// </summary>
		GenericWrite = 0x40000000,
		/// <summary>
		/// Generic Execute
		/// </summary>
		GenericExecute = 0x20000000,
		/// <summary>
		/// Generic
		/// </summary>
		GenericAll = 0x10000000
	}
}
