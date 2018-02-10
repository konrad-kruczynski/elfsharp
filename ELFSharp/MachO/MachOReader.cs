using System;
using System.IO;

namespace ELFSharp.MachO
{
    public static class MachOReader
    {
        public static MachO Load(string fileName)
        {
            switch(TryLoad(fileName, out MachO result))
            {
                case MachOResult.OK:
                    return result;
                case MachOResult.NotMachO:
                    throw new InvalidOperationException("Given file is not a Mach-O file.");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static MachOResult TryLoad(string fileName, out MachO machO)
        {
            machO = null;
            uint magic;
            using(var reader = new BinaryReader(File.OpenRead(fileName)))
            {
                magic = reader.ReadUInt32();
                if(magic != Magic64 && magic != Magic32)
                {
                    return MachOResult.NotMachO;
                }
            }
            machO = new MachO(fileName, magic == Magic64);
            return MachOResult.OK;
        }

        private const uint Magic32 = 0xFEEDFACE;
        private const uint Magic64 = 0xFEEDFACF;

               
    }
}

