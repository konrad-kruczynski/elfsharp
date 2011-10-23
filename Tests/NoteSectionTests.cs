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
		public void ShouldFindNote32() // TODO: move
		{
			var elf = ELFReader.Load32("hello32le");
			var notes = elf.GetSections<NoteSection32>();
            Assert.AreEqual(2, notes.Count());
		}

        [Test]
        public void ShouldReadNote32()
        {
            var elf = ELFReader.Load32("hello32le");
            var noteSection = (NoteSection32)elf.GetSection(".note.ABI-tag");
            Assert.AreEqual("GNU", noteSection.Name);
            Assert.AreEqual(1, noteSection.Type);
        }

        [Test]
        public void ShouldReadNote64()
        {
            var elf = ELFReader.Load64("hello64le");
            var noteSection = (NoteSection64)elf.GetSection(".note.ABI-tag");
            Assert.AreEqual("GNU", noteSection.Name);
            Assert.AreEqual(1, noteSection.Type);
        }
	}
}

