using System.IO;

namespace ELFSharp
{
    public static class ELFReader
    {
        // TODO: replace with sth
        /*public static ELF32 Load32(string fileName)
        {
            return new ELF32(fileName);
        }
		
		public static ELF64 Load64(string fileName)
		{
			return new ELF64(fileName);
		}*/
		
		public static ELF<T> Load<T>(string fileName) where T : struct
		{
            return new ELF<T>(fileName);
            // TODO
			/*if(CheckClass(fileName) == Class.Bit32)
			{
				return new ELF32<uint>(fileName);
			}
			return new ELF64<long>(fileName);*/
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