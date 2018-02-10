using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ELFSharp.Utilities;

namespace ELFSharp.ELF.Sections
{
    public sealed class StringTable<T> : Section<T>, IStringTable where T : struct
    {
        internal StringTable(SectionHeader header, Func<SimpleEndianessAwareReader> readerSource) : base(header, readerSource)
        {
            stringCache = new Dictionary<long, string>
            {
                { 0, string.Empty }
            };
            stringBlob = ReadStringData();
        }

        public IEnumerable<string> Strings
        {
            get
            {
                if(!cachePopulated)
                {
                    PrepopulateCache();
                }
                return stringCache.Values;
            }
        }

        public string this[long index]
        {
            get
            {
                if(stringCache.TryGetValue(index, out string result))
                {
                    return result;
                }
                return HandleUnexpectedIndex(index);
            }
        }

        private string HandleUnexpectedIndex(long index)
        {
            var stringStart = (int)index;
            for(var i = stringStart; i < stringBlob.Length; ++i)
            {
                if(stringBlob[i] == 0)
                {
                    var str = Encoding.UTF8.GetString(stringBlob, stringStart, i - stringStart);
                    stringCache.Add(stringStart, str);
                    return str;
                }
            }
            throw new IndexOutOfRangeException();
        }

        private void PrepopulateCache()
        {
            cachePopulated = true;

            var stringStart = 1;
            for(var i = 1; i < stringBlob.Length; ++i)
            {
                if(stringBlob[i] == 0)
                {
                    if(!stringCache.ContainsKey(stringStart))
                    {
                        stringCache.Add(stringStart, Encoding.UTF8.GetString(stringBlob, stringStart, i - stringStart));
                    }
                    stringStart = i + 1;
                }
            }
        }

        private byte[] ReadStringData()
        {
            using(var reader = ObtainReader())
            {
                var blob = reader.ReadBytes((int)Header.Size);
                Debug.Assert(blob.Length == 0 || (blob[0] == 0 && blob[blob.Length - 1] == 0), "First and last bytes must be the null character (except for empty string tables)");
                return blob;
            }
        }

        private readonly Dictionary<long, string> stringCache;
        private readonly byte[] stringBlob;
        private bool cachePopulated;
    }
}
