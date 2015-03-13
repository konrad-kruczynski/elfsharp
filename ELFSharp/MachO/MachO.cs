using System;
using System.IO;

namespace ELFSharp.MachO
{
    public sealed class MachO
    {
        internal MachO(string fileName)
        {
            using(var reader = new BinaryReader(File.OpenRead(fileName)))
            {
                reader.ReadBytes(4); // header, already checked
                Machine = (Machine)reader.ReadInt32();
            }
        }

        public Machine Machine { get; private set; }

        internal const int Architecture64 = 0x1000000;
    }
}

