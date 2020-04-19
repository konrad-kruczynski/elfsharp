using System;
using System.Diagnostics;

namespace ELFSharp.MachO
{
    [DebuggerDisplay("Symbol({Name,nq},{Value})")]
    public struct Symbol
    {
        public Symbol(string name, long value) : this()
        {
            Name = name;
            Value = value;
        }

        public string Name { get; private set; }
        public Int64 Value { get; private set; }
    }
}

