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
            Assert.AreEqual(flags.HasFlag(HeaderFlags.NOUNDEFS), true);
            Assert.AreEqual(flags.HasFlag(HeaderFlags.DYLDLINK), true);
            Assert.AreEqual(flags.HasFlag(HeaderFlags.TWOLEVEL), true);
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

