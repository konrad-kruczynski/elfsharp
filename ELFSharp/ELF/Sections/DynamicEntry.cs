namespace ELFSharp.ELF.Sections
{
    /// <summary>
    /// Dynamic table entries are made up of a 32 bit or 64 bit "tag"
    /// and a 32 bit or 64 bit union (val/pointer in 64 bit, val/pointer/offset in 32 bit).
    /// 
    /// See LLVM elf.h file for the C/C++ version.
    /// </summary>
    internal class DynamicEntry : IDynamicEntry
    {
        public DynamicEntry(ulong tagValue, ulong unionValue)
        {
            Tag = (DynamicTag)tagValue;
            Union = unionValue;
        }

        public DynamicTag Tag { get; private set; }

        public ulong Union { get; private set; }

        public override string ToString()
        {
            string unionStr = Union.ToString("x16");
            return $"{Tag} \t 0x{unionStr}";
        }
    }
}