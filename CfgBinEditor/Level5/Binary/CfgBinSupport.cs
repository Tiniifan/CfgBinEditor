using System.Runtime.InteropServices;

namespace CfgBinEditor.Level5.Binary
{
    public class CfgBinSupport
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Header
        {
            public int EntriesCount;
            public int StringTableOffset;
            public int StringTableLength;
            public int StringTableCount;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct KeyHeader
        {
            public int KeyLength;
            public int KeyCount;
            public int KeyStringOffset;
            public int keyStringLength;
        }

        public enum Type
        {
            String,
            Int,
            Float,
            Unknown
        }

        public class Variable
        {
            public Type Type;
            public object Value;

            public Variable(Type type, object value)
            {
                Type = type;
                Value = value;
            }

            public Variable(Variable variable)
            {
                Type = variable.Type;
                Value = variable.Value;
            }
        }
    }
}
