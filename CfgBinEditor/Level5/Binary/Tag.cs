using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CfgBinEditor.Level5.Binary
{
    public class Tag
    {
        public string Name { get; set; }
        public Dictionary<string, bool> Properties { get; set; }

        public Tag()
        {
            Properties = new Dictionary<string, bool>();
        }
    }
}
