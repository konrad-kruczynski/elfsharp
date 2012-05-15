using System;
using MiscUtil.IO;
using ELFSharp;

namespace ELFSharp
{
    public sealed class NoteSection<T> : Section<T>, INoteSection where T : struct
    {
        public string NoteName
        {
            get
            {
                return data.Name;
            }
        }
        
        public byte[] Description
        {
            get
            {
                return data.Description;
            }
        }

        public override string ToString()
        {
            return string.Format("[{0}: {1}, Type={2}]", Name, Description, Type);
        }
        
        public T NoteType
        {
            get
            {
                return data.Type.To<T>();
            }
        }
         
        internal NoteSection(SectionHeader header, Class elfClass, Func<EndianBinaryReader> readerSource) : base(header, readerSource)
        {
            data = new NoteData(elfClass, header.Offset, readerSource);
        }
        
        private readonly NoteData data;
    }   

}