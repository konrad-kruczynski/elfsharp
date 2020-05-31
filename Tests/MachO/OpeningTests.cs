using NUnit.Framework;
using ELFSharp.MachO;
using System;

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
            Assert.AreEqual(Machine.X86, machO.Machine);
        }

        [Test]
        public void ShouldOpenBigEndianMachO()
        {
            var machO = MachOReader.Load(Utilities.GetBinaryStream("MachO-OSX-ppc-openssl-1.0.1h"), true);
            Assert.AreEqual(Machine.PowerPc, machO.Machine);
        }

        [Test]
        public void ShouldOpenFatMachO()
        {
            var machOs = MachOReader.LoadFat(Utilities.GetBinaryStream("Undecimus"), true);
            Assert.AreEqual(2, machOs.Count);
            Assert.AreEqual(Machine.Arm64, machOs[0].Machine);
            Assert.AreEqual(Machine.Arm64, machOs[1].Machine);
        }

        [Test]
        public void ShouldThrowOnFatMachOWhenOpenedAsNotFat()
        {
            Assert.Throws<InvalidOperationException>(() => MachOReader.Load(Utilities.GetBinaryStream("Undecimus"), true));
        }

        [Test]
        public void ShouldRecognizeFatMachO()
        {
            var result = MachOReader.TryLoad(Utilities.GetBinaryStream("Undecimus"), true, out _);
            Assert.AreEqual(MachOResult.FatMachO, result);
        }

        [Test]
        public void ShouldOpenNotFatMachOUsingFatMethod()
        {
            var machOs = MachOReader.LoadFat(Utilities.GetBinaryStream("MachO-OSX-ppc-openssl-1.0.1h"), true);
            Assert.AreEqual(1, machOs.Count);
            Assert.AreEqual(Machine.PowerPc, machOs[0].Machine);
        }
    }
}

