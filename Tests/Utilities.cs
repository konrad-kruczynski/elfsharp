using System;
using System.Collections.Generic;
using System.IO;
using Force.Crc32;
using NUnit.Framework;

namespace Tests
{
    [SetUpFixture]
	public class Utilities
	{
		public static Stream GetBinaryStream(string name)
		{
            var result = typeof(Utilities).Assembly.GetManifestResourceStream(ResourcesPrefix + name);
            if(result == null)
            {
                throw new FileNotFoundException($"Could not find resource '{name}'.");
            }

            return result;
        }

        public static uint ComputeCrc32(byte[] data)
        {
            var algorithm = new Crc32Algorithm();
            var crc32AsBytes = algorithm.ComputeHash(data);
            return BitConverter.ToUInt32(crc32AsBytes, 0);
        }

        public static byte[] ReadWholeStream(Stream stream)
        {
            var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }

		private const string ResourcesPrefix = "Tests.Binaries.";
	}
}

