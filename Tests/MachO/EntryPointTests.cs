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
            var machO = MachOReader.Load(Utilities.GetBinaryStream("simple-32-mach-o"), true);
            var entryPoint = machO.GetCommandsOfType<EntryPoint>().Single();
            Assert.AreEqual(0xF60, entryPoint.Value);
            Assert.AreEqual(0, entryPoint.StackSize);
        }

        [Test]
        public void ShouldFind64BitEntryPoint()
        {
            var machO = MachOReader.Load(Utilities.GetBinaryStream("simple-mach-o"), true);
            var entryPoint = machO.GetCommandsOfType<EntryPoint>().Single();
            Assert.AreEqual(0xF6B, entryPoint.Value);
            Assert.AreEqual(0, entryPoint.StackSize);
        }
    }
}

