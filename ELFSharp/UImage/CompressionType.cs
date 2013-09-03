using System;

namespace ELFSharp.UImage
{
	public enum CompressionType : byte
	{
		None = 0,
		Gzip = 1,
		Bzip2 = 2,
		Lzma = 3,
		Lzo = 4
	}
}

