using System.IO;
using MiscUtil.IO;

namespace ELFSharp
{
    public class SectionHeader
    {
        // TODO: make elf consts file with things like SHT_LOUSER
        internal SectionHeader(EndianBinaryReader reader, StringTable table = null)
        {
            this.reader = reader;
            this.table = table;
            ReadSectionHeader();
        }

        public string Name { get; private set; }
        public uint NameIndex { get; private set; }
        public SectionType Type { get; private set; }
        public uint Flags { get; private set; }
        public uint LoadAddress { get; private set; }
		public uint Size { get; private set; }
		
		internal uint Offset { get; private set; }
		
		public override string ToString()
		{
			return string.Format("{0}: {2}, load @0x{4:X}, {5} bytes long", Name, NameIndex, Type, Flags, LoadAddress, Size);
		}

        private void ReadSectionHeader()
        {
            NameIndex = reader.ReadUInt32();
            if(table != null)
            {
                Name = table[NameIndex];
            }
            Type = (SectionType) reader.ReadUInt32();
            Flags = reader.ReadUInt32();
            LoadAddress = reader.ReadUInt32();
            Offset = reader.ReadUInt32();
            Size = reader.ReadUInt32();
            reader.ReadUInt32(); // TODO: sh_link
            // TODO: pozostale elementy naglowka sekcji
        }

        private readonly EndianBinaryReader reader;
        private StringTable table;
    }
}