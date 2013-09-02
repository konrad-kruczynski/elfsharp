using NUnit.Framework;
using ELFSharp.ELF;

namespace Tests
{
    [TestFixture]
    public class OpeningTests
    {
        [Test]
        public void ShouldChooseGoodClass32()
        {
            var elf = ELFReader.Load(Utilities.GetBinaryLocation("hello32le"));
            Assert.AreEqual(Class.Bit32, elf.Class);
        }
        
        [Test]
        public void ShouldChooseGoodClass64()
        {
            var elf = ELFReader.Load(Utilities.GetBinaryLocation("hello64le"));
            Assert.AreEqual(Class.Bit64, elf.Class);
        }
        
        [Test]
        public void ShouldOpenHelloWorld32()
        {
			ELFReader.Load(Utilities.GetBinaryLocation("hello32le"));
        }
        
        [Test]
        public void ShouldOpenHelloWorld64()
        {           
            ELFReader.Load(Utilities.GetBinaryLocation("hello64le"));
        }
        
        [Test]
        public void ShouldProperlyParseClass32()
        {
            var elf32 = ELFReader.Load<uint>(Utilities.GetBinaryLocation("hello32le"));
            Assert.AreEqual(Class.Bit32, elf32.Class);          
        }

        [Test]
        public void ShouldProperlyParseClass64()
        {           
            var elf64 = ELFReader.Load<long>(Utilities.GetBinaryLocation("hello64le"));
            Assert.AreEqual(Class.Bit64, elf64.Class);
        }
        
        [Test]
        public void ShouldProperlyParseEndianess()
        {
			var elf = ELFReader.Load(Utilities.GetBinaryLocation("hello32le"));
            Assert.AreEqual(Endianess.LittleEndian, elf.Endianess);
			elf = ELFReader.Load(Utilities.GetBinaryLocation("vmlinuxOpenRisc"));
            Assert.AreEqual(Endianess.BigEndian, elf.Endianess);
        }
        
        [Test]
        public void ShouldOpenBigEndian()
        {
            ELFReader.Load(Utilities.GetBinaryLocation("vmlinuxOpenRisc"));
        }

        [Test]
        public void GithubIssueNo2()
        {
            ELFReader.Load(Utilities.GetBinaryLocation("mpuG890.axf"));
        }

		[Test]
		public void GithubIssueNo3()
		{
			ELFReader.Load(Utilities.GetBinaryLocation("issue3"));
		}

		[Test]
		public void ShouldNotOpenNonELFFile()
		{
			IELF elf;
			Assert.IsFalse(ELFReader.TryLoad(Utilities.GetBinaryLocation("notelf"), out elf));
		}
    }
}

