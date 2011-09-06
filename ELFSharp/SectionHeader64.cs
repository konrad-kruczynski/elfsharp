using System;
using System.IO;

namespace ELFSharp
{
	public class SectionHeader64 : SectionHeader
	{
		internal SectionHeader64(BinaryReader reader, StringTable table = null) : base(reader, Class.Bit32, table)
        {
            
        }
		
		public ulong Flags
		{
			get
			{
				return FlagsLong;
			}
		}
		
		public ulong LoadAddress
		{
			get
			{
				return LoadAddressLong;
			}
		}
		
		public long Size
		{
			get
			{
				return SizeLong;
			}
		}
		
	}
}

