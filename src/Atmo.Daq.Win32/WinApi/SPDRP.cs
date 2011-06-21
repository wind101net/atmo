
namespace Atmo.Daq.Win32.WinApi {
	public enum SPDRP : uint {
		DeviceDesc = 0x00000000,
		HardwareId = 0x00000001,
		CompatibleIds = 0x00000002,
		NTDevicePaths = 0x00000003,
		Service = 0x00000004,
		Configuration = 0x00000005,
		ConfigutationVector = 0x00000006,
		Class = 0x00000007,
		ClassGuid = 0x00000008,
		Driver = 0x00000009,
		ConfigFlags = 0x0000000A,
		Mfg = 0x0000000B,
		FriendlyName = 0x0000000C,
		LocationInformation = 0x0000000D,
		PhysicalDeviceObjectName = 0x0000000E,
		Capabilities = 0x0000000F,
		UiNumber = 0x00000010,
		UpperFilters = 0x00000011,
		LowerFilters = 0x00000012,
		MaximumProperty = 0x00000013,
	}
}
