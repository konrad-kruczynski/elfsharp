using NUnit.Framework;
using ELFSharp.ELF;
using ELFSharp.ELF.Sections;
using ELFSharp;

namespace Tests.ELF
{
    [TestFixture]
    public class OpeningTests
    {
        [Test]
        public void ShouldChooseGoodClass32()
        {
            using var elf = ELFReader.Load(Utilities.GetBinaryStream("hello32le"), true);
            Assert.AreEqual(Class.Bit32, elf.Class);
        }
        
        [Test]
        public void ShouldChooseGoodClass64()
        {
            using var elf = ELFReader.Load(Utilities.GetBinaryStream("hello64le"), true);
            Assert.AreEqual(Class.Bit64, elf.Class);
        }
        
        [Test]
        public void ShouldOpenHelloWorld32()
        {
			ELFReader.Load(Utilities.GetBinaryStream("hello32le"), true);
        }
        
        [Test]
        public void ShouldOpenHelloWorld64()
        {           
            ELFReader.Load(Utilities.GetBinaryStream("hello64le"), true);
        }
        
        [Test]
        public void ShouldProperlyParseClass32()
        {
            var elf32 = ELFReader.Load<uint>(Utilities.GetBinaryStream("hello32le"), true);
            Assert.AreEqual(Class.Bit32, elf32.Class);          
        }

        [Test]
        public void ShouldProperlyParseClass64()
        {           
            var elf64 = ELFReader.Load<ulong>(Utilities.GetBinaryStream("hello64le"), true);
            Assert.AreEqual(Class.Bit64, elf64.Class);
        }
        
        [Test]
        public void ShouldProperlyParseEndianess()
        {
			using var elfLittleEndian = ELFReader.Load(Utilities.GetBinaryStream("hello32le"), true);
            Assert.AreEqual(Endianess.LittleEndian, elfLittleEndian.Endianess);
			using var elfBigEndian = ELFReader.Load(Utilities.GetBinaryStream("vmlinuxOpenRisc"), true);
            Assert.AreEqual(Endianess.BigEndian, elfBigEndian.Endianess);
        }
        
        [Test]
        public void ShouldOpenBigEndian()
        {
            ELFReader.Load(Utilities.GetBinaryStream("vmlinuxOpenRisc"), true);
        }

        // Github issue no 2
        [Test]
        public void ShouldOpenElfWithNonUniqueSectionNames()
        {
            ELFReader.Load(Utilities.GetBinaryStream("mpuG890.axf"), true);
        }

        // Github issue no 3
		[Test]
		public void ShouldLoadSharedObjectElfWithProgramHeaders()
		{
			ELFReader.Load(Utilities.GetBinaryStream("issue3"), true);
		}

		[Test]
		public void ShouldNotOpenNonELFFile()
		{
            Assert.IsFalse(ELFReader.TryLoad(Utilities.GetBinaryStream("notelf"), true, out var _));
        }

        // Github issue no 9
        [Test]
        public void ShouldOpenElfWithStripAll()
        {
            ELFReader.Load(Utilities.GetBinaryStream("stripped-all-binary"), true);
        }

        // Github issue no 24
        [Test]
        public void ShouldHandleCorruptedNamesInDynSym()
        {
            ELFReader.Load(Utilities.GetBinaryStream("issue24.elf"), true);
        }

        // Github issue no 49
        [Test]
        public void ShouldOpenEmptyStringTableElf()
        {
            using var elf = ELFReader.Load(Utilities.GetBinaryStream("libcoreclr"), true);
            var section = elf.GetSection(".dynstr");
            Assert.AreEqual(SectionType.NoBits, section.Type);
        }

        // Github issue no 91
        [Test]
        public void ShouldCloseOwnedStreamOnNonElf()
        {
            var stream = new System.IO.MemoryStream(new byte[] { 0, 1, 2, 3 });
            Assert.IsFalse(ELFReader.TryLoad(stream, true, out _));
            Assert.IsFalse(stream.CanRead);
        }

        [Test]
        public void ShouldDisposeStream()
        {
            var isDisposed = false;
            var stream = new StreamWrapper(Utilities.GetBinaryStream("hello32le"), () => isDisposed = true);
            ELFReader.Load(stream, shouldOwnStream: true).Dispose();
            Assert.True(isDisposed);
        }

        [Test]
        public void ShouldNotDisposeStream()
        {
            var isDisposed = false;
            var stream = new StreamWrapper(Utilities.GetBinaryStream("hello32le"), () => isDisposed = true);
            ELFReader.Load(stream, shouldOwnStream: false).Dispose();
            Assert.False(isDisposed);
        }
    }
}

