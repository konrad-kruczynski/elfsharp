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

	}
}

