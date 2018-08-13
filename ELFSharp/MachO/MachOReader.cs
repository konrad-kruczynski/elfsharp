using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ELFSharp.MachO
{
    public static class MachOReader
    {
        public static MachO Load(string fileName, bool throwExceptions = true)
        {
            switch(TryLoad(fileName, out MachO result, throwExceptions))
            {
                case MachOResult.OK:
                    return result;
                case MachOResult.NotMachO:
                    throw new InvalidOperationException("Given file is not a Mach-O file.");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static MachO Load(Stream stream, bool throwExceptions = true)
        {
            switch(TryLoad(stream, out MachO result, throwExceptions))
            {
                case MachOResult.OK:
                    return result;
                case MachOResult.NotMachO:
                    throw new InvalidOperationException("Given file is not a Mach-O file.");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static MachOResult TryLoad(string fileName, out MachO machO, bool throwExceptions = true)
        {
            uint magic;
            using(var reader = new BinaryReader(File.OpenRead(fileName), Encoding.UTF8, true))
            {
                magic = reader.ReadUInt32();
                if(magic != Magic64 && magic != Magic32)
                {
                    machO = null;
                    return MachOResult.NotMachO;
                }
            }
            machO = new MachO(fileName, magic == Magic64, throwExceptions);
            return MachOResult.OK;
        }

        public static MachOResult TryLoad(Stream stream, out MachO machO, bool throwExceptions = true)
        {
            uint magic;
            using(var reader = new BinaryReader(stream, Encoding.UTF8, true))
            {
                magic = reader.ReadUInt32();
                if(magic != Magic64 && magic != Magic32)
                {
                    machO = null;
                    return MachOResult.NotMachO;
                }
            }
            machO = new MachO(stream, magic == Magic64, throwExceptions);
            return MachOResult.OK;
        }

        // Check if file is MachO without overhead of initializing MachO object
        public static MachOResult IsMachO(string fileName)
        {
            return IsMachO(File.OpenRead(fileName));
        }

        // Check if file is MachO without overhead of initializing MachO object
        public static MachOResult IsMachO(FileStream stream)
        {
            using(var reader = new BinaryReader(stream, Encoding.UTF8, true))
            {
                var magic = reader.ReadUInt32();
                if(magic != Magic64 && magic != Magic32)
                {
                    return MachOResult.NotMachO;
                }
            }
            return MachOResult.OK;
        }

        // Increment count when exception is encountered and throwExceptions = false
        public static void AddException(Dictionary<string, long> exceptions, string message)
        {
            if(!exceptions.ContainsKey(message))
                exceptions.Add(message, 1);
            else
            {
                exceptions[message]++;
            }
        }


        private const uint Magic32 = 0xFEEDFACE;
        private const uint Magic64 = 0xFEEDFACF;

    }
}

