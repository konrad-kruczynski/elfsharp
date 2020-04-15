using System;
using System.Diagnostics;

namespace ELFSharp.MachO
{
    [DebuggerDisplay("Section({segment.Name,nq},{Name,nq})")]
    public sealed class Section
    {
        public Section(string name, ulong address, ulong size, uint offset, uint alignExponent, Segment segment)
        {
            Name = name;
            Address = address;
            Size = size;
            Offset = offset;
            AlignExponent = alignExponent;
            this.segment = segment;
        }

        public string Name { get; private set; }
        public ulong Address { get; private set; }
        public ulong Size { get; private set; }
        public ulong Offset { get; private set; }
        public uint AlignExponent { get; private set; }

        public byte[] GetData()
        {
            if (Offset < segment.FileOffset || Offset + Size > segment.FileOffset + segment.Size)
            {
                return new byte[0];
            }
            var result = new byte[Size];
            Array.Copy(segment.GetData(), (int)(Offset - segment.FileOffset), result, 0, (int)Size);
            return result;
        }

        private readonly Segment segment;
    }
}

