using System;
using System.Collections.Generic;
using System.IO;
using ELFSharp.MachO;

namespace ELFSharp.Utilities
{
    internal static class Extensions
    {
        public static byte[] ReadBytesOrThrow(this Stream stream, int count, Dictionary<string, long> exceptionsToQueue = null)
        {
            var result = new byte[count];
            while(count > 0)
            {
                var readThisTurn = stream.Read(result, result.Length - count, count);
                if(readThisTurn == 0)
                {
                    if(exceptionsToQueue != null)
                    {
                        MachOReader.AddException(exceptionsToQueue, new EndOfStreamException().Message);
                        Array.Resize(ref result, result.Length - count);
                        return result;
                    }
                    throw new EndOfStreamException($"End of stream reached while {count} bytes more expected.");
                }
                count -= readThisTurn;
            }
            return result;
        }
    }
}
