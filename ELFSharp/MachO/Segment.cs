using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ELFSharp.Utilities;

namespace ELFSharp.MachO
{
    public sealed class Segment : Command
    {
        public Segment(BinaryReader reader, Func<FileStream> streamProvider, bool is64) : base(reader, streamProvider)
        {
            this.is64 = is64;
            Name = ReadSectionOrSegmentName();
            Address = ReadInt32OrInt64();
            Size = ReadInt32OrInt64();
            var fileOffset = ReadInt32OrInt64();
            var fileSize = ReadInt32OrInt64();
            MaximalProtection = ReadProtection();
            InitialProtection = ReadProtection();
            var numberOfSections = Reader.ReadInt32();
            Reader.ReadInt32(); // we ignore flags for now
            if(fileSize > 0)
            {
                data = new byte[Size];
                using(var stream = streamProvider())
                {
                    stream.Seek(fileOffset, SeekOrigin.Begin);
                    var buffer = stream.ReadBytesOrThrow(checked((int)fileSize));
                    Array.Copy(buffer, data, buffer.Length);
                }
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
                var sectionAddress = ReadInt32OrInt64();
                var sectionSize = ReadInt32OrInt64();
                var offsetInSegment = ReadInt32OrInt64() - fileOffset;
                if(offsetInSegment < 0)
                {
                    throw new InvalidOperationException("Unexpected section offset lower than segment offset.");
                }
                var alignExponent = Reader.ReadInt32();
                Reader.ReadBytes(20);
                var section = new Section(sectionName, sectionAddress, sectionSize, offsetInSegment, alignExponent, this);
                sections.Add(section);
            }
            Sections = new ReadOnlyCollection<Section>(sections);
        }

        public string Name { get; private set; }
        public long Address { get; private set; }
        public long Size { get; private set; }
        public Protection InitialProtection { get; private set; }
        public Protection MaximalProtection { get; private set; }
        public ReadOnlyCollection<Section> Sections { get; private set; }

        public byte[] GetData()
        {
            if(data == null)
            {
                return new byte[Size];
            }
            return data.ToArray();
        }

        private long ReadInt32OrInt64()
        {
            return is64 ? Reader.ReadInt64() : Reader.ReadInt32();
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

