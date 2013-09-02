namespace ELFSharp.ELF.Sections
{
    public interface IProgBitsSection : ISection
    {
        void WriteContents(byte[] destination, int offset, int length = 0);
    }
}

