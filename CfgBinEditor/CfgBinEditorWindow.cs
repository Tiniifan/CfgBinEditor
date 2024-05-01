using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using CfgBinEditor.UI;
using CfgBinEditor.Level5.Binary;
using CfgBinEditor.Level5.Binary.Logic;

namespace CfgBinEditor
{
    public partial class CfgBinEditorWindow : Form
    {
        private CfgBin CfgBinFileOpened;

        private TreeNode SelectedRightClickTreeNode;

        private bool VariblesDataGridEditInProgress = false;

        private string SelectedTag = null;

        private Dictionary<string, List<Tag>> Tags;

        private Dictionary<string, Dictionary<string, List<ID>>> IDs;

        private bool Resizing;

        private int Colindex = -1;

        private int StartX;

        private int StartWidth;

        public CfgBinEditorWindow()
        {
            InitializeComponent();
        }

        private ID GetID(int hash)
        {
            ID myID = IDs != null ? IDs.Values
                                       .SelectMany(outerDict => outerDict.Values
                                                        .SelectMany(innerList => innerList
                                                            .Where(id => id.Hash == hash)))
                                                    .FirstOrDefault()
                                                : null;
            return myID;
        }

        private Dictionary<string, List<Tag>> ImportTags(string filePath)
        {
            Dictionary<string, List<Tag>> tagDictionary = new Dictionary<string, List<Tag>>();

            try
            {
                string currentKey = null;
                List<Tag> currentTags = null;

                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    string trimmedLine = line.Trim();

                    if (trimmedLine.EndsWith("["))
                    {
                        currentKey = trimmedLine.Trim('[', ']');
                        currentTags = new List<Tag>();
                        tagDictionary[currentKey] = currentTags;
                    }
                    else if (currentTags != null && trimmedLine.EndsWith("("))
                    {
                        Tag tag = new Tag { Name = trimmedLine.Trim('(', ')').Trim() };
                        currentTags.Add(tag);
                    }
                    else if (currentTags != null && trimmedLine.Contains("|"))
                    {
                        string[] parts = trimmedLine.Split('|');

                        if (parts.Length == 2)
                        {
                            string propName = parts[0].Trim();
                            bool propValue = bool.Parse(parts[1].Trim());
                            currentTags[currentTags.Count - 1].Properties.Add((propName, propValue));
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("An error occurred while loading the tags.");
            }

            return tagDictionary;
        }

        private void ExportTags(Dictionary<string, List<Tag>> tagDictionary, string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (var entry in tagDictionary)
                    {
                        // Write the section header
                        writer.WriteLine($"{entry.Key} [");

                        foreach (var tag in entry.Value)
                        {
                            // Write the tag name
                            writer.WriteLine($"\t{tag.Name} (");

                            // Write tag properties
                            foreach (var property in tag.Properties)
                            {
                                writer.WriteLine($"\t\t{property.Item1}|{property.Item2}");
                            }

                            // Close tag section
                            writer.WriteLine("\t)");
                        }

                        // Close section
                        writer.WriteLine("]");
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("An error occurred while saving the tags.");
            }
        }

        public Dictionary<string, Dictionary<string, List<ID>>> ImportIDs(string filePath)
        {
            Dictionary<string, Dictionary<string, List<ID>>> result = new Dictionary<string, Dictionary<string, List<ID>>>();

            string currentKey = null;
            string currentCategory = null;
            List<ID> currentIDs = null;

            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();

                if (trimmedLine.EndsWith("["))
                {
                    currentKey = trimmedLine.Trim('[', ']');
                    result[currentKey] = new Dictionary<string, List<ID>>();
                }
                else if (currentKey != null && trimmedLine.EndsWith("("))
                {
                    currentCategory = trimmedLine.Trim('(', ')').Trim();
                    currentIDs = new List<ID>();
                    result[currentKey][currentCategory] = currentIDs;
                }
                else if (currentCategory != null && trimmedLine.Contains("|"))
                {
                    string[] parts = trimmedLine.Split('|');
                    if (parts.Length == 2)
                    {
                        int hash = Convert.ToInt32(parts[0], 16);
                        string name = parts[1].Trim();
                        currentIDs.Add(new ID(hash, name));
                    }
                }
            }

            return result;
        }

        public void ExportIDs(Dictionary<string, Dictionary<string, List<ID>>> ids, string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (var outerEntry in ids)
                    {
                        // Write the section header
                        writer.WriteLine($"{outerEntry.Key} [");

                        foreach (var innerEntry in outerEntry.Value)
                        {
                            // Write the category header
                            writer.WriteLine($"\t{innerEntry.Key} (");

                            foreach (var id in innerEntry.Value)
                            {
                                // Write each ID entry
                                writer.WriteLine($"\t\t0x{id.Hash.ToString("X")}|{id.Name}");
                            }

                            // Close the category section
                            writer.WriteLine("\t)");
                        }

                        // Close the section
                        writer.WriteLine("]");
                    }
                }
            }
            catch (Exception)
            {
                // Handle the exception (e.g., show an error message)
                Console.WriteLine("An error occurred while exporting the IDs.");
            }
        }

