using System;
using System.IO;

namespace MiscUtil.IO
{
    public static class Streams
    {
        /// <summary>
        /// Copies the content of one stream to another in buffer-sized steps.
        /// </summary>
        /// <param name="source">The source stream to copy from.</param>
        /// <param name="destination">The destination stream to copy to.</param>
        /// <param name="bufferSize">The size of the buffer to use for copying in bytes.</param>
        /// <remarks>Will try to <see cref="Stream.Seek"/> to the start of <paramref name="source"/>.</remarks>
        public static void CopyTo(this Stream source, Stream destination, long bufferSize)
        {
            #region Sanity checks
            if (source == null) throw new ArgumentNullException("source");
            if (destination == null) throw new ArgumentNullException("destination");
            #endregion

            var buffer = new byte[bufferSize];
            int read;

            if (source.CanSeek) source.Position = 0;

            do
            {
                read = source.Read(buffer, 0, buffer.Length);
                destination.Write(buffer, 0, read);
            } while (read != 0);

            if (destination.CanSeek) destination.Position = 0;
        }

        /// <summary>
        /// Copies the content of one stream to another in one go.
        /// </summary>
        /// <param name="source">The source stream to copy from.</param>
        /// <param name="destination">The destination stream to copy to.</param>
        public static void CopyTo(this Stream source, Stream destination)
        {
            #region Sanity checks
            if (source == null) throw new ArgumentNullException("source");
            if (destination == null) throw new ArgumentNullException("destination");
            #endregion

            source.CopyTo(destination, 4096);
        }
    }
}
