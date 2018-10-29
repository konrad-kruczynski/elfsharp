using System;
using System.Collections.Generic;

namespace ELFSharp.ELF.Sections
{
    public interface IDynamicSection : ISection
    {
        IEnumerable<IELF_Dyn> Entries 
        {
            get;
        }
    }
}