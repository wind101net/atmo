using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Atmo.Device;

namespace Atmo.Daq.Win32 {
	public class UsbDaqBootloaderConnection : BaseDaqUsbConnection {

		private const string DefaultBootloaderDeviceIdValue = "Vid_04d8&Pid_003c";

		public static string DefaultBootloaderDeviceId {
			get { return DefaultBootloaderDeviceIdValue; }
		}

		private static readonly string queryTaskDesciption = "Querying device";
		private static readonly string eraseTaskDesciption = "Erasing device";
		private static readonly string progTaskDesciption = "Programming device";

		private static void NullProgressAction(double a, string b) { ; }

		public UsbDaqBootloaderConnection() : this(DefaultBootloaderDeviceId) { }

		public UsbDaqBootloaderConnection(string deviceId) : base(deviceId) { }

		private QueryResult GetMemoryRegionInfos() {
			byte[] packet = GenerateEmptyPacketData();
			packet[1] = 0x02; // query
			if (!UsbConn.WritePacket(packet))
				return null;

			packet = UsbConn.ReadPacket();
			if (null == packet)
				return null;
			
			var result = new QueryResult {
				BytesPerPacket = packet[2]
			};

			for (int i = 0; i < QueryResult.MaxRegions; i++) {
				var typeFlag = packet[4 + (i * 9)];
				if (0xff == typeFlag) {
					break;
				}
				result.Add(new MemoryRegionInfo(
					typeFlag,
					BitConverter.ToUInt32(packet, 5 + (i * 9)),
					BitConverter.ToUInt32(packet, 9 + (i * 9))
				));
			}
			return result;
		}

		private bool EraseDevice() {
			var packet = GenerateEmptyPacketData();
			packet[1] = 0x04;
			if (!UsbConn.WritePacket(packet))
				return false;
			
			ClearPacket(packet);
			packet[1] = 0x02;
			UsbConn.WritePacket(packet);
			packet = UsbConn.ReadPacket();
			return true;
		}

		public bool Program(MemoryRegionDataCollection memoryRegionDataBlocks, Action<double, string> progressUpdated) {
			if (!UsbConn.IsConnected)
				return false;

			if (null == progressUpdated)
				progressUpdated = NullProgressAction;
			
			progressUpdated(0, queryTaskDesciption);

			QueryResult deviceMemoryRegions = GetMemoryRegionInfos();
			if (null == deviceMemoryRegions || deviceMemoryRegions.Count <= 0)
				return false;

			progressUpdated(0.05, eraseTaskDesciption);
			
			EraseDevice(); // if the erase fails, we should probably keep going.

			double writeBaseProgress = 0.3;
			double writeTotalProgress = 1.0 - writeBaseProgress;

			progressUpdated(writeBaseProgress, progTaskDesciption);

			IEnumerable<MemoryRegionInfo> memoryRegions = deviceMemoryRegions.Where(mri => mri.TypeFlag != 0x03);

			long totalBytes = Math.Max(memoryRegions.Sum(mr => mr.Length), 1);
			long totalBytesWritten = 0;
			long bytesForProgressUpdate = totalBytes / 100;
			long lastProgressUpdateBytes = 0;

			foreach (MemoryRegionInfo currentRegion in memoryRegions.OrderByDescending(mr => mr.TypeFlag)) {
				var lastAddress = currentRegion.Address + currentRegion.Length - 1;
				var packet = GenerateEmptyPacketData();

				IEnumerable<MemoryRegionData> blocksForWrite = memoryRegionDataBlocks
					.Where(dataBlock => dataBlock.Address >= currentRegion.Address && dataBlock.Address <= lastAddress)
					.OrderBy(dataBlock => dataBlock.Address)
				;

				var finalAddress = blocksForWrite.Max(b => (int)b.LastAddress);
				var bytes = new byte[currentRegion.Length];
				var baseOffsetAddress = currentRegion.Address;
				var lastLocalAddress = -1;
				foreach (var block in blocksForWrite) {
					var localBlockAddress = checked((int)(block.Address - baseOffsetAddress));
					for (int i = lastLocalAddress + 1; i < localBlockAddress; i++) {
						bytes[i] = 0xff;
					}
					var chunk = block.Data.ToArray();
					Array.Copy(chunk, 0, bytes, localBlockAddress, chunk.Length);
					lastLocalAddress = (int)block.LastAddress;
				}

				for (int i = lastLocalAddress + 1; i < currentRegion.Length; i++)
					bytes[i] = 0xff;

				for (int txOffset = 0; txOffset < bytes.Length; txOffset += deviceMemoryRegions.BytesPerPacket) {

					ClearPacket(packet);
					packet[1] = 0x05;
					int localAddress = (int)(baseOffsetAddress + txOffset);
					if (localAddress > finalAddress) {
						break;
					}
					Array.Copy(BitConverter.GetBytes(localAddress), 0, packet, 2, 4);
					byte bytesToWrite = (
						(txOffset + deviceMemoryRegions.BytesPerPacket > bytes.Length)
						? (byte)(bytes.Length - txOffset)
						: deviceMemoryRegions.BytesPerPacket
					);
					packet[6] = bytesToWrite;
					//bytes.CopyTo(txOffset, packet, 7, bytesToWrite);
					Array.Copy(bytes, txOffset, packet, 7, bytesToWrite);
					totalBytesWritten += bytesToWrite;

					if (totalBytesWritten >= lastProgressUpdateBytes + bytesForProgressUpdate) {
						lastProgressUpdateBytes = totalBytesWritten;
						progressUpdated(writeBaseProgress + ((totalBytesWritten * writeTotalProgress) / (double)totalBytes), progTaskDesciption);
					}
					System.Diagnostics.Debug.WriteLine("Wrote@: " + (baseOffsetAddress + txOffset).ToString("X2") + ',' + bytesToWrite);
					if (UsbConn.WritePacket(packet)) {
						; // was OK
					}
					else {
						; // failed to write!
					}
				}


				ClearPacket(packet);
				packet[1] = 0x06;
				UsbConn.WritePacket(packet);
			}

			progressUpdated(writeBaseProgress + writeTotalProgress, progTaskDesciption);

			return true;
		}

		public bool Reboot() {
			byte[] packet = GenerateEmptyPacketData();
			packet[1] = 0x08;
			bool ok = UsbConn.WritePacket(packet);
			ok |= UsbConn.WritePacket(packet);
			System.Threading.Thread.Sleep(100);
			ok |= UsbConn.WritePacket(packet);
			return ok;
		}


	}
}
