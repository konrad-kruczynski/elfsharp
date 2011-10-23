using System;
using System.IO;
using MiscUtil.IO;
using ELFSharp;
using System.Text;

namespace ELFSharp
{
	internal class NoteData
	{
		internal string Name { get; private set; }
		internal string Description { get; private set; }
		internal long Type { get; private set; }
		
		internal NoteData(Class elfClass, long sectionOffset, Func<EndianBinaryReader> readerSource)
		{
			this.elfClass = elfClass;
			reader = readerSource();
			reader.BaseStream.Seek(sectionOffset, SeekOrigin.Begin);
			var nameSize = ReadField();
			var descriptionSize = ReadField();
			Type = ReadField();
			long remainder;
			var fields = Math.DivRem(nameSize, FieldSize, out remainder);
			var name = reader.ReadBytesOrThrow((int)nameSize - 1); // minus one to omit terminating NUL
			Name = Encoding.ASCII.GetString(name);
			var descriptionStart = FieldSize*(3 + remainder > 0 ? fields + 1 : fields);
			reader.BaseStream.Seek(sectionOffset + descriptionStart, SeekOrigin.Begin);
			var description = reader.ReadBytesOrThrow((int) descriptionSize - 1);
			Description = Encoding.ASCII.GetString(description);
		}
		
		private long ReadField()
		{
			return elfClass == Class.Bit32 ? reader.ReadUInt32() : reader.ReadInt64();
		}
				
		private int FieldSize
		{
			get
			{
				return elfClass == Class.Bit32 ? 4 : 8;
			}
		}
		
		private readonly Class elfClass;
		private readonly EndianBinaryReader reader;
	}
}
