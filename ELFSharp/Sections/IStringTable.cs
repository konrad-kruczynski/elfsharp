using System.Collections.Generic;

namespace ELFSharp.Sections
{
    public interface IStringTable : ISection
    {
        string this[long index] { get; }
        IEnumerable<string> Strings { get; }
    }
}

