using System;
using System.Linq;
using NUnit.Framework;
using ELFSharp;

namespace Tests
{
	[TestFixture]
	public class ProgBitsSectionTests
	{
		[Test]
		public void ShouldGetLoadAddress32()
		{
			var elf = ELFReader.Load("hello32le");
			var sectionsToLoad = elf.GetSections<ProgBitsSection32>().Where(x => x.Header.LoadAddress != 0);
			Assert.AreEqual(13, sectionsToLoad.Count());
		}
		
		[Test]
		public void ShouldGetLoadAddress64()
		{
			var elf = ELFReader.Load("hello64le");
			var sectionsToLoad = elf.GetSections<ProgBitsSection64>().Where(x => x.Header.LoadAddress != 0);
			Assert.AreEqual(14, sectionsToLoad.Count());
		}
	}
}

