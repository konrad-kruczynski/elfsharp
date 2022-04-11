using NUnit.Framework;
using ELFSharp.ELF.Sections;
using ELFSharp.ELF;
using System.Linq;
using ELFSharp.ELF.Segments;

namespace Tests.ELF
{
    [TestFixture]
    public class NoteSectionTests
    {
        [Test]
        public void ShouldReadNoteName32()
        {
            using var elf = ELFReader.Load(Utilities.GetBinaryStream("hello32le"), true);
            var noteSection = (INoteSection)elf.GetSection(".note.ABI-tag");
            Assert.AreEqual("GNU", noteSection.NoteName);
        }

        [Test]
        public void ShouldReadNoteName64()
        {
            using var elf = ELFReader.Load(Utilities.GetBinaryStream("hello64le"), true);
            var noteSection = (INoteSection)elf.GetSection(".note.ABI-tag");
            Assert.AreEqual("GNU", noteSection.NoteName);
        }

        [Test]
        public void ShouldReadNoteType32()
        {
            var elf = ELFReader.Load<uint>(Utilities.GetBinaryStream("hello32le"), true);
            var noteSection = (NoteSection<uint>)elf.GetSection(".note.ABI-tag");
            Assert.AreEqual(1, noteSection.NoteType);
        }

        [Test]
        public void ShouldReadNoteType64()
        {
            var elf = ELFReader.Load<ulong>(Utilities.GetBinaryStream("hello64le"), true);
            var noteSection = (NoteSection<ulong>)elf.GetSection(".note.ABI-tag");
            Assert.AreEqual(1, noteSection.NoteType);
        }

        [Test]
        public void ShouldNotThrowOnCustomNote()
        {
            // Not all notes conform to the expected format that the NoteSection
            // class uses; this test validates that we can successfully parse a
            // binary containing such a section and retrieve the note, all without
            // throwing an exception.
            const string sectionName = ".note.custom";
            var elf = ELFReader.Load<uint>(Utilities.GetBinaryStream("custom-note"), true);
            var noteSection = (NoteSection<uint>)elf.GetSection(sectionName);
            Assert.AreEqual(sectionName, noteSection.Name);
        }


        [Test]
        public void ShouldReadMultipleNotesWithinNoteSection()
        {
            using var elf = ELFReader.Load(Utilities.GetBinaryStream("elf32-core-adbd_3124"), true);
            var noteSegment = elf.Segments.Where(x => x.Type == SegmentType.Note).First() as INoteSegment;
            Assert.AreEqual(22, noteSegment.Notes.Count);
        }

        [Test]
        public void ShouldReadMultipleNoteDataName()
        {
            using var elf = ELFReader.Load(Utilities.GetBinaryStream("elf32-core-adbd_3124"), true);
            var noteSegment = elf.Segments.Where(x => x.Type == SegmentType.Note).First() as INoteSegment;
            Assert.AreEqual("CORE", noteSegment.Notes[0].Name);
            Assert.AreEqual("LINUX", noteSegment.Notes[6].Name);
        }

        [Test]
        public void ShouldReadMultipleNoteDataType()
        {
            using var elf = ELFReader.Load(Utilities.GetBinaryStream("elf32-core-adbd_3124"), true);
            var noteSegment = elf.Segments.Where(x => x.Type == SegmentType.Note).First() as INoteSegment;
            Assert.AreEqual(1ul, noteSegment.Notes[0].Type);
            Assert.AreEqual(1024ul, noteSegment.Notes[6].Type);
        }
    }
}

