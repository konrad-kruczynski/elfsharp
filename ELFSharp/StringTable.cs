using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ELFSharp
{
    public class StringTable : Section
    {
        internal StringTable(SectionHeader header, Func<BinaryReader> readerSource) : base(header, readerSource)
        {
            stringsByIdx = new Dictionary<uint, string>();
            ReadStrings();
        }

        private void ReadStrings()
        {
            using (var reader = ObtainReader())
            {
                reader.ReadByte(); // NULL char
                stringsByIdx.Add(0, string.Empty);
                // TODO: make buffered reader or sth better
                var currentIdx = 1u;
                var lastKey = 1u;
                var builder = new StringBuilder();
                while (currentIdx < Header.Size)
                {
                    currentIdx += 1;
                    var character = reader.ReadByte();
                    if (character == 0)
                    {
                        stringsByIdx.Add(lastKey, builder.ToString());
                        builder = new StringBuilder();
                        lastKey = currentIdx;
                        continue;
                    }
                    builder.Append((char) character);
                }
            }
        }

        public IEnumerable<string> Strings
        {
            get
            {
                return stringsByIdx.Values;
            }
        }

        internal string this[uint index]
        {
            get
            {
                if(stringsByIdx.ContainsKey(index))
                {
                    return stringsByIdx[index];
                }
                HandleUnexpectedIndex(index);
                return this[index];
            }
        }

        private void HandleUnexpectedIndex(uint index)
        {
            if(index >= Header.Size)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            var previousIndex = index;
            while (!stringsByIdx.ContainsKey(previousIndex))
            {
                previousIndex--;
            }
            stringsByIdx.Add(index, stringsByIdx[previousIndex].Substring(Convert.ToInt32(index - previousIndex)));
        }

        private readonly Dictionary<uint, string> stringsByIdx;
    }
}