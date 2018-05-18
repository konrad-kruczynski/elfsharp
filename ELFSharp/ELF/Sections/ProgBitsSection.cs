using System;
using ELFSharp.Utilities;

namespace ELFSharp.ELF.Sections
{
    public sealed class ProgBitsSection<T> : Section<T>, IProgBitsSection where T : struct
    {
        internal ProgBitsSection(SectionHeader header, Func<SimpleEndianessAwareReader> readerSource) : base(header, readerSource)
        {
        }
        

        public void WriteContents(byte[] destination, int offset, int length = 0)
        {
            using (var reader = ObtainReader())
            {
                if (length == 0 || (ulong)length > Header.Size)
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

        private const int BufferSize = 10 * 1024;
    }
}