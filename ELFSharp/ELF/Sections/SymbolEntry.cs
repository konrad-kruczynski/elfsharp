using System;

namespace ELFSharp.ELF.Sections
{
	public class SymbolEntry<T> : ISymbolEntry where T : struct
	{
		public string Name { get; private set; }

		public SymbolBinding Binding { get; private set; }

		public SymbolType Type { get; private set; }

		public T Value { get; private set; }

		public T Size { get; private set; }

		public bool IsPointedIndexSpecial
		{
			get { return Enum.IsDefined(typeof(SpecialSectionIndex), sectionIdx); }
		}

		public Section<T> PointedSection
		{
			get { return IsPointedIndexSpecial ? null : elf.GetSection(sectionIdx); }
		}

		ISection ISymbolEntry.PointedSection
		{
			get { return PointedSection; }
		}

		public ushort PointedSectionIndex
		{ 
			get { return sectionIdx; }
		}

		public SpecialSectionIndex SpecialPointedSectionIndex
		{
			get
			{
				if(IsPointedIndexSpecial)
				{
					return (SpecialSectionIndex)sectionIdx;
				}
				throw new InvalidOperationException("Given pointed section index does not have special meaning.");
			}
		}

		public override string ToString()
		{
			return string.Format("[{3} {4} {0}: 0x{1:X}, size: {2}, section idx: {5}]",
                                 Name, Value, Size, Binding, Type, (SpecialSectionIndex)sectionIdx);
		}

		public SymbolEntry(string name, T value, T size, SymbolBinding binding, SymbolType type, ELF<T> elf, ushort sectionIdx)
		{
			Name = name;
			Value = value;
			Size = size;
			Binding = binding;
			Type = type;
			this.elf = elf;
			this.sectionIdx = sectionIdx;
		}

		private readonly ELF<T> elf;
		private readonly ushort sectionIdx;
	}
}