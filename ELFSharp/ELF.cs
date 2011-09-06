using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ELFSharp
{
	public abstract class ELF
	{
		
		internal ELF(string fileName)
		{
			this.fileName = fileName;
            stream = GetNewStream();
            ReadHeader();
            readerSource = () => new BinaryReader(GetNewStream());
            ReadStringTable();
            ReadSectionHeaders();
            FindObjectsStringTable();			
		}
		
		public Endianess Endianess { get; private set; }
        public Class Class { get; private set; }
        public FileType Type { get; private set; }
        public Machine Machine { get; private set; }
		
		protected UInt64 EntryPointLong { get; private set; }
        protected UInt64 MachineFlagsLong { get; private set; }
		
		public bool HasProgramHeader
        {
            get { return programHeaderOffset != 0; }
        }
		
        public bool HasSectionHeader
        {
            get { return sectionHeaderOffset != 0; }
        }

        public bool HasSectionsStringTable
        {
            get { return stringTableIndex != 0; }
        }

        public IEnumerable<SectionHeader> SectionHeaders
        {
            get { return sectionHeaders; }
        }

        public StringTable SectionsStringTable { get; private set; }

        public IEnumerable<Section> GetSections()
        {
            var i = 0;
            while (i < sectionHeaders.Count)
            {
                yield return GetSection(i);
                i++;
            }
        }

        public IEnumerable<T> GetSections<T>() where T : Section
        {
            return GetSections().Where(x => x != null && x is T).Cast<T>();
        }

        public Section GetSection(string name)
        {
            if(!HasSectionsStringTable)
            {
                throw new InvalidOperationException(
                    "Given ELF does not contain section header string table, therefore names of sections cannot be obtained.");
            }
            return GetSection(sectionsByName[name]);
        }

        public Section GetSection(int index)
        {
            var header = sectionHeaders[index];
            // TODO: some kind of cache on weak references
            switch(header.Type)
            {
                case SectionType.Null:
                    break;
                case SectionType.ProgBits:
                    return new ProgBitsSection(header, readerSource);
                case SectionType.SymbolTable:
                    return new SymbolTable(header, readerSource, objectsStringTable, this);
                case SectionType.StringTable:
                    return new StringTable(header, readerSource);
                case SectionType.RelocationAddends:
                    break;
                case SectionType.HashTable:
                    break;
                case SectionType.Dynamic:
                    break;
                case SectionType.Note:
                    break;
                case SectionType.NoBits:
                    break;
                case SectionType.Relocation:
                    break;
                case SectionType.Shlib:
                    break;
                case SectionType.DynamicSymbolTable:
                    break;
                default:
                    return null;
            }
            return null; // TODO
        }

        private FileStream GetNewStream()
        {
            return new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        private void ReadSectionHeaders()
        {
            sectionHeaders = new List<SectionHeader>(sectionHeaderEntryCount);
            if(HasSectionsStringTable)
            {
                sectionsByName = new Dictionary<string, int>();
            }
            for(var i = 0; i < sectionHeaderEntryCount; i++)
            {
                var header = ReadSectionHeader(i);
                sectionHeaders.Add(header);
                if(HasSectionsStringTable)
                {
                    sectionsByName.Add(header.Name, i);
                }
            }
        }

        private void FindObjectsStringTable()
        {
            // TODO: const it
            var header = sectionHeaders.FirstOrDefault(x => x.Name == ".strtab");
            if(header != null)
            {
                objectsStringTable = new StringTable(header, readerSource);
            }
        }

        private void ReadStringTable()
        {
            if (!HasSectionHeader || !HasSectionsStringTable)
            {
                return;
            }
            var header = ReadSectionHeader(stringTableIndex);
            if (header.Type != SectionType.StringTable)
            {
                throw new InvalidOperationException("Given index of section header does not point at string table which was expected.");
            }
            SectionsStringTable = new StringTable(header, readerSource);
        }

        private SectionHeader ReadSectionHeader(int index)
        {
            if (index < 0 || index >= sectionHeaderEntryCount)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            stream.Seek(sectionHeaderOffset + index * sectionHeaderEntrySize, SeekOrigin.Begin);
            // TODO: dispose other binary readers
            var reader = new BinaryReader(stream);
            return new SectionHeader(reader, SectionsStringTable);
        }


        private void ReadHeader()
        {
            ReadIdentificator();
            ReadFields();
        }

        private void ReadFields()
        {
            // TODO: take care of endianess
            var reader = new BinaryReader(stream);
            Type = (FileType) reader.ReadUInt16();
            Machine = (Machine) reader.ReadUInt16();
            var version = reader.ReadUInt32();
            if(version != 1)
            {
                throw new ArgumentException(string.Format("Given ELF file is of unknown version {0}.", version));
            }
            EntryPointLong = Class == Class.Bit32 ? reader.ReadUInt32() : reader.ReadUInt64();
			// TODO: assertions for (u)longs
            programHeaderOffset = Class == Class.Bit32 ? reader.ReadUInt32() : reader.ReadInt64();
            sectionHeaderOffset = Class == Class.Bit32 ? reader.ReadUInt32() : reader.ReadInt64();
            MachineFlagsLong = Class == Class.Bit32 ? reader.ReadUInt32() : reader.ReadUInt64();
            elfHeaderSize = reader.ReadUInt16();
            programHeaderEntrySize = reader.ReadUInt16();
            programHeaderEntryCount = reader.ReadUInt16();
            sectionHeaderEntrySize = reader.ReadUInt16();
            sectionHeaderEntryCount = reader.ReadUInt16();
            stringTableIndex = reader.ReadUInt16();
        }

        private void ReadIdentificator()
        {
            var reader = new BinaryReader(stream);
            var magic = reader.ReadBytes(4);
            for(var i = 0; i < 4; i++)
            {
                if(magic[i] != Magic[i])
                {
                    throw new ArgumentException("Given file is not proper ELF binary.");
                }
            }
            var classByte = reader.ReadByte();
            switch(classByte)
            {
                case 1:
                    Class = Class.Bit32;
                    break;
                case 2:
                    Class = Class.Bit64;
					break;
                default:
                    throw new ArgumentException(string.Format("Given ELF file is of unknown class {0}.", classByte));
            }
            var endianessByte = reader.ReadByte();
            switch(endianessByte)
            {
                case 1:
                    Endianess = Endianess.LittleEndian;
                    break;
                case 2:
                    Endianess = Endianess.BigEndian;
                    break;
                default:
                    throw new ArgumentException(string.Format("Given ELF file uses unknown endianess {0}.", endianessByte));
            }
			if((Endianess == Endianess.BigEndian && BitConverter.IsLittleEndian)
			   || (Endianess == Endianess.LittleEndian && !BitConverter.IsLittleEndian))
			{
				throw new ArgumentException("Given ELF is of different endianess than your machine.");
			}
            reader.ReadBytes(10); // padding bytes of section e_ident
        }

        private readonly FileStream stream;
        private Int64 programHeaderOffset;
        private Int64 sectionHeaderOffset;
        private UInt16 elfHeaderSize;
        private UInt16 programHeaderEntrySize;
        private UInt16 programHeaderEntryCount;
        private UInt16 sectionHeaderEntrySize;
        private UInt16 sectionHeaderEntryCount;
        private UInt16 stringTableIndex;
        private List<SectionHeader> sectionHeaders;
        private Dictionary<string, int> sectionsByName;
        private StringTable objectsStringTable;
        private readonly string fileName;
        private readonly Func<BinaryReader> readerSource;

        private static readonly byte[] Magic = new byte[] { 0x7F, 0x45, 0x4C, 0x46 }; // 0x7F 'E' 'L' 'F'
	}
}

