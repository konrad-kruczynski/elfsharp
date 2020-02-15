using System.Linq;
using NUnit.Framework;
using ELFSharp.ELF.Sections;
using ELFSharp.ELF;

namespace Tests.ELF
{
    [TestFixture]
    public class ProgBitsSectionTests
    {
        [Test]
        public void ShouldGetLoadAddress32()
        {
            var elf = ELFReader.Load<uint>(Utilities.GetBinaryStream("hello32le"), true);
            var sectionsToLoad = elf.GetSections<ProgBitsSection<uint>>().Where(x => x.LoadAddress != 0);
            Assert.AreEqual(13, sectionsToLoad.Count());
        }
        
        [Test]
        public void ShouldGetLoadAddress64()
        {
            var elf = ELFReader.Load<ulong>(Utilities.GetBinaryStream("hello64le"), true);
            var sectionsToLoad = elf.GetSections<ProgBitsSection<ulong>>().Where(x => x.LoadAddress != 0);
            Assert.AreEqual(12, sectionsToLoad.Count());
        }
    }
}

