using NUnit.Framework;
using ELFSharp.ELF;

namespace Tests.ELF
{
    [TestFixture]
    public class OpeningTests
    {
        [Test]
        public void ShouldChooseGoodClass32()
        {
            var elf = ELFReader.Load(Utilities.GetBinary("hello32le"));
            Assert.AreEqual(Class.Bit32, elf.Class);
        }
        
        [Test]
        public void ShouldChooseGoodClass64()
        {
            var elf = ELFReader.Load(Utilities.GetBinary("hello64le"));
            Assert.AreEqual(Class.Bit64, elf.Class);
        }
        
        [Test]
        public void ShouldOpenHelloWorld32()
        {
			ELFReader.Load(Utilities.GetBinary("hello32le"));
        }
        
        [Test]
        public void ShouldOpenHelloWorld64()
        {           
            ELFReader.Load(Utilities.GetBinary("hello64le"));
        }
        
        [Test]
        public void ShouldProperlyParseClass32()
        {
            var elf32 = ELFReader.Load<uint>(Utilities.GetBinary("hello32le"));
            Assert.AreEqual(Class.Bit32, elf32.Class);          
        }

        [Test]
        public void ShouldProperlyParseClass64()
        {           
            var elf64 = ELFReader.Load<ulong>(Utilities.GetBinary("hello64le"));
            Assert.AreEqual(Class.Bit64, elf64.Class);
        }
        
        [Test]
        public void ShouldProperlyParseEndianess()
        {
			var elf = ELFReader.Load(Utilities.GetBinary("hello32le"));
            Assert.AreEqual(Endianess.LittleEndian, elf.Endianess);
			elf = ELFReader.Load(Utilities.GetBinary("vmlinuxOpenRisc"));
            Assert.AreEqual(Endianess.BigEndian, elf.Endianess);
        }
        
        [Test]
        public void ShouldOpenBigEndian()
        {
            ELFReader.Load(Utilities.GetBinary("vmlinuxOpenRisc"));
        }

        [Test]
        public void GithubIssueNo2()
        {
            ELFReader.Load(Utilities.GetBinary("mpuG890.axf"));
        }

		[Test]
		public void GithubIssueNo3()
		{
			ELFReader.Load(Utilities.GetBinary("issue3"));
		}

		[Test]
		public void ShouldNotOpenNonELFFile()
		{
            Assert.IsFalse(ELFReader.TryLoad(Utilities.GetBinary("notelf"), out var _));
        }

        [Test]
        public void GithubIssueNo9()
        {
            ELFReader.Load(Utilities.GetBinary("stripped-all-binary"));
        }

        [Test]
        public void GithubIssueNo24()
        {
            ELFReader.Load(Utilities.GetBinary("issue24.elf"));
        }
    }
}