        private TreeNode CreateTreeNode(Entry entry)
        {
            TreeNode entryNode = null;

            if (entry.Variables.Count > 0 && entry.Variables[0].Type == Level5.Binary.Logic.Type.Int)
            {
                ID myId = GetID(Convert.ToInt32(entry.Variables[0].Value));

                if (myId != null && myId.Name != "")
                {
                    entryNode = new TreeNode(myId.Name);
                } else
                {
                    entryNode = new TreeNode(entry.Name);
                }
            } else
            {
                entryNode = new TreeNode(entry.Name);
            }
            
            entryNode.Tag = entry;

            if (entry.Children.Count > 0)
            {
                foreach (Entry subEntry in entry.Children)
                {
                    entryNode.Nodes.Add(CreateTreeNode(subEntry));
                }

                entryNode.ContextMenuStrip = contextMenuStrip1;
            }
            else
            {
                entryNode.ContextMenuStrip = contextMenuStrip2;
            }

            return entryNode;
        }

        private void DrawTreeView(string rootName, List<Entry> entries)
        {
            TreeNode rootNode = new TreeNode(rootName);
            rootNode.ContextMenuStrip = contextMenuStrip3;
            rootNode.Expand();

            foreach (Entry entry in entries)
            {
                rootNode.Nodes.Add(CreateTreeNode(entry));
            }

            mainTreeView.Nodes.Clear();
            mainTreeView.Nodes.Add(rootNode);
        }

        private void DrawVariablesDataGridView(TreeNode node)
        {
            Tag tag = null;
            Entry entry = node.Tag as Entry;

            if (entry != null)
            {
                if (SelectedTag != null && Tags.ContainsKey(SelectedTag))
                {
                    tag = Tags[SelectedTag].Find(x => x.Name == entry.GetName());
                }

                variablesDataGridView.Rows.Clear();
                List<Variable> variables = (node.Tag as Entry).Variables;

                if (variables != null)
                {
                    for (int i = 0; i < variables.Count; i++)
                    {
                        string variableName = "Variable " + i;
                        bool showAsHex = false;

                        if (tag != null)
                        {
                            variableName = tag.Properties[i].Item1;
                            showAsHex = tag.Properties[i].Item2;
                        }

                        Variable variable = variables[i];
                        variablesDataGridView.Rows.Add();

                        DataGridViewComboBoxCell comboBox = (variablesDataGridView.Rows[0].Cells[1] as DataGridViewComboBoxCell);

                        if (variable.Type is Level5.Binary.Logic.Type.String)
                        {
                            variablesDataGridView.Rows[variablesDataGridView.Rows.Count - 1].SetValues(new object[] { variableName, comboBox.Items[0], variable.Value, false });
                            variablesDataGridView.Rows[variablesDataGridView.Rows.Count - 1].Cells[3].ReadOnly = true;
                        }
                        else if (variable.Type is Level5.Binary.Logic.Type.Int || variable.Type is Level5.Binary.Logic.Type.Unknown)
                        {
                            ID myID = IDs != null
                                ? IDs.Values
                                    .SelectMany(outerDict => outerDict.Values
                                        .SelectMany(innerList => innerList
                                            .Where(id => id.Hash == Convert.ToInt32(variable.Value))))
                                    .FirstOrDefault()
                                : null;

                            if (myID != null)
                            {
                                variablesDataGridView.Rows[variablesDataGridView.Rows.Count - 1].SetValues(new object[] { variableName, comboBox.Items[1], myID.Name, showAsHex });
                            }
                            else
                            {
                                if (showAsHex == true)
                                {
                                    variablesDataGridView.Rows[variablesDataGridView.Rows.Count - 1].SetValues(new object[] { variableName, comboBox.Items[1], Convert.ToInt32(variable.Value).ToString("X8"), showAsHex });
                                }
                                else
                                {
                                    variablesDataGridView.Rows[variablesDataGridView.Rows.Count - 1].SetValues(new object[] { variableName, comboBox.Items[1], variable.Value, showAsHex });
                                }
                            }
                        }
                        else if (variable.Type is Level5.Binary.Logic.Type.Float)
                        {
                            if (showAsHex == true)
                            {
                                byte[] byteArray = BitConverter.GetBytes((float)variable.Value);
                                Array.Reverse(byteArray);
                                variablesDataGridView.Rows[variablesDataGridView.Rows.Count - 1].SetValues(new object[] { variableName, comboBox.Items[2], BitConverter.ToString(byteArray).Replace("-", ""), showAsHex });
                            }
                            else
                            {
                                variablesDataGridView.Rows[variablesDataGridView.Rows.Count - 1].SetValues(new object[] { variableName, comboBox.Items[2], variable.Value, showAsHex });
                            }
                        }
                    }
                }
            }
        }

        private void FillStrings()
        {
            stringsDataGridView.Rows.Clear();

            foreach (string myString in CfgBinFileOpened.GetDistinctStrings())
            {
                stringsDataGridView.Rows.Add(myString);
            }
        }

