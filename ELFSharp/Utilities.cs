using System;
using System.IO;

namespace ELFSharp
{
    public static class Utilities
    {
        public static byte[] ReadBytesOrThrow(this Stream stream, int count)
        {
            var result = new byte[count];
            while(count > 0)
            {
                count -= stream.Read(result, result.Length - count, count);
            }
            return result;
        }
    }
}

