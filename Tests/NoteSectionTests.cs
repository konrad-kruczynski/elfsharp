using System;
using NUnit.Framework;
using ELFSharp;

namespace Tests
{
	[TestFixture]
	public class NoteSectionTests
	{
		[Test]
		public void ShouldReadNote32()
		{
			var elf = ELFReader.Load32("hello32le");
			

		}
	}
}