        private int ConvertLittleEndianHexToInt(string hexString)
        {
            int byteCount = (hexString.Length + 1) / 2;
            byte[] byteArray = new byte[byteCount];

            for (int i = 0; i < byteCount; i++)
            {
                int startIndex = Math.Max(hexString.Length - (i + 1) * 2, 0);
                string byteHex = hexString.Substring(startIndex, Math.Min(2, hexString.Length - startIndex));
                byteArray[i] = Convert.ToByte(byteHex, 16);
            }

            return BitConverter.ToInt32(byteArray, 0);
        }

        private float ConvertLittleEndianHexToFloat(string hexString)
        {
            int byteCount = (hexString.Length + 1) / 2;
            byte[] byteArray = new byte[byteCount];

            for (int i = 0; i < byteCount; i++)
            {
                int startIndex = Math.Max(hexString.Length - (i + 1) * 2, 0);
                string byteHex = hexString.Substring(startIndex, Math.Min(2, hexString.Length - startIndex));
                byteArray[i] = Convert.ToByte(byteHex, 16);
            }

            return BitConverter.ToSingle(byteArray, 0);
        }

        private void SetSelectecTag()
        {
            if (Tags == null) return;

            List<string> tagNames = Tags.Keys.ToList();
            tagNames.Add("None");

            InputValueWindow inputValueWindow = new InputValueWindow("Select Tag", "List", tagNames.ToArray(), false, selectedItem: SelectedTag);
            if (inputValueWindow.ShowDialog() == DialogResult.OK)
            {
                object retrievedValue = inputValueWindow.Value;

                if (inputValueWindow.Value != null && retrievedValue.ToString() != "None")
                {
                    SelectedTag = retrievedValue.ToString();
                } else
                {
                    SelectedTag = null;
                }
            }
        }

        private string SetSelectecID()
        {
            if (IDs == null) return null;

            List<string> tagNames = IDs.Keys.ToList();
            tagNames.Add("None");

            InputValueWindow inputValueWindow = new InputValueWindow("Select Tag", "List", tagNames.ToArray(), false, selectedItem: SelectedTag);
            if (inputValueWindow.ShowDialog() == DialogResult.OK)
            {
                object retrievedValue = inputValueWindow.Value;

                if (inputValueWindow.Value != null && retrievedValue.ToString() != "None")
                {
                    return retrievedValue.ToString();
                }
                else
                {
                    return null;
                }
            } else
            {
                return null;
            }
        }

        private void OpenFile(string fileName)
        {
            if (Path.GetExtension(fileName).Equals(".json", StringComparison.OrdinalIgnoreCase))
            {
                CfgBinFileOpened = new CfgBin();
                CfgBinFileOpened.ImportJson(fileName);
            }
            else if (Path.GetExtension(fileName).Equals(".bin", StringComparison.OrdinalIgnoreCase) ||
                     Path.GetExtension(fileName).Equals(".npcbin", StringComparison.OrdinalIgnoreCase))
            {
                CfgBinFileOpened = new CfgBin();
                CfgBinFileOpened.Open(new FileStream(fileName, FileMode.Open, FileAccess.Read));
            }
            else
            {
                MessageBox.Show("Unsupported file format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            variablesDataGridView.Rows.Clear();

            SetSelectecTag();
            DrawTreeView(Path.GetFileNameWithoutExtension(openFileDialog1.FileName), CfgBinFileOpened.Entries);
            FillStrings();

            // Select first item
            if (mainTreeView.Nodes.Count > 0)
            {
                TreeNode nodeToSelect = mainTreeView.Nodes[0];

                if (mainTreeView.Nodes[0].Nodes.Count > 0)
                {
                    nodeToSelect = mainTreeView.Nodes[0].Nodes[0];
                }

                mainTreeView.SelectedNode = nodeToSelect;

                nodeToSelect.Expand();
                nodeToSelect.EnsureVisible();
            }

            searchTextBox.Visible = true;
            saveToolStripMenuItem.Enabled = true;
            variablesDataGridView.Enabled = true;
            stringsDataGridView.Enabled = true;
        }

        private void CfgBinEditorWindow_Load(object sender, EventArgs e)
        {
            Tags = new Dictionary<string, List<Tag>>();

            if (File.Exists("./MyTags.txt"))
            {
                Tags = ImportTags("./MyTags.txt");
            }

            if (File.Exists("./MyIDs.txt"))
            {
                IDs = ImportIDs("./MyIDs.txt");
            }

            variablesDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "All Supported Formats|*.bin;*.npcbin;*.json|Level 5 Bin files (*.bin;*.npcbin)|*.bin;*.npcbin|JSON files (*.json)|*.json";
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.FilterIndex = 1;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                OpenFile(openFileDialog1.FileName);
            }
        }

        private void NewStripMenuItem_Click(object sender, EventArgs e)
        {
            InputValueWindow inputValueWindow = new InputValueWindow("New File", "String", "", false);

            if (inputValueWindow.ShowDialog() == DialogResult.OK)
            {
                CfgBinFileOpened = new CfgBin();
                SetSelectecTag();

                object retrievedValue = inputValueWindow.Value;
                TreeNode rootNode = new TreeNode(retrievedValue.ToString());
                rootNode.Expand();
                rootNode.ContextMenuStrip = contextMenuStrip3;
                mainTreeView.Nodes.Add(rootNode);

                searchTextBox.Visible = true;
                saveToolStripMenuItem.Enabled = true;
                variablesDataGridView.Enabled = true;
                stringsDataGridView.Enabled = true;
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Level 5 Bin files (*.bin)|*.bin|JSON files (*.json)|*.json";
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveFileDialog1.FileName;

                if (Path.GetExtension(fileName).Equals(".json", StringComparison.OrdinalIgnoreCase))
                {
                    CfgBinFileOpened.ToJson(fileName, Tags[SelectedTag]);
                }
                else if (Path.GetExtension(fileName).Equals(".bin", StringComparison.OrdinalIgnoreCase))
                {
                    CfgBinFileOpened.Save(fileName);
                }

                MessageBox.Show(Path.GetFileName(fileName) + " saved!");
            }
        }

        private void ExpandAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainTreeView.ExpandAll();
        }

