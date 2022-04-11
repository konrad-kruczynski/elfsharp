using System;
using System.IO;
using ELFSharp;
using System.Text;
using ELFSharp.Utilities;
using ELFSharp.ELF.Segments;
using System.Collections.ObjectModel;

namespace ELFSharp.ELF.Sections
{
    public class NoteData : INoteData
    {
        public const ulong NoteDataHeaderSize = 12; // name size + description size + field

        public string Name { get; private set; }

        public ReadOnlyCollection<byte> Description 
        { 
            get
            {
                return new ReadOnlyCollection<byte>(DescriptionBytes);
            }
        }

        internal byte[] DescriptionBytes { get; private set; }

        public ulong Type { get; private set; }

        internal ulong NoteOffset { get; private set; }
        internal ulong NoteFileSize { get; private set; }
        internal ulong NoteFileEnd { get { return NoteOffset + NoteFileSize; } }

        public override string ToString()
        {
            return $"Name={Name} DataSize=0x{DescriptionBytes.Length.ToString("x8")}";
        }

        internal NoteData(ulong sectionOffset, ulong sectionSize, SimpleEndianessAwareReader reader)
        {
            this.reader = reader;
            var sectionEnd = (long)(sectionOffset + sectionSize);
            reader.BaseStream.Seek((long)sectionOffset, SeekOrigin.Begin);
            var nameSize = ReadSize();
            var descriptionSize = ReadSize();
            Type = ReadField();
            int remainder;
            var fields = Math.DivRem(nameSize, FieldSize, out remainder);
            var alignedNameSize = FieldSize * (remainder > 0 ? fields + 1 : fields);

            fields = Math.DivRem(descriptionSize, FieldSize, out remainder);
            var alignedDescriptionSize = FieldSize * (remainder > 0 ? fields + 1 : fields);

            // We encountered binaries where nameSize and descriptionSize are
            // invalid (i.e. significantly larger than the size of the binary itself).
            // To avoid throwing on such binaries, we only read in name and description
            // if the sizes are within range of the containing section.
            if (reader.BaseStream.Position + alignedNameSize <= sectionEnd)
            {
                var name = reader.ReadBytes(alignedNameSize);
                if (nameSize > 0)
                {
                    Name = Encoding.UTF8.GetString(name, 0, nameSize - 1); // minus one to omit terminating NUL
                }
                if (reader.BaseStream.Position + descriptionSize <= sectionEnd)
                {
                    DescriptionBytes = descriptionSize > 0 ? reader.ReadBytes(descriptionSize) : new byte[0];
                }
            }

            // If there are multiple notes inside one segment, keep track of the end position so we can read them
            // all when parsing the segment
            NoteOffset = sectionOffset;
            NoteFileSize = (ulong)alignedNameSize + (ulong)alignedDescriptionSize + NoteDataHeaderSize;
        }

        public Stream ToStream()
        {
            return new MemoryStream(DescriptionBytes);
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

        private readonly SimpleEndianessAwareReader reader;
    }
}
