using System;
using System.Collections.Generic;
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

            using var reader = new BinaryReader(stream, Encoding.UTF8, true);
            var magic = reader.ReadUInt32();
            if(!MagicToMachOType.TryGetValue(magic, out var machOType))
            {
                return MachOResult.NotMachO;
            }

            machO = new MachO(stream, machOType.Is64Bit, machOType.Endianess, shouldOwnStream);
            return MachOResult.OK;
        }

        private static readonly IReadOnlyDictionary<uint, (bool Is64Bit, Endianess Endianess)> MagicToMachOType = new Dictionary<uint, (bool, Endianess)>
        {
            { 0xFEEDFACE, (false, Endianess.LittleEndian) },
            { 0xFEEDFACF, (true, Endianess.LittleEndian) },
            { 0xCEFAEDFE, (false, Endianess.BigEndian) },
            { 0xCFFEEDFE, (false, Endianess.LittleEndian) }
        };

    }
}

