using System;
using System.IO;

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

		private static UImageResult TryLoad(string fileName, out UImage uImage)
		{
			using(var reader = new BinaryReader(File.OpenRead(fileName)))
			{
				uImage = null;
				var magic = reader.ReadUInt32();
				if(magic != Magic)
				{
					return UImageResult.BadMagic;
				}
				// TODO: check CRC of the header
				uImage = new UImage(fileName);
				return UImageResult.OK;
			}
		}

		private const uint Magic = 0x27051956;
	}
}

