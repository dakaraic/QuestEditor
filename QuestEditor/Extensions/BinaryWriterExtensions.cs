// Copyright 2019 RED Software, LLC. All Rights Reserved.

using System.IO;
using System.Text;

namespace QuestEditor.Extensions
{
    public static class BinaryWriterExtensions
    {
        public static void Write(this BinaryWriter writer, string value, int length)
        {
            var buffer = Encoding.ASCII.GetBytes(value);

            writer.Write(buffer);

            for (var i = 0; i < length - buffer.Length; i++)
            {
                writer.Write((byte) 0);
            }
        }
    }
}