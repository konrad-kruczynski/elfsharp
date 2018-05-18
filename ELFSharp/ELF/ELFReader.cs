using System;
using System.IO;

namespace ELFSharp.ELF
{
	public static class ELFReader
	{
		public static IELF Load(string fileName)
		{
            if(!TryLoad(fileName, out IELF elf))
            {
                throw new ArgumentException("Given file is not proper ELF file.");
            }
            return elf;
		}

		public static bool TryLoad(string fileName, out IELF elf)
		{
			switch(CheckELFType(fileName))
			{
			case Class.Bit32:
				elf = new ELF<uint>(fileName);
				return true;
			case Class.Bit64:
				elf = new ELF<ulong>(fileName);
				return true;
			default:
				elf = null;
				return false;
			}
		}

		public static Class CheckELFType(string fileName)
		{
			var size = new FileInfo(fileName).Length;
			if(size < Consts.MinimalELFSize)
			{
				return Class.NotELF;
			}
			using(var reader = new BinaryReader(File.OpenRead(fileName)))
			{
				var magic = reader.ReadBytes(4);
				for(var i = 0; i < 4; i++)
				{
					if(magic[i] != Magic[i])
					{
						return Class.NotELF;
					}
				}
				var value = reader.ReadByte();
				return value == 1 ? Class.Bit32 : Class.Bit64;
			}
		}
        
		public static ELF<T> Load<T>(string fileName) where T : struct
		{
			return new ELF<T>(fileName);
		}

		public static bool TryLoad<T>(string fileName, out ELF<T> elf) where T : struct
		{
			switch(CheckELFType(fileName))
			{
			case Class.Bit32:
			case Class.Bit64:
				elf = new ELF<T>(fileName);
				return true;
			default:
				elf = null;
				return false;
			}
		}

		private static readonly byte[] Magic = {
			0x7F,
			0x45,
			0x4C,
			0x46
		}; // 0x7F 'E' 'L' 'F'
        
	}
}