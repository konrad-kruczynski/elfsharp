using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MiscUtil.IO;
using MiscUtil.Conversion;
using System.Collections.ObjectModel;

namespace ELFSharp
{
    public sealed class ELF<T> : IELF where T : struct
    {
        internal ELF(string fileName) : this()
        {
            inMemory = false;
            this.fileName = fileName;
            stream = GetNewStream();
        }

        internal ELF(Stream stream) : this()
        {
            inMemory = true;

        }

        private ELF()
        {
            CheckSize();
            ReadHeader();            
            ReadStringTable();
            ReadSections();
            ReadSegmentHeaders();
        }

        public Endianess Endianess { get; private set; }
        public Class Class { get; private set; }
        public FileType Type { get; private set; }
        public Machine Machine { get; private set; }
        public T EntryPoint { get; private set; }
        public T MachineFlags { get; private set; }
     
        public bool HasSegmentHeader
        {
            get { return segmentHeaderOffset != 0; }
        }
     
        public bool HasSectionHeader
        {
            get { return sectionHeaderOffset != 0; }
        }

        public bool HasSectionsStringTable
        {
            get { return stringTableIndex != 0; }
        }
     
        public IEnumerable<Segment<T>> Segments
        {
            get { return segments; }
        }

        IEnumerable<ISegment> IELF.Segments
        {
            get { return Segments; }
        }

        public IStringTable SectionsStringTable { get; private set; }

        public IEnumerable<Section<T>> Sections
        {
            get
            {
               return new ReadOnlyCollection<Section<T>>(sections);
            }
        }

        IEnumerable<S> IELF.GetSections<S>()
        {
            return Sections.Where(x => x is S).Cast<S>();
        }

        public IEnumerable<S> GetSections<S>() where S : Section<T>
        {
            return Sections.Where(x => x is S).Cast<S>();
        }

        IEnumerable<ISection> IELF.Sections
        {
            get
            {
                return Sections;
            }
        }

        public Section<T> GetSection(string name)
        {
            if(!HasSectionsStringTable)
            {
                throw new InvalidOperationException(
                    "Given ELF does not contain section header string table, therefore names of sections cannot be obtained.");
            }
            var index = sectionIndicesByName[name];
            if(index != -1)
            {
                return GetSection(index);
            }
            throw new InvalidOperationException("Given section name is not unique, order is ambigous.");
        }

        ISection IELF.GetSection(string name)
        {
            return GetSection(name);
        }

        public Section<T> GetSection(int index)
        {
            if(sections[index] != null)
            {
                return sections[index];
            }
            if(currentStage != Stage.Initalizing)
            {
                throw new InvalidOperationException(
                    "Assert not met: null section by proper index in not initializing stage.");
            }
            TouchSection(index);
            return sections[index];
        }

        ISection IELF.GetSection(int index)
        {
            return GetSection(index);
        }

        private Section<T> GetSectionFromSectionHeader(SectionHeader header)
        {
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
            return returned;
        }

        private FileStream GetNewStream()
        {
            return new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
        }
     
        private void ReadSegmentHeaders()
        {
            segments = new List<Segment<T>>(segmentHeaderEntryCount);
            for(var i = 0u; i < segmentHeaderEntryCount; i++)
            {
                var header = new Segment<T>(segmentHeaderOffset + i*segmentHeaderEntrySize, Class, readerSource);
                segments.Add(header);
            }
        }

        private void ReadSections()
        {
            sectionHeaders = new List<SectionHeader>();
            if(HasSectionsStringTable)
            {
                sectionIndicesByName = new Dictionary<string, int>();
            }
            for(var i = 0; i < sectionHeaderEntryCount; i++)
            {
                var header = ReadSectionHeader(i);
                sectionHeaders.Add(header);
                if(HasSectionsStringTable)
                {
                    var name = header.Name;
                    if(!sectionIndicesByName.ContainsKey(name))
                    {
                        sectionIndicesByName.Add(name, i);
                    }
                    else
                    {
                        sectionIndicesByName[name] = -1;
                    }
                }
            }
            sections = new List<Section<T>>(Enumerable.Repeat<Section<T>>(null, sectionHeaders.Count));
            FindObjectsStringTable();
            for(var i = 0; i < sectionHeaders.Count; i++)
            {
                TouchSection(i);
            }
            sectionHeaders = null;
            currentStage = Stage.AfterSectionsAreRead;
        }

        private void TouchSection(int index)
        {
            if(currentStage != Stage.Initalizing)
            {
                throw new InvalidOperationException("TouchSection invoked in improper state.");
            }
            if(sections[index] != null)
            {
                return;
            }
            var section = GetSectionFromSectionHeader(sectionHeaders[index]);
            sections[index] = section;
        }
     
        private void CheckClass()
        {
            var size = System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));
            if((size != 4 && Class == Class.Bit32)
                || (size != 8 && Class == Class.Bit64))
            {
                throw new InvalidOperationException("Bad class.");
            }
        }

        private void CheckSize()
        {
            var size = stream.Length;
            if(size < Consts.MinimalELFSize)
            {
                throw new ArgumentException(string.Format("Given ELF file is too short, has size {0}", size));
            }

        }

        private void FindObjectsStringTable()
        {
            // TODO: check if there is string table
            if(!sectionIndicesByName.ContainsKey(Consts.ObjectsStringTableName))
            {
                return;
            }
            var sectionIdx = sectionIndicesByName[Consts.ObjectsStringTableName];
            objectsStringTable = (StringTable<T>)GetSection(sectionIdx);
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
                segmentHeaderOffset = Class == Class.Bit32 ? reader.ReadUInt32() : reader.ReadInt64();
                sectionHeaderOffset = Class == Class.Bit32 ? reader.ReadUInt32() : reader.ReadInt64();
                MachineFlags = reader.ReadUInt32().To<T>(); // TODO: always 32bit?
                reader.ReadUInt16(); // elf header size
                segmentHeaderEntrySize = reader.ReadUInt16();
                segmentHeaderEntryCount = reader.ReadUInt16();
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

        private readonly Stream stream;
        private Int64 segmentHeaderOffset;
        private Int64 sectionHeaderOffset;
        private UInt16 segmentHeaderEntrySize;
        private UInt16 segmentHeaderEntryCount;
        private UInt16 sectionHeaderEntrySize;
        private UInt16 sectionHeaderEntryCount;
        private UInt16 stringTableIndex;
        private List<Segment<T>> segments;
        private List<Section<T>> sections;
        private Dictionary<string, int> sectionIndicesByName;
        private List<SectionHeader> sectionHeaders;
        private StringTable<T> objectsStringTable;
        private Func<EndianBinaryReader> readerSource;
        private Func<EndianBinaryReader> localReaderSource;
        private Stage currentStage;
        private readonly string fileName;
        private readonly bool inMemory;
        private readonly byte[] contents;
        private static readonly byte[] Magic = new byte[] { 0x7F, 0x45, 0x4C, 0x46 }; // 0x7F 'E' 'L' 'F'

        private enum Stage
        {
            Initalizing,
            AfterSectionsAreRead
        }
    }
}

