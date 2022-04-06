using System;
using System.Diagnostics;

namespace ELFSharp.MachO
{
    [DebuggerDisplay("Symbol({Name,nq},{Value}) in {Section}")]
    public struct Symbol
    {
        public Symbol(string name, long value, Section section) : this()
        {
            Name = name;
            Value = value;
            Section = section;
        }

        public string Name { get; private set; }
        public Int64 Value { get; private set; }
        public Section Section { get; private set; }
    }
}

