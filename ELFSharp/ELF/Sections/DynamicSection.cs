using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using ELFSharp;
using ELFSharp.Utilities;
using System.Text;

namespace ELFSharp.ELF.Sections
{
    /// <summary>
    /// Dynamic Section class
    /// </summary>
    internal sealed class DynamicSection<T> : Section<T>, IDynamicSection where T : struct
    {
        public DynamicSection(SectionHeader header, Class elfClass, Func<SimpleEndianessAwareReader> readerSource) : base(header, readerSource)
        {
            data = new DynamicData(elfClass, header.Offset, header.Size ,readerSource);
        }

        public IEnumerable<IELF_Dyn> Entries
        {
            get 
            {
                return data.entries.AsEnumerable<ELF_Dyn>();
            }
        }

        public override string ToString()
        {
            return string.Format("{0}: {2}, load @0x{4:X}, {5} entries", Name, NameIndex, Type, RawFlags, LoadAddress, data.entries.Count());
        }

        private DynamicData data;
    }
}