using System;
using NUnit.Framework;
using ELFSharp.UImage;
using System.IO;

namespace Tests.UImage
{
	[TestFixture]
	public class SimpleTests
	{
		[Test]
		public void ShouldOpenUImage()
		{
            Assert.AreEqual(UImageResult.OK,
                UImageReader.TryLoad(Utilities.GetBinaryStream("uImage-panda"), true, out _));
        }

		[Test]
		public void ShouldNotOpenNotUImageFile()
		{
            // not elf, nor uImage
            Assert.AreEqual(UImageResult.NotUImage, UImageReader.TryLoad(Utilities.GetBinaryStream("notelf"), true, out _));
		}

		[Test]
		public void ShouldProperlyReadHeader()
		{
			var uImage = UImageReader.Load(Utilities.GetBinaryStream("uImage-panda"), true);
			Assert.AreEqual(3120712, uImage.Size);
			Assert.AreEqual(0x80008000, uImage.EntryPoint);
			Assert.AreEqual(0x80008000, uImage.LoadAddress);
			Assert.AreEqual(0x6C77B32E, uImage.CRC);
			Assert.AreEqual("Linux-3.2.0", uImage.Name);
		}

		[Test]
		public void ShouldProperlyReadTimestamp()
		{
			var uImage = UImageReader.Load(Utilities.GetBinaryStream("uImage-panda"), true);
			Assert.AreEqual(new DateTime(2012, 4, 10, 19, 11, 06, DateTimeKind.Utc).ToLocalTime(), uImage.Timestamp);
		}

		[Test]
		public void ShouldFailOnImageWithWrongChecksum()
		{
            Assert.AreEqual(UImageResult.BadChecksum,
                UImageReader.TryLoad(Utilities.GetBinaryStream("uImage-panda-wrng-cksm"), true, out _));
		}

		[Test]
		public void ShouldFindCorrectImageType()
		{
			var uImage = UImageReader.Load(Utilities.GetBinaryStream("uImage-panda"), true);
			Assert.AreEqual(ImageType.Kernel, uImage.Type);
		}

		[Test]
		public void ShouldExtractCorrectImage()
		{
			var extracted = Utilities.ReadWholeStream(Utilities.GetBinaryStream("vexpress-image-extracted"));

			CollectionAssert.AreEqual(extracted,
                UImageReader.Load(Utilities.GetBinaryStream("uImage-vexpress"), true).GetImageData());
		}

		[Test]
		public void ShouldGetProperOSValue()
		{
			var uImage = UImageReader.Load(Utilities.GetBinaryStream("uImage-vexpress"), true);
			Assert.AreEqual(OS.Linux, uImage.OperatingSystem);
		}

		[Test]
		public void ShouldGetProperArchitecture()
		{
			var uImage = UImageReader.Load(Utilities.GetBinaryStream("uImage-vexpress"), true);
			Assert.AreEqual(Architecture.ARM, uImage.Architecture);
		}
	}
}

