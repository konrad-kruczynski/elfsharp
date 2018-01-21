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
            var fileName = Utilities.GetBinary("simple-32-mach-o");
            var machO = MachOReader.Load(fileName);
            var segment = machO.GetCommandsOfType<Segment>().Single(x => x.Name == "__TEXT");
            var data = segment.Sections.Single(x => x.Name == "__cstring").GetData();
            Assert.AreEqual(Encoding.ASCII.GetBytes("Hello world!\n\0"), data);
        }

        [Test]
        public void ShouldFindSectionWithProperData64()
        {
            var fileName = Utilities.GetBinary("simple-mach-o");
            var machO = MachOReader.Load(fileName);
            var segment = machO.GetCommandsOfType<Segment>().Single(x => x.Name == "__TEXT");
            var data = segment.Sections.Single(x => x.Name == "__cstring").GetData();
            Assert.AreEqual(Encoding.ASCII.GetBytes("Hello world\0"), data);
        }
    }
}

