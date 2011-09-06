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
        
    }
}