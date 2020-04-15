using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using ELFSharp.Utilities;

namespace ELFSharp.MachO
{
    [DebuggerDisplay("{Type}({Name,nq})")]
    public sealed class Segment : Command
    {
        public Segment(BinaryReader reader, Stream stream, bool is64) : base(reader, stream)
        {
            this.is64 = is64;
            Name = ReadSectionOrSegmentName();
            Address = ReadUInt32OrUInt64();
            Size = ReadUInt32OrUInt64();
            FileOffset = ReadUInt32OrUInt64();
            var fileSize = ReadUInt32OrUInt64();
            MaximalProtection = ReadProtection();
            InitialProtection = ReadProtection();
            var numberOfSections = Reader.ReadInt32();
            Reader.ReadInt32(); // we ignore flags for now

            if(fileSize > 0)
            {
                var streamPosition = Stream.Position;
                Stream.Seek((long)FileOffset, SeekOrigin.Begin);
                data = new byte[Size];                
                var buffer = stream.ReadBytesOrThrow(checked((int)fileSize));
                Array.Copy(buffer, data, buffer.Length);
                Stream.Position = streamPosition;
            }

            var sections = new List<Section>();
            for(var i = 0; i < numberOfSections; i++)
            {
                var sectionName = ReadSectionOrSegmentName();
                var segmentName = ReadSectionOrSegmentName();
                if(segmentName != Name)
                {
                    throw new InvalidOperationException("Unexpected name of the section's segment.");
                }
                var sectionAddress = ReadUInt32OrUInt64();
                var sectionSize = ReadUInt32OrUInt64();
                var offset = Reader.ReadUInt32();
                var alignExponent = Reader.ReadUInt32();
                Reader.ReadBytes(is64 ? 24 : 20);
                var section = new Section(sectionName, sectionAddress, sectionSize, offset, alignExponent, this);
                sections.Add(section);
            }

            Sections = new ReadOnlyCollection<Section>(sections);
        }

        public string Name { get; private set; }
        public ulong Address { get; private set; }
        public ulong Size { get; private set; }
        public ulong FileOffset { get; private set; }
        public Protection InitialProtection { get; private set; }
        public Protection MaximalProtection { get; private set; }
        public ReadOnlyCollection<Section> Sections { get; private set; }
        private CommandType Type => is64 ? CommandType.Segment64 : CommandType.Segment;

        public byte[] GetData()
        {
            if(data == null)
            {
                return new byte[Size];
            }
            return data.ToArray();
        }

        private ulong ReadUInt32OrUInt64()
        {
            return is64 ? Reader.ReadUInt64() : Reader.ReadUInt32();
        }

        private Protection ReadProtection()
        {
            return (Protection)Reader.ReadInt32();
        }

        private string ReadSectionOrSegmentName()
        {
            var nameAsBytes = Reader.ReadBytes(16).TakeWhile(x => x != 0).ToArray();
            return Encoding.UTF8.GetString(nameAsBytes);
        }

        private readonly bool is64;
        private readonly byte[] data;
    }
}

