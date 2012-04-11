using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MiscUtil.IO;
using MiscUtil.Conversion;

namespace ELFSharp
{
    public sealed class ELF<T> where T : struct
    {
     
        internal ELF(string fileName)
        {
            sectionCache = new Dictionary<int, WeakReference>();
            this.fileName = fileName;
            stream = GetNewStream();
            CheckSize();
            ReadHeader();            
            ReadStringTable();
            ReadSectionHeaders();
            ReadProgramHeaders();
            FindObjectsStringTable();
        }

        public Endianess Endianess { get; private set; }

        public Class Class { get; private set; }

        public FileType Type { get; private set; }

        public Machine Machine { get; private set; }
     
        public T EntryPoint { get; private set; }

        public T MachineFlags { get; private set; }
     
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
     
        public IEnumerable<ProgramHeader<T>> ProgramHeaders
        {
            get { return programHeaders; }
        }

        public IStringTable SectionsStringTable { get; private set; }

        // TODO: iface
        public IEnumerable<Section<T>> GetSections()
        {
            var i = 0;
            while(i < sectionHeaders.Count)
            {
                yield return GetSection(i);
                i++;
            }
        }

        public IEnumerable<S> GetSections<S>() where S : Section<T>
        {
            return GetSections().Where(x => x != null && x is S).Cast<S>();
        }

        public Section<T> GetSection(string name)
        {
            if(!HasSectionsStringTable)
            {
                throw new InvalidOperationException(
                    "Given ELF does not contain section header string table, therefore names of sections cannot be obtained.");
            }
            var sectionNo = sectionsByName[name];
            if(sectionNo != -1)
            {
                return GetSection(sectionNo);
            }
            throw new InvalidOperationException("Given section name is not unique, order is ambigous.");
        }

        public Section<T> GetSection(int index)
        {
            if(sectionCache.ContainsKey(index))
            {
                var section = (Section<T>)sectionCache[index].Target;
                if(section != null)
                {
                    return section;
                }
                else
                {
                    sectionCache.Remove(index);
                }
            }
            var header = sectionHeaders[index];
            Section<T> returned;
            switch(header.Type)
            {
                case SectionType.Null:
                    goto default;
                case SectionType.ProgBits:
                    returned = new ProgBitsSection<T>(header, readerSource);
                    break;
                case SectionType.SymbolTable:
                    returned = new SymbolTable<T>(header, readerSource, objectsStringTable, this);
                    break;
                case SectionType.StringTable:
                    returned = new StringTable<T>(header, readerSource);
                    break;
                case SectionType.RelocationAddends:
                    goto default;
                case SectionType.HashTable:
                    goto default;
                case SectionType.Dynamic:
                    goto default;                    
                case SectionType.Note:
                    returned = new NoteSection<T>(header, Class, readerSource);
                    break;
                case SectionType.NoBits:
                    goto default;
                case SectionType.Relocation:
                    goto default;
                case SectionType.Shlib:
                    goto default;
                case SectionType.DynamicSymbolTable:
                    returned = new SymbolTable<T>(header, readerSource, (IStringTable)GetSection(".dynstr"), this);
                    break;
                default:
                    returned = new Section<T>(header, readerSource);
                    break;
            }
            sectionCache.Add(index, new WeakReference(returned));
            return returned;
        }

        private FileStream GetNewStream()
        {
            return new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
        }
     
