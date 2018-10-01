using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using MiscUtil.IO;
using ELFSharp;
using System.Text;

namespace ELFSharp.ELF.Sections
{
    /// <summary>
    /// Dynamic Section class
    /// </summary>
    internal class DynamicSection<T> : Section<T>, IDynamicSection where T : struct
    {
        public DynamicSection(SectionHeader header, Class elfClass, Func<EndianBinaryReader> readerSource) : base(header, readerSource)
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
            return data.ToString();
        }

        private DynamicData data;
    }
}