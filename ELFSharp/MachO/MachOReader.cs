using System;
using System.IO;

namespace ELFSharp.MachO
{
    public static class MachOReader
    {
        public static MachO Load(string fileName)
        {
            MachO result;
            switch(TryLoad(fileName, out result))
            {
            case MachOResult.OK:
                return result;
            case MachOResult.NotSupported:
                throw new InvalidOperationException("32 bit Mach-O files are not supported yet.");
            case MachOResult.NotMachO:
                throw new InvalidOperationException("Given file is not a Mach-O file.");
            default:
                throw new ArgumentOutOfRangeException();
            }
        }

        public static MachOResult TryLoad(string fileName, out MachO machO)
        {
            machO = null;
            using(var reader = new BinaryReader(File.OpenRead(fileName)))
            {
                var magic = reader.ReadUInt32();
                if(magic != Magic64)
                {
                    return magic == Magic32 ? MachOResult.NotSupported : MachOResult.NotMachO;
                }
            }
            machO = new MachO(fileName);
            return MachOResult.OK;
        }

        private const uint Magic32 = 0xFEEDFACE;
        private const uint Magic64 = 0xFEEDFACF;

               
    }
}

