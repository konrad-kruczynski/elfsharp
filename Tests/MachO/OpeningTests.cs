using System.IO;
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
            var fileName = Utilities.GetBinary("simple-mach-o");
            Assert.AreEqual(MachOResult.OK, MachOReader.IsMachO(fileName));
        }

        [Test]
        public void ShouldHaveZeroExceptions()
        {
            var fileName = Utilities.GetBinary("simple-mach-o");
            var machO = MachOReader.Load(fileName, false);
            Assert.AreEqual(0, machO.Exceptions.Count);
        }

        [Test]
        public void ShouldFindCorrectMachine()
        {
            var fileName = Utilities.GetBinary("simple-mach-o");
            var machO = MachOReader.Load(fileName);
            Assert.AreEqual(Machine.X86_64, machO.Machine);
        }

        [Test]
        public void ShouldOpen32BitMachO()
        {
            var fileName = Utilities.GetBinary("simple-32-mach-o");
            var machO = MachOReader.Load(fileName);
            Assert.AreEqual(Machine.I386, machO.Machine);
        }

        [Test]
        public void ShouldOpen32BitMachOAsStream()
        {
            var fileName = Utilities.GetBinary("simple-32-mach-o");
            var machO = MachOReader.Load(File.OpenRead(fileName));
            Assert.AreEqual(Machine.I386, machO.Machine);
        }
    }
}

