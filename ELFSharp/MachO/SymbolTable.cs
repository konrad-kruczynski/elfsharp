using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ELFSharp.Utilities;

namespace ELFSharp.MachO
{
    public class SymbolTable : Command
    {
        public SymbolTable(SimpleEndianessAwareReader reader, Stream stream, bool is64) : base(reader, stream)
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

            var streamPosition = Stream.Position;
            Stream.Seek(symbolTableOffset, SeekOrigin.Begin);

            try
            {
                for(var i = 0; i < numberOfSymbols; i++)
                {
                    var nameOffset = Reader.ReadInt32();
                    var name = ReadStringFromOffset(stringTableOffset + nameOffset);
                    Reader.ReadBytes(4); // ignoring for now
                    long value = is64 ? Reader.ReadInt64() : Reader.ReadInt32();
                    var symbol = new Symbol(name, value);
                    symbols[i] = symbol;
                }
            }
            finally
            {
                Stream.Position = streamPosition;
            }
        }

        private string ReadStringFromOffset(int offset)
        {
            var streamPosition = Stream.Position;
            Stream.Seek(offset, SeekOrigin.Begin);
            try
            {
                var asBytes = new List<byte>();
                int readByte;
                while((readByte = Stream.ReadByte()) != 0)
                {
                    if(readByte == -1)
                    {
                        throw new EndOfStreamException("Premature end of the stream while reading string.");
                    }
                    asBytes.Add((byte)readByte);
                }
                return Encoding.UTF8.GetString(asBytes.ToArray());
            }
            finally
            {
                Stream.Position = streamPosition;
            }
        }

        private Symbol[] symbols;
        private readonly bool is64;
    }
}

