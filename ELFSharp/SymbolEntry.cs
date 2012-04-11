using System;

namespace ELFSharp
{
    public class SymbolEntry<T> where T : struct
    {
        public string Name { get; private set; }        
        public SymbolBinding Binding { get; private set; }
        public SymbolType Type { get; private set; }
		public T Value { get; private set; }
        public T Size { get; private set; }

        public bool IsSpecialSection
        {
            get { return Enum.IsDefined(typeof (SpecialSection), sectionIdx); } // TODO: test this
        }

        public Section<T> PointedSection
        {
            get { return IsSpecialSection ? null : elf.GetSection(sectionIdx); }
        }

        public ushort PointedSectionIndex 
        { 
            get { return IsSpecialSection ? (ushort)0 : sectionIdx; }
        }

        internal SymbolEntry(string name, T value, T size, SymbolBinding binding, SymbolType type, ELF<T> elf, ushort sectionIdx)
        {
            Name = name;
            Value = value;
            Size = size;
            Binding = binding;
            Type = type;
            this.elf = elf;
            this.sectionIdx = sectionIdx;
        }

        public override string ToString()
        {
            return string.Format("[{3} {4} {0}: 0x{1:X}, size: {2}, section: {5}]",
                                 Name, Value, Size, Binding, Type, (SpecialSection)sectionIdx);
        }

        private readonly ELF<T> elf;
        private readonly ushort sectionIdx;
    }
}