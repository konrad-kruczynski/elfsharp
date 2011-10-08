using System;

namespace ELFSharp
{
	public class SymbolEntry64 : SymbolEntry
	{
		internal SymbolEntry64(string name, ulong value, ulong size, SymbolBinding binding, SymbolType type, ELF elf, ushort sectionIdx) : base(name, value, size, binding, type, elf, sectionIdx)
		{
			
		}
		
		public UInt64 Value
		{
			get
			{
				return (uint) LongValue;
			}
		}
		
		public UInt64 Size
		{
			get
			{
				return (uint) LongSize;
			}
		}
	}
}

