using System;
using System.IO;
using System.Text;

namespace ELFSharp.MachO
{
    public static class MachOReader
    {
        public static MachO Load(string fileName)
        {
            return Load(File.OpenRead(fileName), true);
        }

        public static MachO Load(Stream stream, bool shouldOwnStream)
        {
            return (TryLoad(stream, shouldOwnStream, out MachO result)) switch
            {
                MachOResult.OK => result,
                MachOResult.NotMachO => throw new InvalidOperationException("Given file is not a Mach-O file."),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        public static MachOResult TryLoad(string fileName, out MachO machO)
        {
            return TryLoad(File.OpenRead(fileName), true, out machO);
        }

        public static MachOResult TryLoad(Stream stream, bool shouldOwnStream, out MachO machO)
        {
            machO = null;
            uint magic;

            var currentStreamPosition = stream.Position;
            using(var reader = new BinaryReader(stream, Encoding.UTF8, true))
            {
                magic = reader.ReadUInt32();
                if(magic != Magic64 && magic != Magic32)
                {
                    return MachOResult.NotMachO;
                }
            }

            stream.Position = currentStreamPosition;
            machO = new MachO(stream, magic == Magic64, shouldOwnStream);
            return MachOResult.OK;
        }

        private const uint Magic32 = 0xFEEDFACE;
        private const uint Magic64 = 0xFEEDFACF;

               
    }
}

