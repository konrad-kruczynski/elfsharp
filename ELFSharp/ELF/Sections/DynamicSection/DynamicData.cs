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
    /// Represents the internal data in the .dynamic section (contains a set of tags and values; the meaning of the 'value'
    /// is determined by the tag--can be addresses, flags, etc.).
    /// </summary>
    internal class DynamicData
    {
        /// <summary>
        /// An array of the entries in the .dynamic section.
        /// </summary>
        public ELF_Dyn[] entries;


        /// <summary>
        /// Dynamic data constructor.  Parses the contents of the .dynamic section.
        /// </summary>
        /// <param name="elf">Type of ELF file.</param>
        /// <param name="sectionOffset">Offset that the section starts at.</param>
        /// <param name="size">Size of the section (defined by section header)</param>
        /// <param name="readerSource">Binary data reader.</param>
        public DynamicData(Class elf, long sectionOffset, long size, Func<EndianBinaryReader> readerSource)
        {
            elfClass = elf;
            ulong entryCount = elfClass == Class.Bit32 ? (ulong) size / 8 : (ulong) size / 16;
            entries = new ELF_Dyn[entryCount];
            /// "Kind-of" Bug:
            /// So, this winds up with "extra" DT_NULL entries for some executables.  The issue
            /// is basically that sometimes the .dynamic section's size (and # of entries) per the 
            /// header is higher than the actual # of entries.  The extra space gets filled with null
            /// entries in all of the ELF files I tested, so we shouldn't end up with any 'incorrect' entries 
            /// here unless someone is messing with the ELF structure.
            /// 
            using(reader = readerSource())
            {
                reader.BaseStream.Seek(sectionOffset, SeekOrigin.Begin);
                for(ulong i = 0; i < entryCount; i++){
                    entries[i] = new ELF_Dyn(ReadEntry(), ReadEntry());
                }
            }
        }

        /// <summary>
        /// Returns a printable representation of the contents as a text table.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder b = new StringBuilder();
            b.Append("Tag \t Value\n");
            foreach(ELF_Dyn e in entries)
            {
                b.Append($"{e.ToString()}\n");
            }
            return b.ToString();
        }

        /// <summary>
        /// Reads the entry (differs for 32 bit/64 bit ELF files.)
        /// </summary>
        /// <returns></returns>
        private ulong ReadEntry()
        {
            return elfClass == Class.Bit32 ? reader.ReadUInt32() : reader.ReadUInt64();
        }

        private readonly EndianBinaryReader reader;
        private Class elfClass;
    }
}