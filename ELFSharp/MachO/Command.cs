using System;
using System.IO;

namespace ELFSharp.MachO
{
    public class Command
    {
        internal Command(BinaryReader reader, Func<FileStream> streamProvider)
        {
            this.streamProvider = streamProvider;
            Reader = reader;
        }

        protected FileStream ProvideStream()
        {
            return streamProvider();
        }

        protected readonly BinaryReader Reader;
        private readonly Func<FileStream> streamProvider;
    }
}

