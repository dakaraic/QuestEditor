// Copyright 2019 RED Software, LLC. All Rights Reserved.

using System.IO;
using System.Text;

namespace QuestEditor.IO
{
    /// <summary>
    /// Class to read from a <see cref="Stream"/> of binary.
    /// </summary>
    public class BinaryReader : System.IO.BinaryReader
    {
        /// <summary>
        /// The length of the stream.
        /// </summary>
        public long Length => BaseStream.Length;

        /// <summary>
        /// Creates a new instance of the <see cref="BinaryReader"/> class.
        /// </summary>
        /// <param name="input">The stream to read from.</param>
        public BinaryReader(Stream input) : base(input)
        {
        }

        /// <summary>
        /// Reads a string from the binary stream.
        /// </summary>
        /// <param name="length">The length of the bytes to read.</param>
        /// <returns>The string that was read.</returns>
        public string ReadString(int length)
        {
            var ret = string.Empty;
            var offset = 0;
            var buffer = ReadBytes(length);

            while (offset < length && buffer[offset] != 0x00)
            {
                offset++;
            }

            if (length > 0)
            {
                ret = Encoding.UTF8.GetString(buffer, 0, offset);
            }

            return ret;
        }
    }
}
