using System;

namespace ELFSharp
{
    [Flags]
    public enum SegmentFlags : uint
    {
        Execute = 1,
        Write = 2,
        Read = 4
    }
}

