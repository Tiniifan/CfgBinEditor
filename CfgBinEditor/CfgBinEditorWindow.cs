using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CfgBinEditor.Level5.Binary;

namespace CfgBinEditor
{
    public partial class CfgBinEditorWindow : Form
    {
        private CfgBin CfgBinFileOpened;

        private Dictionary<string, object> SelectedEtry;

        private int SelectedIndex;

        private TreeNode SelectedRightClickTreeNode;

        private bool UserEditInProgress = false;

        private Dictionary<string, List<Tag>> Tags;

        private string SelectedTag;

        public CfgBinEditorWindow()
        {
            InitializeComponent();
        }

        private void CfgBinEditorWindow_Load(object sender, EventArgs e)
        {
            Tags = new Dictionary<string, List<Tag>>();

            if (File.Exists("./MyTags.txt"))
            {
                Tags = ImportTags("./MyTags.txt");
                SetTagMenu();

            }
        }

        private void SetTagMenu()
        {
            tagsToolStripMenuItem.DropDownItems.Clear();

            foreach (string key in Tags.Keys)
            {
                ToolStripMenuItem newMenu = new ToolStripMenuItem(key);
                newMenu.Click += MenuItem_Click;
                tagsToolStripMenuItem.DropDownItems.Add(newMenu);
            }

            if (Tags.Count > 0)
            {
                tagsToolStripMenuItem.DropDownItems[0].PerformClick();
            }
        }

