using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using CfgBinEditor.Level5.Binary;

namespace CfgBinEditor
{
    public partial class InputValueWindow : Form
    {
        public string Type;

        public bool ShowAsHex;

        public object Value { get; private set; }

        private object SelectedItem;

        Dictionary<string, Dictionary<string, List<ID>>> IDs;

        public InputValueWindow(string name, string type, object value, bool showAsHex, bool canOpenIdsWindow = false, Dictionary<string, Dictionary<string, List<ID>>> ids = null, object selectedItem = null)
        {
            IDs = ids;
            Type = type;
            Value = value;
            ShowAsHex = showAsHex;
            SelectedItem = selectedItem;
            InitializeComponent();

            this.Text = name;
            myIDsButton.Enabled = canOpenIdsWindow;
        }

        private void InputValueWindow_Load(object sender, EventArgs e)
        {
            if (ShowAsHex == true)
            {
                hexTextBox.Visible = true;
                hexTextBox.Text = Value.ToString();
            }
            else
            {
                switch (Type)
                {
                    case "String":
                        stringTextBox.Visible = true;
                        stringTextBox.Text = Value.ToString();
                        break;
                    case "Float":
                        floatNumericUpDown.Visible = true;
                        floatNumericUpDown.Value = Convert.ToDecimal(Value);
                        break;
                    case "List":
                        listComboBox.Visible = true;
                        listComboBox.Items.AddRange(Value as object[]);
                        if (SelectedItem != null)
                        {
                            listComboBox.SelectedItem = SelectedItem;
                        }
                        break;
                    default:
                        integerNumericUpDown.Visible = true;
                        integerNumericUpDown.Value = Convert.ToInt32(Value);
                        break;
                }
            }
        }

        private void HexTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string inputText = textBox.Text;

            // Remove non-hexadecimal characters using a regular expression
            string hexPattern = "^[0-9A-Fa-f]*$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(inputText, hexPattern))
            {
                // The text is not hexadecimal, remove the last entered character
                textBox.Text = inputText.Substring(0, inputText.Length - 1);
                textBox.SelectionStart = textBox.TextLength; // Place the cursor at the end
            }
        }

        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            if (ShowAsHex)
            {
                string hexString = hexTextBox.Text;

                if (Type == "Float")
                {
                    // Ensure the hexadecimal string has at least 8 characters
                    while (hexString.Length < 8)
                    {
                        hexString = "0" + hexString;
                    }

                    // Convert the hexadecimal string into a byte array
                    byte[] byteArray = Enumerable.Range(0, hexString.Length)
                        .Where(x => x % 2 == 0)
                        .Select(x => Convert.ToByte(hexString.Substring(x, 2), 16))
                        .ToArray();

                    // Reverse the array if necessary (depending on byte order)
                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(byteArray);
                    }

                    // Convert the byte array into a float
                    Value = BitConverter.ToSingle(byteArray, 0);
                }
                else
                {
                    Value = int.Parse(hexString, System.Globalization.NumberStyles.HexNumber);
                }
            }
            else
            {
                if (Type == "String")
                {
                    if (stringTextBox.Text == null)
                    {
                        MessageBox.Show("String can't be empty");
                        return;
                    }

                    Value = stringTextBox.Text;
                }
                else if (Type == "Float")
                {
                    Value = floatNumericUpDown.Value;
                }
                else if (Type == "List")
                {
                    Value = listComboBox.SelectedItem;
                }
                else
                {
                    Value = (int)integerNumericUpDown.Value;
                }
            }

            // Close the dialog window
            DialogResult = DialogResult.OK;
            Close();
        }

        private void MyIDsButton_Click(object sender, EventArgs e)
        {
            IDsWindow idsWindow = new IDsWindow(IDs);

            if (idsWindow.ShowDialog() == DialogResult.OK)
            {
                if (ShowAsHex)
                {
                    hexTextBox.Text = idsWindow.Hash.ToString("X8");
                }
                else
                {
                    integerNumericUpDown.Value = Convert.ToUInt32(idsWindow.Hash);
                }
            }
        }
    }
}
