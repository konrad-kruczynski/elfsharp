using System;

namespace ELFSharp
{
    public interface IStringTable
    {
        string this[long index] { get; }
    }
}

