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
			case UImageResult.NotUImage:
				throw new InvalidOperationException("Given file is not an UBoot image.");
			case UImageResult.BadChecksum:
				throw new InvalidOperationException("Wrong header checksum of the given UImage file.");
			case UImageResult.NotSupportedImageType:
				throw new InvalidOperationException("Given image type is not supported.");
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		public static UImageResult TryLoad(string fileName, out UImage uImage)
		{
			uImage = null;
			if(new FileInfo(fileName).Length < 64)
			{
				return UImageResult.NotUImage;
			}
			byte[] headerForCrc;
			using(var reader = new BinaryReader(File.OpenRead(fileName)))
			{
				headerForCrc = reader.ReadBytes(64);
				// we need to zero crc part
				for(var i = 4; i < 8; i++)
				{
					headerForCrc[i] = 0;
				}
			}
			using(var reader = new BinaryReader(File.OpenRead(fileName)))
			{
				var magic = reader.ReadUInt32BigEndian();
				if(magic != Magic)
				{
					return UImageResult.NotUImage;
				}
				var crc = reader.ReadUInt32BigEndian();
				if(crc != GzipCrc32(headerForCrc))
				{
					return UImageResult.BadChecksum;
				}
				reader.ReadBytes(22);
				var imageType = (ImageType)reader.ReadByte();
				if(!Enum.IsDefined(typeof(ImageType), imageType))
				{
					return UImageResult.NotSupportedImageType;
				}
				// TODO: check CRC of the header
				uImage = new UImage(fileName);
				return UImageResult.OK;
			}
		}

		internal static uint GzipCrc32(byte[] data)
		{
			var remainder = Seed;
			for(var i = 0; i < data.Length; i++)
			{
				remainder ^= data[i];
				for(var j = 0; j < 8; j++)
				{
					if((remainder & 1) != 0)
					{
						remainder = (remainder >> 1) ^ Polynomial;
					}
					else
					{
						remainder >>= 1;
					}
				}
			}
			return remainder ^ Seed;
		}

		internal static uint ReadUInt32BigEndian(this BinaryReader reader)
		{
			return (uint)IPAddress.HostToNetworkOrder(reader.ReadInt32());
		}

		internal static int ReadInt32BigEndian(this BinaryReader reader)
		{
			return IPAddress.HostToNetworkOrder(reader.ReadInt32());
		}

		private const uint Magic = 0x27051956;
		private const uint Polynomial = 0xEDB88320;
		private const uint Seed = 0xFFFFFFFF;
	}
}

