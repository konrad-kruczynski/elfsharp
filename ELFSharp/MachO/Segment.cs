﻿using System;
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
        public const string UNEXPECTED_SEGMENT_NAME = "Unexpected name of the section's segment.";
        public const string UNEXPECTED_SECTION_OFFSET = "Unexpected section offset lower than segment offset.";

        public Segment(BinaryReader reader, Func<FileStream> streamProvider, bool is64, Dictionary<String, long> exceptionsToQueue = null) : base(reader, streamProvider)
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
                var stream = streamProvider();
                var previousPosition = stream.Position;
                stream.Seek(fileOffset, SeekOrigin.Begin);
                var buffer = stream.ReadBytesOrThrow(checked((int)fileSize), exceptionsToQueue);
                Array.Copy(buffer, data, buffer.Length);
                stream.Position = previousPosition;

            }
            var sections = new List<Section>();
            try
            {
                for(var i = 0; i < numberOfSections; i++)
                {
                    var sectionName = ReadSectionOrSegmentName();
                    var segmentName = ReadSectionOrSegmentName();
                    // set name to segment name in edge case where name is empty string
                    if(string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(segmentName)) {
                        Name = segmentName;
                    }
                    if(segmentName != Name)
                    {
                        if(exceptionsToQueue == null)
                        {
                            throw new InvalidOperationException(UNEXPECTED_SEGMENT_NAME);
                        }
                        else
                        {
                            MachOReader.AddException(exceptionsToQueue, UNEXPECTED_SEGMENT_NAME);
                        }
                    }
                    var sectionAddress = ReadInt32OrInt64();
                    var sectionSize = ReadInt32OrInt64();
                    var offsetInSegment = ReadInt32OrInt64() - fileOffset;
                    if(offsetInSegment < 0)
                    {
                        if(exceptionsToQueue == null)
                        {
                            throw new InvalidOperationException(UNEXPECTED_SECTION_OFFSET);
                        }
                        else
                        {
                            MachOReader.AddException(exceptionsToQueue, UNEXPECTED_SECTION_OFFSET);
                        }
                    }
                    var alignExponent = Reader.ReadInt32();
                    Reader.ReadBytes(20);
                    var section = new Section(sectionName, sectionAddress, sectionSize, offsetInSegment, alignExponent, this);
                    sections.Add(section);
                }
            }
            finally
            {
                Sections = new ReadOnlyCollection<Section>(sections);
            }
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

