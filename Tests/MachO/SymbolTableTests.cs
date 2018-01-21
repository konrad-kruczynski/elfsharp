using System;
using NUnit.Framework;
using ELFSharp.MachO;
using System.Linq;

namespace Tests.MachO
{
    [TestFixture]
    public class SymbolTableTests
    {
        [Test]
        public void ShouldListSymbols()
        {
            var fileName = Utilities.GetBinary("shared-32-mach-o");
            var machO = MachOReader.Load(fileName);
            var symbolTable = machO.GetCommandsOfType<SymbolTable>().Single();
            CollectionAssert.IsSubsetOf(new [] { "_funkcja", "_inna_funkcja", "_jeszcze_inna_funkcja" }, symbolTable.Symbols.Select(x => x.Name).ToArray());
        }
    }
}

