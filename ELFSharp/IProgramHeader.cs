using System;

namespace ELFSharp
{
    public interface ISegment
    {
        ProgramHeaderType Type { get; }
        ProgramHeaderFlags Flags { get; }
        byte[] GetContents();
    }
}

