using System;
using System.IO;
using MiscUtil.IO;
using ELFSharp;
using System.Text;

namespace ELFSharp
{
	public class NoteSection64 : NoteSection, ISection64
	{
		public NoteSection64(SectionHeader64 header, Func<EndianBinaryReader> readerSource) : base(header, readerSource)
		{
			
		}
		
		public new SectionHeader64 Header
		{
			get
			{
				return (SectionHeader64) base.Header;
			}
		}
		
		public long Type
		{
			get
			{
				return LongType;
			}
		}
	}
}
