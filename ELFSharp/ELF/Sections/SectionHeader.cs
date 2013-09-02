using MiscUtil.IO;

namespace ELFSharp.ELF.Sections
{
    internal sealed class SectionHeader
    {
        // TODO: make elf consts file with things like SHT_LOUSER
        internal SectionHeader(EndianBinaryReader reader, Class elfClass, IStringTable table = null)
        {
            this.reader = reader;
            this.table = table;
			this.elfClass = elfClass;
            ReadSectionHeader();
        }

        internal string Name { get; private set; }
        internal uint NameIndex { get; private set; }
        internal SectionType Type { get; private set; }
        internal SectionFlags Flags { get; private set; }
        internal ulong RawFlags { get; private set; }
        internal ulong LoadAddress { get; private set; }
        internal ulong Alignment { get; private set; }
        internal ulong EntrySize { get; private set; }
        internal long Size { get; private set; }
		internal long Offset { get; private set; }
        internal uint Link { get; private set; }
        internal uint Info { get; private set; }
		
		public override string ToString()
		{
			return string.Format("{0}: {2}, load @0x{4:X}, {5} bytes long", Name, NameIndex, Type, RawFlags, LoadAddress, Size);
		}

        private void ReadSectionHeader()
        {
            NameIndex = reader.ReadUInt32();
            if(table != null)
            {
                Name = table[NameIndex];
            }
            Type = (SectionType)reader.ReadUInt32();
            RawFlags = ReadAddress();
            Flags = unchecked((SectionFlags)RawFlags);
            LoadAddress = ReadAddress();
            Offset = ReadOffset();
            Size = ReadOffset();
            Link = reader.ReadUInt32();
            Info = reader.ReadUInt32();
            Alignment = ReadAddress();
            EntrySize = ReadAddress();
        }

        private ulong ReadAddress()
        {
            return elfClass == Class.Bit32 ? reader.ReadUInt32() : reader.ReadUInt64();
        }

        private long ReadOffset()
        {
            return elfClass == Class.Bit32 ? reader.ReadUInt32() : reader.ReadInt64();
        }

        private readonly EndianBinaryReader reader;
        private IStringTable table;
		private readonly Class elfClass;
    }
	
}