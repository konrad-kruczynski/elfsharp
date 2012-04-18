using System;

namespace ELFSharp
{
    public interface INoteSection : ISection
    {
        string NoteName { get; }
        byte[] Description { get; }
    }
}

