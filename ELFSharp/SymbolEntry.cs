using System;

namespace ELFSharp
{
    public abstract class SymbolEntry
    {
        public string Name { get; private set; }        
        public SymbolBinding Binding { get; private set; }
        public SymbolType Type { get; private set; }
		
		protected UInt64 LongValue { get; private set; }
        protected UInt64 LongSize { get; private set; }

        public bool IsSpecialSection
        {
            get { return Enum.IsDefined(typeof (SpecialSection), sectionIdx); } // TODO: test this
        }

        public Section PointedSection
        {
            get { return IsSpecialSection ? null : elf.GetSection(sectionIdx); }
        }

        public ushort PointedSectionIndex 
        { 
            get { return IsSpecialSection ? (ushort)0 : sectionIdx; }
        }

        internal SymbolEntry(string name, ulong value, ulong size, SymbolBinding binding, SymbolType type, ELF elf, ushort sectionIdx)
        {
            Name = name;
            LongValue = value;
            LongSize = size;
            Binding = binding;
            Type = type;
            this.elf = elf;
            this.sectionIdx = sectionIdx;
        }

        public override string ToString()
        {
            return string.Format("[{3} {4} {0}: 0x{1:X}, size: {2}, section: {5}]",
                                 Name, LongValue, LongSize, Binding, Type, (SpecialSection)sectionIdx);
        }

        private readonly ELF elf;
        private readonly ushort sectionIdx;
    }
}