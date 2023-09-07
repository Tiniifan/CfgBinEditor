using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using CfgBinEditor.Level5.Logic;
using CfgBinEditor.Level5.Binary;

namespace CfgBinEditor
{
    public partial class SearchWindow : Form
    { 
        private CfgBin CfgBinFileOpened;

        public object SearchValue { get; private set; }

        public List<Entry> MatchesEntries { get; private set; }

        readonly Dictionary<string, Dictionary<string, List<ID>>> IDs;

        public SearchWindow(Dictionary<string, Dictionary<string, List<ID>>> ids, CfgBin cfgBinFileOpened)
        {
            InitializeComponent();

            IDs = ids;
            CfgBinFileOpened = cfgBinFileOpened;
        }

        private void SearchWindow_Load(object sender, EventArgs e)
        {
            typeComboBox.SelectedIndex = 0;
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            object value;
            bool showAsHex = showAsHexCheckBox.Checked;
            string type = typeComboBox.SelectedItem.ToString();

            if (showAsHex)
            {
                value = "";
            } else
            {
                switch (type)
                {
                    case "String":
                        value = "";
                        break;
                    case "Float":
                        value = 0f;
                        break;
                    default:
                        value = 0;
                        break;
                }
            }           

            InputValueWindow inputValueWindow = new InputValueWindow("Search value", type, value, showAsHex, type == "Int" && IDs != null, IDs);
            if (inputValueWindow.ShowDialog() == DialogResult.OK)
            {
                object retrievedValue = inputValueWindow.Value;
                SearchValue = retrievedValue;

                MatchesEntries = new List<Entry>();

                if (type == "String")
                {
                    if (showAsHex)
                    {
                        foreach (Entry entry in CfgBinFileOpened.Entries)
                        {
                            Entry[] foundEntries = entry.FindAll(x =>
                                x.Variables != null &&
                                x.Variables.Any(y =>
                                    y.Type == Level5.Logic.Type.String &&
                                    (y.Value as OffsetTextPair).Offset == Convert.ToInt32(retrievedValue)
                                )
                            );

                            if (foundEntries != null)
                            {
                                MatchesEntries.AddRange(foundEntries);
                            }
                        }
                    } 
                    else
                    {
                        foreach (Entry entry in CfgBinFileOpened.Entries)
                        {
                            Entry[] foundEntries = entry.FindAll(x =>
                                x.Variables != null &&
                                x.Variables.Any(y =>
                                    y.Type == Level5.Logic.Type.String &&
                                    (y.Value as OffsetTextPair).Text != null &&
                                    (y.Value as OffsetTextPair).Text.StartsWith(retrievedValue.ToString(), StringComparison.OrdinalIgnoreCase)
                                )
                            );

                            if (foundEntries != null)
                            {
                                MatchesEntries.AddRange(foundEntries);
                            }
                        }
                    }
                } 
                else if (type == "Float")
                {
                    foreach (Entry entry in CfgBinFileOpened.Entries)
                    {
                        Entry[] foundEntries = entry.FindAll(x =>
                            x.Variables != null &&
                            x.Variables.Any(y =>
                                y.Type == Level5.Logic.Type.Float &&
                                Convert.ToSingle(y.Value) == Convert.ToSingle(retrievedValue)
                            )
                        );

                        if (foundEntries != null)
                        {
                            MatchesEntries.AddRange(foundEntries);
                        }
                    }
                }
                else
                {
                    foreach (Entry entry in CfgBinFileOpened.Entries)
                    {
                        Entry[] foundEntries = entry.FindAll(x =>
                            x.Variables != null &&
                            x.Variables.Any(y =>
                                y.Type == Level5.Logic.Type.Int &&
                                Convert.ToInt32(y.Value) == Convert.ToInt32(retrievedValue)
                            )
                        );

                        if (foundEntries != null)
                        {
                            MatchesEntries.AddRange(foundEntries);
                        }
                    }
                }

                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
