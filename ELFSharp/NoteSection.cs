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
		
		public byte[] Description
		{
			get
			{
				return data.Description;
			}
		}

        public override string ToString()
        {
            return string.Format("[{0}: {1}, Type={2}]", Name, Description, LongType);
        }
		
		internal ulong LongType
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