using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace Tests
{
    [SetUpFixture]
	public class Utilities
	{
		public static Stream GetBinaryStream(string name)
		{
            return typeof(Utilities).Assembly.GetManifestResourceStream(ResourcesPrefix + name);
        }

        public static string GetBinary(string name)
        {
            var fileName = Path.GetTempFileName();
            using(var fileStream = File.OpenWrite(fileName))
            using(var resourceStream = GetBinaryStream(name))
            {
                resourceStream.CopyTo(fileStream);
            }
            TemporaryFiles.Add(fileName);
            return fileName;
        }

        [OneTimeTearDown]
        public static void DeleteAllTemporaries()
        {
            foreach(var file in TemporaryFiles)
            {
                File.Delete(file);
            }
        }

        private static List<string> TemporaryFiles = new List<string>();

		private const string ResourcesPrefix = "Tests.Binaries.";
	}
}

