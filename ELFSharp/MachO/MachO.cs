using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ELFSharp.MachO
{
    public sealed class MachO : IDisposable
    {
        internal MachO(string fileName, bool is64, bool throwExceptions = true) : this(fileName, null, is64, throwExceptions)
        {
        }

        internal MachO(Stream stream, bool is64, bool throwExceptions = true) : this(null, stream, is64, throwExceptions)
        {
        }

        MachO(string fileName, Stream stream, bool is64, bool throwExceptions)
        {
            this.fileName = fileName;
            this.stream = stream;
            this.is64 = is64;
            this.throwExceptions = throwExceptions; // throw immediately or store exceptions for handling atypical files
            using(var reader = new BinaryReader(OpenStream(), Encoding.UTF8, true))
            {
                this.stream.Seek(0, SeekOrigin.Begin);
                reader.ReadBytes(4); // header, already checked
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
                ReadCommands(noOfCommands, reader);
            }
        }


        public IEnumerable<T> GetCommandsOfType<T>()
        {
            return commands.Where(x => x != null).OfType<T>();
        }

        public Machine Machine { get; private set; }

        public FileType FileType { get; private set; }

        //Dictionary of exception messages and their counts. Only applicable when throwExceptions is false
        public readonly Dictionary<string, long> Exceptions = new Dictionary<string, long>();

        private void ReadCommands(int noOfCommands, BinaryReader reader)
        {
            for(var i = 0; i < noOfCommands; i++)
            {
                try
                {
                    var loadCommandType = reader.ReadUInt32();
                    var commandSize = reader.ReadUInt32();
                    switch((CommandType)loadCommandType)
                    {
                        case CommandType.SymbolTable:
                            commands[i] = new SymbolTable(reader, OpenStream, is64, throwExceptions ? null : Exceptions);
                            break;
                        case CommandType.Main:
                            commands[i] = new EntryPoint(reader, OpenStream);
                            break;
                        case CommandType.Segment:
                        case CommandType.Segment64:
                            commands[i] = new Segment(reader, OpenStream, is64, throwExceptions ? null : Exceptions);
                            break;
                        default:
                            reader.ReadBytes((int)commandSize - 8); // 8 bytes is the size of the common command header
                            break;
                    }
                }
                catch (Exception e)
                {
                    if(!throwExceptions)
                    {
                        MachOReader.AddException(Exceptions, e.Message);
                    }
                    throw;
                }
            }
        }

        private Stream OpenStream()
        {
            if(stream == null)
            {
                stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            return stream;
        }

        public void Dispose()
        {
            stream.Dispose();
        }

        private readonly bool is64;
        private readonly bool throwExceptions;
        private readonly string fileName;
        private readonly Command[] commands;
        private Stream stream;

        internal const int Architecture64 = 0x1000000;
    }
}

