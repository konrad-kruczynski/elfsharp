using System;
using System.IO;
using System.Text;
using System.Linq;

namespace ELFSharp.UImage
{
	public sealed class UImage
	{
		internal UImage(string fileName)
		{
			using(var reader = new BinaryReader(File.OpenRead(fileName)))
			{
				reader.ReadBytes(8); // magic and CRC
				Timestamp = (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc) + TimeSpan.FromSeconds(reader.ReadInt32BigEndian())).ToLocalTime();
				Size = reader.ReadUInt32BigEndian();
				LoadAddress = reader.ReadUInt32BigEndian();
				EntryPoint = reader.ReadUInt32BigEndian();
				CRC = reader.ReadUInt32BigEndian();
				reader.ReadByte(); // OS
				reader.ReadByte(); // architecture
				reader.ReadByte(); // image type
				reader.ReadByte(); // compression type
				var nameAsBytes = reader.ReadBytes(32);
				Name = Encoding.ASCII.GetString(nameAsBytes.Reverse().SkipWhile(x => x == 0).Reverse().ToArray());
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

		public byte[] GetImageData()
		{
			throw new NotImplementedException();
		}

		private const int MaximumNameLength = 32;
	}
}

