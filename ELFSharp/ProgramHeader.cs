using System;
using MiscUtil.IO;
using System.IO;

namespace ELFSharp
{
	public sealed class ProgramHeader<T> : IProgramHeader
	{
		internal ProgramHeader(long headerOffset, Class elfClass, Func<EndianBinaryReader> readerSource)
        {            
            this.readerSource = readerSource;			
			this.headerOffset = headerOffset;
			this.elfClass = elfClass;
			ReadHeader();
        }
		
		public ProgramHeaderType Type { get; private set; }
        public ProgramHeaderFlags Flags { get; private set; }
		public T Address { get; private set; }
		public T PhysicalAddress { get; private set; }
		public T Size { get; private set; }
        public T Alignment { get; private set; }
		
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
				var fileImage = reader.ReadBytesOrThrow((int)fileSize);
				fileImage.CopyTo(result, 0);
				return result;
			}
		}
		
		public override string ToString ()
		{
			return string.Format ("{2}: size {3}, @ 0x{0:X}", Address, PhysicalAddress, Type, Size);
		}
		
		private void ReadHeader()
        {
            using(var reader = ObtainReader(headerOffset))
            {
                Type = (ProgramHeaderType)reader.ReadUInt32();
                if(elfClass == Class.Bit64)
                {
                    Flags = (ProgramHeaderFlags)reader.ReadUInt32();
                }
                // TODO: some functions?s
                offset = elfClass == Class.Bit32 ? reader.ReadUInt32() : reader.ReadInt64();
                Address = (elfClass == Class.Bit32 ? reader.ReadUInt32() : reader.ReadUInt64()).To<T>();
                PhysicalAddress = (elfClass == Class.Bit32 ? reader.ReadUInt32() : reader.ReadUInt64()).To<T>();
                fileSize = elfClass == Class.Bit32 ? reader.ReadUInt32() : reader.ReadUInt64();
                Size = (elfClass == Class.Bit32 ? reader.ReadUInt32() : reader.ReadUInt64()).To<T>();
                if(elfClass == Class.Bit32)
                {
                    Flags = (ProgramHeaderFlags)reader.ReadUInt32();
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
		private ulong fileSize;
		private Func<EndianBinaryReader> readerSource;
		
	}
}

