using System.Linq;
using NUnit.Framework;
using ELFSharp.ELF.Segments;
using ELFSharp.ELF;

namespace Tests.ELF
{
    [TestFixture]
    public class SegmentsTests
    {
        [Test]
        public void ShouldFindAllSegmentsH32LE()
        {
            using var elf = ELFReader.Load(Utilities.GetBinaryStream("hello32le"), true);
            Assert.AreEqual(8, elf.Segments.Count());
        }
        
        [Test]
        public void ShouldFindAllSegmentsOR32BE()
        {
            using var elf = ELFReader.Load(Utilities.GetBinaryStream("vmlinuxOpenRisc"), true);
            Assert.AreEqual(2, elf.Segments.Count());
        }

        [Test]
        public void ShouldFindProperFlags32()
        {
            var elf = ELFReader.Load<uint>(Utilities.GetBinaryStream("hello32le"), true);
            var segment = elf.Segments.First(x => x.Address == 0x08048034);
            Assert.IsTrue(segment.Flags.HasFlag(SegmentFlags.Execute));
            Assert.IsTrue(segment.Flags.HasFlag(SegmentFlags.Read));
            Assert.IsFalse(segment.Flags.HasFlag(SegmentFlags.Write));
        }

        [Test]
        public void ShouldFindProperFlags64()
        {
            var elf = ELFReader.Load<ulong>(Utilities.GetBinaryStream("hello64le"), true);
            var segment = elf.Segments.First(x => x.Address == 0x400000);
            Assert.IsTrue(segment.Flags.HasFlag(SegmentFlags.Execute));
            Assert.IsTrue(segment.Flags.HasFlag(SegmentFlags.Read));
            Assert.IsFalse(segment.Flags.HasFlag(SegmentFlags.Write));
        }

        [Test]
        public void ShouldFindProperAlignment32()
        {
            var elf = ELFReader.Load<uint>(Utilities.GetBinaryStream("hello32le"), true);
            var segment = elf.Segments.First(x => x.Address == 0x08048000);
            Assert.AreEqual(0x1000, segment.Alignment);
        }

        [Test]
        public void ShouldFindProperAlignment64()
        {
            var elf = ELFReader.Load<ulong>(Utilities.GetBinaryStream("hello64le"), true);
            var segment = elf.Segments.First(x => x.Address == 0x6006c8);
            Assert.AreEqual(8, segment.Alignment);
        }

        [Test]
        public void ShouldGetFileContents()
        {
            var elf = ELFReader.Load<int>(Utilities.GetBinaryStream("hello32le"), true);
            var segment = elf.Segments.Single(x => x.Address == 0x8049F14 && x.Type == SegmentType.Load);
            Assert.AreEqual(256, segment.GetFileContents().Length);
        }

        [Test]
        public void ShouldGetMemoryContents()
        {
            var elf = ELFReader.Load<int>(Utilities.GetBinaryStream("hello32le"), true);
            var segment = elf.Segments.Single(x => x.Address == 0x8049F14 && x.Type == SegmentType.Load);
            Assert.AreEqual(264, segment.GetMemoryContents().Length);
        }

        [Test]
        public void GithubIssueNo45()
        {
            var elf = ELFReader.Load<int>(Utilities.GetBinaryStream("hello32le"), true);
            var segment = elf.Segments.Single(x => x.Address == 0x8049F14 && x.Type == SegmentType.Load);

            byte[] memoryContents = segment.GetMemoryContents();
            var endingZeroes = memoryContents.Skip((int)segment.FileSize);
            Assert.IsTrue(endingZeroes.All(x => x == 0), "Not all additional bytes were zero.");
        }

        // Github issue no 60.
        [Test]
        public void SegmentsCountShouldBeAvailable()
        {
            var elf = ELFReader.Load(Utilities.GetBinaryStream("hello32le"), true);
            Assert.AreEqual(8, elf.Segments.Count);
        }
    }
}

