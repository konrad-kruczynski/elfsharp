using NUnit.Framework;
using ELFSharp.ELF.Sections;
using System.IO;
using ELFSharp.ELF;

namespace Tests.ELF
{
    [TestFixture]
    public class NoteSectionTests
    {
        [Test]
        public void ShouldReadNoteName32()
        {
            var elf = ELFReader.Load(Utilities.GetBinaryLocation("hello32le"));
            var noteSection = (INoteSection)elf.GetSection(".note.ABI-tag");
            Assert.AreEqual("GNU", noteSection.NoteName);
        }

        [Test]
        public void ShouldReadNoteName64()
        {
            var elf = ELFReader.Load(Utilities.GetBinaryLocation("hello64le"));
            var noteSection = (INoteSection)elf.GetSection(".note.ABI-tag");
            Assert.AreEqual("GNU", noteSection.NoteName);
        }

        [Test]
        public void ShouldReadNoteType32()
        {
            var elf = ELFReader.Load<uint>(Utilities.GetBinaryLocation("hello32le"));
            var noteSection = (NoteSection<uint>)elf.GetSection(".note.ABI-tag");
            Assert.AreEqual(1, noteSection.NoteType);
        }

        [Test]
        public void ShouldReadNoteType64()
        {
            var elf = ELFReader.Load<long>(Utilities.GetBinaryLocation("hello64le"));
            var noteSection = (NoteSection<long>)elf.GetSection(".note.ABI-tag");
            Assert.AreEqual(1, noteSection.NoteType);
        }
    }
}

