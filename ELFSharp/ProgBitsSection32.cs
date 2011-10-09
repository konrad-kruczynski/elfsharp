using System;
using MiscUtil.IO;

namespace ELFSharp
{
	public class ProgBitsSection32 : ProgBitsSection, ISection32
	{
		public new SectionHeader32 Header 
		{
			get 
			{
				return (SectionHeader32) base.Header;
			}
		}
		
		internal ProgBitsSection32(SectionHeader32 header, Func<EndianBinaryReader> readerSource) : base(header, readerSource)
        {
        }
	}
}

