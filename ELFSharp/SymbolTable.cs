using System;
using System.Collections.Generic;
using System.IO;
using MiscUtil.IO;

namespace ELFSharp
{
    public abstract class SymbolTable : Section
    {
        internal SymbolTable(SectionHeader header, Func<EndianBinaryReader> readerSource, StringTable table, ELF elf) : base(header, readerSource)
        {
            this.table = table;
            this.elf = elf;
            ReadSymbols();
        }
		
		public IEnumerable<SymbolEntry> Entries
        {
            get { return entries32.Count == 0 ? (IEnumerable<SymbolEntry>) entries64 : (IEnumerable<SymbolEntry>) entries32; }
        }

        private void ReadSymbols()
        {
            using (var reader = ObtainReader())
            {
                entries32 = new List<SymbolEntry32>();
				entries64 = new List<SymbolEntry64>();
				var adder = elf.Class == Class.Bit32 ? Consts.SymbolEntrySize32 : Consts.SymbolEntrySize64;
                for (var i = 0; i < Header.SizeLong; i += adder)
                {
					var value = 0UL;
					var size = 0UL;
                    var nameIdx = reader.ReadUInt32();
					if(elf.Class == Class.Bit32)
					{
						value = reader.ReadUInt32();
	                    size = reader.ReadUInt32();
					}
                    var info = reader.ReadByte();
                    reader.ReadByte(); // other is read, which holds zero					
                    var sectionIdx = reader.ReadUInt16();
					if(elf.Class == Class.Bit64)
					{
						value = reader.ReadUInt64();
	                    size = reader.ReadUInt64();
					}					
                    var name = table[nameIdx];
                    var binding = (SymbolBinding) (info >> 4);
                    var type = (SymbolType) (info & 0x0F);
					if(elf.Class == Class.Bit32)
					{
						entries32.Add(new SymbolEntry32(name, value, size, binding, type, elf, sectionIdx));
					}
					else
					{
						entries64.Add(new SymbolEntry64(name, value, size, binding, type, elf, sectionIdx));
					}
                }
            }
        }

        protected List<SymbolEntry32> entries32;
		protected List<SymbolEntry64> entries64;
        private readonly StringTable table;
        private readonly ELF elf;
    }
}