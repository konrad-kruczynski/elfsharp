using System;
using NUnit.Framework;
using ELFSharp;

namespace Tests
{
	[TestFixture]
	public class OpeningTests
	{
		[Test]
		public void ShouldChooseGoodClass32()
		{
			var elf = ELFReader.Load<uint>("hello32le");
			Assert.AreEqual(Class.Bit32, elf.Class);
		}
		
		[Test]
		public void ShouldChooseGoodClass64()
		{
			var elf = ELFReader.Load<long>("hello64le");
			Assert.AreEqual(Class.Bit64, elf.Class);
		}
		
		[Test]
		public void ShouldOpenHelloWorld32()
		{
			ELFReader.Load<uint>("hello32le");			
		}
		
		[Test]
		public void ShouldOpenHelloWorld64()
		{			
			ELFReader.Load<long>("hello64le");
		}
		
		[Test]
		public void ShouldProperlyParseClass32()
		{
			var elf32 = ELFReader.Load<uint>("hello32le");
			Assert.AreEqual(Class.Bit32, elf32.Class);			
		}

		[Test]
		public void ShouldProperlyParseClass64()
		{			
			var elf64 = ELFReader.Load<long>("hello64le");
			Assert.AreEqual(Class.Bit64, elf64.Class);
		}
		
		[Test]
		public void ShouldProperlyParseEndianess()
		{
			var elf = ELFReader.Load<uint>("hello32le");			
			Assert.AreEqual(Endianess.LittleEndian, elf.Endianess);
			elf = ELFReader.Load<uint>("vmlinuxOpenRisc");
			Assert.AreEqual(Endianess.BigEndian, elf.Endianess);
		}
		
		[Test]
		public void ShouldOpenBigEndian()
		{
			ELFReader.Load<uint>("vmlinuxOpenRisc");
		}

        [Test]
        public void GithubIssueNo2()
        {
            ELFReader.Load<uint>("mpuG890.axf");
        }
		
	}
}

