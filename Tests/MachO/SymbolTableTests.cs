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
            var machO = MachOReader.Load(Utilities.GetBinaryStream("shared-32-mach-o"), true);
            var symbolTable = machO.GetCommandsOfType<SymbolTable>().Single();
            CollectionAssert.IsSubsetOf(new [] { "_funkcja", "_inna_funkcja", "_jeszcze_inna_funkcja" }, symbolTable.Symbols.Select(x => x.Name).ToArray());
        }
    }
}

