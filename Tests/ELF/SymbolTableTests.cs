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
            using var elf = ELFReader.Load(Utilities.GetBinaryStream("hello32le"), true);
            var symtab = (ISymbolTable)elf.GetSection(".symtab");
            Assert.AreEqual(64, symtab.Entries.Count());            
        }
        
        [Test]
        public void ShouldFindAllSymbols64()
        {
			using var elf = ELFReader.Load(Utilities.GetBinaryStream("hello64le"), true);
			var symtab = (ISymbolTable)elf.GetSection(".symtab");           
            Assert.AreEqual(64, symtab.Entries.Count());           
        }

        [Test]
        public void GithubIssueNo24()
        {
            using var elf = ELFReader.Load(Utilities.GetBinaryStream("issue24.elf"), true);
            var dynamicSymbolSection = (SymbolTable<uint>)elf.GetSection(".dynsym");
            dynamicSymbolSection.Entries.Any(x => x.Name == "<corrupt>");
        }
    }
}

