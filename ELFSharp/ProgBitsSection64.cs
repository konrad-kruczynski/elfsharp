using System;
using MiscUtil.IO;

namespace ELFSharp
{
	public class ProgBitsSection64 : ProgBitsSection, ISection64
	{
		public new SectionHeader64 Header 
		{
			get 
			{
				return (SectionHeader64) base.Header;
			}
		}
		
		internal ProgBitsSection64(SectionHeader64 header, Func<EndianBinaryReader> readerSource) : base(header, readerSource)
        {
        }
	}
}

