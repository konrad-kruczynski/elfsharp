using System;
using System.Collections.Generic;

namespace ELFSharp
{
    public interface IStringTable : ISection
    {
        string this[long index] { get; }
        IEnumerable<string> Strings { get; }
    }
}

