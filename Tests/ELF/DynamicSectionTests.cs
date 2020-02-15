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
            using var elf = ELFReader.Load(Utilities.GetBinaryStream("hello32le"), true);
            var dynamicSection = (IDynamicSection)elf.GetSection(".dynamic");
            Assert.AreEqual(SectionType.Dynamic, dynamicSection.Type);
        }

        [Test]
        public void ShouldReadDynamicSection64()
        {
            using var elf = ELFReader.Load(Utilities.GetBinaryStream("hello64le"), true);
            var dynamicSection = (IDynamicSection)elf.GetSection(".dynamic");
            Assert.AreEqual(SectionType.Dynamic, dynamicSection.Type);
        }

        [Test]
        public void ShouldHaveCorrectDynamicEntryCount32()
        {
            using var elf = ELFReader.Load(Utilities.GetBinaryStream("hello32le"), true);
            var dynamicSection = (IDynamicSection)elf.GetSection(".dynamic");
            Assert.AreEqual(25, dynamicSection.Entries.Count());
        }

        [Test]
        public void ShouldHaveCorrectDynamicEntryCount64()
        {
            using var elf = ELFReader.Load(Utilities.GetBinaryStream("hello64le"), true);
            var dynamicSection = (IDynamicSection)elf.GetSection(".dynamic");
            Assert.AreEqual(29, dynamicSection.Entries.Count());
        }

    }
}
