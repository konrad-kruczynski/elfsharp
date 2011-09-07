using System;
using NUnit.Framework;
using ELFSharp;

namespace Tests
{
	[TestFixture]
	public class OpeningTests
	{
		
		[Test]
		public void ShouldOpenHelloWorld()
		{
			ELFReader.Load("hello32le");
		}
		
		[Test]
		public void ShouldProperlyParseClass()
		{
			var elf = ELFReader.Load("hello32le");
			Assert.AreEqual(Class.Bit32, elf.Class);
		}
		
		[Test]
		public void ShouldProperlyParseEndianess()
		{
			var elf = ELFReader.Load("hello32le");			
			Assert.AreEqual(Endianess.LittleEndian, elf.Endianess);			
		}
		
		[Test]
		public void ShouldOpenBigEndian()
		{
			ELFReader.Load("vmlinuxOpenRisc");
		}
		
	}
}

