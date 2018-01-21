using System;
using NUnit.Framework;
using ELFSharp.UImage;
using System.IO;

namespace Tests.UImage
{
	[TestFixture]
	public class GzipTests
	{
		[Test]
		public void ShouldExtractGzipCompressedUImage()
		{
			var fileName = Utilities.GetBinary("uImage-gzip");
			ELFSharp.UImage.UImage image;
			Assert.AreEqual(UImageResult.OK, UImageReader.TryLoad(fileName, out image));
			Assert.AreEqual(CompressionType.Gzip, image.Compression);
			Assert.AreEqual(File.ReadAllBytes(Utilities.GetBinary("uImage-gzip-extracted")), image.GetImageData());
		}
	}
}

