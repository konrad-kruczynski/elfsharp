namespace ELFSharp
{
    public interface ISegment
    {
        SegmentType Type { get; }
        SegmentFlags Flags { get; }
        byte[] GetContents();
    }
}

