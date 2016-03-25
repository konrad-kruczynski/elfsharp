using System;

namespace ELFSharp.MachO
{
    public enum CommandType : uint
    {
        SymbolTable = 0x2,
        Main = 0x80000028u
    }
}

