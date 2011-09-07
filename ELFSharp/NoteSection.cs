using System;
using System.IO;
using MiscUtil.IO;

namespace ELFSharp
{
    public class NoteSection : Section
    {
        internal NoteSection(SectionHeader header, Func<EndianBinaryReader> readerSource) : base(header, readerSource)
        {
        }
    }
}