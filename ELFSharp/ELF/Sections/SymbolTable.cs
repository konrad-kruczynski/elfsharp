using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using ELFSharp.Utilities;

namespace ELFSharp.ELF.Sections
{
    public sealed class SymbolTable<T> : Section<T>, ISymbolTable where T : struct
    {
        internal SymbolTable(SectionHeader header, Func<SimpleEndianessAwareReader> readerSource, IStringTable table, ELF<T> elf) : base(header, readerSource)
        {
            this.table = table;
            this.elf = elf;
            ReadSymbols();
        }

        public IEnumerable<SymbolEntry<T>> Entries
        {
            get { return new ReadOnlyCollection<SymbolEntry<T>>(entries); }
        }

        IEnumerable<ISymbolEntry> ISymbolTable.Entries
        {
            get { return Entries.Cast<ISymbolEntry>(); }
        }

        private void ReadSymbols()
        {
            using(var reader = ObtainReader())
            {
                entries = new List<SymbolEntry<T>>();
                var adder = (ulong)(elf.Class == Class.Bit32 ? Consts.SymbolEntrySize32 : Consts.SymbolEntrySize64);
                for(var i = 0UL; i < Header.Size; i += adder)
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
                    var name = table == null ? "<corrupt>" : table[nameIdx];
                    var binding = (SymbolBinding)(info >> 4);
                    var type = (SymbolType)(info & 0x0F);
                    entries.Add(new SymbolEntry<T>(name, value.To<T>(), size.To<T>(), binding, type, elf, sectionIdx));
                }
            }
        }

        private List<SymbolEntry<T>> entries;
        private readonly IStringTable table;
        private readonly ELF<T> elf;
    }
}