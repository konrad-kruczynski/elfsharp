using System.Linq;
using NUnit.Framework;
using ELFSharp;

namespace Tests
{
    [TestFixture]
    public class ProgBitsSectionTests
    {
        [Test]
        public void ShouldGetLoadAddress32()
        {
            var elf = ELFReader.Load<uint>("hello32le");
            var sectionsToLoad = elf.GetSections<ProgBitsSection<uint>>().Where(x => x.LoadAddress != 0);
            Assert.AreEqual(13, sectionsToLoad.Count());
        }
        
        [Test]
        public void ShouldGetLoadAddress64()
        {
            var elf = ELFReader.Load<long>("hello64le");
            var sectionsToLoad = elf.GetSections<ProgBitsSection<long>>().Where(x => x.LoadAddress != 0);
            Assert.AreEqual(14, sectionsToLoad.Count());
        }
    }
}

