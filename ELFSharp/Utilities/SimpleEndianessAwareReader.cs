using System;
using System.IO;
using System.Net;
using ELFSharp.ELF;

namespace ELFSharp.Utilities
{
    public sealed class SimpleEndianessAwareReader : IDisposable
    {
        public SimpleEndianessAwareReader(Stream stream, Endianess endianess, bool beNonClosing = false)
        {
            this.beNonClosing = beNonClosing;
            this.stream = stream;
            needsAdjusting = endianess == Endianess.LittleEndian ^ BitConverter.IsLittleEndian;
        }

        public void Dispose()
        {
            if(beNonClosing)
            {
                return;
            }
            stream.Dispose();
        }

        public byte[] ReadBytes(int count)
        {
            return stream.ReadBytesOrThrow(count);
        }

        public byte ReadByte()
        {
            var result = stream.ReadByte();
            if(result == -1)
            {
                throw new EndOfStreamException("End of stream reached while trying to read one byte.");
            }
            return (byte)result;
        }

        public short ReadInt16()
        {
            var value = BitConverter.ToInt16(ReadBytes(2), 0);
            if(needsAdjusting)
            {
                value = IPAddress.NetworkToHostOrder(value);
            }
            return value;
        }

        public ushort ReadUInt16()
        {
            return (ushort)ReadInt16();
        }

        public int ReadInt32()
        {
            var value = BitConverter.ToInt32(ReadBytes(4), 0);
            if(needsAdjusting)
            {
                value = IPAddress.NetworkToHostOrder(value);
            }
            return value;
        }

        public uint ReadUInt32()
        {
            return (uint)ReadInt32();
        }

        public long ReadInt64()
        {
            var value = BitConverter.ToInt64(ReadBytes(8), 0);
            if(needsAdjusting)
            {
                value = IPAddress.NetworkToHostOrder(value);
            }
            return value;
        }

        public ulong ReadUInt64()
        {
            return (ulong)ReadInt64();
        }

        public Stream BaseStream => stream;

        private readonly bool needsAdjusting;
        private readonly bool beNonClosing;
        private readonly Stream stream;
    }
}
