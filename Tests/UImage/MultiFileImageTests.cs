using System;
using System.Linq;
using ELFSharp.UImage;
using NUnit.Framework;

namespace Tests.UImage
{
	[TestFixture]
	public sealed class MultiFileTests
	{
        [Test]
        public void ShouldOpenMultiFileImage()
        {
            Assert.AreEqual(UImageResult.OK,
                UImageReader.TryLoad(Utilities.GetBinaryStream("mImage"), true, out _));
        }

        [Test]
        public void ShouldGetFirstImageFromMultiFileImage()
        {
            Assert.AreEqual(UImageResult.OK,
                UImageReader.TryLoad(Utilities.GetBinaryStream("mImage"), true, out var uImage));

            var imageData = uImage.GetImageData();
            CollectionAssert.AreEqual(new byte[] { 0x7F, (byte)'E', (byte)'L', (byte)'F' }, imageData[0..4]);
        }

        [Test]
        public void ShouldGetFirstImageFromMultiFileImageOnZeroIndex()
        {
            Assert.AreEqual(UImageResult.OK,
                UImageReader.TryLoad(Utilities.GetBinaryStream("mImage"), true, out var uImage));

            var imageData = uImage.GetImageData(0);
            CollectionAssert.AreEqual(new byte[] { 0x7F, (byte)'E', (byte)'L', (byte)'F' }, imageData[0..4]);
        }

        [Test]
        public void ShouldGetSecondImageFromMultiFileImage()
        {
            Assert.AreEqual(UImageResult.OK,
                UImageReader.TryLoad(Utilities.GetBinaryStream("mImage"), true, out var uImage));

            var imageData = uImage.GetImageData(1);
            CollectionAssert.AreEqual(new byte[] { 0xD0, 0x0D, 0xFE, 0xED }, imageData[0..4]);
        }
    }
}

