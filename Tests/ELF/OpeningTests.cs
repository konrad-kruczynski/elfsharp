using NUnit.Framework;
using ELFSharp.ELF;
using ELFSharp.ELF.Sections;

namespace Tests.ELF
{
    [TestFixture]
    public class OpeningTests
    {
        [Test]
        public void ShouldChooseGoodClass32()
        {
            using var elf = ELFReader.Load(Utilities.GetBinaryStream("hello32le"), true);
            Assert.AreEqual(Class.Bit32, elf.Class);
        }
        
        [Test]
        public void ShouldChooseGoodClass64()
        {
            using var elf = ELFReader.Load(Utilities.GetBinaryStream("hello64le"), true);
            Assert.AreEqual(Class.Bit64, elf.Class);
        }
        
        [Test]
        public void ShouldOpenHelloWorld32()
        {
			ELFReader.Load(Utilities.GetBinaryStream("hello32le"), true);
        }
        
        [Test]
        public void ShouldOpenHelloWorld64()
        {           
            ELFReader.Load(Utilities.GetBinaryStream("hello64le"), true);
        }
        
        [Test]
        public void ShouldProperlyParseClass32()
        {
            var elf32 = ELFReader.Load<uint>(Utilities.GetBinaryStream("hello32le"), true);
            Assert.AreEqual(Class.Bit32, elf32.Class);          
        }

        [Test]
        public void ShouldProperlyParseClass64()
        {           
            var elf64 = ELFReader.Load<ulong>(Utilities.GetBinaryStream("hello64le"), true);
            Assert.AreEqual(Class.Bit64, elf64.Class);
        }
        
        [Test]
        public void ShouldProperlyParseEndianess()
        {
			using var elfLittleEndian = ELFReader.Load(Utilities.GetBinaryStream("hello32le"), true);
            Assert.AreEqual(Endianess.LittleEndian, elfLittleEndian.Endianess);
			using var elfBigEndian = ELFReader.Load(Utilities.GetBinaryStream("vmlinuxOpenRisc"), true);
            Assert.AreEqual(Endianess.BigEndian, elfBigEndian.Endianess);
        }
        
        [Test]
        public void ShouldOpenBigEndian()
        {
            ELFReader.Load(Utilities.GetBinaryStream("vmlinuxOpenRisc"), true);
        }

        [Test]
        public void GithubIssueNo2()
        {
            ELFReader.Load(Utilities.GetBinaryStream("mpuG890.axf"), true);
        }

		[Test]
		public void GithubIssueNo3()
		{
			ELFReader.Load(Utilities.GetBinaryStream("issue3"), true);
		}

		[Test]
		public void ShouldNotOpenNonELFFile()
		{
            Assert.IsFalse(ELFReader.TryLoad(Utilities.GetBinaryStream("notelf"), true, out var _));
        }

        [Test]
        public void GithubIssueNo9()
        {
            ELFReader.Load(Utilities.GetBinaryStream("stripped-all-binary"), true);
        }

        [Test]
        public void GithubIssueNo24()
        {
            ELFReader.Load(Utilities.GetBinaryStream("issue24.elf"), true);
        }

        // Github issue no 49
        [Test]
        public void ShouldOpenEmptyStringTableElf()
        {
            using var elf = ELFReader.Load(Utilities.GetBinaryStream("libcoreclr"), true);
            var section = elf.GetSection(".dynstr");
            Assert.AreEqual(SectionType.NoBits, section.Type);
        }
    }
}

