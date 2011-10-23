using System;
using System.Linq;
using NUnit.Framework;
using ELFSharp;

namespace Tests
{
	[TestFixture]
	public class SectionHeadersParsingTests
	{
		[Test]
		public void ShouldFind29Sections32()
		{
			var elf = ELFReader.Load32("hello32le");
			Assert.AreEqual(29, elf.SectionHeaders.Count());
		}
		
		[Test]
		public void ShouldFind29Sections64()
        {
            var elf = ELFReader.Load64("hello64le");
            Assert.AreEqual(27, elf.SectionHeaders.Count());
        }

        [Test]
        public void ShouldFindProperAlignment32()
        {
            var elf = ELFReader.Load32("hello32le");
            var header = elf.SectionHeaders.First(x => x.Name == ".init");
            Assert.AreEqual(4, header.Alignment);
        }

        [Test]
        public void ShouldFindProperAlignment64()
        {
            var elf = ELFReader.Load64("hello64le");
            var header = elf.SectionHeaders.First(x => x.Name == ".text");
            Assert.AreEqual(16, header.Alignment);
        }

        [Test]
        public void ShouldFindProperEntrySize32()
        {
            var elf = ELFReader.Load32("hello32le");
            var header = elf.SectionHeaders.First(x => x.Name == ".dynsym");
            Assert.AreEqual(0x10, header.EntrySize);
        }

        [Test]
        public void ShouldFindProperEntrySize64()
        {
            var elf = ELFReader.Load64("hello64le");
            var header = elf.SectionHeaders.First(x => x.Name == ".dynsym");
            Assert.AreEqual(0x18, header.EntrySize);
        }

        [Test]
        public void ShouldFindAllNotes32()
        {
            var elf = ELFReader.Load32("hello32le");
            var notes = elf.GetSections<NoteSection32>();
            Assert.AreEqual(2, notes.Count());
        }

        [Test]
        public void ShouldFindAllNotes64()
        {
            var elf = ELFReader.Load64("hello64le");
            var notes = elf.GetSections<NoteSection64>();
            Assert.AreEqual(1, notes.Count());
        }
	}
}

