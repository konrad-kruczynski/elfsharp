using System;
using System.Linq;
using ELFSharp;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class ProgramHeaderTests
	{
		[Test]
		public void ShouldFindAllHeadersH32LE()
		{
			var elf = ELFReader.Load32("hello32le");
			Assert.AreEqual(8, elf.ProgramHeaders.Count());
		}
		
		[Test]
		public void ShouldFindAllHeadersOR32BE()
		{
			var elf = ELFReader.Load32("vmlinuxOpenRisc");
			Assert.AreEqual(2, elf.ProgramHeaders.Count());
		}
	}
}

