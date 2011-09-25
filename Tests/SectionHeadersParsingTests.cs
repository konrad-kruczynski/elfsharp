using System;
using System.Linq;
using NUnit.Framework;
using ELFSharp;

namespace Tests
{
	[TestFixture]
	public class SectionHeadersParsingTests
	{
		[Test]
		public void ShouldFind29Sections32()
		{
			var elf = ELFReader.Load32("hello32le");
			Assert.AreEqual(29, elf.SectionHeaders.Count());
		}
		
		[Test]
		public void ShouldFind29Sections64()
		{
			var elf = ELFReader.Load64("hello64le");
			Assert.AreEqual(27, elf.SectionHeaders.Count());
		}
	}
}

