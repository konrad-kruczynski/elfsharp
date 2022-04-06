using System;
using System.Collections.Generic;
using NUnit.Framework;
using ELFSharp.MachO;
using System.Linq;

namespace Tests.MachO
{
    [TestFixture]
    public class SymbolTableTests
    {
        private IEnumerable<Symbol> symbols;

        [OneTimeSetUp]
        public void SetUp()
        {
            var machO = MachOReader.Load(Utilities.GetBinaryStream("3dengine_libretro_ios.dylib"), true);
            symbols = machO.GetCommandsOfType<SymbolTable>().Single().Symbols;
        }

        [Test]
        public void ShouldListSymbols()
        {
            var machO = MachOReader.Load(Utilities.GetBinaryStream("shared-32-mach-o"), true);
            var symbolTable = machO.GetCommandsOfType<SymbolTable>().Single();
            CollectionAssert.IsSubsetOf(new [] { "_funkcja", "_inna_funkcja", "_jeszcze_inna_funkcja" }, symbolTable.Symbols.Select(x => x.Name).ToArray());
        }

        // picked a few symbols with `nm -m 3dengine_libretro_ios.dylib`
        [TestCase("__ZL10first_init", "__DATA,__bss")]
        [TestCase("_video_cb", "__DATA,__common")]
        [TestCase("__ZL11cube_stride", "__DATA,__data")]
        [TestCase("GCC_except_table0", "__TEXT,__gcc_except_tab")]
        [TestCase("__Z14coll_detectionRN3glm6detail5tvec3IfEES3_", "__TEXT,__text")]
        [TestCase("__ZL11vertex_data", "__TEXT,__const")]
        [TestCase("__DefaultRuneLocale", null)]
        public void ShouldHaveCorrectSection(string symbolName, string section)
        {
            var symbol = symbols.First(e => e.Name == symbolName);
            Assert.AreEqual(section, symbol.Section == null ? null : $"{symbol.Section.SegmentName},{symbol.Section.Name}");
        }
    }
}

