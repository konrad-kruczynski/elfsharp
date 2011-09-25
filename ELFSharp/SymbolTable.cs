using System;
using System.Collections.Generic;
using System.IO;
using MiscUtil.IO;

namespace ELFSharp
{
    public class SymbolTable : Section
    {
        internal SymbolTable(SectionHeader header, Func<EndianBinaryReader> readerSource, StringTable table, ELF elf) : base(header, readerSource)
        {
            this.table = table;
            this.elf = elf;
            ReadSymbols();
        }

        public IEnumerable<SymbolEntry> Entries
        {
            get { return entries; }
        }

        private void ReadSymbols()
        {
            using (var reader = ObtainReader())
            {
                entries = new List<SymbolEntry>();
                for (var i = 0; i < Header.SizeLong; i += 16) // TODO: convert to const
                {
                    var nameIdx = reader.ReadUInt32();
                    var value = reader.ReadUInt32();
                    var size = reader.ReadUInt32();
                    var info = reader.ReadByte();
                    reader.ReadByte(); // other is read, which holds zero
                    var sectionIdx = reader.ReadUInt16();
                    var name = table[nameIdx];
                    var binding = (SymbolBinding) (info >> 4);
                    var type = (SymbolType) (info & 0x0F);
                    entries.Add(new SymbolEntry(name, value, size, binding, type, elf, sectionIdx));
                }
            }
        }

        private List<SymbolEntry> entries;
        private readonly StringTable table;
        private readonly ELF elf;
    }
}