using System;
using System.IO;

namespace ELFSharp
{
    public class NoteSection : Section
    {
        internal NoteSection(SectionHeader header, Func<BinaryReader> readerSource) : base(header, readerSource)
        {
        }
    }
}