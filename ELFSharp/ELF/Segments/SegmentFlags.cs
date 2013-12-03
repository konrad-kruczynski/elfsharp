using System;

namespace ELFSharp.ELF.Segments
{
    [Flags]
    public enum SegmentFlags : uint
    {
        Execute = 1,
        Write = 2,
        Read = 4
    }
}

