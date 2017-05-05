using System;
using System.IO;
using MiscUtil.IO;
using ELFSharp;
using System.Text;

namespace ELFSharp.ELF.Sections
{
    internal class NoteData
    {
        internal string Name { get; private set; }

        internal byte[] Description { get; private set; }

        internal ulong Type { get; private set; }
        
        internal NoteData(Class elfClass, long sectionOffset, Func<EndianBinaryReader> readerSource)
        {
            using(reader = readerSource())
            {
                reader.BaseStream.Seek(sectionOffset, SeekOrigin.Begin);
                var nameSize = ReadSize();
                var descriptionSize = ReadSize();
                Type = ReadField();
                int remainder = nameSize % FieldSize;
                var fields = nameSize / FieldSize;
                var alignedNameSize = FieldSize * (remainder > 0 ? fields + 1 : fields);
                var name = reader.ReadBytesOrThrow(alignedNameSize);
                if(nameSize > 0)
                {
                    Name = Encoding.UTF8.GetString(name, 0, nameSize - 1); // minus one to omit terminating NUL
                }
                Description = descriptionSize > 0 ? reader.ReadBytesOrThrow(descriptionSize) : new byte[0];
            }
            reader = null;
        }
        
        private int ReadSize()
        {
            /*
             * According to some versions of ELF64 specfication, in 64-bit ELF files words, of which
             * such section consists, should have 8 byte length. However, this is not the case in
             * some other specifications (some of theme contradicts with themselves like the 64bit MIPS
             * one). In real life scenarios I also observed that note sections are identical in both
             * ELF classes. There is also only one structure (i.e. Elf_External_Note) in existing and
             * well tested GNU tools.
             *
             * Nevertheless I leave here the whole machinery as it is already written and may be useful
             * some day.
             */
            return reader.ReadInt32();
        }

        private ulong ReadField()
        {
            // see comment above
            return reader.ReadUInt32();
        }
                
        private int FieldSize
        {
            get
            {
                return 4;
            }
        }

        private readonly EndianBinaryReader reader;
    }
}
