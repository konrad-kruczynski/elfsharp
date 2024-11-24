namespace ELFSharp.ELF.Sections
{
    public interface ISection
    {
        string Name { get; }
        uint NameIndex { get; }
        SectionType Type { get; }
        SectionFlags Flags { get; }
        byte[] GetContents();
    }
}