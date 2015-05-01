using System;
using System.IO;
using MiscUtil.IO;

namespace ELFSharp.ELF.Sections
{
    public class Section<T> : ISection where T : struct
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

        private readonly Func<EndianBinaryReader> readerSourceSourceSource;
    }
}

