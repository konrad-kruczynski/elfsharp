using System.IO;
using ELFSharp.Utilities;

namespace ELFSharp.MachO
{
    public class Command
    {
        internal Command(SimpleEndianessAwareReader reader, Stream stream)
        {
            Stream = stream;
            Reader = reader;
        }
        
        protected readonly SimpleEndianessAwareReader Reader;
        protected readonly Stream Stream;
    }
}

