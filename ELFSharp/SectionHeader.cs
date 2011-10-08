using System.Linq;
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
        public SectionFlags Flags { get; private set; }

		internal long Offset { get; private set; }
        internal uint Link { get; private set; }
        internal uint Info { get; private set; }
		
		protected ulong FlagsLong { get; private set; }
        protected ulong LoadAddressLong { get; private set; }
        protected ulong AlignmentLong { get; private set; }
        protected ulong EntrySizeLong { get; private set; }
        
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
            Type = (SectionType)reader.ReadUInt32();
            FlagsLong = ReadAddress();
            Flags = unchecked((SectionFlags)FlagsLong);
            LoadAddressLong = ReadAddress();
            Offset = ReadOffset();
            SizeLong = ReadOffset();
            Link = reader.ReadUInt32();
            Info = reader.ReadUInt32();
            AlignmentLong = ReadAddress();
            EntrySizeLong = ReadAddress();
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
        private StringTable table;
		private readonly Class elfClass;
    }
	
}