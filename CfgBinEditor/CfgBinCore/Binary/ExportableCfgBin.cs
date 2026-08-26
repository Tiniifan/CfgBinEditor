using StudioElevenLib.Level5.Binary.Collections;
using StudioElevenLib.Level5.Binary;

namespace CfgBinEditor.CfgBinCore.Binary
{
    public class ExportableCfgBin : CfgBin<CfgTreeNode>
    {
        public void SetRoot(CfgTreeNode root)
        {
            Entries = root;
        }
    }
}
