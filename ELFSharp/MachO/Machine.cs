using System;

namespace ELFSharp.MachO
{
    public enum Machine : int
    {
        Any = -1,
        Vax = 1,
        Romp = 2,
        NS32032 = 4,
        NS32332 = 5,
        M68k = 6,
        I386 = 7,
        X86_64 = I386 | MachO.Architecture64,
        Mips = 8,
        PaRisc = 11,
        ARM = 12,
        M88k = 13,
        Sparc = 14,
        I860BE = 15,
        I860LE = 16,
        RS6000 = 17,
        M98k = 18,
        PowerPC = 19,
        PowerPC64 = PowerPC | MachO.Architecture64
    }


}

