using System;

namespace ELFSharp
{
    public interface IProgramHeader
    {
        ProgramHeaderType Type { get; }
        ProgramHeaderFlags Flags { get; }
        byte[] GetContents();
    }
}

