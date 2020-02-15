using System.Linq;
using NUnit.Framework;
using ELFSharp.ELF.Sections;
using ELFSharp.ELF;

namespace Tests.ELF
{
    [TestFixture]
    public class SectionHeadersParsingTests
    {
        [Test]
        public void ShouldFindProperNumberOfSections32()
        {
            using var elf = ELFReader.Load(Utilities.GetBinaryStream("hello32le"), true);
            Assert.AreEqual(29, elf.Sections.Count());
        }
        
        [Test]
        public void ShouldFindProperNumberOfSections64()
        {
            using var elf = ELFReader.Load(Utilities.GetBinaryStream("hello64le"), true);
            Assert.AreEqual(30, elf.Sections.Count());
        }

        [Test]
        public void ShouldFindProperAlignment32()
        {
            var elf = ELFReader.Load<uint>(Utilities.GetBinaryStream("hello32le"), true);
            var header = elf.Sections.First(x => x.Name == ".init");
            Assert.AreEqual(4, header.Alignment);
        }

        [Test]
        public void ShouldFindProperAlignment64()
        {
            var elf = ELFReader.Load<ulong>(Utilities.GetBinaryStream("hello64le"), true);
            var header = elf.Sections.First(x => x.Name == ".text");
            Assert.AreEqual(16, header.Alignment);
        }

        [Test]
        public void ShouldFindProperEntrySize32()
        {
            var elf = ELFReader.Load<uint>(Utilities.GetBinaryStream("hello32le"), true);
            var header = elf.Sections.First(x => x.Name == ".dynsym");
            Assert.AreEqual(0x10, header.EntrySize);
        }

        [Test]
        public void ShouldFindProperEntrySize64()
        {
            var elf = ELFReader.Load<ulong>(Utilities.GetBinaryStream("hello64le"), true);
            var header = elf.Sections.First(x => x.Name == ".dynsym");
            Assert.AreEqual(0x18, header.EntrySize);
        }

        [Test]
        public void ShouldFindAllNotes32()
        {
            using var elf = ELFReader.Load(Utilities.GetBinaryStream("hello32le"), true);
            var notes = elf.GetSections<INoteSection>();
            Assert.AreEqual(2, notes.Count());
        }

        [Test]
        public void ShouldFindAllNotes64()
        {
            using var elf = ELFReader.Load(Utilities.GetBinaryStream("hello64le"), true);
            var notes = elf.GetSections<INoteSection>();
            Assert.AreEqual(2, notes.Count());
        }

        [Test]
        public void ShouldFindProperOffset()
        {
            var elf = ELFReader.Load<ulong>(Utilities.GetBinaryStream("hello64le"), true);
            var section = elf.Sections.First(x => x.Name == ".strtab");
            Assert.AreEqual(0x17A0, section.Offset);
        }
    }
}

