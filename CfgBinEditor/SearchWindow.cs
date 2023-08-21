using System;
using System.Text.RegularExpressions;
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
    public partial class SearchWindow : Form
    {
        private CfgBinEditorWindow EditorWindow;

        private Dictionary<string, object> Entries;

        private Dictionary<int, string> Strings;

        public List<(string, object)> FiltredEntries;

        public SearchWindow(Dictionary<string, object> entries, Dictionary<int, string> strings, CfgBinEditorWindow editorWindow)
        {
            InitializeComponent();

            Entries = entries;
            Strings = strings;
            EditorWindow = editorWindow;
        }

        private bool IsHexDigit(string input)
        {
            return Regex.IsMatch(input, @"\A\b[0-9a-fA-F]+\b\Z");
        }

        private List<(string, object)> FlattenEntry(Dictionary<string, object> entryDictionary, Func<CfgBinSupport.Variable, bool> predicate)
        {
            List<(string, object)> result = new List<(string, object)>();

            foreach (var kvp in entryDictionary)
            {
                if (kvp.Value is Dictionary<string, object> innerDict)
                {
                    result.AddRange(FlattenEntry(innerDict, predicate));
                }
                else if (kvp.Value is List<CfgBinSupport.Variable> variables)
                {
                    var matchingVariables = variables.Where(predicate).ToList();

                    if (matchingVariables.Count > 0)
                    {
                        Console.WriteLine(kvp.Key + " " + result.Count);
                        result.Add((kvp.Key, entryDictionary));
                    }
                }
            }

            return result;
        }

        private List<int> GetKeysByValue(Dictionary<int, string> dictionary, string value)
        {
            List<int> keys = new List<int>();

            foreach (var kvp in dictionary)
            {
                if (kvp.Value.Contains(value))
                {
                    keys.Add(kvp.Key);
                }
            }

            return keys;
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

        private void SearchWindow_Load(object sender, EventArgs e)
        {
            typeComboBox.SelectedIndex = 1;
        }

        private void ValueTextBox_TextChanged(object sender, EventArgs e)
        {
            if (typeComboBox.SelectedIndex == -1)
            {
                valueTextBox.Clear();
                return;
            }

            valueTextBox.TextChanged -= null;
            valueTextBox.KeyPress -= null;

            if (showAsHexCheckBox.Checked)
            {
                valueTextBox.TextChanged += (s, eventArgs) =>
                {
                    string hexString = valueTextBox.Text;
                    if (hexString.Length > 8)
                        hexString = hexString.Substring(hexString.Length - 8);

                    if (!IsHexDigit(hexString))
                    {
                        hexString = hexString.Length > 0 ? hexString.Substring(0, hexString.Length - 1) : "";
                    }

                    valueTextBox.Text = hexString;
                    valueTextBox.SelectionStart = valueTextBox.Text.Length;
                };
            }
            else if (typeComboBox.Text == "Int" || typeComboBox.Text == "Unknown")
            {
                valueTextBox.KeyPress += (s, eventArgs) =>
                {
                    if ((!char.IsDigit(eventArgs.KeyChar) && eventArgs.KeyChar != '-' && eventArgs.KeyChar != '\b') ||
                        (eventArgs.KeyChar == '-' && valueTextBox.Text.Length > 0) ||
                        (valueTextBox.Text.Length >= 12)) // Max length of characters for "-4294967295"
                        eventArgs.Handled = true;
                };
            }
            else if (typeComboBox.Text == "Float")
            {
                string decimalSeparator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

                if (!showAsHexCheckBox.Checked)
                {
                    valueTextBox.KeyPress += (s, eventArgs) =>
                    {
                        if ((!char.IsDigit(eventArgs.KeyChar) && eventArgs.KeyChar != '-' && eventArgs.KeyChar != decimalSeparator[0] && eventArgs.KeyChar != '\b') ||
                            (eventArgs.KeyChar == '-' && valueTextBox.Text.Length > 0) ||
                            (eventArgs.KeyChar == decimalSeparator[0] && valueTextBox.Text.Contains(decimalSeparator)) ||
                            (valueTextBox.Text.Length >= 12)) // Max length of characters for "-4294967295"
                            eventArgs.Handled = true;
                    };
                }
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            if (typeComboBox.Text == "String")
            {
                Func<CfgBinSupport.Variable, bool> predicate;

                try
                {
                    if (showAsHexCheckBox.Checked)
                    {
                        predicate = v => v.Type == CfgBinSupport.Type.String && (int)v.Value == ConvertLittleEndianHexToInt(valueTextBox.Text);
                    }
                    else
                    {
                        predicate = v => v.Type == CfgBinSupport.Type.String && GetKeysByValue(Strings, valueTextBox.Text).Contains((int)v.Value);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("The inputed text cannot be converted to an String");
                    return;
                }

                FiltredEntries = FlattenEntry(Entries, predicate);
            }
            else if (typeComboBox.Text == "Int" || typeComboBox.Text == "Unknown")
            {
                int number;

                try
                {
                    if (showAsHexCheckBox.Checked)
                    {
                        number = ConvertLittleEndianHexToInt(valueTextBox.Text);
                    }
                    else
                    {
                        number = Convert.ToInt32(valueTextBox.Text);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("The inputed text cannot be converted to an Int");
                    return;
                }

                FiltredEntries = FlattenEntry(Entries, v => v.Type == CfgBinSupport.Type.Int && (int)v.Value == number);
            }
            else if (typeComboBox.Text == "Float")
            {
                float number;

                try
                {
                    if (showAsHexCheckBox.Checked)
                    {
                        number = ConvertLittleEndianHexToFloat(valueTextBox.Text);
                    }
                    else
                    {
                        number = Convert.ToSingle(valueTextBox.Text);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("The inputed text cannot be converted to an Float");
                    return;
                }

                FiltredEntries = FlattenEntry(Entries, v => v.Type == CfgBinSupport.Type.Float && (float)v.Value == number);
            }

            EditorWindow.tabControl1.SelectedIndex = 1;
            EditorWindow.filtredListBox.Items.Clear();
            EditorWindow.filtredListBox.Items.AddRange(
                FiltredEntries.Select(x => x.Item1).ToArray()
           );
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            EditorWindow.filtredListBox.Items.Clear();
            EditorWindow.SelectedItem = null;
            EditorWindow.SelectedEntry = null;
            EditorWindow.variablesDataGridView.Rows.Clear();
        }
    }
}
