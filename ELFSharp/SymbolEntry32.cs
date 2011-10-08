using System;

namespace ELFSharp
{
	public class SymbolEntry32 : SymbolEntry
	{
		internal SymbolEntry32(string name, ulong value, ulong size, SymbolBinding binding, SymbolType type, ELF elf, ushort sectionIdx) : base(name, value, size, binding, type, elf, sectionIdx)
		{
			
		}
		
		public UInt32 Value
		{
			get
			{
				return (uint) LongValue;
			}
		}
		
		public UInt32 Size
		{
			get
			{
				return (uint) LongSize;
			}
		}
	}
}

