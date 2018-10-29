using System;
using System.IO;
using ELFSharp.Utilities;

namespace ELFSharp.ELF.Segments
{
    public sealed class Segment<T> : ISegment
    {
        internal Segment(long headerOffset, Class elfClass, Func<SimpleEndianessAwareReader> readerSource)
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

        public long Offset 
        {
            get 
            {
                return offset;
            }
        }

        /// <summary>
        /// Returns content of the section as it is given in the file.
        /// Note that it may be an array of length 0.
        /// </summary>
        /// <returns>Segment contents as byte array.</returns>
        public byte[] GetFileContents()
        {
            if(FileSize == 0)
            {
                return new byte[0];
            }
            using(var reader = ObtainReader(offset))
            {
                var result = new byte[checked((int)FileSize)];
                var fileImage = reader.ReadBytes(result.Length);
                fileImage.CopyTo(result, 0);
                return result;
            }
        }

        /// <summary>
        /// Returns content of the section, possibly padded or truncated to the memory size.
        /// Note that it may be an array of length 0.
        /// </summary>
        /// <returns>Segment image as a byte array.</returns>
        public byte[] GetMemoryContents()
        {
            var sizeAsInt = Size.To<int>();
            if(sizeAsInt == 0)
            {
                return new byte[0];
            }
            using(var reader = ObtainReader(offset))
            {
                var result = new byte[sizeAsInt];
                var fileImage = reader.ReadBytes(Math.Min(result.Length, checked((int)FileSize)));
                fileImage.CopyTo(result, 0);
                return result;
            }
        }

        public byte[] GetRawHeader()
        {
            using(var reader = ObtainReader(headerOffset))
            {
                return reader.ReadBytes(elfClass == Class.Bit32 ? 32 : 56);
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

        private SimpleEndianessAwareReader ObtainReader(long givenOffset)
        {
            var reader = readerSource();
            reader.BaseStream.Seek(givenOffset, SeekOrigin.Begin);
            return reader;
        }

        private long headerOffset;
        private Class elfClass;
        private long offset;
        private Func<SimpleEndianessAwareReader> readerSource;
    }
}

