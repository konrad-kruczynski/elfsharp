using ELFSharp.ELF;
using ELFSharp.ELF.Sections;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.ELF
{
    [TestFixture]
    public class DynamicSectionTests
    {
        [Test]
        public void ShouldReadDynamicSection32()
        {
            var elf = ELFReader.Load(Utilities.GetBinary("hello32le"));
            var dynamicSection = (IDynamicSection)elf.GetSection(".dynamic");
            Assert.AreEqual(SectionType.Dynamic, dynamicSection.Type);
        }

        [Test]
        public void ShouldReadDynamicSection64()
        {
            var elf = ELFReader.Load(Utilities.GetBinary("hello64le"));
            var dynamicSection = (IDynamicSection)elf.GetSection(".dynamic");
            Assert.AreEqual(SectionType.Dynamic, dynamicSection.Type);
        }

        [Test]
        public void ShouldHaveCorrectDynamicEntryCount32()
        {
            var elf = ELFReader.Load(Utilities.GetBinary("hello32le"));
            var dynamicSection = (IDynamicSection)elf.GetSection(".dynamic");
            Assert.AreEqual(25, dynamicSection.Entries.Count());
        }

        [Test]
        public void ShouldHaveCorrectDynamicEntryCount64()
        {
            var elf = ELFReader.Load(Utilities.GetBinary("hello64le"));
            var dynamicSection = (IDynamicSection)elf.GetSection(".dynamic");
            Assert.AreEqual(29, dynamicSection.Entries.Count());
        }

    }
}
