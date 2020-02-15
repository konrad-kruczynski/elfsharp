using System.Linq;
using NUnit.Framework;
using ELFSharp.ELF;

namespace Tests.ELF
{
    [TestFixture]
    public class StringTableTests
    {
        [Test]
        public void ShouldFindAllStrings()
        {
            using var elf = ELFReader.Load(Utilities.GetBinaryStream("hello32le"), true);
            Assert.IsTrue(elf.HasSectionsStringTable, 
                          "Sections string table was not found in 32 bit ELF.");
            Assert.AreEqual(29, elf.SectionsStringTable.Strings.Count());
        }
        
        [Test]
        public void ShouldFindAllStrings64()
        {
            using var elf = ELFReader.Load(Utilities.GetBinaryStream("hello64le"), true);
            Assert.IsTrue(elf.HasSectionsStringTable, 
                          "Sections string table was not found in 64 bit ELF.");
            Assert.AreEqual(30, elf.SectionsStringTable.Strings.Count());
        }
        
        [Test]
        public void ShouldFindAllStringsBigEndian()
        {
            using var elf = ELFReader.Load(Utilities.GetBinaryStream("vmlinuxOpenRisc"), true);
            Assert.IsTrue(
                elf.HasSectionsStringTable,
                "Sections string table was not found in 32 bit big endian ELF."
            );
            Assert.AreEqual(28, elf.SectionsStringTable.Strings.Count());
        }
    }
}

