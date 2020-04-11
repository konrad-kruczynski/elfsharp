using System;
using ELFSharp.ELF.Sections;
using ELFSharp.Utilities;

namespace ELFSharp.ELF.Segments
{
    public sealed class NoteSegment<T> : Segment<T>, INoteSegment
    {
        internal NoteSegment(long headerOffset, Class elfClass, SimpleEndianessAwareReader reader)
            : base(headerOffset, elfClass, reader)
        {
            data = new NoteData((ulong)base.Offset, (ulong)base.FileSize, reader);
        }

        public string NoteName => data.Name;

        public ulong NoteType => data.Type;

        public byte[] NoteDescription => data.Description;

        private readonly NoteData data;
    }
}
