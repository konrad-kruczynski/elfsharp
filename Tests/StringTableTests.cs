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
			var elf = ELFReader.Load32("hello32le");
			Assert.IsTrue(elf.HasSectionsStringTable, 
			              "Sections string table was not found in 32 bit ELF.");
			Assert.AreEqual(29, elf.SectionsStringTable.Strings.Count());
		}
		
		[Test]
		public void ShouldFindAllStrings64()
		{
			var elf = ELFReader.Load64("hello64le");
			Assert.IsTrue(elf.HasSectionsStringTable, 
			              "Sections string table was not found in 64 bit ELF.");
			Assert.AreEqual(27, elf.SectionsStringTable.Strings.Count());
		}
		
		[Test]
		public void ShouldFindAllStringsBigEndian()
		{
			var elf = ELFReader.Load32("vmlinuxOpenRisc");
			Assert.IsTrue(elf.HasSectionsStringTable,
			              "Sections string table was not found in 32 bit big endian ELF.");
			Assert.AreEqual(28, elf.SectionsStringTable.Strings.Count());
		}
	}
}

