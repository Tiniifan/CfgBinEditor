﻿using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CfgBinEditor.Tools;
using CfgBinEditor.Level5.Logic;

namespace CfgBinEditor.Level5.Binary
{
    public class CfgBin
    {
        public List<Entry> Entries;

        public Dictionary<int, string> Strings;

        public CfgBin()
        {
            Entries = new List<Entry>();
            Strings = new Dictionary<int, string>();
        }

        public CfgBin(Stream stream)
        {
            using (var reader = new BinaryDataReader(stream))
            {
                var header = reader.ReadStruct<CfgBinSupport.Header>();

                byte[] entriesBuffer = reader.GetSection(0x10, header.StringTableOffset);

                byte[] stringTableBuffer = reader.GetSection((uint)header.StringTableOffset, header.StringTableLength);
                Strings = ParseStrings(header.StringTableCount, stringTableBuffer);

                long keyTableOffset = RoundUp(header.StringTableOffset + header.StringTableLength, 16);
                reader.Seek((uint)keyTableOffset);
                int keyTableSize = reader.ReadValue<int>();
                byte[] keyTableBlob = reader.GetSection((uint)keyTableOffset, keyTableSize);
                Dictionary<uint, string> keyTable = ParseKeyTable(keyTableBlob);

                Entries = ParseEntries(header.EntriesCount, entriesBuffer, keyTable);
            }
        }

        public void Save(string fileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                BinaryDataWriter writer = new BinaryDataWriter(stream);

                CfgBinSupport.Header header;
                header.EntriesCount = Count(Entries);
                header.StringTableOffset = 0;
                header.StringTableLength = 0;
                header.StringTableCount = Strings.Count;

                writer.Seek(0x10);

                foreach (Entry entry in Entries)
                {
                    writer.Write(entry.EncodeEntry());
                }

                writer.WriteAlignment(0x10, 0xFF);
                header.StringTableOffset = (int)writer.Position;

                if (Strings.Count > 0)
                {
                    writer.Write(EncodeStrings(Strings));
                    header.StringTableLength = (int)writer.Position - header.StringTableOffset;
                    writer.WriteAlignment(0x10, 0xFF);
                }

                List<string> uniqueKeysList = Entries
                    .SelectMany(entry => entry.GetUniqueKeys())
                    .Distinct()
                    .ToList();

                writer.Write(EncodeKeyTable(uniqueKeysList));

                writer.Write(new byte[5] { 0x01, 0x74, 0x32, 0x62, 0xFE });
                writer.Write(new byte[4] { 0x01, 0x01, 0x00, 0x01 });
                writer.WriteAlignment();

                writer.Seek(0);
                writer.WriteStruct(header);
            }
        }

        private Dictionary<int, string> ParseStrings(int stringCount, byte[] stringTableBuffer)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();

            using (BinaryDataReader reader = new BinaryDataReader(stringTableBuffer))
            {
                for (int i = 0; i < stringCount; i++)
                {
                    result.Add((int)reader.Position, reader.ReadString(Encoding.UTF8));
                }
            }

