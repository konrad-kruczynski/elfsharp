using System;
using System.Collections.Generic;
using MiscUtil.IO;

namespace ELFSharp
{
	public class SymbolTable32 : SymbolTable
	{
		public SymbolTable32(SectionHeader header, Func<EndianBinaryReader> readerSource, StringTable table, ELF elf) : base(header, readerSource, table, elf)
		{
			
		}
		
		public new IEnumerable<SymbolEntry32> Entries
        {
            get { return entries32; }
        }
	}
}

