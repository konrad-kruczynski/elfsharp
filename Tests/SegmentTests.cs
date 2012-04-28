using System;
using System.Linq;
using ELFSharp;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class SegmentTests
	{
		[Test]
		public void ShouldFindAllSegmentsH32LE()
		{
			var elf = ELFReader.Load("hello32le");
			Assert.AreEqual(8, elf.Segments.Count());
		}
		
		[Test]
		public void ShouldFindAllSegmentsOR32BE()
        {
            var elf = ELFReader.Load("vmlinuxOpenRisc");
            Assert.AreEqual(2, elf.Segments.Count());
        }

        [Test]
        public void ShouldFindProperFlags32()
        {
            var elf = ELFReader.Load<uint>("hello32le");
            var header = elf.Segments.First(x => x.Address == 0x08048034);
            Assert.IsTrue(header.Flags.HasFlag(SegmentFlags.Execute));
            Assert.IsTrue(header.Flags.HasFlag(SegmentFlags.Read));
            Assert.IsFalse(header.Flags.HasFlag(SegmentFlags.Write));
        }

        [Test]
        public void ShouldFindProperFlags64()
        {
            var elf = ELFReader.Load<long>("hello64le");
            var header = elf.Segments.First(x => x.Address == 0x400000);
            Assert.IsTrue(header.Flags.HasFlag(SegmentFlags.Execute));
            Assert.IsTrue(header.Flags.HasFlag(SegmentFlags.Read));
            Assert.IsFalse(header.Flags.HasFlag(SegmentFlags.Write));
        }

        [Test]
        public void ShouldFindProperAlignment32()
        {
            var elf = ELFReader.Load<uint>("hello32le");
            var header = elf.Segments.First(x => x.Address == 0x08048000);
            Assert.AreEqual(0x1000, header.Alignment);
        }

        [Test]
        public void ShouldFindProperAlignment64()
        {
            var elf = ELFReader.Load<long>("hello64le");
            var header = elf.Segments.First(x => x.Address == 0x62b178);
            Assert.AreEqual(8, header.Alignment);
        }

	}
}

