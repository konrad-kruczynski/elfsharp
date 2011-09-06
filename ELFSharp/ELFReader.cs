using System.IO;

namespace ELFSharp
{
    public static class ELFReader
    {
        public static ELF Load(string fileName)
        {
            return new ELF(fileName);
        }
        
    }
}