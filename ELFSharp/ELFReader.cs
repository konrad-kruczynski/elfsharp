using System;
using System.IO;

namespace ELFSharp
{
	public static class ELFReader
	{
		public static IELF Load(string fileName)
		{
			switch(CheckClass(fileName))
			{
			case Class.Bit32:
				return new ELF<uint>(fileName);
			case Class.Bit64:
				return new ELF<long>(fileName);
			}
			throw new InvalidOperationException("Should not reach here.");
		}
        
		public static ELF<T> Load<T>(string fileName) where T : struct
		{
			return new ELF<T>(fileName);
		}
        
		private static Class CheckClass(string fileName)
		{
			if(new FileInfo(fileName).Length < 5)
			{
				// return any, cause file is corrupted
				return Class.Bit32;
			}
			using(var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				stream.Seek(4, SeekOrigin.Begin);
				var value = stream.ReadByte();
				// if not 32, then 64 or unknown
				return value == 1 ? Class.Bit32 : Class.Bit64;
			}
		}
        
	}
}