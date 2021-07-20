using ELFSharp.MachO;
using NUnit.Framework;
using System.Linq;

namespace Tests.MachO
{
    [TestFixture]
    public class FileHeaderTests
    {
        [Test]
        public void ShouldLoadFileHeaderHasFlags()
        {
            var machO = MachOReader.Load(Utilities.GetBinaryStream("simple-mach-o"), true);
            var flags = machO.Flags;
            Assert.AreEqual(flags.HasFlag(HeaderFlags.NoUndefs), true);
            Assert.AreEqual(flags.HasFlag(HeaderFlags.DyldLink), true);
            Assert.AreEqual(flags.HasFlag(HeaderFlags.TwoLevel), true);
            Assert.AreEqual(flags.HasFlag(HeaderFlags.PIE), true);
        }

        [Test]
        public void ShouldLoadFileHasNoHeaderFlags()
        {
            var machO = MachOReader.Load(Utilities.GetBinaryStream("mach-o-dSYM-dwarf"), true);
            var flags = machO.Flags;
            Assert.AreEqual(flags, (HeaderFlags)0);
        }
    }
}

