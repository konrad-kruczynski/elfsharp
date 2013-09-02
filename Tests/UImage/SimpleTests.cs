using System;
using NUnit.Framework;
using ELFSharp.UImage;

namespace Tests.UImage
{
	[TestFixture]
	public class SimpleTests
	{
		[Test]
		public void ShouldOpenUImage()
		{
			var fileName = Utilities.GetBinaryLocation("uImage-panda");
			ELFSharp.UImage.UImage image;
			Assert.AreEqual(UImageResult.OK, UImageReader.TryLoad(fileName, out image));
		}

		[Test]
		public void ShouldNotOpenNotUImageFile()
		{
			var fileName = Utilities.GetBinaryLocation("notelf"); // not elf, nor uImage
			ELFSharp.UImage.UImage image;
			Assert.AreEqual(UImageResult.BadMagic, UImageReader.TryLoad(fileName, out image));
		}

		[Test]
		public void ShouldProperlyReadHeader()
		{
			var uImage = UImageReader.Load(Utilities.GetBinaryLocation("uImage-panda"));
			Assert.AreEqual(3120712, uImage.Size);
			Assert.AreEqual(0x80008000, uImage.EntryPoint);
			Assert.AreEqual(0x80008000, uImage.LoadAddress);
			Assert.AreEqual(0x6C77B32E, uImage.CRC);
			Assert.AreEqual("Linux-3.2.0", uImage.Name);
		}
	}
}

