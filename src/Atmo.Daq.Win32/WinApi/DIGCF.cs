using System;

namespace Atmo.Daq.Win32.WinApi {


	[Flags]
	public enum DIGCF : uint {
		Default = 0x1,
		Present = 0x2,
		AllClasses = 0x4,
		Profile = 0x8,
		DeviceInterface = 0x10
	}
}
