using System;
using System.Linq;
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
            var elf = ELFReader.Load32("hello32le");
            var noteSection = (NoteSection32)elf.GetSection(".note.ABI-tag");
            Assert.AreEqual("GNU", noteSection.Name);
        }

        [Test]
        public void ShouldReadNoteName64()
        {
            var elf = ELFReader.Load64("hello64le");
            var noteSection = (NoteSection64)elf.GetSection(".note.ABI-tag");
            Assert.AreEqual("GNU", noteSection.Name);
        }

        [Test]
        public void ShouldReadNoteType32()
        {
            var elf = ELFReader.Load32("hello32le");
            var noteSection = (NoteSection32)elf.GetSection(".note.ABI-tag");
            Assert.AreEqual(1, noteSection.Type);
        }

        [Test]
        public void ShouldReadNoteType64()
        {
            var elf = ELFReader.Load64("hello64le");
            var noteSection = (NoteSection64)elf.GetSection(".note.ABI-tag");
            Assert.AreEqual(1, noteSection.Type);
        }
	}
}

