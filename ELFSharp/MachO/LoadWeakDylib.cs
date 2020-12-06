using System.IO;
using ELFSharp.Utilities;

namespace ELFSharp.MachO
{
    public class LoadWeakDylib : Dylib
    {
        public LoadWeakDylib(SimpleEndianessAwareReader reader, Stream stream, uint commandSize) : base(reader, stream, commandSize)
        {
        }
    }
}