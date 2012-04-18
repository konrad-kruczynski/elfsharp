using System;

namespace ELFSharp
{
    public interface INoteSection
    {
        string NoteName { get; }
        byte[] Description { get; }
    }
}

