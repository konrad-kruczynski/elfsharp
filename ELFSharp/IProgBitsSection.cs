using System;

namespace ELFSharp
{
    public interface IProgBitsSection : ISection
    {
        void WriteContents(byte[] destination, int offset, int length = 0);
    }
}

