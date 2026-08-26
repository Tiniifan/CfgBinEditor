using System.Collections.Generic;

namespace CfgBinEditor.CfgBinCore.Common
{
    public class Tag
    {
        public string Name { get; set; }
        public List<(string, bool)> Properties { get; set; }

        public Tag()
        {
            Properties = new List<(string, bool)>();
        }
    }
}
