using System;
using System.Linq;
using NUnit.Framework;
using ELFSharp;

namespace Tests
{
	[TestFixture]
	public class SymbolTableTests
	{
		[Test]
		public void ShouldFindAllSymbols32()
		{
			var elf = ELFReader.Load("hello32le");
			var symtab = (SymbolTable) elf.GetSection(".symtab");
			Assert.AreEqual(64, symtab.Entries.Count());			
		}
		
		[Test]
		public void ShouldFindAllSymbols64()
		{
			var elf = ELFReader.Load("hello64le");			
			var symtab = (SymbolTable) elf.GetSection(".dynsym");			
			Assert.AreEqual(171, symtab.Entries.Count());			
		}
	}
}

