using System;
using System.Collections.Generic;

namespace ELFSharp
{
    public interface ISymbolTable : ISection
    {
        IEnumerable<ISymbolEntry> Entries { get; }
    }
}

