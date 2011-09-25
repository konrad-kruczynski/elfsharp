using System;
using NUnit.Framework;
using ELFSharp;

namespace Tests
{
	[TestFixture]
	public class OpeningTests
	{
		
		[Test]
		public void ShouldOpenHelloWorld32()
		{
			ELFReader.Load32("hello32le");			
		}
		
		[Test]
		public void ShouldOpenHelloWorld64()
		{			
			ELFReader.Load64("hello64le");
		}
		
		[Test]
		public void ShouldProperlyParseClass32()
		{
			var elf32 = ELFReader.Load32("hello32le");
			Assert.AreEqual(Class.Bit32, elf32.Class);			
		}

		[Test]
		public void ShouldProperlyParseClass64()
		{			
			var elf64 = ELFReader.Load64("hello64le");
			Assert.AreEqual(Class.Bit64, elf64.Class);
		}
		
		[Test]
		public void ShouldProperlyParseEndianess()
		{
			ELF elf = ELFReader.Load32("hello32le");			
			Assert.AreEqual(Endianess.LittleEndian, elf.Endianess);
			elf = ELFReader.Load32("vmlinuxOpenRisc");
			Assert.AreEqual(Endianess.BigEndian, elf.Endianess);
		}
		
		[Test]
		public void ShouldOpenBigEndian()
		{
			ELFReader.Load32("vmlinuxOpenRisc");
		}
		
	}
}

