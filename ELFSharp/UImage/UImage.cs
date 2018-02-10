using System;
using System.IO;
using System.Text;
using System.Linq;
using System.IO.Compression;

namespace ELFSharp.UImage
{
	public sealed class UImage
	{
		internal UImage(string fileName)
		{
			using(var reader = new BinaryReader(File.OpenRead(fileName)))
			{
				reader.ReadBytes(8); // magic and CRC, already checked
				Timestamp = (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc) + TimeSpan.FromSeconds(reader.ReadInt32BigEndian())).ToLocalTime();
				Size = reader.ReadUInt32BigEndian();
				LoadAddress = reader.ReadUInt32BigEndian();
				EntryPoint = reader.ReadUInt32BigEndian();
				CRC = reader.ReadUInt32BigEndian();
				OperatingSystem = (OS)reader.ReadByte();
				Architecture = (Architecture)reader.ReadByte();
				Type = (ImageType)reader.ReadByte();
				Compression = (CompressionType)reader.ReadByte();
				var nameAsBytes = reader.ReadBytes(32);
				Name = Encoding.UTF8.GetString(nameAsBytes.Reverse().SkipWhile(x => x == 0).Reverse().ToArray());
				image = reader.ReadBytes((int)Size);
			}
		}

		public uint CRC { get; private set; }
		public bool IsChecksumOK { get; private set; }
		public uint Size { get; private set; }
		public uint LoadAddress { get; private set; }
		public uint EntryPoint { get; private set; }
		public string Name { get; private set; }
		public DateTime Timestamp { get; private set; }
		public CompressionType Compression { get; private set; }
		public ImageType Type { get; private set; }
		public OS OperatingSystem { get; private set; }
		public Architecture Architecture { get; private set; }

		public ImageDataResult TryGetImageData(out byte[] result)
		{
			result = null;
			if(Compression != CompressionType.None && Compression != CompressionType.Gzip)
			{
				return ImageDataResult.UnsupportedCompressionFormat;
			}
			if(CRC != UImageReader.GzipCrc32(image))
			{
				return ImageDataResult.BadChecksum;
			}
			result = new byte[image.Length];
			Array.Copy(image, result, result.Length);
			if(Compression == CompressionType.Gzip)
			{
				using(var stream = new GZipStream(new MemoryStream(result), CompressionMode.Decompress))
				{
					using(var decompressed = new MemoryStream())
					{
						stream.CopyTo(decompressed);
						result = decompressed.ToArray();
					}
				}
			}
			return ImageDataResult.OK;
		}

		public byte[] GetImageData()
		{
			byte[] result;
			switch(TryGetImageData(out result))
			{
			case ImageDataResult.OK:
				return result;
			case ImageDataResult.BadChecksum:
				throw new InvalidOperationException("Bad checksum of the image, probably corrupted image.");
			case ImageDataResult.UnsupportedCompressionFormat:
				throw new InvalidOperationException(string.Format("Unsupported compression format '{0}'.", Compression));
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		public byte[] GetRawImageData()
		{
			var result = new byte[image.Length];
			Array.Copy(image, result, result.Length);
			return result;
		}

		private const int MaximumNameLength = 32;
		private readonly byte[] image;
	}
}

