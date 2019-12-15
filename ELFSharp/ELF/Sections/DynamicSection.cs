using System;
using System.Collections.Generic;
using System.Linq;
using ELFSharp.Utilities;

namespace ELFSharp.ELF.Sections
{
    internal sealed class DynamicSection<T> : Section<T>, IDynamicSection where T : struct
    {
        public DynamicSection(SectionHeader header, Class elfClass, Func<SimpleEndianessAwareReader> readerSource) : base(header, readerSource)
        {
            data = new DynamicData(elfClass, header.Offset, header.Size, readerSource);
        }

        public IEnumerable<IDynamicEntry> Entries
        {
            get 
            {
                return data.Entries.AsEnumerable();
            }
        }

        public override string ToString()
        {
            return string.Format("{0}: {2}, load @0x{4:X}, {5} entries", Name, NameIndex, Type, RawFlags, LoadAddress, data.Entries.Count());
        }

        private readonly DynamicData data;
    }
}