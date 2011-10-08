using System;

namespace ELFSharp
{
    [Flags]
    public enum SectionFlags
    {
        Writable = 1,
        Allocatable = 2,
        Executable = 4
    }
}

