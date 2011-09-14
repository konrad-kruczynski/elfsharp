using System;
using MiscUtil.IO;
using System.IO;

namespace ELFSharp
{
	public sealed class ProgramHeader
	{
		internal ProgramHeader(uint headerOffset, Func<EndianBinaryReader> readerSource)
        {            
            this.readerSource = readerSource;			
			this.headerOffset = headerOffset;
			ReadHeader();
        }
		
		public uint Address { get; private set; }
		public uint PhysicalAddress { get; private set; }
		public ProgramHeaderType Type { get; private set; }
		
		/// <summary>
		/// Size of the segment image in memory.
		/// </summary>
		public uint Size { get; private set; }
		
		/// <summary>
		/// Gets array containing complete segment image, including
		/// the zeroed section.
		/// </summary>
		/// <returns>
		/// Segment image as array.
		/// </returns>
		public byte[] GetContents()
		{
			var reader = ObtainReader(offset);
			var result = new byte[Size];
			var fileImage = reader.ReadBytesOrThrow((int)fileSize);
			fileImage.CopyTo(result, 0);
			return result;
		}
		
		public override string ToString ()
		{
			return string.Format ("{2}: size {3}, @ 0x{0:X}", Address, PhysicalAddress, Type, Size);
		}
		
		private void ReadHeader()
		{
			var reader = ObtainReader(headerOffset);
			Type = (ProgramHeaderType) reader.ReadUInt32();
			offset = reader.ReadUInt32();
			Address = reader.ReadUInt32();
			PhysicalAddress = reader.ReadUInt32();
			fileSize = reader.ReadUInt32();
			Size = reader.ReadUInt32();
			// TODO: flags & alignment
		}	
		
		private EndianBinaryReader ObtainReader(uint givenOffset)
		{
			var reader = readerSource();
			reader.BaseStream.Seek(givenOffset, SeekOrigin.Begin);
			return reader;
		}
		
		private uint offset;
		private uint headerOffset;
		private uint fileSize;
		private Func<EndianBinaryReader> readerSource;
		
	}
}

