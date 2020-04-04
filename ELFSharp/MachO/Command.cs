using System;
using System.IO;

namespace ELFSharp.MachO
{
    public class Command
    {
        internal Command(BinaryReader reader, Stream stream)
        {
            Stream = stream;
            Reader = reader;
        }
        
        protected readonly BinaryReader Reader;
        protected readonly Stream Stream;
    }
}

