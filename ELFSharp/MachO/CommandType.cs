using System;

namespace ELFSharp.MachO
{
    public enum CommandType : uint
    {
        Segment = 0x1,
        SymbolTable = 0x2,
        Segment64 = 0x19,
        Main = 0x80000028u
    }
}

