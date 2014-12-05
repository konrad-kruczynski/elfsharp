using System;
using NUnit.Framework;
using ELFSharp.ELF;
using ELFSharp.ELF.Sections;

namespace Tests.ELF
{
    [TestFixture]
    public class SectionGettingTests
    {
        [Test]
        public void ShouldGetSection()
        {
            var elf = ELFReader.Load(Utilities.GetBinaryLocation("issue3"));
            elf.GetSection(".rodata");
        }

        [Test]
        public void ShouldHandleNonExistingSection()
        {
            ISection section;
            Assert.IsFalse(ELFReader.Load(Utilities.GetBinaryLocation("issue3")).TryGetSection(".nonexisting", out section));
        }

        [Test]
        public void ShouldHandleOutOfRangeSection()
        {
            ISection section;
            Assert.IsFalse(ELFReader.Load(Utilities.GetBinaryLocation("issue3")).TryGetSection(28, out section));
        }

    }
}

