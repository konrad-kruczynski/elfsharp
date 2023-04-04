using System;
using NUnit.Framework;
using ELFSharp.MachO;
using System.IO;
using System.Linq;

namespace Tests.MachO
{
    [TestFixture]
    public class UUIDTests
	{
        [Test]
        public void ReadsUUID()
        {
            var archs = MachOReader.LoadFat(Utilities.GetBinaryStream("3dengine_libretro_ios.dylib"), true);

            foreach(var arch in archs)
            {
                var ids = arch.GetCommandsOfType<UUID>();

                // a valid macho can't have 0 LC_UUIDs 
                Assert.AreNotEqual(ids.Count(), 0);

                foreach(var id in ids)
                {
                    // for each arch the uuid must be specified
                    Assert.AreNotEqual(id.ID, null);
                }
            }
        }
    }
}

