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
		public void ShouldFind29Sections()
		{
			var elf = ELFReader.Load("hello32le");
			Assert.AreEqual(29, elf.SectionHeaders.Count());
		}
	}
}

