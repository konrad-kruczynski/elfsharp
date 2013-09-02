using System;
using System.IO;
using System.Net;

namespace ELFSharp.UImage
{
	public static class UImageReader
	{
		public static UImage Load(string fileName)
		{
			UImage result;
			switch(TryLoad(fileName, out result))
			{
			case UImageResult.OK:
				return result;
			case UImageResult.BadMagic:
				throw new InvalidOperationException("Bad MAGIC value of the UImage file, i.e. this is not an UBoot image.");
			case UImageResult.BadChecksum:
				throw new InvalidOperationException("Wrong header checksum of the given UImage file.");
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		public static UImageResult TryLoad(string fileName, out UImage uImage)
		{
			using(var reader = new BinaryReader(File.OpenRead(fileName)))
			{
				uImage = null;
				var magic = reader.ReadUInt32BigEndian();
				if(magic != Magic)
				{
					return UImageResult.BadMagic;
				}
				// TODO: check CRC of the header
				uImage = new UImage(fileName);
				return UImageResult.OK;
			}
		}

		internal static uint ReadUInt32BigEndian(this BinaryReader reader)
		{
			return (uint)IPAddress.HostToNetworkOrder(reader.ReadInt32());
		}

		private const uint Magic = 0x27051956;
	}
}

