using System.Linq;
using NUnit.Framework;
using ELFSharp;
using ELFSharp.Sections;

namespace Tests
{
    [TestFixture]
    public class SymbolTableTests
    {
        [Test]
        public void ShouldFindAllSymbols32()
        {
            var elf = ELFReader.Load(Utilities.GetBinaryLocation("hello32le"));
            var symtab = (ISymbolTable)elf.GetSection(".symtab");
            Assert.AreEqual(64, symtab.Entries.Count());            
        }
        
        [Test]
        public void ShouldFindAllSymbols64()
        {
			var elf = ELFReader.Load(Utilities.GetBinaryLocation("hello64le"));
            var symtab = (ISymbolTable)elf.GetSection(".dynsym");           
            Assert.AreEqual(171, symtab.Entries.Count());           
        }
    }
}

