// Copyright 2019 RED Software, LLC. All Rights Reserved.

using System;
using System.Data;
using System.IO;

namespace QuestEditor.IO
{
    /// <summary>
    /// Class to load and access SHN files.
    /// </summary>
    public class SHNFile : IDisposable
    {
        /// <summary>
        /// The path of the file.
        /// </summary>
        private readonly string fileName;

        /// <summary>
        /// The underlying DataTable, which holds all of the file's
        /// data.
        /// </summary>
        public DataTable Table { get; set; }

        public byte[] ChecksumData { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="SHNFile"/> class.
        /// </summary>
        public SHNFile(string path)
        {
            fileName = path;
            Table = new DataTable(Path.GetFileName(path));

            PopulateTable();
        }

        /// <summary>
        /// Decrypts an array of bytes.
        /// </summary>
        /// <param name="input">The byte array being decrypted.</param>
        /// <param name="length">The length of the byte array.</param>
        /// <param name="output">The result of the decryption.</param>
        private void DecryptBuffer(byte[] input, int length, out byte[] output)
        {
            output = input;

            var xor = (byte)length;

            for (var i = length - 1; i >= 0; i--)
            {
                output[i] ^= xor;
                ChecksumData[i + 36] ^= xor;

                var nextXor = (byte)i;

                nextXor &= 0xF;
                nextXor += 0x55;
                nextXor ^= (byte)(i * 0xB);
                nextXor ^= xor;
                nextXor ^= 0xAA;

                xor = nextXor;
            }
        }

        /// <summary>
        /// Gets the data type for a column.
        /// </summary>
        /// <param name="type">The byte value in the file.</param>
        /// <returns>The column's data type.</returns>
        private static Type GetColumnDataType(uint type)
        {
            switch (type)
            {
                case 1:
                case 12:
                case 16:
                    return typeof(byte);
                case 2:
                    return typeof(ushort);
                case 3:
                case 11:
                case 18:
                case 27:
                    return typeof(uint);
                case 5:
                    return typeof(float);
                case 13:
                case 21:
                    return typeof(short);
                case 20:
                    return typeof(sbyte);
                case 22:
                    return typeof(int);
                case 9:
                case 24:
                case 26:
                    return typeof(string);
                default:
                    return typeof(object);
            }
        }

        /// <summary>
        /// Disposes the file handle.
        /// </summary>
        public void Dispose()
        {
            Table.Dispose();
        }

        /// <summary>
        /// Populates the SHNFile's table with the file's data.
        /// </summary>
        private void PopulateTable()
        {
            if (!File.Exists(fileName))
            {
                return;
            }

            byte[] buffer;
            ChecksumData = File.ReadAllBytes(fileName);

            using (var file = File.OpenRead(fileName))
            using (var reader = new BinaryReader(file))
            {
                reader.ReadBytes(32); // Crypt header, unused.

                var length = reader.ReadInt32();
                var bytes = reader.ReadBytes(length - 36);

                DecryptBuffer(bytes, bytes.Length, out buffer);
            }

            using (var stream = new MemoryStream(buffer))
            using (var reader = new BinaryReader(stream))
            {
                reader.ReadBytes(4); // Header, unused.

                var rowCount = reader.ReadUInt32();
                var defaultRowLength = reader.ReadUInt32(); // Default row length, unused.
                var columnCount = reader.ReadUInt32();

                var columnTypes = new uint[columnCount];
                var columnLengths = new int[columnCount];

                var unkColumnCount = 0;

                for (var i = 0; i < columnCount; i++)
                {
                    var name = reader.ReadString(48);
                    var type = reader.ReadUInt32();
                    var length = reader.ReadInt32();

                    var column = new DataColumn(name, GetColumnDataType(type));

                    if (name.Trim().Length < 2)
                    {
                        column.ColumnName = $"UnkCol{unkColumnCount++}";
                    }

                    columnTypes[i] = type;
                    columnLengths[i] = length;

                    Table.Columns.Add(column);
                }

                var row = new object[columnCount];

                for (var i = 0; i < rowCount; i++)
                {
                    var rowLength = reader.ReadUInt16();

                    for (var j = 0; j < columnCount; j++)
                    {
                        switch (columnTypes[j])
                        {
                            case 1:
                            case 12:
                            case 16:
                                row[j] = reader.ReadByte();
                                break;
                            case 2:
                                row[j] = reader.ReadUInt16();
                                break;
                            case 3:
                            case 11:
                            case 18:
                            case 27:
                                row[j] = reader.ReadUInt32();
                                break;
                            case 5:
                                row[j] = reader.ReadSingle();
                                break;
                            case 9:
                            case 24:
                                row[j] = reader.ReadString(columnLengths[j]);
                                break;
                            case 13:
                            case 21:
                                row[j] = reader.ReadInt16();
                                break;
                            case 20:
                                row[j] = reader.ReadSByte();
                                break;
                            case 22:
                                row[j] = reader.ReadInt32();
                                break;
                            case 26:
                                row[j] = reader.ReadString(rowLength - (int)defaultRowLength + 1);
                                break;
                            case 29:
                                var bytes = reader.ReadBytes(columnLengths[j]);

                                var val1 = BitConverter.ToUInt32(bytes, 0);
                                var val2 = BitConverter.ToUInt32(bytes, 4);

                                row[j] = new Tuple<uint, uint>(val1, val2);
                                break;
                        }
                    }

                    Table.Rows.Add(row);
                }
            }
        }
    }
}
