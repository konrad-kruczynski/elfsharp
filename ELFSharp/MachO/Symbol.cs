using System;

namespace ELFSharp.MachO
{
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

