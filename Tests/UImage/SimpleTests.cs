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
			var fileName = Utilities.GetBinary("uImage-panda");
            Assert.AreEqual(UImageResult.OK, UImageReader.TryLoad(fileName, out var image));
        }

		[Test]
		public void ShouldNotOpenNotUImageFile()
		{
			var fileName = Utilities.GetBinary("notelf"); // not elf, nor uImage
			Assert.AreEqual(UImageResult.NotUImage, UImageReader.TryLoad(fileName, out var image));
		}

		[Test]
		public void ShouldProperlyReadHeader()
		{
			var uImage = UImageReader.Load(Utilities.GetBinary("uImage-panda"));
			Assert.AreEqual(3120712, uImage.Size);
			Assert.AreEqual(0x80008000, uImage.EntryPoint);
			Assert.AreEqual(0x80008000, uImage.LoadAddress);
			Assert.AreEqual(0x6C77B32E, uImage.CRC);
			Assert.AreEqual("Linux-3.2.0", uImage.Name);
		}

		[Test]
		public void ShouldProperlyReadTimestamp()
		{
			var uImage = UImageReader.Load(Utilities.GetBinary("uImage-panda"));
			Assert.AreEqual(new DateTime(2012, 4, 10, 19, 11, 06, DateTimeKind.Utc).ToLocalTime(), uImage.Timestamp);
		}

		[Test]
		public void ShouldFailOnImageWithWrongChecksum()
		{
			Assert.AreEqual(UImageResult.BadChecksum, UImageReader.TryLoad(Utilities.GetBinary("uImage-panda-wrng-cksm"), out var image));
		}

		[Test]
		public void ShouldFindCorrectImageType()
		{
			var uImage = UImageReader.Load(Utilities.GetBinary("uImage-panda"));
			Assert.AreEqual(ImageType.Kernel, uImage.Type);
		}

		[Test]
		public void ShouldExtractCorrectImage()
		{
			Assert.AreEqual(File.ReadAllBytes(Utilities.GetBinary("vexpress-image-extracted")),
			                UImageReader.Load(Utilities.GetBinary("uImage-vexpress")).GetImageData());
		}

		[Test]
		public void ShouldGetProperOSValue()
		{
			var uImage = UImageReader.Load(Utilities.GetBinary("uImage-vexpress"));
			Assert.AreEqual(OS.Linux, uImage.OperatingSystem);
		}

		[Test]
		public void ShouldGetProperArchitecture()
		{
			var uImage = UImageReader.Load(Utilities.GetBinary("uImage-vexpress"));
			Assert.AreEqual(Architecture.ARM, uImage.Architecture);
		}
	}
}

