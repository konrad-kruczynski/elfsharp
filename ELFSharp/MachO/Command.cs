using System;
using System.IO;

namespace ELFSharp.MachO
{
    public class Command
    {
        internal Command(BinaryReader reader, Func<Stream> streamProvider)
        {
            this.streamProvider = streamProvider;
            Reader = reader;
        }

        protected Stream ProvideStream()
        {
            return streamProvider();
        }

        protected readonly BinaryReader Reader;
        private readonly Func<Stream> streamProvider;
    }
}

