namespace ELFSharp.ELF.Segments
{
    public interface ISegment
    {
        SegmentType Type { get; }
        SegmentFlags Flags { get; }
        byte[] GetRawHeader();
        byte[] GetFileContents();
        byte[] GetMemoryContents();
    }
}

