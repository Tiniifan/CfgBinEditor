using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CfgBinEditor.Tools;
using System.Runtime.InteropServices;

namespace CfgBinEditor.Level5.Binary
{
    public class CfgBin
    {
        public Dictionary<string, object> Entries;

        public Dictionary<int, string> Strings;

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

                foreach (KeyValuePair<string, object> entry in Entries)
                {
                    writer.Write(EncodeEntry(entry));
                }

                writer.WriteAlignment(0x10, 0xFF);
                header.StringTableOffset = (int)writer.Position;

                if (Strings.Count > 0)
                {
                    writer.Write(EncodeStrings(Strings));
                    header.StringTableOffset = (int)writer.Position - header.StringTableOffset;
                    writer.WriteAlignment(0x10, 0xFF);
                }

                List<string> distinctEntry = GetUniqueKeys(Entries);
                writer.Write(EncodeKeyTable(distinctEntry));

                writer.Write(new byte[5] {0x01, 0x74, 0x32, 0x62, 0xFE});
                writer.Write(new byte[4] { 0x01, 0x01, 0x00, 0x01});
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

        private byte[] EncodeStrings(Dictionary<int, string> strings)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryDataWriter writer = new BinaryDataWriter(memoryStream))
                {
                    foreach(KeyValuePair<int, string> kvp in strings)
                    {
                        writer.Write(Encoding.UTF8.GetBytes(kvp.Value));
                        writer.Write((byte)0x00);
                    }

                    return memoryStream.ToArray();
                }
            }
        }

        private Dictionary<string, object> ParseEntries(int entriesCount, byte[] entriesBuffer, Dictionary<uint, string> keyTable)
        {
            var outputDict = new Dictionary<string, object>();
            var stack = new Stack<Dictionary<string, object>>();
            stack.Push(outputDict);

            var nameIndices = new Dictionary<string, int>(); // New dictionary to store the current index for each name

            using (BinaryDataReader reader = new BinaryDataReader(entriesBuffer))
            {
                int i = 0;

                while (i < entriesCount)
                {
                    uint crc32 = reader.ReadValue<uint>();
                    string name = keyTable[crc32];

                    int paramCount = reader.ReadValue<byte>();
                    CfgBinSupport.Type[] paramTypes = new CfgBinSupport.Type[paramCount];
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
                                        paramTypes[paramIndex] = CfgBinSupport.Type.String;
                                        break;
                                    case 1:
                                        paramTypes[paramIndex] = CfgBinSupport.Type.Int;
                                        break;
                                    case 2:
                                        paramTypes[paramIndex] = CfgBinSupport.Type.Float;
                                        break;
                                    default:
                                        paramTypes[paramIndex] = CfgBinSupport.Type.Unknown;
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

                    List<CfgBinSupport.Variable> variables = new List<CfgBinSupport.Variable>();

                    for (int j = 0; j < paramCount; j++)
                    {
                        if (paramTypes[j] == CfgBinSupport.Type.String)
                        {
                            variables.Add(new CfgBinSupport.Variable(CfgBinSupport.Type.String, reader.ReadValue<int>()));
                        }
                        else if (paramTypes[j] == CfgBinSupport.Type.Int)
                        {
                            variables.Add(new CfgBinSupport.Variable(CfgBinSupport.Type.Int, reader.ReadValue<int>()));
                        }
                        else if (paramTypes[j] == CfgBinSupport.Type.Float)
                        {
                            variables.Add(new CfgBinSupport.Variable(CfgBinSupport.Type.Float, reader.ReadValue<float>()));
                        }
                        else if (paramTypes[j] == CfgBinSupport.Type.Unknown)
                        {
                            variables.Add(new CfgBinSupport.Variable(CfgBinSupport.Type.Unknown, reader.ReadValue<int>()));
                        }
                    }

                    if (name.EndsWith("_BEGIN"))
                    {
                        var key = name.Substring(0, name.Length - "_BEGIN".Length);

                        // Retrieve the current index for this name, default to 0 if it's not present
                        int index = nameIndices.TryGetValue(name, out int currentIndex) ? currentIndex : 0;

                        var newDict = new Dictionary<string, object>();
                        stack.Peek().Add(key + "_BEGIN_" + index, newDict);

                        stack.Push(newDict);

                        // Increment the index for this name
                        nameIndices[name] = index + 1;
                    } else if (name.EndsWith("_BEG"))
                    {
                        var key = name.Substring(0, name.Length - "_BEG".Length);

                        // Retrieve the current index for this name, default to 0 if it's not present
                        int index = nameIndices.TryGetValue(name, out int currentIndex) ? currentIndex : 0;

                        var newDict = new Dictionary<string, object>();
                        stack.Peek().Add(key + "_BEG_" + index, newDict);

                        stack.Push(newDict);

                        // Increment the index for this name
                        nameIndices[name] = index + 1;
                    }
                    else if (name.EndsWith("_END"))
                    {
                        stack.Pop();
                        nameIndices[name.Replace("_END", "")] = 0; // Reset the index for this name when encountering the "_END" marker
                    }
                    else
                    {
                        // Retrieve the current index for this name, default to 0 if it's not present
                        int index = nameIndices.TryGetValue(name, out int currentIndex) ? currentIndex : 0;

                        if (!stack.Peek().ContainsKey(name + "_" + index))
                        {
                            stack.Peek().Add(name + "_" + index, new List<CfgBinSupport.Variable>());
                        }

                        ((List<CfgBinSupport.Variable>)stack.Peek()[name + "_" + index]).AddRange(variables);

                        // Increment the index for this name
                        nameIndices[name] = index + 1;
                    }

                    i++;
                }
            }

            return outputDict;
        }

        private byte[] EncodeEntry(KeyValuePair<string, object> entry)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryDataWriter writer = new BinaryDataWriter(memoryStream))
                {
                    string entryName = string.Join("_", entry.Key.Split('_').Reverse().Skip(1).Reverse());
                    writer.Write(Crc32.Compute(Encoding.UTF8.GetBytes(entryName)));
                    List<CfgBinSupport.Type> types;

                    if (entry.Value.GetType() == typeof(Dictionary<string, object>))
                    {
                        Dictionary<string, object> item = entry.Value as Dictionary<string, object>;

                        Dictionary<string, int> distinctEntry = TransformAndCount(item);
                        writer.Write((byte)distinctEntry.Count);

                        types = Enumerable.Repeat(CfgBinSupport.Type.Int, distinctEntry.Count).ToList();
                        writer.Write(EncodeTypes(types));


                        foreach (int count in distinctEntry.Values)
                        {
                            writer.Write(count);
                        }

                        foreach (KeyValuePair<string, object> itemValue in item)
                        {
                            writer.Write(EncodeEntry(itemValue));
                        }

                        writer.Write(Crc32.Compute(Encoding.UTF8.GetBytes(entryName.Replace("BEGIN", "") + "END")));
                        writer.Write(new byte[4] { 0x00, 0xFF, 0xFF, 0xFF });
                    }
                    else
                    {
                        List<CfgBinSupport.Variable> item = entry.Value as List<CfgBinSupport.Variable>;
                        writer.Write((byte)item.Count);

                        types = item.Select(x => x.Type).ToList();
                        writer.Write(EncodeTypes(types));

                        foreach (CfgBinSupport.Variable variable in item)
                        {
                            switch (variable.Type)
                            {
                                case CfgBinSupport.Type.String:
                                    writer.Write(Convert.ToInt32(variable.Value));
                                    break;
                                case CfgBinSupport.Type.Int:
                                    writer.Write(Convert.ToInt32(variable.Value));
                                    break;
                                case CfgBinSupport.Type.Float:
                                    writer.Write(Convert.ToSingle(variable.Value));
                                    break;
                                default:
                                    writer.Write(Convert.ToInt32(variable.Value));
                                    break;
                            }
                        }
                    }

                    return memoryStream.ToArray();
                }
            }
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

                header.KeyStringOffset = (int) writer.Position;

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

        private byte[] EncodeTypes(List<CfgBinSupport.Type> types)
        {
            List<byte> byteArray = new List<byte>();

            // Iterate through types and create type descriptors
            for (int i = 0; i < Math.Ceiling((double)types.Count / 4); i++)
            {
                int typeDesc = 0;

                // Create a type descriptor for each set of 4 types
                for (int j = 4 * i; j < Math.Min(4 * (i + 1), types.Count); j++)
                {
                    int tagValue = 0;

                    // Map CfgBinSupport.Type to tag values
                    switch (types[j])
                    {
                        case CfgBinSupport.Type.String:
                            tagValue = 0;
                            break;
                        case CfgBinSupport.Type.Int:
                            tagValue = 1;
                            break;
                        case CfgBinSupport.Type.Float:
                            tagValue = 2;
                            break;
                        default:
                            tagValue = 0;
                            break;
                    }

                    // Combine tag values to create the type descriptor
                    typeDesc |= tagValue << ((j % 4) * 2);
                }

                // Convert type descriptor to byte array and add to byteArray
                byteArray.Add((byte)typeDesc);
            }

            // Fill the byte array with FF to ensure a size multiple of 4
            while ((byteArray.Count + 1) % 4 != 0)
            {
                byteArray.Add(0xFF);
            }

            return byteArray.ToArray();
        }

        private long RoundUp(int n, int exp)
        {
            return ((n + exp - 1) / exp) * exp;
        }

        public int Count(Dictionary<string, object> dictionary)
        {
            int totalCount = 0;

            foreach (var kvp in dictionary)
            {
                string keyName = TransformKey(kvp.Key);

                totalCount++;

                if (keyName.EndsWith("BEGIN"))
                {
                    totalCount++;
                }

                if (kvp.Value is Dictionary<string, object> subDictionary)
                {
                    totalCount += Count(subDictionary);
                }
            }

            return totalCount;
        }

        public List<string> GetUniqueKeys(Dictionary<string, object> dictionary)
        {
            HashSet<string> uniqueKeys = new HashSet<string>();
            RecursiveGetKeys(dictionary, uniqueKeys);
            return new List<string>(uniqueKeys);
        }

        public void RecursiveGetKeys(Dictionary<string, object> dictionary, HashSet<string> uniqueKeys)
        {
            foreach (var kvp in dictionary)
            {
                uniqueKeys.Add(TransformKey(kvp.Key));

                if (kvp.Value is Dictionary<string, object> nestedDictionary)
                {
                    RecursiveGetKeys(nestedDictionary, uniqueKeys);
                }

                uniqueKeys.Add(TransformKey(kvp.Key).Replace("BEGIN", "END"));
            }
        }

        public Dictionary<string, int> TransformAndCount(Dictionary<string, object> inputDictionary)
        {
            List<string> listB = ExtractKeysRecursively(inputDictionary);
            Dictionary<string, int> resultDictionary = new Dictionary<string, int>();

            foreach (string key in listB)
            {
                string transformedKey = TransformKey(key);
                if (resultDictionary.ContainsKey(transformedKey))
                {
                    resultDictionary[transformedKey]++;
                }
                else
                {
                    resultDictionary[transformedKey] = 1;
                }
            }

            return resultDictionary;
        }

        private List<string> ExtractKeysRecursively(Dictionary<string, object> dictionary)
        {
            List<string> keys = new List<string>();

            foreach (var kvp in dictionary)
            {
                keys.Add(kvp.Key);
                if (kvp.Value is Dictionary<string, object> subDictionary)
                {
                    keys.AddRange(ExtractKeysRecursively(subDictionary));
                }
            }

            return keys;
        }

        public string TransformKey(string input)
        {
            return string.Join("_", input.Split('_').Reverse().Skip(1).Reverse());
        }

        public static void CountOccurrences(object value, Dictionary<string, int> counter)
        {
            if (value is Dictionary<string, object> nestedDictionary)
            {
                foreach (var nestedKeyValuePair in nestedDictionary)
                {
                    CountOccurrences(nestedKeyValuePair.Value, counter);
                }
            }
            else if (value is List<CfgBinSupport.Variable> variableList)
            {
                foreach (var variable in variableList)
                {
                    string variableName = variable.ToString(); // You might need to adjust this based on your Variable class
                    if (counter.ContainsKey(variableName))
                    {
                        counter[variableName]++;
                    }
                    else
                    {
                        counter[variableName] = 1;
                    }
                }
            }
        }

        public void UpdateStrings(int key, string newText)
        {
            if (Strings.ContainsKey(key))
            {
                int offset = 0;
                Strings[key] = newText;
                Dictionary<int, string> newStrings = new Dictionary<int, string>();
                Dictionary<int, int> indexes = new Dictionary<int, int>();

                foreach (KeyValuePair<int, string> kvp in Strings)
                {
                    newStrings.Add(offset, kvp.Value);
                    indexes.Add(kvp.Key, offset);
                    offset += Encoding.UTF8.GetByteCount(kvp.Value) + 1;
                }

                UpdateStringsEntries(Entries, indexes);
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
                else if (kvp.Value is List<CfgBinSupport.Variable> variables)
                {
                    foreach (CfgBinSupport.Variable variable in variables)
                    {
                        if (variable.Type == CfgBinSupport.Type.String)
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

        public void InsertStrings(int key, string newText)
        {
            if (!Strings.ContainsKey(key))
            {
                Strings[key] = newText;
            }
        }
    }
}
