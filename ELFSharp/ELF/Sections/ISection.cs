namespace ELFSharp.ELF.Sections
{
    public interface ISection
    {
        byte[] GetContents();
        string Name { get; }
        uint NameIndex { get; }
        SectionType Type { get; }
        SectionFlags Flags { get; }
    }
}

