using System;
using NUnit.Framework;
using ELFSharp.MachO;

namespace Tests.MachO
{
    [TestFixture]
    public class OpeningTests
    {
        [Test]
        public void ShouldOpenMachO()
        {
            Assert.AreEqual(MachOResult.OK,
                MachOReader.TryLoad(Utilities.GetBinaryStream("simple-mach-o"), true, out _));
        }

        [Test]
        public void ShouldFindCorrectMachine()
        {
            var machO = MachOReader.Load(Utilities.GetBinaryStream("simple-mach-o"), true);
            Assert.AreEqual(Machine.X86_64, machO.Machine);
        }

        [Test]
        public void ShouldOpen32BitMachO()
        {
            var machO = MachOReader.Load(Utilities.GetBinaryStream("simple-32-mach-o"), true);
            Assert.AreEqual(Machine.I386, machO.Machine);
        }
    }
}

