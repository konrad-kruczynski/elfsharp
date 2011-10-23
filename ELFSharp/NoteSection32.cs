using System;
using System.IO;
using MiscUtil.IO;
using ELFSharp;
using System.Text;

namespace ELFSharp
{
	public class NoteSection32 : NoteSection, ISection32
	{
		public NoteSection32(SectionHeader32 header, Func<EndianBinaryReader> readerSource) : base(header, readerSource)
		{
			
		}
		
		public new SectionHeader32 Header 
		{
			get
			{
				return (SectionHeader32) base.Header;
			}
		}
		
		public uint Type
		{
			get
			{
				return unchecked(((uint)LongType));
			}
		}
	}
}
