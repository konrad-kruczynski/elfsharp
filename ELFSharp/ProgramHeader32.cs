using System;
using MiscUtil.IO;

namespace ELFSharp
{
	public sealed class ProgramHeader32 : ProgramHeader
	{
		public ProgramHeader32(long headerOffset, Func<EndianBinaryReader> readerSource) : base(headerOffset, Class.Bit32, readerSource)
		{
			
		}
		
		public uint Address 
		{ 
			get
			{
				return (uint) LongAddress;
			}
		}
		
		public uint PhysicalAddress 
		{ 
			get
			{
				return (uint) LongPhysicalAddress;
			}
		}
		
		/// <summary>
		/// Size of the segment image in memory.
		/// </summary>
		public uint Size 
		{ 
			get
			{
				return (uint) LongSize;
			}			
		}
	}
}

