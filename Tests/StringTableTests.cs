using System;
using System.Linq;
using NUnit.Framework;
using ELFSharp;

namespace Tests
{
	[TestFixture]
	public class StringTableTests
	{
		[Test]
		public void ShouldFindAllStrings()
		{
			var elf32 = ELFReader.Load32("hello32le");
			Assert.IsTrue(elf32.HasSectionsStringTable, 
			              "Sections string table was not found in 32 bit ELF.");
			Assert.AreEqual(29, elf32.SectionsStringTable.Strings.Count());
		}
	}
}