        private void ReadProgramHeaders()
        {
            programHeaders = new List<ProgramHeader<T>>(programHeaderEntryCount);
            for(var i = 0u; i < programHeaderEntryCount; i++)
            {
                var header = new ProgramHeader<T>(programHeaderOffset + i*programHeaderEntrySize, Class, readerSource);
                programHeaders.Add(header);
            }
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
                    if(!sectionsByName.ContainsKey(header.Name))
                    {
                        sectionsByName.Add(header.Name, i);
                    }
                    else
                    {
                        sectionsByName[header.Name] = -1;
                    }
                }
            }
        }
     
        private void CheckClass()
        {
            // TODO
            if((typeof(T) != typeof(uint) && Class == Class.Bit32)
                || (typeof(T) != typeof(long) && Class == Class.Bit64))
            {
                throw new InvalidOperationException("Bad class.");
            }
        }

        private void CheckSize()
        {
            var size = stream.Length < 16;
            if(size)
            {
                throw new ArgumentException(string.Format("Given ELF file is too short, has size {0}", size));
            }

        }

        private void FindObjectsStringTable()
        {
            var header = sectionHeaders.FirstOrDefault(x => x.Name == Consts.ObjectsStringTableName);
            if(header != null)
            {
                objectsStringTable = new StringTable<T>(header, readerSource);
            }
        }

        private void ReadStringTable()
        {
            if(!HasSectionHeader || !HasSectionsStringTable)
            {                
                return;
            }
            var header = ReadSectionHeader(stringTableIndex);
            if(header.Type != SectionType.StringTable)
            {
                throw new InvalidOperationException("Given index of section header does not point at string table which was expected.");
            }
            SectionsStringTable = new StringTable<T>(header, readerSource);
        }

        private SectionHeader ReadSectionHeader(int index)
        {
            if(index < 0 || index >= sectionHeaderEntryCount)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            stream.Seek(sectionHeaderOffset + index*sectionHeaderEntrySize, SeekOrigin.Begin);
            using(var reader = localReaderSource())
            {
                return new SectionHeader(reader, Class, SectionsStringTable);
            }
        }

        private void ReadHeader()
        {
            ReadIdentificator();
            EndianBitConverter converter;
            if(Endianess == Endianess.LittleEndian)
            {
                converter = new LittleEndianBitConverter();
            }
            else
            {
                converter = new BigEndianBitConverter();
            }
            readerSource = () => new EndianBinaryReader(converter, GetNewStream());
            localReaderSource = () => new EndianBinaryReader(converter, 
             new NonClosingStreamWrapper(stream));
            CheckClass();
            ReadFields();
        }

        private void ReadFields()
        {
            using(var reader = localReaderSource())
            {
                Type = (FileType)reader.ReadUInt16();
                Machine = (Machine)reader.ReadUInt16();
                var version = reader.ReadUInt32();
                if(version != 1)
                {
                    throw new ArgumentException(string.Format("Given ELF file is of unknown version {0}.", version));
                }
                EntryPoint = (Class == Class.Bit32 ? reader.ReadUInt32() : reader.ReadUInt64()).To<T>();
                // TODO: assertions for (u)longs
                programHeaderOffset = Class == Class.Bit32 ? reader.ReadUInt32() : reader.ReadInt64();
                sectionHeaderOffset = Class == Class.Bit32 ? reader.ReadUInt32() : reader.ReadInt64();
                MachineFlags = reader.ReadUInt32().To<T>(); // TODO: always 32bit?
                reader.ReadUInt16(); // elf header size
                programHeaderEntrySize = reader.ReadUInt16();
                programHeaderEntryCount = reader.ReadUInt16();
                sectionHeaderEntrySize = reader.ReadUInt16();
                sectionHeaderEntryCount = reader.ReadUInt16();
                stringTableIndex = reader.ReadUInt16();
            }
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
            reader.ReadBytes(10); // padding bytes of section e_ident
        }

        private readonly FileStream stream;
        private Int64 programHeaderOffset;
        private Int64 sectionHeaderOffset;
        private UInt16 programHeaderEntrySize;
        private UInt16 programHeaderEntryCount;
        private UInt16 sectionHeaderEntrySize;
        private UInt16 sectionHeaderEntryCount;
        private UInt16 stringTableIndex;
        private List<SectionHeader> sectionHeaders;
        private List<ProgramHeader<T>> programHeaders;
        private Dictionary<string, int> sectionsByName;
        private StringTable<T> objectsStringTable;
        private Func<EndianBinaryReader> readerSource;
        private Func<EndianBinaryReader> localReaderSource;
        private Dictionary<int, WeakReference> sectionCache;
        private readonly string fileName;
        private static readonly byte[] Magic = new byte[] { 0x7F, 0x45, 0x4C, 0x46 }; // 0x7F 'E' 'L' 'F'
    }
}