        private void CollapseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainTreeView.CollapseAll();
        }

        private void ResetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DrawTreeView(Path.GetFileNameWithoutExtension(openFileDialog1.FileName), CfgBinFileOpened.Entries);
        }

        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag != null)
            {
                vsTabControl1.SelectedIndex = 0;
                DrawVariablesDataGridView(e.Node);
            } else
            {
                variablesDataGridView.Rows.Clear();
            }
        }

        private void TreeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                SelectedRightClickTreeNode = e.Node;
            }
        }

        private void ImportItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedRightClickTreeNode != null)
            {
                TreeNode selectedNode = SelectedRightClickTreeNode;
                Entry entry = selectedNode.Tag as Entry;

                openFileDialog3.FileName = "";
                openFileDialog3.Filter = "Level 5 Bin files (*.bin)|*.bin";
                openFileDialog3.RestoreDirectory = true;

                if (openFileDialog3.ShowDialog() == DialogResult.OK)
                {
                    CfgBin newCfgBin = new CfgBin();
                    CfgBinFileOpened.Open(new FileStream(openFileDialog3.FileName, FileMode.Open, FileAccess.Read));

                    if (newCfgBin != null)
                    {
                        // Get all entries names
                        Dictionary<string, int> nameOccurrences = new Dictionary<string, int>();
                        foreach (var myEntry in CfgBinFileOpened.Entries)
                        {
                            myEntry.GetEntryNameOccurrences(nameOccurrences);
                        }

                        // Update import entries names
                        foreach (var newEntry in newCfgBin.Entries)
                        {
                            newEntry.UpdateEntryNames(nameOccurrences);

                            if (entry != null)
                            {
                                entry.Children.Add(newEntry);
                            } else
                            {
                                CfgBinFileOpened.Entries.Add(newEntry);
                            }
                       
                            selectedNode.Nodes.Add(CreateTreeNode(newEntry));
                        }

                        stringsDataGridView.Rows.Clear();
                        FillStrings();

                        // Trigger update event
                        if (vsTabControl1.SelectedIndex == 1)
                        {
                            DrawVariablesDataGridView(mainTreeView.SelectedNode);
                        }

                        MessageBox.Show(Path.GetFileName(openFileDialog3.FileName) + " has been imported on " + SelectedRightClickTreeNode.Text);
                    }
                }

                // Reset
                SelectedRightClickTreeNode = null;
            }
        }

        private void ExportItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedRightClickTreeNode != null)
            {
                TreeNode selectedNode = SelectedRightClickTreeNode;
                Entry entry = selectedNode.Tag as Entry;

                saveFileDialog2.Filter = "Level 5 Bin files (*.bin)|*.bin";
                saveFileDialog2.RestoreDirectory = true;
                saveFileDialog2.FileName = SelectedRightClickTreeNode.Text;

                if (saveFileDialog2.ShowDialog() == DialogResult.OK)
                {
                    byte[] encodedData = entry.EntryToBin();

                    if (encodedData != null && encodedData.Length > 0)
                    {
                        File.WriteAllBytes(saveFileDialog2.FileName, encodedData);
                        MessageBox.Show(Path.GetFileName(saveFileDialog2.FileName) + " saved!");
                    }
                    else
                    {
                        MessageBox.Show("Error encoding data or no data to save.");
                    }
                }

                // Reset
                SelectedRightClickTreeNode = null;
            }
        }

        private void RemoveEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedRightClickTreeNode != null)
            {
                TreeNode selectedNode = SelectedRightClickTreeNode;
                Entry entry = selectedNode.Tag as Entry;

                mainTreeView.SelectedNode = mainTreeView.Nodes[0];

                CfgBinFileOpened.Entries.Remove(entry);
                selectedNode.Remove();

                // Reset
                SelectedRightClickTreeNode = null;
            }
        }

        private void ReplaceItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedRightClickTreeNode != null)
            {
                TreeNode selectedNode = SelectedRightClickTreeNode;
                Entry entry = selectedNode.Tag as Entry;

                openFileDialog4.FileName = "";
                openFileDialog4.Filter = "Level 5 Bin files (*.bin)|*.bin";
                openFileDialog4.RestoreDirectory = true;

                if (openFileDialog4.ShowDialog() == DialogResult.OK)
                {
                    CfgBin newCfgBin = new CfgBin();
                    CfgBinFileOpened.Open(new FileStream(openFileDialog4.FileName, FileMode.Open, FileAccess.Read));

                    if (newCfgBin != null)
                    {
                        TreeNode newNode = null;

                        // Get all entries names
                        Dictionary<string, int> nameOccurrences = new Dictionary<string, int>();
                        foreach (var myEntry in CfgBinFileOpened.Entries)
                        {
                            myEntry.GetEntryNameOccurrences(nameOccurrences);
                        }

                        // Update import entries names & strings
                        newCfgBin.Entries[0].UpdateEntryNames(nameOccurrences);
                        newNode = CreateTreeNode(newCfgBin.Entries[0]);

                        entry = newCfgBin.Entries[0];

                        // Replace the old node with the new node
                        TreeNode parent = SelectedRightClickTreeNode.Parent;
                        if (parent != null)
                        {
                            int index = parent.Nodes.IndexOf(SelectedRightClickTreeNode);
                            parent.Nodes.Remove(SelectedRightClickTreeNode);
                            parent.Nodes.Insert(index, newNode);
                        }
                        else
                        {
                            // If there's no parent, it's the root node
                            mainTreeView.Nodes.Remove(SelectedRightClickTreeNode);
                            mainTreeView.Nodes.Add(newNode);
                        }

                        // Select the new node
                        mainTreeView.SelectedNode = newNode;

                        stringsDataGridView.Rows.Clear();
                        FillStrings();

                        // Trigger update event
                        if (vsTabControl1.SelectedIndex == 1)
                        {
                            DrawVariablesDataGridView(mainTreeView.SelectedNode);
                        }

                        MessageBox.Show(SelectedRightClickTreeNode.Text + " has been replaced by " + Path.GetFileName(openFileDialog4.FileName));
                    }
                }

                // Reset
                SelectedRightClickTreeNode = null;
            }
        }

        private void DuplicateStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (SelectedRightClickTreeNode == null) return;

            TreeNode selectedNode = SelectedRightClickTreeNode;
            TreeNode parentNode = SelectedRightClickTreeNode.Parent;

            Entry entry = selectedNode.Tag as Entry;
            Entry entryParent = selectedNode.Tag as Entry;
            Entry clonedEntry = entry.Clone();
            
            // Get all entries names
            Dictionary<string, int> nameOccurrences = new Dictionary<string, int>();
            foreach (var myEntry in CfgBinFileOpened.Entries)
            {
                myEntry.GetEntryNameOccurrences(nameOccurrences);
            }

            // Update import entries names
            clonedEntry.UpdateEntryNames(nameOccurrences);
            entryParent.Children.Add(clonedEntry);
            parentNode.Nodes.Add(CreateTreeNode(clonedEntry));

            TreeNode latestNode = parentNode.Nodes[parentNode.Nodes.Count - 1];
            mainTreeView.SelectedNode = latestNode;
            latestNode.EnsureVisible();
            MessageBox.Show(SelectedRightClickTreeNode.Text + " has been duplicated on " + latestNode.Text + "!");

            // Reset
            SelectedRightClickTreeNode = null;
        }

        private void VariableDataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (variablesDataGridView.IsCurrentCellDirty)
            {
                if (!VariblesDataGridEditInProgress)
                {
                    VariblesDataGridEditInProgress = true;
                    variablesDataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            }
        }

        private void VariableDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (VariblesDataGridEditInProgress)
            {
                Entry entry = mainTreeView.SelectedNode.Tag as Entry;

                variablesDataGridView.Rows[e.RowIndex].Cells[2].ReadOnly = true;

                string type = variablesDataGridView.Rows[e.RowIndex].Cells[1].Value.ToString();
                object value = variablesDataGridView.Rows[e.RowIndex].Cells[2].Value;
                bool showAsHex = Convert.ToBoolean(variablesDataGridView.Rows[e.RowIndex].Cells[3].Value);

                if (e.ColumnIndex == 1)
                {
                    VariblesDataGridEditInProgress = false;

                    if (type == "Float")
                    {
                        if (showAsHex)
                        {
                            entry.Variables[e.RowIndex].Value = ConvertLittleEndianHexToFloat(value.ToString());
                            variablesDataGridView.Rows[e.RowIndex].Cells[2].Value = ConvertLittleEndianHexToFloat(value.ToString()).ToString("X8");
                        }
                        else
                        {
                            try
                            {
                                float convertedValue = Convert.ToSingle(value);
                                entry.Variables[e.RowIndex].Value = convertedValue;
                                variablesDataGridView.Rows[e.RowIndex].Cells[2].Value = convertedValue;
                            }
                            catch (FormatException)
                            {
                                entry.Variables[e.RowIndex].Value = 0f;
                                variablesDataGridView.Rows[e.RowIndex].Cells[2].Value = 0f;
                            }
                        }

                        entry.Variables[e.RowIndex].Type = Level5.Binary.Logic.Type.Float;
                        variablesDataGridView.Rows[e.RowIndex].Cells[3].ReadOnly = false;
                    }
                    else if (type == "String")
                    {
                        if (showAsHex)
                        {
                            entry.Variables[e.RowIndex].Value = ConvertLittleEndianHexToInt(value.ToString());
                            variablesDataGridView.Rows[e.RowIndex].Cells[2].Value = ConvertLittleEndianHexToInt(value.ToString()).ToString("X8");
                        }
                        else
                        {
                            entry.Variables[e.RowIndex].Value = value.ToString();
                            variablesDataGridView.Rows[e.RowIndex].Cells[2].Value = value.ToString();
                        }

                        entry.Variables[e.RowIndex].Type = Level5.Binary.Logic.Type.String;
                        variablesDataGridView.Rows[e.RowIndex].Cells[3].Value = false;
                        variablesDataGridView.Rows[e.RowIndex].Cells[3].ReadOnly = true;
                        FillStrings();
                    }
                    else
                    {
                        if (showAsHex)
                        {
                            entry.Variables[e.RowIndex].Value = ConvertLittleEndianHexToInt(value.ToString());
                            variablesDataGridView.Rows[e.RowIndex].Cells[2].Value = ConvertLittleEndianHexToInt(value.ToString()).ToString("X8");
                        }
                        else
                        {
                            try
                            {
                                int convertedValue = Convert.ToInt32(value);
                                entry.Variables[e.RowIndex].Value = convertedValue;
                                variablesDataGridView.Rows[e.RowIndex].Cells[2].Value = convertedValue;
                            }
                            catch (FormatException)
                            {
                                entry.Variables[e.RowIndex].Value = 0;
                                variablesDataGridView.Rows[e.RowIndex].Cells[2].Value = 0;
                            }
                        }

                        entry.Variables[e.RowIndex].Type = Level5.Binary.Logic.Type.Int;
                        variablesDataGridView.Rows[e.RowIndex].Cells[3].ReadOnly = false;
                    }
                }
                else if (e.ColumnIndex == 3)
                {
                    VariblesDataGridEditInProgress = false;

                    if (type == "Float")
                    {
                        if (showAsHex)
                        {
                            byte[] byteArray = BitConverter.GetBytes((float)value);
                            Array.Reverse(byteArray);
                            variablesDataGridView.Rows[e.RowIndex].Cells[2].Value = BitConverter.ToString(byteArray).Replace("-", "");
                        }
                        else
                        {
                            variablesDataGridView.Rows[e.RowIndex].Cells[2].Value = ConvertLittleEndianHexToFloat(value.ToString());
                        }
                    }
                    else
                    {
                        int myInt = Convert.ToInt32(entry.Variables[e.RowIndex].Value);

                        if (showAsHex)
                        {
                            variablesDataGridView.Rows[e.RowIndex].Cells[2].Value = myInt.ToString("X8");
                        }
                        else
                        {
                            variablesDataGridView.Rows[e.RowIndex].Cells[2].Value = ConvertLittleEndianHexToInt(myInt.ToString("X8"));
                        }
                    }
                }

                VariblesDataGridEditInProgress = false;
            }
        }

        private void VariablesDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                if (SelectedTag != null)
                {
                    Entry entry = mainTreeView.SelectedNode.Tag as Entry;

                    Tag tag = Tags[SelectedTag].Find(x => x.Name == entry.GetName());

                    if (tag == null)
                    {
                        tag = new Tag();
                        tag.Name = entry.GetName();

                        for (int i = 0; i < entry.Variables.Count(); i++)
                        {
                            tag.Properties.Add(("Variable " + i, false));
                        }

                        Tags[SelectedTag].Add(tag);
                    }

                    string name = variablesDataGridView.Rows[variablesDataGridView.CurrentRow.Index].Cells[0].Value.ToString();

                    InputValueWindow inputValueWindow = new InputValueWindow(name, "String", name, false, false, IDs);

                    if (inputValueWindow.ShowDialog() == DialogResult.OK)
                    {
                        string retrievedValue = inputValueWindow.Value.ToString();
                        variablesDataGridView.Rows[variablesDataGridView.CurrentRow.Index].Cells[0].Value = retrievedValue;

                        (string, bool) property = tag.Properties[variablesDataGridView.CurrentRow.Index];
                        tag.Properties[variablesDataGridView.CurrentRow.Index] = (retrievedValue, property.Item2);

                        ExportTags(Tags, "./MyTags.txt");
                    }                      
                }
            }
            if (e.ColumnIndex == 2)
            {
                Entry entry = mainTreeView.SelectedNode.Tag as Entry;

                variablesDataGridView.Rows[variablesDataGridView.CurrentRow.Index].Cells[2].ReadOnly = true;

                object value = null;
                string name = variablesDataGridView.Rows[variablesDataGridView.CurrentRow.Index].Cells[0].Value.ToString();
                string type = variablesDataGridView.Rows[variablesDataGridView.CurrentRow.Index].Cells[1].Value.ToString();
                bool showAsHex = Convert.ToBoolean(variablesDataGridView.Rows[variablesDataGridView.CurrentRow.Index].Cells[3].Value);

                if (type == "String")
                {
                    value = Convert.ToString(entry.Variables[variablesDataGridView.CurrentRow.Index].Value);
                }
                else if (type == "Float")
                {
                    float myFloat = Convert.ToSingle(entry.Variables[variablesDataGridView.CurrentRow.Index].Value);

                    if (showAsHex)
                    {
                        byte[] byteArray = BitConverter.GetBytes((float)myFloat);
                        Array.Reverse(byteArray);
                        value = BitConverter.ToString(byteArray).Replace("-", "");
                    }
                    else
                    {
                        value = myFloat;
                    }
                }
                else
                {
                    int myInt = Convert.ToInt32(entry.Variables[e.RowIndex].Value);

                    if (showAsHex)
                    {
                        value = myInt.ToString("X8");
                    }
                    else
                    {
                        value = myInt;
                    }
                }

                var result = IDs?.SelectMany(dict => dict.Value
                        .SelectMany(pair => pair.Value
                            .Select((innerValue, innerIndex) => new { Value = innerValue, Index = innerIndex, DictKey = dict.Key, PairKey = pair.Key })
                        )
                    )
                    .FirstOrDefault(item =>
                    {
                        if (value.GetType() == typeof(string))
                        {
                            return Convert.ToInt32(value.ToString(), 16) == item.Value.Hash;
                        }
                        else
                        {
                            return Convert.ToInt32(value) == item.Value.Hash;
                        }
                    });

                InputValueWindow inputValueWindow = null;

                if (IDs != null)
                {
                    string hashName = variablesDataGridView.Rows[variablesDataGridView.CurrentRow.Index].Cells[2].Value.ToString();

                    if (result != null)
                    {
                        inputValueWindow = new InputValueWindow(name, type, value, showAsHex, type == "Int" && IDs != null, IDs, null, hashName, result.DictKey, result.PairKey);
                    }
                     else
                    {
                        inputValueWindow = new InputValueWindow(name, type, value, showAsHex, type == "Int" && IDs != null, IDs, null, hashName);
                    }
                    
                } else
                {
                    inputValueWindow = new InputValueWindow(name, type, value, showAsHex, type == "Int" && IDs != null, IDs);
                }

                if (inputValueWindow.ShowDialog() == DialogResult.OK)
                {
                    object retrievedValue = inputValueWindow.Value;

                    if (type == "String")
                    {
                        entry.Variables[variablesDataGridView.CurrentRow.Index].Value = retrievedValue.ToString();
                        variablesDataGridView.Rows[variablesDataGridView.CurrentRow.Index].Cells[2].Value = retrievedValue.ToString();
                        CfgBinFileOpened.ReplaceString(Convert.ToString(value), retrievedValue.ToString());
                        FillStrings();
                    }
                    else if (type == "Float")
                    {
                        entry.Variables[variablesDataGridView.CurrentRow.Index].Value = Convert.ToSingle(retrievedValue);

                        if (showAsHex)
                        {
                            byte[] byteArray = BitConverter.GetBytes(Convert.ToSingle(retrievedValue));
                            Array.Reverse(byteArray);

                            variablesDataGridView.Rows[variablesDataGridView.CurrentRow.Index].Cells[2].Value = BitConverter.ToString(byteArray).Replace("-", "");
                        }
                        else
                        {
                            variablesDataGridView.Rows[variablesDataGridView.CurrentRow.Index].Cells[2].Value = Convert.ToSingle(retrievedValue);
                        }
                    }
                    else
                    {
                        entry.Variables[variablesDataGridView.CurrentRow.Index].Value = Convert.ToInt32(retrievedValue);

                        if (IDs != null)
                        {
                            int finalValue = 0;

                            if (value.GetType() == typeof(string))
                            {
                                finalValue = Convert.ToInt32(value.ToString(), 16);
                            }
                            else
                            {
                                finalValue = Convert.ToInt32(value);
                            }

                            string hashName = inputValueWindow.Hash;
                            string tagName = inputValueWindow.TagName;
                            string tagGroupName = inputValueWindow.TagGroupName;

                            if (tagName != null && tagName != "" && tagGroupName != null && tagGroupName != "" && hashName != null && hashName != "")
                            {
                                if (!IDs.ContainsKey(tagName))
                                {
                                    IDs.Add(tagName, new Dictionary<string, List<ID>>());
                                }

                                if (!IDs[tagName].ContainsKey(tagGroupName)) {
                                    IDs[tagName].Add(tagGroupName, new List<ID>());
                                }

                                int index = IDs[tagName][tagGroupName].FindIndex(x => x.Hash == finalValue);
                                if (index > -1)
                                {
                                    IDs[tagName][tagGroupName][index].Name = hashName;
                                } else
                                {
                                    IDs[tagName][tagGroupName].Add(new ID(finalValue, hashName));
                                }

                                ExportIDs(IDs, "./MyIDs.txt");
                            }

                            DrawVariablesDataGridView(mainTreeView.SelectedNode);

                            variablesDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
                            variablesDataGridView.FirstDisplayedScrollingRowIndex = e.RowIndex;
                            variablesDataGridView.FirstDisplayedScrollingColumnIndex = e.ColumnIndex;
                        } else
                        {
                            if (showAsHex)
                            {
                                variablesDataGridView.Rows[variablesDataGridView.CurrentRow.Index].Cells[2].Value = Convert.ToInt32(retrievedValue).ToString("X8");
                            }
                            else
                            {
                                variablesDataGridView.Rows[variablesDataGridView.CurrentRow.Index].Cells[2].Value = Convert.ToInt32(retrievedValue);
                            }
                        }                    
                    }
                }

                variablesDataGridView.Rows[variablesDataGridView.CurrentRow.Index].Cells[2].ReadOnly = false;
                variablesDataGridView.Focus();
            }
        }

        private void CfgBinEditorWindow_SizeChanged(object sender, EventArgs e)
        {
            variablesDataGridView.AutoResizeColumns();
        }

        private void StringsDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string name = "Edit String " + e.RowIndex;
            string type = "String";
            object value = stringsDataGridView.Rows[e.RowIndex].Cells[0].Value;
            bool showAsHex = false;

            InputValueWindow inputValueWindow = new InputValueWindow(name, type, value, showAsHex);
            if (inputValueWindow.ShowDialog() == DialogResult.OK)
            {
                string retrievedValue = Convert.ToString(inputValueWindow.Value);
                CfgBinFileOpened.ReplaceString(Convert.ToString(value), retrievedValue);

                BeginInvoke(new Action(() =>
                {
                    FillStrings();
                }));
            }
        }

        private void VsTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (vsTabControl1.SelectedIndex != 0) return;

            // Trigger update event
            if (mainTreeView.SelectedNode != null && mainTreeView.SelectedNode.Tag != null)
            {
                vsTabControl1.SelectedIndex = 0;
                DrawVariablesDataGridView(mainTreeView.SelectedNode);
            }
        }

        private void TableLayoutPanel1_MouseDown(object sender, MouseEventArgs e)
        {
            Resizing = true;
            StartX = e.X;

            if (Colindex != -1)
            {
                StartWidth = tableLayoutPanel1.GetColumnWidths()[Colindex];
            }
        }

        private void TableLayoutPanel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!Resizing)
            {
                Colindex = -1;
                tableLayoutPanel1.Cursor = Cursors.Default;

                for (int i = 0; i < tableLayoutPanel1.ColumnCount - 1; i++)
                {
                    int columnLeft = tableLayoutPanel1.GetColumnWidths().Take(i + 1).Sum();
                    int columnRight = tableLayoutPanel1.GetColumnWidths().Take(i + 2).Sum();

                    if (i == 0)
                    {
                        // Handle the left side of the first column
                        if (e.X >= 0 && e.X <= 3)
                        {
                            Colindex = 0;
                            tableLayoutPanel1.Cursor = Cursors.SizeWE;
                            break;
                        }
                    }

                    // Handle the right side of the first column
                    if (i == 0 && e.X >= columnRight - 3 && e.X <= columnRight + 3)
                    {
                        Colindex = i + 1;
                        tableLayoutPanel1.Cursor = Cursors.SizeWE;
                        break;
                    }

                    // Handle the left side of a column
                    if (e.X >= columnLeft - 3 && e.X <= columnLeft + 3)
                    {
                        Colindex = i;
                        tableLayoutPanel1.Cursor = Cursors.SizeWE;
                        break;
                    }

                    // Handle the right side of a column
                    if (e.X >= columnRight - 3 && e.X <= columnRight + 3)
                    {
                        Colindex = i + 1;
                        tableLayoutPanel1.Cursor = Cursors.SizeWE;
                        break;
                    }
                }
            }
            if (Resizing && Colindex > -1)
            {
                int newWidth = StartWidth + (e.X - StartX);
                if (newWidth < 0) return;

                tableLayoutPanel1.ColumnStyles[Colindex].Width = newWidth;
            }
        }


        private void TableLayoutPanel1_MouseUp(object sender, MouseEventArgs e)
        {
            Resizing = false;
            tableLayoutPanel1.Cursor = null;
        }

        private void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!searchTextBox.Focused || searchTextBox.Text == "Search...") return;

            if (searchTextBox.Text == null || searchTextBox.Text == "")
            {
                DrawTreeView(Path.GetFileNameWithoutExtension(openFileDialog1.FileName), CfgBinFileOpened.Entries);
                searchTextBox.Text = "Search...";
            }
            else
            {
                List<Entry> entries = CfgBinFileOpened.FindEntry(searchTextBox.Text);

                if (entries.Count > 0)
                {
                    DrawTreeView(searchTextBox.Text, entries);
                }
            }

            // Select first item
            if (mainTreeView.Nodes.Count > 0)
            {
                TreeNode nodeToSelect = mainTreeView.Nodes[0];

                if (mainTreeView.Nodes[0].Nodes.Count > 0)
                {
                    nodeToSelect = mainTreeView.Nodes[0].Nodes[0];
                }

                mainTreeView.SelectedNode = nodeToSelect;

                nodeToSelect.Expand();
                nodeToSelect.EnsureVisible();
            }
        }

        private void CfgBinEditorWindow_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            string dragPath = Path.GetFullPath(files[0]);
            string dragExt = Path.GetExtension(files[0]);

            if (files.Length > 1) return;
            if (dragExt != ".json" & dragExt != ".bin" & dragExt != ".npcbin") return;

            openFileDialog1.FileName = dragPath;
            OpenFile(openFileDialog1.FileName);
        }

        private void CfgBinEditorWindow_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }
    }
}
