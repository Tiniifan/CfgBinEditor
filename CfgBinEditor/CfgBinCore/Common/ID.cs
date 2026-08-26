namespace CfgBinEditor.CfgBinCore.Common
{
    public class ID
    {
        public int Hash { get; set; }
        public string Name { get; set; }

        public ID()
        {
        }

        public ID(int hash, string name)
        {
            Hash = hash;
            Name = name;
        }
    }
}
