using System.IO;

namespace ELFSharp
{
    public static class ELFReader
    {
        public static ELF32 Load32(string fileName)
        {
            return new ELF32(fileName);
        }
		
		public static ELF64 Load64(string fileName)
		{
			return new ELF64(fileName);
		}
		
		public static ELF Load(string fileName)
		{
			if(CheckClass(fileName) == Class.Bit32)
			{
				return Load32(fileName);
			}
			return Load64(fileName);
		}
		
		private static Class CheckClass(string fileName)
		{
			if(new FileInfo(fileName).Length < 5)
			{
				// return any, cause file is corrupted
				return Class.Bit32;
			}
			using(var stream = new FileStream(fileName,FileMode.Open))
			{
				stream.Seek(4, SeekOrigin.Begin);
				var value = stream.ReadByte();
				// if not 32, then 64 or unknown
				return value == 1 ? Class.Bit32 : Class.Bit64;
			}
		}
        
    }
}