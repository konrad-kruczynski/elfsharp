using System;
using MiscUtil.IO;
using System.IO;

namespace ELFSharp.ELF.Segments
{
    public sealed class Segment<T> : ISegment
    {
        internal Segment(long headerOffset, Class elfClass, Func<EndianBinaryReader> readerSource)
        {            
            this.readerSource = readerSource;			
            this.headerOffset = headerOffset;
            this.elfClass = elfClass;
            ReadHeader();
        }

        public SegmentType Type { get; private set; }

        public SegmentFlags Flags { get; private set; }

        public T Address { get; private set; }

        public T PhysicalAddress { get; private set; }

        public T Size { get; private set; }

        public T Alignment { get; private set; }

        public long FileSize { get; private set; }

        /// <summary>
        /// Gets array containing complete segment image, including
        /// the zeroed section.
        /// </summary>
        /// <returns>
        /// Segment image as array.
        /// </returns>
        public byte[] GetContents()
        {
            // TODO: large segments
            using(var reader = ObtainReader(offset))
            {
                var result = new byte[Size.To<int>()];
                var fileImage = reader.ReadBytesOrThrow(checked((int)FileSize));
                fileImage.CopyTo(result, 0);
                return result;
            }
        }

        public override string ToString()
        {
            return string.Format("{2}: size {3}, @ 0x{0:X}", Address, PhysicalAddress, Type, Size);
        }

        private void ReadHeader()
        {
            using(var reader = ObtainReader(headerOffset))
            {
                Type = (SegmentType)reader.ReadUInt32();
                if(elfClass == Class.Bit64)
                {
                    Flags = (SegmentFlags)reader.ReadUInt32();
                }
                // TODO: some functions?s
                offset = elfClass == Class.Bit32 ? reader.ReadUInt32() : reader.ReadInt64();
                Address = (elfClass == Class.Bit32 ? reader.ReadUInt32() : reader.ReadUInt64()).To<T>();
                PhysicalAddress = (elfClass == Class.Bit32 ? reader.ReadUInt32() : reader.ReadUInt64()).To<T>();
                FileSize = elfClass == Class.Bit32 ? reader.ReadInt32() : reader.ReadInt64();
                Size = (elfClass == Class.Bit32 ? reader.ReadUInt32() : reader.ReadUInt64()).To<T>();
                if(elfClass == Class.Bit32)
                {
                    Flags = (SegmentFlags)reader.ReadUInt32();
                }
                Alignment = (elfClass == Class.Bit32 ? reader.ReadUInt32() : reader.ReadUInt64()).To<T>();
            }
        }

        private EndianBinaryReader ObtainReader(long givenOffset)
        {
            var reader = readerSource();
            reader.BaseStream.Seek(givenOffset, SeekOrigin.Begin);
            return reader;
        }

        private long headerOffset;
        private Class elfClass;
        private long offset;
        private Func<EndianBinaryReader> readerSource;
    }
}

