using System;
using System.IO;
using MiscUtil.IO;
using ELFSharp;
using System.Text;

namespace ELFSharp
{
    public abstract class NoteSection : Section
    {
		public string Name
		{
			get
			{
				return data.Name;
			}
		}
		
		public string Description
		{
			get
			{
				return data.Description;
			}
		}
		
		internal long LongType
		{
			get
			{
				return data.Type;
			}
		}
		
        internal NoteSection(SectionHeader header, Func<EndianBinaryReader> readerSource) : base(header, readerSource)
        {
			data = new NoteData(header.ElfClass, header.Offset, readerSource);
        }
		
		private readonly NoteData data;
    }	

}