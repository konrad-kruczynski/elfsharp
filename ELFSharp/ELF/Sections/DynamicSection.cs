using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ELFSharp.Utilities;

namespace ELFSharp.ELF.Sections
{
    public sealed class DynamicSection<T> : Section<T>, IDynamicSection where T : struct
    {
        internal DynamicSection(SectionHeader header, Func<SimpleEndianessAwareReader> readerSource, ELF<T> elf) : base(header, readerSource)
        {
            this.elf = elf;
            ReadEntries();
        }

        public IEnumerable<DynamicEntry<T>> Entries
        {
            get { return new ReadOnlyCollection<DynamicEntry<T>>(entries); }
        }

        IEnumerable<IDynamicEntry> IDynamicSection.Entries
        {
            get { return entries.Cast<IDynamicEntry>(); }
        }

        public override string ToString()
        {
            return string.Format("{0}: {2}, load @0x{4:X}, {5} entries", Name, NameIndex, Type, RawFlags, LoadAddress, Entries.Count());
        }

        private void ReadEntries()
        {
            /// "Kind-of" Bug:
            /// So, this winds up with "extra" DT_NULL entries for some executables.  The issue
            /// is basically that sometimes the .dynamic section's size (and # of entries) per the 
            /// header is higher than the actual # of entries.  The extra space gets filled with null
            /// entries in all of the ELF files I tested, so we shouldn't end up with any 'incorrect' entries 
            /// here unless someone is messing with the ELF structure.
            /// 
            using (var reader = ObtainReader())
            {
                var entryCount = elf.Class == Class.Bit32 ? Header.Size / 8 : Header.Size / 16;

                entries = new List<DynamicEntry<T>>();

                for (ulong i = 0; i < entryCount; i++)
                {
                    if (elf.Class == Class.Bit32)
                    {
                        entries.Add(new DynamicEntry<T>(reader.ReadUInt32().To<T>(), reader.ReadUInt32().To<T>()));
                    }
                    else if (elf.Class == Class.Bit64)
                    {
                        entries.Add(new DynamicEntry<T>(reader.ReadUInt64().To<T>(), reader.ReadUInt64().To<T>()));
                    }
                }
            }
        }

        private List<DynamicEntry<T>> entries;
        private readonly ELF<T> elf;
    }
}