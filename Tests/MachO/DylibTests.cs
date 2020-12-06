using System;
using System.Linq;
using ELFSharp.MachO;
using NUnit.Framework;

namespace Tests.MachO
{
    [TestFixture]
    public class DylibTests
    {
        [Test]
        public void ShouldFindIdDylib()
        {
            var machO = MachOReader.Load(Utilities.GetBinaryStream("3dengine_libretro_ios.dylib"), true);
            var idDylib = machO.GetCommandsOfType<IdDylib>().Single();

            Assert.AreEqual("3dengine_libretro_ios.dylib", idDylib.Name);
            Assert.AreEqual(new DateTime(1970, 1, 1, 0, 0, 1, 0, DateTimeKind.Utc), idDylib.Timestamp);
            Assert.AreEqual(new Version(0, 0, 0), idDylib.CurrentVersion);
            Assert.AreEqual(new Version(0, 0, 0), idDylib.CompatibilityVersion);
        }

        [Test]
        public void ShouldFindLoadDylibs()
        {
            var machO = MachOReader.Load(Utilities.GetBinaryStream("3dengine_libretro_ios.dylib"), true);
            var loadDylibs = machO.GetCommandsOfType<LoadDylib>().ToList();

            Assert.AreEqual(3, loadDylibs.Count);

            Assert.AreEqual("/System/Library/Frameworks/OpenGLES.framework/OpenGLES", loadDylibs[0].Name);
            Assert.AreEqual(new DateTime(1970, 1, 1, 0, 0, 2, 0, DateTimeKind.Utc), loadDylibs[0].Timestamp);
            Assert.AreEqual(new Version(1, 0, 0), loadDylibs[0].CurrentVersion);
            Assert.AreEqual(new Version(1, 0, 0), loadDylibs[0].CompatibilityVersion);

            Assert.AreEqual("/usr/lib/libSystem.B.dylib", loadDylibs[1].Name);
            Assert.AreEqual(new DateTime(1970, 1, 1, 0, 0, 2, 0, DateTimeKind.Utc), loadDylibs[1].Timestamp);
            Assert.AreEqual(new Version(1281, 0, 0), loadDylibs[1].CurrentVersion);
            Assert.AreEqual(new Version(1, 0, 0), loadDylibs[1].CompatibilityVersion);

            Assert.AreEqual("/usr/lib/libc++.1.dylib", loadDylibs[2].Name);
            Assert.AreEqual(new DateTime(1970, 1, 1, 0, 0, 2, 0, DateTimeKind.Utc), loadDylibs[2].Timestamp);
            Assert.AreEqual(new Version(800, 7, 0), loadDylibs[2].CurrentVersion);
            Assert.AreEqual(new Version(1, 0, 0), loadDylibs[2].CompatibilityVersion);
        }
    }
}