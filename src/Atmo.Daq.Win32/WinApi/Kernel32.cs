using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Atmo.Daq.Win32.WinApi {
	public static class Kernel32 {

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		public static extern SafeFileHandle CreateFile(
			string lpFileName,
			EFileAccess dwDesiredAccess,
			EFileShare dwShareMode,
			IntPtr SecurityAttributes,
			ECreationDisposition dwCreationDisposition,
			EFileAttributes dwFlagsAndAttributes,
			IntPtr hTemplateFile
		);

		[DllImport("kernel32.dll")]
		public static extern bool WriteFile(
			SafeFileHandle hFile,
			byte[] lpBuffer,
			uint nNumberOfBytesToWrite,
			out uint lpNumberOfBytesWritten,
			IntPtr lpOverlapped
		);

		[DllImport("kernel32.dll")]
		public static extern bool ReadFile(
			SafeFileHandle hFile,
			byte[] lpBuffer,
			uint nNumberOfBytesToRead,
			out uint lpNumberOfBytesRead,
			IntPtr lpOverlapped
		);

	}
}
