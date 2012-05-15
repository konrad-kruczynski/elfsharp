using System.Collections.Generic;

namespace ELFSharp.Sections
{
    public interface ISymbolTable : ISection
    {
        IEnumerable<ISymbolEntry> Entries { get; }
    }
}

