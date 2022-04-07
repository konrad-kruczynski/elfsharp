using System;
using System.Collections.Generic;
using ELFSharp.ELF.Sections;
using ELFSharp.Utilities;

namespace ELFSharp.ELF.Segments
{
    public sealed class NoteSegment<T> : Segment<T>, INoteSegment
    {
        private List<NoteData> mNotes = new List<NoteData>();

        internal NoteSegment(long headerOffset, Class elfClass, SimpleEndianessAwareReader reader)
            : base(headerOffset, elfClass, reader)
        {
            ulong offset = (ulong)base.Offset;
            ulong fileSize = (ulong)base.FileSize;
            ulong remainingSize = fileSize;

            // Keep the first NoteData as a property for backwards compatibility
            data = new NoteData(offset, remainingSize, reader);
            mNotes.Add(data);

            offset += data.NoteFileSize;

            // Read all additional notes within the segment
            // Multiple notes are common in ELF core files
            if (data.NoteFileSize < remainingSize)
            {
                remainingSize -= data.NoteFileSize;

                while (remainingSize > NoteData.NOTE_DATA_HEADER_SIZE)
                {
                    var note = new NoteData(offset, remainingSize, reader);
                    mNotes.Add(note);
                    offset += note.NoteFileSize;
                    if (note.NoteFileSize < remainingSize)
                    {
                        remainingSize -= note.NoteFileSize;
                    }
                    else
                    {
                        // File is damaged
                        break;
                    }
                }
            }
        }

        public string NoteName => data.Name;

        public ulong NoteType => data.Type;

        public byte[] NoteDescription => data.Description;

        public IReadOnlyList<INoteData> Notes { get => mNotes.AsReadOnly(); }

        private readonly NoteData data;
    }
}