        private void MenuItem_Click(object sender, EventArgs e)
        {
            // Décocher tous les éléments du menu
            foreach (ToolStripMenuItem item in menuStrip1.Items)
            {
                if (item is ToolStripMenuItem)
                {
                    item.Checked = false;
                }
            }

            // Marquer l'élément de menu cliqué
            if (sender is ToolStripMenuItem clickedItem)
            {
                clickedItem.Checked = true;
                SelectedTag = clickedItem.Text; // Mettre à jour la variable SelectedTag avec le nom du menu
            }
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
                            currentTags[currentTags.Count - 1].Properties[propName] = propValue;
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

        private void ImportTagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog2.Filter = "Text File (*.txt)|*.txt";
            openFileDialog2.RestoreDirectory = true;

            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                Dictionary<string, List<Tag>> newTags = ImportTags(openFileDialog2.FileName);

                foreach(KeyValuePair<string, List<Tag>> kvp in newTags)
                {
                    foreach(Tag tag in kvp.Value)
                    {
                        if (Tags.ContainsKey(kvp.Key) == false)
                        {
                            Tags.Add(kvp.Key, new List<Tag>());
                        }

                        if (Tags[kvp.Key].Any(x => x.Name != tag.Name))
                        {
                            Tags[kvp.Key].Add(tag);
                        }
                    }
                }

                // Update tag
                SetTagMenu();

                // Save new tag
                string filePath = "./MyTags.txt";

                try
                {
                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        foreach (var kvp in Tags)
                        {
                            writer.WriteLine(kvp.Key + " [");
                            foreach (Tag tag in kvp.Value)
                            {
                                writer.WriteLine("\t" + tag.Name + " (");
                                foreach (var property in tag.Properties)
                                {
                                    writer.WriteLine("\t\t" + property.Key + "|" + property.Value.ToString());
                                }
                                writer.WriteLine("\t)");
                            }
                            writer.WriteLine("]");
                        }
                    }

                    MessageBox.Show("MyTags.txt was updated!");
                }
                catch (Exception)
                {
                    MessageBox.Show("An error occurred while saving the tags.");
                }
            }
        }

        private TreeNode CreateTreeNode(KeyValuePair<string, object> input)
        {
            var rootNode = new TreeNode(input.Key);

            if (input.Value is Dictionary<string, object> dict)
            {
                foreach (var entry in dict)
                {
                    if (entry.Key.Contains("_ITEMS_"))
                    {
                        rootNode.Nodes.Add(new TreeNode(entry.Key.Replace("_ITEMS", "")));
                    }
                    else
                    {
                        rootNode.ContextMenuStrip = contextMenuStrip1;

                        var childNode = CreateTreeNode(entry);

                        if (childNode != null)
                        {
                            rootNode.Nodes.Add(childNode);
                        }
                    }
                }
            }

            return rootNode;
        }

        private void DrawTreeView()
        {
            TreeNode rootNode = new TreeNode(Path.GetFileNameWithoutExtension(openFileDialog1.FileName));

            foreach (var entry in CfgBinFileOpened.Entries)
            {
                rootNode.Nodes.Add(CreateTreeNode(entry));
            }

            treeView1.Nodes.Clear();
            treeView1.Nodes.Add(rootNode);
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Level 5 Bin files (*.bin)|*.bin";
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                variableDataGridView.Rows.Clear();

                CfgBinFileOpened = new CfgBin(new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read));
                DrawTreeView();

                saveToolStripMenuItem.Enabled = true;
                variableDataGridView.Enabled = true;
            }
        }

        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode selectedNode = e.Node;
            string firstSelectedNodeText = e.Node.Text;

            List<TreeNode> parentNodes = new List<TreeNode>();

            while (selectedNode.Parent != null)
            {
                parentNodes.Add(selectedNode.Parent);
                selectedNode = selectedNode.Parent;
            }

            Dictionary<string, object> selectedEntry = CfgBinFileOpened.Entries;

            for (int i = parentNodes.Count - 1; i >= 0; i--)
            {
                TreeNode node = parentNodes[i];
                if (selectedEntry.ContainsKey(node.Text))
                {
                    selectedEntry = selectedEntry[node.Text] as Dictionary<string, object>;
                }
            }

            if (firstSelectedNodeText != Path.GetFileNameWithoutExtension(openFileDialog1.FileName))
            {
                Tag tag = null;
                SelectedIndex = Convert.ToInt32(firstSelectedNodeText.Split('_').Last());
                
                if (SelectedTag != null && Tags.ContainsKey(SelectedTag))
                {
                    tag = Tags[SelectedTag].Find(x => x.Name == CfgBinFileOpened.TransformKey(firstSelectedNodeText));
                }
                
                if (selectedEntry.Values.ToList()[SelectedIndex].GetType() == typeof(List<CfgBinSupport.Variable>))
                {
                    SelectedEtry = selectedEntry;

                    variableDataGridView.Rows.Clear();
                    List<CfgBinSupport.Variable> variables = SelectedEtry.Values.ToList()[SelectedIndex] as List<CfgBinSupport.Variable>;

                    for (int i = 0; i < variables.Count; i++)
                    {
                        string variableName = "Variable " + i;
                        bool showAsHex = false;

                        if (tag != null)
                        {
                            variableName = tag.Properties.Keys.ToArray()[i];
                            showAsHex = tag.Properties[variableName];
                        }

                        CfgBinSupport.Variable variable = variables[i];
                        variableDataGridView.Rows.Add();

                        DataGridViewComboBoxCell comboBox = (variableDataGridView.Rows[0].Cells[1] as DataGridViewComboBoxCell);

                        if (variable.Type is CfgBinSupport.Type.String)
                        {
                            if (showAsHex == true)
                            {
                                variableDataGridView.Rows[variableDataGridView.Rows.Count - 1].SetValues(new object[] { variableName, comboBox.Items[0], Convert.ToInt32(variable.Value).ToString("X8"), showAsHex });
                            } else
                            {
                                if (CfgBinFileOpened.Strings.ContainsKey((int)variable.Value))
                                {
                                    variableDataGridView.Rows[variableDataGridView.Rows.Count - 1].SetValues(new object[] { variableName, comboBox.Items[0], CfgBinFileOpened.Strings[(int)variable.Value], showAsHex });
                                }
                                else
                                {
                                    variableDataGridView.Rows[variableDataGridView.Rows.Count - 1].SetValues(new object[] { variableName, comboBox.Items[0], "", showAsHex });
                                }
                            }                         
                        }
                        else if (variable.Type is CfgBinSupport.Type.Int || variable.Type is CfgBinSupport.Type.Unknown)
                        {
                            if (showAsHex == true)
                            {
                                variableDataGridView.Rows[variableDataGridView.Rows.Count - 1].SetValues(new object[] { variableName, comboBox.Items[1], Convert.ToInt32(variable.Value).ToString("X8"), showAsHex });
                            } else {
                                variableDataGridView.Rows[variableDataGridView.Rows.Count - 1].SetValues(new object[] { variableName, comboBox.Items[1], variable.Value, showAsHex });
                            }
                        }
                        else if (variable.Type is CfgBinSupport.Type.Float)
                        {
                            if (showAsHex == true)
                            {
                                byte[] byteArray = BitConverter.GetBytes((float)variable.Value);
                                variableDataGridView.Rows[variableDataGridView.Rows.Count - 1].SetValues(new object[] { variableName, comboBox.Items[2], BitConverter.ToString(byteArray).Replace("-", ""), showAsHex });
                            } else
                            {
                                variableDataGridView.Rows[variableDataGridView.Rows.Count - 1].SetValues(new object[] { variableName, comboBox.Items[2], variable.Value, showAsHex });
                            }
                        }
                    }

                }
            }
        }

        private void TreeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                SelectedRightClickTreeNode = e.Node;
            }
        }

        private void AddEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedRightClickTreeNode != null)
            {
                TreeNode selectedNode = SelectedRightClickTreeNode;

                List<TreeNode> pathNodes = new List<TreeNode>() { selectedNode };

                while (selectedNode.Parent != null)
                {
                    pathNodes.Add(selectedNode.Parent);
                    selectedNode = selectedNode.Parent;
                }

                Dictionary<string, object> selectedEntry = CfgBinFileOpened.Entries;

                for (int i = pathNodes.Count - 1; i >= 0; i--)
                {
                    TreeNode node = pathNodes[i];
                    
                    if (selectedEntry.ContainsKey(node.Text))
                    {
                        selectedEntry = selectedEntry[node.Text] as Dictionary<string, object>;
                    }
                }

                var firstItem = selectedEntry.ElementAt(0);

                if (firstItem.Value.GetType() != typeof(List<CfgBinSupport.Variable>))
                {
                    string entryName = string.Join("", firstItem.Key.Take(firstItem.Key.LastIndexOf('_')));
                    selectedEntry.Add(entryName + "_" + (selectedEntry.Count), firstItem.Value);
                    MessageBox.Show(entryName + "_" + (selectedEntry.Count - 1) + " has been added");
                } else
                {
                    // Info

                    string entryName = string.Join("", firstItem.Key.Take(firstItem.Key.LastIndexOf('_')));
                    List<CfgBinSupport.Variable> variables = firstItem.Value as List<CfgBinSupport.Variable>;
                    List<CfgBinSupport.Variable> newVariables = new List<CfgBinSupport.Variable>();

                    for (int i = 0; i < variables.Count; i++)
                    {
                        if (variables[i].Type == CfgBinSupport.Type.String)
                        {
                            newVariables.Add(new CfgBinSupport.Variable(CfgBinSupport.Type.String, -1));
                        } else
                        {
                            newVariables.Add(new CfgBinSupport.Variable(variables[i].Type, 0));
                        }
                    }

                    selectedEntry.Add(entryName + "_" + (selectedEntry.Count), newVariables);
                    MessageBox.Show(entryName + "_" + (selectedEntry.Count -1) + " has been added");
                }

                // Reset
                SelectedEtry = null;
                SelectedIndex = -1;
                SelectedRightClickTreeNode = null;

                // Reload
                DrawTreeView();
                treeView1.Focus();
            }
        }

        private void RemoveEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Level 5 Bin files (*.bin)|*.bin";
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                CfgBinFileOpened.Save(saveFileDialog1.FileName);
                MessageBox.Show(Path.GetFileName(saveFileDialog1.FileName) + " saved!");
            }
        }

        private void VariableDataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            string type = variableDataGridView.Rows[variableDataGridView.CurrentRow.Index].Cells[1].Value.ToString();
            bool showAsHex = Convert.ToBoolean(variableDataGridView.Rows[variableDataGridView.CurrentRow.Index].Cells[3].Value);

            if (variableDataGridView.CurrentCell.ColumnIndex == 2)
            {
                TextBox textBox = e.Control as TextBox;
                textBox.TextChanged -= null;

                if (showAsHex)
                {
                    textBox.TextChanged += (s, eventArgs) =>
                    {
                        string hexString = textBox.Text;
                        if (hexString.Length > 8)
                            hexString = hexString.Substring(hexString.Length - 8);

                        if (!IsHexDigit(hexString))
                        {
                            hexString = hexString.Length > 0 ? hexString.Substring(0, hexString.Length - 1) : "";
                        }

                        textBox.Text = hexString;
                        textBox.SelectionStart = textBox.Text.Length;
                    };
                } else if (type == "Int" || type == "Unknown")
                {
                    textBox.KeyPress += (s, eventArgs) =>
                    {
                        if ((!char.IsDigit(eventArgs.KeyChar) && eventArgs.KeyChar != '-' && eventArgs.KeyChar != '\b') ||
                            (eventArgs.KeyChar == '-' && textBox.Text.Length > 0) ||
                            (textBox.Text.Length >= 12)) // Max length of characters for "-4294967295"
                            eventArgs.Handled = true;
                    };
                }
                else if (type == "Float")
                {
                    string decimalSeparator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

                    if (!showAsHex)
                    {
                        textBox.KeyPress += (s, eventArgs) =>
                        {
                            if ((!char.IsDigit(eventArgs.KeyChar) && eventArgs.KeyChar != '-' && eventArgs.KeyChar != decimalSeparator[0] && eventArgs.KeyChar != '\b') ||
                                (eventArgs.KeyChar == '-' && textBox.Text.Length > 0) ||
                                (eventArgs.KeyChar == decimalSeparator[0] && textBox.Text.Contains(decimalSeparator)) ||
                                (textBox.Text.Length >= 12)) // Max length of characters for "-4294967295"
                                eventArgs.Handled = true;
                        };
                    }
                }
            }
        }

        private bool IsHexDigit(string input)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(input, @"\A\b[0-9a-fA-F]+\b\Z");
        }

        private void VariableDataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (variableDataGridView.IsCurrentCellDirty)
            {
                if (!UserEditInProgress)
                {
                    UserEditInProgress = true;
                    variableDataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            }
        }

        private void VariableDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (UserEditInProgress)
            {
                int index = e.RowIndex;
                List<CfgBinSupport.Variable> variables = SelectedEtry.Values.ToList()[SelectedIndex] as List<CfgBinSupport.Variable>;

                string type = variableDataGridView.Rows[e.RowIndex].Cells[1].Value.ToString();
                object value = variableDataGridView.Rows[e.RowIndex].Cells[2].Value;
                bool showAsHex = Convert.ToBoolean(variableDataGridView.Rows[e.RowIndex].Cells[3].Value);

                if (e.ColumnIndex == 1)
                {
                    UserEditInProgress = false;

                    if (type == "Int" || type == "Unknown")
                    {
                        if (showAsHex)
                        {
                            variables[index].Value = ConvertLittleEndianHexToInt(value.ToString());
                            variableDataGridView.Rows[e.RowIndex].Cells[2].Value = ConvertLittleEndianHexToInt(value.ToString()).ToString("X8");
                        }
                        else
                        {
                            try
                            {
                                int convertedValue = Convert.ToInt32(value);
                                variables[index].Value = convertedValue;
                                variableDataGridView.Rows[e.RowIndex].Cells[2].Value = convertedValue;
                            }
                            catch (FormatException)
                            {
                                variables[index].Value = 0;
                                variableDataGridView.Rows[e.RowIndex].Cells[2].Value = 0;
                            }
                        }
                    }
                    else if (type == "Float")
                    {
                        if (showAsHex)
                        {
                            variables[index].Value = ConvertLittleEndianHexToFloat(value.ToString());
                            variableDataGridView.Rows[e.RowIndex].Cells[2].Value = ConvertLittleEndianHexToFloat(value.ToString()).ToString("X8");
                        }
                        else
                        {
                            try
                            {
                                float convertedValue = Convert.ToSingle(value);
                                variables[index].Value = convertedValue;
                                variableDataGridView.Rows[e.RowIndex].Cells[2].Value = convertedValue;
                            }
                            catch (FormatException)
                            {
                                variables[index].Value = 0f;
                                variableDataGridView.Rows[e.RowIndex].Cells[2].Value = 0f;
                            }
                        }
                    }
                    else if (type == "String")
                    {
                        if (showAsHex)
                        {
                            variables[index].Value = ConvertLittleEndianHexToInt(value.ToString());
                            variableDataGridView.Rows[e.RowIndex].Cells[2].Value = ConvertLittleEndianHexToInt(value.ToString()).ToString("X8");
                        }
                        else
                        {
                            variables[index].Value = value.ToString();
                            variableDataGridView.Rows[e.RowIndex].Cells[2].Value = value.ToString();
                        }
                    }
                }
                else if (e.ColumnIndex == 2)
                {
                    if (type == "Int" || type == "Unknown")
                    {
                        if (showAsHex)
                        {
                            variables[index].Value = ConvertLittleEndianHexToInt(value.ToString());
                        } else
                        {
                            variables[index].Value = value;
                        }
                    } 
                    else if (type == "Float")
                    {
                        if (showAsHex)
                        {
                            variables[index].Value = ConvertLittleEndianHexToFloat(value.ToString());
                        }
                        else
                        {
                            variables[index].Value = value;
                        }
                    } 
                    else if (type == "String")
                    {
                        if (showAsHex)
                        {
                            variables[index].Value = ConvertLittleEndianHexToInt(value.ToString());
                        }
                        else
                        {
                            CfgBinFileOpened.UpdateStrings((int)variables[index].Value, value.ToString());
                        }
                    }
                }
                else if (e.ColumnIndex == 3)
                {
                    UserEditInProgress = false;

                    if (type == "Int" || type == "Unknown")
                    {
                        if (showAsHex)
                        {
                            variableDataGridView.Rows[e.RowIndex].Cells[2].Value = Convert.ToInt32(value).ToString("X8");
                        }
                        else
                        {
                            variableDataGridView.Rows[e.RowIndex].Cells[2].Value = ConvertLittleEndianHexToInt(value.ToString());
                        }                 
                    }
                    else if (type == "Float")
                    {
                        if (showAsHex)
                        {
                            byte[] byteArray = BitConverter.GetBytes((float)value);
                            variableDataGridView.Rows[e.RowIndex].Cells[2].Value = BitConverter.ToString(byteArray).Replace("-", "");
                        }
                        else
                        {
                            variableDataGridView.Rows[e.RowIndex].Cells[2].Value = ConvertLittleEndianHexToFloat(value.ToString());
                        }
                    }
                    else if (type == "String")
                    {
                        if (showAsHex)
                        {
                            variableDataGridView.Rows[e.RowIndex].Cells[2].Value = Convert.ToInt32((int)variables[index].Value).ToString("X8");
                        }
                        else
                        {
                            if (CfgBinFileOpened.Strings.ContainsKey((int)variables[index].Value)) 
                            {
                                variableDataGridView.Rows[e.RowIndex].Cells[2].Value = CfgBinFileOpened.Strings[(int)variables[index].Value];
                            }
                        }
                    }
                }
                
                UserEditInProgress = false; // Réinitialisez le drapeau après le traitement
            }
            
        }

        private int ConvertLittleEndianHexToInt(string hexString)
        {
            int byteCount = (hexString.Length + 1) / 2; // Calcul du nombre d'octets nécessaires
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
            int byteCount = (hexString.Length + 1) / 2; // Calcul du nombre d'octets nécessaires
            byte[] byteArray = new byte[byteCount];

            for (int i = 0; i < byteCount; i++)
            {
                int startIndex = Math.Max(hexString.Length - (i + 1) * 2, 0);
                string byteHex = hexString.Substring(startIndex, Math.Min(2, hexString.Length - startIndex));
                byteArray[i] = Convert.ToByte(byteHex, 16);
            }

            Array.Reverse(byteArray); // Inverser l'ordre des octets pour little endian

            return BitConverter.ToSingle(byteArray, 0);
        }
    }
}
