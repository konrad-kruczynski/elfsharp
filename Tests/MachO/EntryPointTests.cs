using System;
using NUnit.Framework;
using ELFSharp.MachO;
using System.Linq;

namespace Tests.MachO
{
    [TestFixture]
    public class EntryPointTests
    {
        [Test]
        public void ShouldFind32BitEntryPoint()
        {
            var fileName = Utilities.GetBinary("simple-32-mach-o");
            var machO = MachOReader.Load(fileName);
            var entryPoint = machO.GetCommandsOfType<EntryPoint>().Single();
            Assert.AreEqual(0xF60, entryPoint.Value);
            Assert.AreEqual(0, entryPoint.StackSize);
        }

        [Test]
        public void ShouldFind64BitEntryPoint()
        {
            var fileName = Utilities.GetBinary("simple-mach-o");
            var machO = MachOReader.Load(fileName);
            var entryPoint = machO.GetCommandsOfType<EntryPoint>().Single();
            Assert.AreEqual(0xF6B, entryPoint.Value);
            Assert.AreEqual(0, entryPoint.StackSize);
        }
    }
}

