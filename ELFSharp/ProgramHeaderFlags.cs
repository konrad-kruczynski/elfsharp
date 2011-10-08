using System;

namespace ELFSharp
{
    [Flags]
    public enum ProgramHeaderFlags : uint
    {
        Execute = 1,
        Write = 2,
        Read = 4
    }
}

