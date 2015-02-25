using System.Linq;
using NUnit.Framework;
using ELFSharp.ELF.Sections;
using ELFSharp.ELF;

namespace Tests.ELF
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
			var symtab = (ISymbolTable)elf.GetSection(".symtab");           
            Assert.AreEqual(64, symtab.Entries.Count());           
        }

        [Test]
        public void GithubIssueNo24()
        {
            var elf = ELFReader.Load(Utilities.GetBinaryLocation("issue24.elf"));
            var dynamicSymbolSection = (SymbolTable<uint>)elf.GetSection(".dynsym");
            dynamicSymbolSection.Entries.Any(x => x.Name == "<corrupt>");
        }
    }
}

