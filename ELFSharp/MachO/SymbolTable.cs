using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ELFSharp.MachO
{
    public class SymbolTable : Command
    {
        public SymbolTable(BinaryReader reader, Func<FileStream> streamProvider, bool is64) : base(reader, streamProvider)
        {
            this.is64 = is64;
            ReadSymbols();
        }

        public IEnumerable<Symbol> Symbols
        {
            get
            {
                return symbols.Select(x => x);
            }
        }

        private void ReadSymbols()
        {
            var symbolTableOffset = Reader.ReadInt32();
            var numberOfSymbols = Reader.ReadInt32();
            symbols = new Symbol[numberOfSymbols];
            var stringTableOffset = Reader.ReadInt32();
            Reader.ReadInt32(); // string table size

            var symbolStream = ProvideStream();
            symbolStream.Seek(symbolTableOffset, SeekOrigin.Begin);
            var symbolReader = new BinaryReader(symbolStream);
            var stringTableStream = ProvideStream();
            try
            {
                for(var i = 0; i < numberOfSymbols; i++)
                {
                    var nameOffset = symbolReader.ReadInt32();
                    var name = ReadStringFromOffset(stringTableStream, stringTableOffset + nameOffset);
                    symbolReader.ReadBytes(4); // ignoring for now
                    long value = is64 ? symbolReader.ReadInt64() : symbolReader.ReadInt32();
                    var symbol = new Symbol(name, value);
                    symbols[i] = symbol;
                }
            }
            finally
            {
                symbolReader.Close();
                stringTableStream.Close();
            }
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
                    throw new EndOfStreamException("Premature end of the stream while reading string.");
                }
                asBytes.Add((byte)readByte);
            }
            return Encoding.UTF8.GetString(asBytes.ToArray());
        }

        private Symbol[] symbols;
        private readonly bool is64;
    }
}

