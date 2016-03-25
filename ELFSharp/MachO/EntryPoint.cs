using System;
using System.IO;

namespace ELFSharp.MachO
{
    public class EntryPoint : Command
    {
        public EntryPoint(BinaryReader reader, Func<FileStream> streamProvider) : base(reader, streamProvider)
        {
            Value = Reader.ReadInt64();
            StackSize = Reader.ReadInt64();
        }

        public long Value { get; private set; }

        public long StackSize { get; private set; }
        
    }
}

