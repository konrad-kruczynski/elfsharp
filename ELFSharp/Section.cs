using System;
using System.IO;

namespace ELFSharp
{
    public class Section
    {
        internal Section(SectionHeader header, Func<BinaryReader> readerSourceSourceSource) // + rozne property sekcji wyczytane z naglowka sekcji
        {
            Header = header;
            this.readerSourceSourceSource = readerSourceSourceSource;
        }

        public SectionHeader Header { get; private set; }

        public virtual byte[] GetContents()
        {
            using(var reader = ObtainReader())
            {
                return reader.ReadBytes(Convert.ToInt32(Header.SizeLong));
            }
        }

        protected BinaryReader ObtainReader()
        {
            var reader = readerSourceSourceSource();
            reader.BaseStream.Seek(Header.Offset, SeekOrigin.Begin);
            return reader;
        }

        private readonly Func<BinaryReader> readerSourceSourceSource;
    }
}