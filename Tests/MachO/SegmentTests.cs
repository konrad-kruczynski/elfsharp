using System;
using NUnit.Framework;
using ELFSharp.MachO;
using System.Linq;
using System.Text;

namespace Tests.MachO
{
    [TestFixture]
    public class SegmentTests
    {
        [Test]
        public void ShouldFindSectionWithProperData32()
        {
            var machO = MachOReader.Load(Utilities.GetBinaryStream("simple-32-mach-o"), true);
            var segment = machO.GetCommandsOfType<Segment>().Single(x => x.Name == "__TEXT");
            var data = segment.Sections.Single(x => x.Name == "__cstring").GetData();
            Assert.AreEqual(Encoding.ASCII.GetBytes("Hello world!\n\0"), data);
        }

        [Test]
        public void ShouldFindSectionWithProperData64()
        {
            var machO = MachOReader.Load(Utilities.GetBinaryStream("simple-mach-o"), true);
            var segment = machO.GetCommandsOfType<Segment>().Single(x => x.Name == "__TEXT");
            var data = segment.Sections.Single(x => x.Name == "__cstring").GetData();
            Assert.AreEqual(Encoding.ASCII.GetBytes("Hello world\0"), data);
        }

        [TestCase("__TEXT", "__text")]
        [TestCase("__DATA", "__bss")]
        public static void ShouldFindSectionWithDataAndNoDataArm64(string segmentName, string sectionName)
        {
            var machO = MachOReader.Load(Utilities.GetBinaryStream(@"3dengine_libretro_ios.dylib"), true);
            var segment = machO.GetCommandsOfType<Segment>().Single(x => x.Name == "__TEXT");
            var section = segment.Sections.Single(x => x.Name == "__text");
            section.GetData();
        }
    }
}

