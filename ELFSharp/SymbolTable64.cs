using System;
using MiscUtil.IO;
using System.Collections.Generic;

namespace ELFSharp
{
	public class SymbolTable64 : SymbolTable
	{
		public SymbolTable64(SectionHeader header, Func<EndianBinaryReader> readerSource, StringTable table, ELF elf) : base(header, readerSource, table, elf)
		{
			
		}
		
		public new IEnumerable<SymbolEntry64> Entries
        {
            get { return entries64; }
        }
	}
}

