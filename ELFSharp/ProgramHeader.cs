using System;
using MiscUtil.IO;
using System.IO;

namespace ELFSharp
{
	public abstract class ProgramHeader
	{
		internal ProgramHeader(long headerOffset, Class elfClass, Func<EndianBinaryReader> readerSource)
        {            
            this.readerSource = readerSource;			
			this.headerOffset = headerOffset;
			this.elfClass = elfClass;
			ReadHeader();
        }
		
		public ProgramHeaderType Type { get; private set; }
		protected ulong LongAddress { get; private set; }
		protected ulong LongPhysicalAddress { get; private set; }
		protected uint Flags { get; private set; }		
		protected ulong LongSize { get; private set; }
		
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
			var reader = ObtainReader(offset);
			var result = new byte[(int)LongSize];
			var fileImage = reader.ReadBytesOrThrow((int)fileSize);
			fileImage.CopyTo(result, 0);
			return result;
		}
		
		public override string ToString ()
		{
			return string.Format ("{2}: size {3}, @ 0x{0:X}", LongAddress, LongPhysicalAddress, Type, LongSize);
		}
		
		private void ReadHeader()
		{
			var reader = ObtainReader(offset); // TODO: using!
			
			Type = (ProgramHeaderType) reader.ReadUInt32();
			if(elfClass == Class.Bit64)
			{
				Flags = reader.ReadUInt32();
			}
			offset = elfClass == Class.Bit32 ? reader.ReadUInt32() : reader.ReadInt64();
			LongAddress = elfClass == Class.Bit32 ? reader.ReadUInt32() : reader.ReadUInt64();
			LongPhysicalAddress = elfClass == Class.Bit32 ? reader.ReadUInt32() : reader.ReadUInt64();
			fileSize = elfClass == Class.Bit32 ? reader.ReadUInt32() : reader.ReadUInt64();
			LongSize = elfClass == Class.Bit32 ? reader.ReadUInt32() : reader.ReadUInt64();
			// TODO: flags & alignment
		}
		
		protected EndianBinaryReader ObtainReader(long givenOffset)
		{
			var reader = readerSource();
			reader.BaseStream.Seek(givenOffset, SeekOrigin.Begin);
			return reader;
		}
		
		protected long headerOffset;
		protected Class elfClass;
		private long offset;		
		private ulong fileSize;
		private Func<EndianBinaryReader> readerSource;
		
	}
}

