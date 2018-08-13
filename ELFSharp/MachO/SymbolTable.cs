using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ELFSharp.MachO
{
    public class SymbolTable : Command
    {
        public SymbolTable(BinaryReader reader, Func<Stream> streamProvider, bool is64, Dictionary<string, long> exceptions) : base(reader, streamProvider)
        {
            this.is64 = is64;
            ReadSymbols(exceptions);
        }

        public IEnumerable<Symbol> Symbols
        {
            get
            {
                return symbols.Select(x => x);
            }
        }

        private void ReadSymbols(Dictionary<string, long> exceptions)
        {
            var symbolTableOffset = Reader.ReadInt32();
            var numberOfSymbols = Reader.ReadInt32();
            symbols = new Symbol[numberOfSymbols];
            var stringTableOffset = Reader.ReadInt32();
            Reader.ReadInt32(); // string table size

            var symbolStream = ProvideStream();
            var headerPosition = symbolStream.Position; // location before seeking to symbol table
            symbolStream.Seek(symbolTableOffset, SeekOrigin.Begin);
            using(var symbolReader = new BinaryReader(symbolStream, Encoding.UTF8, true))
            {
                var stringTableStream = ProvideStream();
                for(var i = 0; i < numberOfSymbols; i++)
                {
                    try
                    {
                        var nameOffset = symbolReader.ReadInt32();
                        var symbolTablePosition = stringTableStream.Position; // location before reading from string table
                        var name = ReadStringFromOffset(stringTableStream, stringTableOffset + nameOffset);
                        stringTableStream.Position = symbolTablePosition;
                        symbolReader.ReadBytes(4); // ignoring for now
                        long value = is64 ? symbolReader.ReadInt64() : symbolReader.ReadInt32();
                        symbols[i] = new Symbol(name, value);
                    }
                    catch(Exception e)
                    {
                        if(exceptions != null)
                        {
                            MachOReader.AddException(exceptions, e.Message);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
            symbolStream.Position = headerPosition;
        }

        private static string ReadStringFromOffset(Stream stream, int offset)
        {
            stream.Seek(offset, SeekOrigin.Begin);
            var asBytes = new List<byte>();
            int readByte;
            while((readByte = stream.ReadByte()) != 0)
            {
                if(readByte == -1)
                {
                    throw new EndOfStreamException();
                }
                asBytes.Add((byte)readByte);
            }
            return Encoding.UTF8.GetString(asBytes.ToArray());
        }

        private Symbol[] symbols;
        private readonly bool is64;
    }
}

