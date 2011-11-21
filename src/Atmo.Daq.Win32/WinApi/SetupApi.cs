using System;
using System.Runtime.InteropServices;

namespace Atmo.Daq.Win32.WinApi {
	public static class SetupApi {

		[DllImport("setupapi.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SetupDiGetClassDevs(
			ref Guid classGuid,
			[MarshalAs(UnmanagedType.LPTStr)] string enumerator,
			IntPtr hwndParent,
			DIGCF flags // uint flags
		);

		[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern Boolean SetupDiEnumDeviceInterfaces(
			IntPtr hDevInfo,
			IntPtr devInfo,
			ref Guid interfaceClassGuid,
			UInt32 memberIndex,
			ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData
		);

		[DllImport("setupapi.dll", SetLastError = true)]
		public static extern bool SetupDiEnumDeviceInfo(
			IntPtr deviceInfoSet,
			uint memberIndex,
			ref SP_DEVINFO_DATA deviceInfoData
		);

		/// <summary>
		/// The SetupDiGetDeviceRegistryProperty function retrieves the specified device property.
		/// This handle is typically returned by the SetupDiGetClassDevs or SetupDiGetClassDevsEx function.
		/// </summary>
		/// <param Name="DeviceInfoSet">Handle to the device information set that contains the interface and its underlying device.</param>
		/// <param Name="DeviceInfoData">Pointer to an SP_DEVINFO_DATA structure that defines the device instance.</param>
		/// <param Name="Property">Device property to be retrieved. SEE MSDN</param>
		/// <param Name="PropertyRegDataType">Pointer to a variable that receives the registry data Type. This parameter can be NULL.</param>
		/// <param Name="PropertyBuffer">Pointer to a buffer that receives the requested device property.</param>
		/// <param Name="PropertyBufferSize">Size of the buffer, in bytes.</param>
		/// <param Name="RequiredSize">Pointer to a variable that receives the required buffer size, in bytes. This parameter can be NULL.</param>
		/// <returns>If the function succeeds, the return value is nonzero.</returns>
		[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool SetupDiGetDeviceRegistryProperty(
			IntPtr deviceInfoSet,
			ref SP_DEVINFO_DATA deviceInfoData,
			SPDRP property,
			out UInt32 propertyRegDataType,
			IntPtr propertyBuffer,
			uint propertyBufferSize,
			out UInt32 requiredSize
		);

		[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern Boolean SetupDiGetDeviceInterfaceDetail(
			IntPtr hDevInfo,
			ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
			ref SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData,
			UInt32 deviceInterfaceDetailDataSize,
			out UInt32 requiredSize,
			ref SP_DEVINFO_DATA deviceInfoData
		);

		[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool SetupDiDestroyDeviceInfoList(
			IntPtr hDevInfo
		);

	}
}
