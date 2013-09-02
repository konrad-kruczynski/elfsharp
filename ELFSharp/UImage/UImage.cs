using System;

namespace ELFSharp.UImage
{
	public sealed class UImage
	{
		internal UImage(string fileName)
		{
			throw new NotImplementedException();
		}

		public uint CRC { get; private set; }
		public bool IsChecksumOK { get; private set; }
		public uint Size { get; private set; }
		public uint LoadAddress { get; private set; }
		public uint EntryPoint { get; private set; }
		public string Name { get; private set; }
		public CompressionType Compression { get; private set; }

		public byte[] GetImageData()
		{
			throw new NotImplementedException();
		}

		private const int MaximumNameLength = 32;
	}
}

