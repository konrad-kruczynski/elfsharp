using System.Linq;
using ELFSharp.ELF;
using ELFSharp.ELF.Sections;
using NUnit.Framework;

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

        // Github issue no 24
        [Test]
        public void ShouldHandleCorruptedNamesInDynSym()
        {
            using var elf = ELFReader.Load(Utilities.GetBinaryStream("issue24.elf"), true);
            var dynamicSymbolSection = (SymbolTable<uint>)elf.GetSection(".dynsym");
            dynamicSymbolSection.Entries.Any(x => x.Name == "<corrupt>");
        }

        [Test]
        public void SymbolsShouldHaveCorrectVisibility()
        {
            using var elf = ELFReader.Load(Utilities.GetBinaryStream("hello32le"), true);
            var symtab = (ISymbolTable)elf.GetSection(".symtab");
            var visibilites = symtab.Entries.GroupBy(x => x.Visibility).ToDictionary(x => x.Key, x => x.Count());
            Assert.AreEqual(61, visibilites[SymbolVisibility.Default]);
            Assert.AreEqual(3, visibilites[SymbolVisibility.Hidden]);
        }
    }
}