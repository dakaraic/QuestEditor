// Copyright 2019 RED Software, LLC. All Rights Reserved.

using System.IO;

namespace QuestEditor.IO
{
    public static class BinaryWriterExtensions
    {
        public static void Fill(this BinaryWriter writer, int length)
        {
            for (var i = 0; i < length; i++)
            {
                writer.Write((byte) 0);
            }
        }
    }
}
