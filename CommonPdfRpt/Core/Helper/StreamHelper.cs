﻿using System;
using System.IO;
using System.Reflection;

namespace PdfRpt.Core.Helper
{
    /// <summary>
    /// Embedded resources helper class.
    /// </summary>
    public static class StreamHelper
    {
        /// <summary>
        /// Reads an embedded resource
        /// </summary>
        /// <param name="fileName">embedded resource file</param>
        /// <returns>embedded resource</returns>
        public static byte[] GetResourceByName(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream(fileName);
            return StreamToBytes(stream);
        }

        /// <summary>
        /// Converts StreamToBytes 
        /// </summary>
        /// <param name="input">stream</param>
        /// <returns>array of bytes</returns>
        public static byte[] StreamToBytes(this Stream input)
        {
            var capacity = input.CanSeek ? (int)input.Length : 0;
            using (var output = new MemoryStream(capacity))
            {
                int readLength;
                var buffer = new byte[4096];

                do
                {
                    readLength = input.Read(buffer, 0, buffer.Length);
                    output.Write(buffer, 0, readLength);
                }
                while (readLength != 0);

                return output.ToArray();
            }
        }

        /// <summary>
        /// Tries to Reopen the stream for writing.
        /// </summary>
        /// <param name="stream">input stream</param>
        /// <returns></returns>
        public static Stream ReopenForWriting(this Stream stream)
        {
            if (stream.CanRead && stream.CanSeek && stream.CanWrite)
            {
                stream.Position = 0;
                stream.SetLength(0);
                return stream;
            }

            var fileStream = stream as FileStream;
            if (fileStream != null)
            {
                return new FileStream(fileStream.Name, FileMode.Create, FileAccess.Write);
            }
            else
            {
                return new MemoryStream();
            }
        }

        /// <summary>
        /// Tries to Reopen the stream for reading.
        /// </summary>
        /// <param name="stream">input stream</param>
        /// <returns></returns>
        public static Stream ReopenForReading(this Stream stream)
        {
            if (stream.CanRead && stream.CanSeek && stream.CanWrite)
            {
                stream.Position = 0;                
                return stream;
            }

            var fileStream = stream as FileStream;
            if (fileStream != null)
            {
                return new FileStream(fileStream.Name, FileMode.Open, FileAccess.Read, FileShare.Read);
            }

            throw new InvalidOperationException("Can not ReopenForReading.");
        }
    }
}