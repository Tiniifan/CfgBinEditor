using System.Collections.Generic;

namespace CfgBinEditor.CfgBinCore.Json
{
    public class JsonEntry
    {
        public string Name { get; set; }
        public List<JsonVariable> Variables { get; set; } = new List<JsonVariable>();
        public List<JsonEntry> Children { get; set; } = new List<JsonEntry>();
    }
}
