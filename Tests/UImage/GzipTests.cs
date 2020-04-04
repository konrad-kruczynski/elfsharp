using NUnit.Framework;
using ELFSharp.UImage;

namespace Tests.UImage
{
    [TestFixture]
	public class GzipTests
	{
		[Test]
		public void ShouldExtractGzipCompressedUImage()
		{
            Assert.AreEqual(UImageResult.OK, UImageReader.TryLoad(Utilities.GetBinaryStream("uImage-gzip"), true, out var image));
            Assert.AreEqual(CompressionType.Gzip, image.Compression);

			CollectionAssert.AreEqual(
				Utilities.ReadWholeStream(Utilities.GetBinaryStream("uImage-gzip-extracted")),
				image.GetImageData());
		}
	}
}

