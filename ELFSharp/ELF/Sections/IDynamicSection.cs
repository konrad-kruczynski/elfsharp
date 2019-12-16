using System;
using System.Collections.Generic;

namespace ELFSharp.ELF.Sections
{
    public interface IDynamicSection : ISection
    {
        IEnumerable<IDynamicEntry> Entries { get; }
    }
}