using System;
using System.Linq;
using ELFSharp;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class ProgramHeaderTests
	{
		[Test]
		public void ShouldFindAllHeadersH32LE()
		{
			var elf = ELFReader.Load("hello32le");
			Assert.AreEqual(8, elf.ProgramHeaders.Count());
		}
		
		[Test]
		public void ShouldFindAllHeadersOR32BE()
        {
            var elf = ELFReader.Load("vmlinuxOpenRisc");
            Assert.AreEqual(2, elf.ProgramHeaders.Count());
        }

        [Test]
        public void ShouldFindProperFlags32()
        {
            var elf = ELFReader.Load<uint>("hello32le");
            var header = elf.ProgramHeaders.First(x => x.Address == 0x08048034);
            Assert.IsTrue(header.Flags.HasFlag(ProgramHeaderFlags.Execute));
            Assert.IsTrue(header.Flags.HasFlag(ProgramHeaderFlags.Read));
            Assert.IsFalse(header.Flags.HasFlag(ProgramHeaderFlags.Write));
        }

        [Test]
        public void ShouldFindProperFlags64()
        {
            var elf = ELFReader.Load<long>("hello64le");
            var header = elf.ProgramHeaders.First(x => x.Address == 0x400000);
            Assert.IsTrue(header.Flags.HasFlag(ProgramHeaderFlags.Execute));
            Assert.IsTrue(header.Flags.HasFlag(ProgramHeaderFlags.Read));
            Assert.IsFalse(header.Flags.HasFlag(ProgramHeaderFlags.Write));
        }

        [Test]
        public void ShouldFindProperAlignment32()
        {
            var elf = ELFReader.Load<uint>("hello32le");
            var header = elf.ProgramHeaders.First(x => x.Address == 0x08048000);
            Assert.AreEqual(0x1000, header.Alignment);
        }

        [Test]
        public void ShouldFindProperAlignment64()
        {
            var elf = ELFReader.Load<long>("hello64le");
            var header = elf.ProgramHeaders.First(x => x.Address == 0x62b178);
            Assert.AreEqual(8, header.Alignment);
        }

	}
}

