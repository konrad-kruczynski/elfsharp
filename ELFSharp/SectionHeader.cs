using System.IO;
using MiscUtil.IO;
using System;

namespace ELFSharp
{
    public abstract class SectionHeader
    {
        // TODO: make elf consts file with things like SHT_LOUSER
        internal SectionHeader(EndianBinaryReader reader, Class elfClass, StringTable table = null)
        {
            this.reader = reader;
            this.table = table;
			this.elfClass = elfClass;
            ReadSectionHeader();
        }

        public string Name { get; private set; }
        public uint NameIndex { get; private set; }
        public SectionType Type { get; private set; }        
		
		internal long Offset { get; private set; }
		
		protected ulong FlagsLong { get; private set; }
        protected ulong LoadAddressLong { get; private set; }
		internal long SizeLong { get; private set; }
		
		public override string ToString()
		{
			return string.Format("{0}: {2}, load @0x{4:X}, {5} bytes long", Name, NameIndex, Type, FlagsLong, LoadAddressLong, SizeLong);
		}

        private void ReadSectionHeader()
        {
            NameIndex = reader.ReadUInt32();
            if(table != null)
            {
                Name = table[NameIndex];
            }
            Type = (SectionType) reader.ReadUInt32();
            FlagsLong = elfClass == Class.Bit32 ? reader.ReadUInt32() : reader.ReadUInt64();
            LoadAddressLong = elfClass == Class.Bit32 ? reader.ReadUInt32() : reader.ReadUInt64();
            Offset = elfClass == Class.Bit32 ? reader.ReadUInt32() : reader.ReadInt64();
            SizeLong = elfClass == Class.Bit32 ? reader.ReadUInt32() : reader.ReadInt64();
            // TODO: remaining section header items
        }

        private readonly EndianBinaryReader reader;
        private StringTable table;
		private readonly Class elfClass;
    }
	
}