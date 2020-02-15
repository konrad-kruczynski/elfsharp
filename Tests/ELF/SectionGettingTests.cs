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
            using var elf = ELFReader.Load(Utilities.GetBinaryStream("issue3"), true);
            elf.GetSection(".rodata");
        }

        [Test]
        public void ShouldHandleNonExistingSection()
        {
            ISection section;
            Assert.IsFalse(ELFReader.Load(Utilities.GetBinaryStream("issue3"), true).TryGetSection(".nonexisting", out section));
        }

        [Test]
        public void ShouldHandleOutOfRangeSection()
        {
            ISection section;
            Assert.IsFalse(ELFReader.Load(Utilities.GetBinaryStream("issue3"), true).TryGetSection(28, out section));
        }

    }
}

