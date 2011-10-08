using System;
using MiscUtil.IO;

namespace ELFSharp
{
	public class ProgramHeader64 : ProgramHeader
	{
		public ProgramHeader64(long headerOffset, Func<EndianBinaryReader> readerSource) : base(headerOffset, Class.Bit64, readerSource)
		{
			
		}
		
		public ulong Address 
		{ 
			get
			{
				return LongAddress;
			}
		}
		
		public ulong PhysicalAddress 
		{ 
			get
			{
				return LongPhysicalAddress;
			}
		}
		
		/// <summary>
		/// Size of the segment image in memory.
		/// </summary>
		public ulong Size
        { 
            get
            {
                return LongSize;
            }			
        }

        public ulong Alignment
        {
            get
            {
                return LongAlignment;
            }
        }
	}
}

