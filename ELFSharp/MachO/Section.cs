using System;

namespace ELFSharp.MachO
{
    public sealed class Section
    {
        public Section(string name, long address, long size, long offsetInSegment, int alignExponent, Segment segment)
        {
            Name = name;
            Address = address;
            Size = size;
            this.offsetInSegment = offsetInSegment;
            AlignExponent = alignExponent;
            this.segment = segment;
        }

        public string Name { get; private set; }
        public long Address { get; private set; }
        public long Size { get; private set; }
        public int AlignExponent { get; private set; }

        public byte[] GetData()
        {
            var result = new byte[Size];
            Array.Copy(segment.GetData(), (int)offsetInSegment, result, 0, (int)Size);
            return result;
        }

        private readonly long offsetInSegment;
        private readonly Segment segment;
    }
}

