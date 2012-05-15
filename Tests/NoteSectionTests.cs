using NUnit.Framework;
using ELFSharp;

namespace Tests
{
    [TestFixture]
    public class NoteSectionTests
    {
        [Test]
        public void ShouldReadNoteName32()
        {
            var elf = ELFReader.Load("hello32le");
            var noteSection = (INoteSection)elf.GetSection(".note.ABI-tag");
            Assert.AreEqual("GNU", noteSection.NoteName);
        }

        [Test]
        public void ShouldReadNoteName64()
        {
            var elf = ELFReader.Load("hello64le");
            var noteSection = (INoteSection)elf.GetSection(".note.ABI-tag");
            Assert.AreEqual("GNU", noteSection.NoteName);
        }

        [Test]
        public void ShouldReadNoteType32()
        {
            var elf = ELFReader.Load<uint>("hello32le");
            var noteSection = (NoteSection<uint>)elf.GetSection(".note.ABI-tag");
            Assert.AreEqual(1, noteSection.NoteType);
        }

        [Test]
        public void ShouldReadNoteType64()
        {
            var elf = ELFReader.Load<long>("hello64le");
            var noteSection = (NoteSection<long>)elf.GetSection(".note.ABI-tag");
            Assert.AreEqual(1, noteSection.NoteType);
        }
    }
}

