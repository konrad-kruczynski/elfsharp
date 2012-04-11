using System;
using System.IO;
using MiscUtil.IO;


namespace ELFSharp
{
    public class Section<T> where T : struct
    {
        internal Section(SectionHeader header, Func<EndianBinaryReader> readerSourceSourceSource)
        {
            Header = header;
            this.readerSourceSourceSource = readerSourceSourceSource;
        }

        public virtual byte[] GetContents()
        {
            using(var reader = ObtainReader())
            {
                return reader.ReadBytes(Convert.ToInt32(Header.Size));
            }
        }

        protected EndianBinaryReader ObtainReader()
        {
            var reader = readerSourceSourceSource();
            reader.BaseStream.Seek(Header.Offset, SeekOrigin.Begin);
            return reader;
        }

        public T RawFlags
        {
            get
            {
                return Header.RawFlags.To<T>();
            }
        }
        
        public T LoadAddress
        {
            get
            {
                return Header.LoadAddress.To<T>();
            }
        }

        public T Alignment
        {
            get
            {
                return Header.Alignment.To<T>();
            }
        }

        public T EntrySize
        {
            get
            {
                return Header.EntrySize.To<T>();
            }
        }
        
        public T Size
        {
            get
            {
                return Header.Size.To<T>();
            }
        }

        // TODO: should be protected
        public SectionHeader Header { get; private set; }

        private readonly Func<EndianBinaryReader> readerSourceSourceSource;
    }
}

