using System.IO;
using System.Collections.Generic;
using System.Linq;
using ELFSharp.Utilities;

namespace ELFSharp.MachO
{
    public sealed class MachO
    {
        internal MachO(Stream stream, bool is64, Endianess endianess, bool ownsStream)
        {
            this.is64 = is64;

            using var reader = new SimpleEndianessAwareReader(stream, endianess, ownsStream);

            Machine = (Machine)reader.ReadInt32();
            reader.ReadBytes(4); // we don't support the cpu subtype now
            FileType = (FileType)reader.ReadUInt32();
            var noOfCommands = reader.ReadInt32();
            reader.ReadInt32(); // size of commands
            reader.ReadBytes(4); // we don't support flags now
            if(is64)
            {
                reader.ReadBytes(4); // reserved
            }
            commands = new Command[noOfCommands];
            ReadCommands(noOfCommands, stream, reader);
        }

        public IEnumerable<T> GetCommandsOfType<T>()
        {
            return commands.Where(x => x != null).OfType<T>();
        }

        public Machine Machine { get; private set; }

        public FileType FileType { get; private set; }

        private void ReadCommands(int noOfCommands, Stream stream, SimpleEndianessAwareReader reader)
        {
            for(var i = 0; i < noOfCommands; i++)
            {
                var loadCommandType = reader.ReadUInt32();
                var commandSize = reader.ReadUInt32();
                switch((CommandType)loadCommandType)
                {
                case CommandType.SymbolTable:
                    commands[i] = new SymbolTable(reader, stream, is64);
                    break;
                case CommandType.Main:
                    commands[i] = new EntryPoint(reader, stream);
                    break;
                case CommandType.Segment:
                case CommandType.Segment64:
                    commands[i] = new Segment(reader, stream, is64);
                    break;
                default:
                    reader.ReadBytes((int)commandSize - 8); // 8 bytes is the size of the common command header
                    break;
                }
            }
        }

        private readonly bool is64;
        private readonly Command[] commands;

        internal const int Architecture64 = 0x1000000;
    }
}

