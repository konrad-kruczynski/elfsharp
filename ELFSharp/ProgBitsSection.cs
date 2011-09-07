using System;
using System.IO;
using MiscUtil.IO;

namespace ELFSharp
{
    public class ProgBitsSection : Section
    {
        internal ProgBitsSection(SectionHeader header, Func<EndianBinaryReader> readerSource) : base(header, readerSource)
        {
        }
        

        public void WriteContents(byte[] destination, int offset, int length = 0)
        {
            using (var reader = ObtainReader())
            {
                if (length == 0 || length > Header.Size)
                {
                    length = Convert.ToInt32(Header.Size);
                }
                var remaining = length;
                while (remaining > 0)
                {
                    var buffer = reader.ReadBytes(Math.Min(BufferSize, remaining));
                    buffer.CopyTo(destination, offset + (length - remaining));
                    remaining -= buffer.Length;
                }
            }
        }

        private const int BufferSize = 1024;
    }
}