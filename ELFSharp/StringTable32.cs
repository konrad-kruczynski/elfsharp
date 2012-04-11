using System;
namespace ELFSharp
{
    public class StringTable32 : StringTable<uint, uint>
    {
        internal StringTable32(SectionHeader header, Func<EndianBinaryReader> readerSource)
            : base(header, readerSource)
        {

        }
    }
}

