using System;
using System.Collections.Generic;

namespace ELFSharp
{
    public interface IELF
    {
        Endianess Endianess { get; }
        Class Class { get; }
        FileType Type { get; }
        Machine Machine { get; }
        bool HasProgramHeader { get; }
        bool HasSectionHeader { get; }
        bool HasSectionsStringTable { get; }
        IEnumerable<IProgramHeader> ProgramHeaders { get; }
        IStringTable SectionsStringTable { get; }
        IEnumerable<ISection> Sections { get; }
    }
}

