using System;
namespace ELFSharp.ELF.Segments
{
    public interface INoteSegment : ISegment
    {
        string NoteName { get; }
        ulong NoteType { get; }
        byte[] NoteDescription { get; }
    }
}
