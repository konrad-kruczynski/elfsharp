using System.Collections.Generic;

namespace ELFSharp.ELF.Sections
{
    public interface ISymbolTable : ISection
    {
        IEnumerable<ISymbolEntry> Entries { get; }
    }
}

