using System;
using System.IO;
using ELFSharp.Utilities;

namespace ELFSharp.ELF.Sections
{
    public class Section<T> : ISection where T : struct
    {
        internal Section(SectionHeader header, Func<SimpleEndianessAwareReader> readerSource)
        {
            Header = header;
            this.readerSource = readerSource;
        }

        public virtual byte[] GetContents()
        {
            using(var reader = ObtainReader())
            {
                return reader.ReadBytes(Convert.ToInt32(Header.Size));
            }
        }

        protected SimpleEndianessAwareReader ObtainReader()
        {
            var reader = readerSource();
            reader.BaseStream.Seek((long)Header.Offset, SeekOrigin.Begin);
            return reader;
        }

        public string Name
        {
            get
            {
                return Header.Name;
            }
        }

        public uint NameIndex
        {
            get
            {
                return Header.NameIndex;
            }
        }

        public SectionType Type
        {
            get
            {
                return Header.Type;
            }
        }

        public SectionFlags Flags
        {
            get
            {
                return Header.Flags;
            }
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

        public T Offset
        {
            get
            {
                return Header.Offset.To<T>();
            }
        }

		public override string ToString()
		{
			return Header.ToString();
		}

        internal SectionHeader Header { get; private set; }

        private readonly Func<SimpleEndianessAwareReader> readerSource;
    }
}

