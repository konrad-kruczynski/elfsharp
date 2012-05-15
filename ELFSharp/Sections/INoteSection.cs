namespace ELFSharp.Sections
{
    public interface INoteSection : ISection
    {
        string NoteName { get; }
        byte[] Description { get; }
    }
}