            return result;
        }

        private Dictionary<uint, string> ParseKeyTable(byte[] buffer)
        {
            Dictionary<uint, string> keyTable = new Dictionary<uint, string>();

            using (var reader = new BinaryDataReader(buffer))
            {
                keyTable = new Dictionary<uint, string>();

                var header = reader.ReadStruct<CfgBinSupport.KeyHeader>();
                byte[] keyStringBlob = reader.GetSection((uint)header.KeyStringOffset, header.keyStringLength);

                for (int i = 0; i < header.KeyCount; i++)
                {
                    uint crc32 = reader.ReadValue<uint>();
                    int stringStart = reader.ReadValue<int>();
                    int stringEnd = Array.IndexOf(keyStringBlob, (byte)0, stringStart);
                    byte[] stringBuf = new byte[stringEnd - stringStart];
                    Array.Copy(keyStringBlob, stringStart, stringBuf, 0, stringEnd - stringStart);
                    string key = Encoding.UTF8.GetString(stringBuf);
                    keyTable[crc32] = key;
                }
            }

            return keyTable;
        }

        private List<Entry> ParseEntries(int entriesCount, byte[] entriesBuffer, Dictionary<uint, string> keyTable)
        {
            List<Entry> temp = new List<Entry>();

            // Get All entries
            using (BinaryDataReader reader = new BinaryDataReader(entriesBuffer))
            {
                for (int i = 0; i < entriesCount; i++)
                {
                    uint crc32 = reader.ReadValue<uint>();
                    string name = keyTable[crc32];

                    int paramCount = reader.ReadValue<byte>();
                    Logic.Type[] paramTypes = new Logic.Type[paramCount];
                    int paramIndex = 0;

                    for (int j = 0; j < (int)Math.Ceiling((double)paramCount / 4); j++)
                    {
                        byte paramType = reader.ReadValue<byte>();
                        for (int k = 0; k < 4; k++)
                        {
                            if (paramIndex < paramTypes.Length)
                            {
                                int tag = (paramType >> (2 * k)) & 3;

                                switch (tag)
                                {
                                    case 0:
                                        paramTypes[paramIndex] = Logic.Type.String;
                                        break;
                                    case 1:
                                        paramTypes[paramIndex] = Logic.Type.Int;
                                        break;
                                    case 2:
                                        paramTypes[paramIndex] = Logic.Type.Float;
                                        break;
                                    default:
                                        paramTypes[paramIndex] = Logic.Type.Unknown;
                                        break;
                                }

                                paramIndex++;
                            }
                        }
                    }

                    if ((Math.Ceiling((double)paramCount / 4) + 1) % 4 != 0)
                    {
                        reader.Seek((uint)(reader.Position + 4 - (reader.Position % 4)));
                    }

                    List<Variable> variables = new List<Variable>();

                    for (int j = 0; j < paramCount; j++)
                    {
                        if (paramTypes[j] == Logic.Type.String)
                        {
                            int offset = reader.ReadValue<int>();
                            string text = null;

                            if (offset != -1)
                            {
                                text = Strings[offset];
                            }

                            variables.Add(new Variable(Logic.Type.String, new OffsetTextPair(offset, text)));
                        }
                        else if (paramTypes[j] == Logic.Type.Int)
                        {
                            variables.Add(new Variable(Logic.Type.Int, reader.ReadValue<int>()));
                        }
                        else if (paramTypes[j] == Logic.Type.Float)
                        {
                            variables.Add(new Variable(Logic.Type.Float, reader.ReadValue<float>()));
                        }
                        else if (paramTypes[j] == Logic.Type.Unknown)
                        {
                            variables.Add(new Variable(Logic.Type.Unknown, reader.ReadValue<int>()));
                        }
                    }

                    temp.Add(new Entry(name, variables));
                }
            }

            // Reorganize entries
            Dictionary<string, int> entriesKey = new Dictionary<string, int>();
            for (int i = 0; i < temp.Count; i++)
            {
                string entryName = temp[i].Name;

                if (!entriesKey.ContainsKey(entryName))
                {
                    entriesKey[entryName] = 0;
                }

                temp[i].Name = entryName + "_" + entriesKey[entryName];
                entriesKey[entryName] += 1;
            }

            return ProcessEntries(temp);
        }

        public List<Entry> ProcessEntries(List<Entry> entries)
        {
            List<Entry> stack = new List<Entry>();
            List<Entry> output = new List<Entry>();
            Dictionary<string, int> depth = new Dictionary<string, int>();

            int i = 0;  // Indice pour parcourir les entrées

            while (i < entries.Count)
            {
                string name = entries[i].Name;
                List<Variable> variables = entries[i].Variables;

                string[] nameParts = name.Split('_');
                string nodeType = nameParts[nameParts.Length - 2].ToLower();
                string nodeName = string.Join("_", nameParts, 0, nameParts.Length - 1).ToLower();

                if (nodeType.EndsWith("beg") || nodeType.EndsWith("begin") || nodeType.EndsWith("ptree"))
                {
                    Entry newNode = new Entry(name, variables);

                    if (stack.Count > 0)
                    {
                        stack[stack.Count - 1].Children.Add(newNode);
                    }
                    else
                    {
                        output.Add(newNode);
                    }

                    stack.Add(newNode);
                    depth[name] = stack.Count;
                }
                else if (nodeType.EndsWith("end") || nodeType.EndsWith("_ptree"))
                {
                    stack[stack.Count - 1].EndTerminator = true;

                    string key = "";
                    if (depth.ContainsKey(name.Replace("_END_", "_BEG_")))
                    {
                        key = name.Replace("_END_", "_BEG_");
                    }
                    else if (depth.ContainsKey(name.Replace("_END_", "_BEGIN_")))
                    {
                        key = name.Replace("_END_", "_BEGIN_");
                    }
                    else if (depth.ContainsKey(name.Replace("_PTREE", "PTREE")))
                    {
                        key = name.Replace("_PTREE", "PTREE");
                    }

                    if (depth.Count > 1)
                    {
                        string[] keys = new string[depth.Keys.Count];
                        depth.Keys.CopyTo(keys, 0);

                        int currentDepth = depth[key];
                        int previousDepth = depth[keys[Array.IndexOf(keys, key) - 1]];

                        int popCount = currentDepth - previousDepth;
                        for (int j = 0; j < popCount; j++)
                        {
                            stack.RemoveAt(stack.Count - 1);
                        }

                        depth.Remove(key);
                    }
                    else
                    {
                        stack.RemoveAt(stack.Count - 1);
                        depth.Remove(key);
                    }
                }
                else
                {
                    Entry newItem = new Entry(name, variables);

                    if (i + 1 < entries.Count)
                    {
                        string[] nextNameParts = entries[i + 1].Name.Split('_');
                        string nextNodeType = nextNameParts[nextNameParts.Length - 2].ToLower();
                        string nextNodeName = string.Join("_", nextNameParts, 0, nextNameParts.Length - 1).ToLower();

                        if (nextNodeName != nodeName && nextNodeType != "end")
                        {
                            stack[stack.Count - 1].Children.Add(newItem);
                            stack.Add(newItem);
                        }
                        else
                        {
                            stack[stack.Count - 1].Children.Add(newItem);
                        }
                    }
                    else
                    {
                        stack[stack.Count - 1].Children.Add(newItem);
                    }
                }

                i++;
            }

            return output;
        }

        private byte[] EncodeStrings(Dictionary<int, string> strings)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryDataWriter writer = new BinaryDataWriter(memoryStream))
                {
                    foreach (KeyValuePair<int, string> kvp in strings)
                    {
                        writer.Write(Encoding.UTF8.GetBytes(kvp.Value));
                        writer.Write((byte)0x00);
                    }

                    return memoryStream.ToArray();
                }
            }
        }

        public byte[] EncodeKeyTable(List<string> keyList)
        {
            using (MemoryStream stream = new MemoryStream())
            using (BinaryDataWriter writer = new BinaryDataWriter(stream))
            {
                // Calculate the total size required for the header and key strings
                uint headerSize = (uint)Marshal.SizeOf(typeof(CfgBinSupport.KeyHeader));
                uint keyStringsSize = 0;

                foreach (var key in keyList)
                {
                    keyStringsSize += (uint)Encoding.UTF8.GetByteCount(key) + 1; // +1 for null-terminator
                }

                // Write header
                var header = new CfgBinSupport.KeyHeader
                {
                    KeyCount = keyList.Count,
                    keyStringLength = (int)keyStringsSize
                };

                writer.Seek(0x10);

                int stringOffset = 0;

                // Calculate CRC32 for each key and write key entries
                foreach (var key in keyList)
                {
                    uint crc32 = Crc32.Compute(Encoding.UTF8.GetBytes(key));
                    writer.Write(crc32);
                    writer.Write(stringOffset);
                    stringOffset += Encoding.UTF8.GetBytes(key).Count() + 1;
                }

                writer.WriteAlignment(0x10, 0xFF);

                header.KeyStringOffset = (int)writer.Position;

                // Write key strings
                foreach (var key in keyList)
                {
                    byte[] stringBytes = Encoding.UTF8.GetBytes(key);
                    writer.Write(stringBytes);
                    writer.Write((byte)0); // Null-terminator
                }

                writer.WriteAlignment(0x10, 0xFF);
                header.KeyLength = (int)writer.Position;
                writer.Seek(0x00);
                writer.WriteStruct(header);

                return stream.ToArray();
            }
        }

        private long RoundUp(int n, int exp)
        {
            return ((n + exp - 1) / exp) * exp;
        }

        public int Count(List<Entry> entries)
        {
            int totalCount = 0;

            foreach (Entry entry in entries)
            {
                totalCount += entry.Count();
            }

            return totalCount;
        }

        public void UpdateStrings(int key, string newText)
        {
            if (Strings.ContainsKey(key))
            {
                int offset = 0;
                Strings[key] = newText;

                Dictionary<int, string> newStrings = new Dictionary<int, string>();
                Dictionary<int, int> newOffset = new Dictionary<int, int>();

                foreach (KeyValuePair<int, string> kvp in Strings.OrderBy(kv => kv.Key))
                {
                    newStrings.Add(offset, kvp.Value);
                    newOffset.Add(kvp.Key, offset);
                    offset += Encoding.UTF8.GetByteCount(kvp.Value) + 1;
                }

                foreach (Entry entry in Entries)
                {
                    entry.UpdateString(newOffset, newStrings);
                }

                Strings = newStrings;
            }
        }

        private void UpdateStringsEntries(Dictionary<string, object> dictionary, Dictionary<int, int> indexes)
        {
            foreach (var kvp in dictionary)
            {
                if (kvp.Value is Dictionary<string, object> nestedDictionary)
                {
                    UpdateStringsEntries(nestedDictionary, indexes);
                }
                else if (kvp.Value is List<Variable> variables)
                {
                    foreach (Variable variable in variables)
                    {
                        if (variable.Type == Logic.Type.String)
                        {
                            int offset = (int)variable.Value;

                            if (Strings.ContainsKey(offset))
                            {
                                variable.Value = indexes[(int)variable.Value];
                            }
                        }
                    }
                }
            }
        }

        public void InsertStrings(string newText)
        {
            int offset = 0;

            if (Strings.Count > 0)
            {
                KeyValuePair<int, string> lastItem = Strings.ElementAt(Strings.Count - 1);
                offset = lastItem.Key + Encoding.UTF8.GetBytes(lastItem.Value).Length + 1;
            }

            Strings[offset] = newText;
        }
    }
}
