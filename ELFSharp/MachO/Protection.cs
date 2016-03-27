using System;

namespace ELFSharp.MachO
{
    [Flags]
    public enum Protection
    {
        Read = 1,
        Write = 2,
        Execute = 4
    }
}

