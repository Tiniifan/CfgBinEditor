using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using CfgBinEditor.Level5.Binary;

namespace CfgBinEditor
{
    public partial class IDsWindow : Form
    {
        private TreeNode[] RootNodes;

        public int Hash { get; private set; }

        readonly Dictionary<string, Dictionary<string, List<ID>>> IDs;
        public IDsWindow(Dictionary<string, Dictionary<string, List<ID>>> ids)
        {
            IDs = ids;
            InitializeComponent();
        }

        public TreeNode CloneNode(TreeNode sourceNode)
        {
            TreeNode newNode = new TreeNode(sourceNode.Text);

            // Clone properties from the source node to the new node
            newNode.Tag = sourceNode.Tag;
            newNode.ImageIndex = sourceNode.ImageIndex;
            newNode.SelectedImageIndex = sourceNode.SelectedImageIndex;

            // Clone child nodes recursively
            foreach (TreeNode childNode in sourceNode.Nodes)
            {
                TreeNode clonedChild = CloneNode(childNode);
                newNode.Nodes.Add(clonedChild);
            }

            return newNode;
        }

        public TreeNode[] CloneTreeNodes(TreeView treeView)
        {
            return treeView.Nodes
                .Cast<TreeNode>()
                .Select(rootNode => CloneNode(rootNode))
                .ToArray();
        }

        private IEnumerable<TreeNode> GetMatchingNodesRecursive(TreeNode parentNode, string searchText)
        {
            Console.WriteLine(parentNode.Text + " -> " + searchText);
            if (parentNode.Text.ToLower().StartsWith(searchText))
            {
                yield return parentNode;
            }

            foreach (TreeNode childNode in parentNode.Nodes)
            {
                foreach (TreeNode matchingNode in GetMatchingNodesRecursive(childNode, searchText))
                {
                    yield return matchingNode;
                }
            }
        }

        private void IDsWindow_Load(object sender, EventArgs e)
        {
            TreeNode treeNode = new TreeNode("MyIDs.txt");
            treeNode.Expand();

            foreach (var kvp in IDs)
            {
                TreeNode keyNode = new TreeNode(kvp.Key);

                foreach (var category in kvp.Value)
                {
                    TreeNode categoryNode = new TreeNode(category.Key);

                    foreach (var id in category.Value)
                    {
                        TreeNode itemNode = new TreeNode(id.Name);
                        itemNode.Tag = id.Hash;
                        categoryNode.Nodes.Add(itemNode);
                    }

                    keyNode.Nodes.Add(categoryNode);
                }

                treeNode.Nodes.Add(keyNode);
            }

            idTreeView.Nodes.Add(treeNode);

            RootNodes = CloneTreeNodes(idTreeView);
        }

        private void IdTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag == null) return;

            Hash = Convert.ToInt32(e.Node.Tag);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            idTreeView.Nodes.Clear();

            if (!string.IsNullOrWhiteSpace(searchTextBox.Text))
            {
                string searchText = searchTextBox.Text.ToLower();

                TreeNode[] matchingNodes = RootNodes
                    .Cast<TreeNode>()
                    .SelectMany(node => GetMatchingNodesRecursive(node, searchText))
                    .Distinct()
                    .ToArray();

                TreeNode newRootNode = new TreeNode(searchText);
                newRootNode.ExpandAll();
                newRootNode.Nodes.AddRange(matchingNodes.ToList().ToArray());

                idTreeView.Nodes.Add(newRootNode);
            }
            else
            {
                idTreeView.Nodes.AddRange(RootNodes);
                idTreeView.Nodes[0].Expand();
            }
        }
    }
}