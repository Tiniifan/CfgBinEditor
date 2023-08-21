namespace CfgBinEditor
{
    partial class SearchWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.typeComboBox = new System.Windows.Forms.ComboBox();
            this.valueTextBox = new System.Windows.Forms.TextBox();
            this.searchButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.showAsHexCheckBox = new System.Windows.Forms.CheckBox();
            this.resetButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // typeComboBox
            // 
            this.typeComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.typeComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.typeComboBox.ForeColor = System.Drawing.Color.White;
            this.typeComboBox.FormattingEnabled = true;
            this.typeComboBox.Items.AddRange(new object[] {
            "String",
            "Int",
            "Float",
            "Unknown"});
            this.typeComboBox.Location = new System.Drawing.Point(48, 15);
            this.typeComboBox.Name = "typeComboBox";
            this.typeComboBox.Size = new System.Drawing.Size(156, 21);
            this.typeComboBox.TabIndex = 1;
            // 
            // valueTextBox
            // 
            this.valueTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.valueTextBox.ForeColor = System.Drawing.Color.White;
            this.valueTextBox.Location = new System.Drawing.Point(48, 42);
            this.valueTextBox.Name = "valueTextBox";
            this.valueTextBox.Size = new System.Drawing.Size(156, 20);
            this.valueTextBox.TabIndex = 2;
            this.valueTextBox.TextChanged += new System.EventHandler(this.ValueTextBox_TextChanged);
            // 
            // searchButton
            // 
            this.searchButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.searchButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.searchButton.ForeColor = System.Drawing.Color.White;
            this.searchButton.Location = new System.Drawing.Point(48, 68);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(75, 23);
            this.searchButton.TabIndex = 3;
            this.searchButton.Text = "Search";
            this.searchButton.UseVisualStyleBackColor = false;
            this.searchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Type";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(12, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Value";
            // 
            // showAsHexCheckBox
            // 
            this.showAsHexCheckBox.AutoSize = true;
            this.showAsHexCheckBox.ForeColor = System.Drawing.Color.White;
            this.showAsHexCheckBox.Location = new System.Drawing.Point(210, 44);
            this.showAsHexCheckBox.Name = "showAsHexCheckBox";
            this.showAsHexCheckBox.Size = new System.Drawing.Size(45, 17);
            this.showAsHexCheckBox.TabIndex = 6;
            this.showAsHexCheckBox.Text = "Hex";
            this.showAsHexCheckBox.UseVisualStyleBackColor = true;
            // 
            // resetButton
            // 
            this.resetButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.resetButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.resetButton.ForeColor = System.Drawing.Color.White;
            this.resetButton.Location = new System.Drawing.Point(129, 68);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(75, 23);
            this.resetButton.TabIndex = 7;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = false;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // SearchWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.ClientSize = new System.Drawing.Size(256, 101);
            this.Controls.Add(this.resetButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.showAsHexCheckBox);
            this.Controls.Add(this.typeComboBox);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.valueTextBox);
            this.Name = "SearchWindow";
            this.Text = "SearchWindow";
            this.Load += new System.EventHandler(this.SearchWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox typeComboBox;
        private System.Windows.Forms.TextBox valueTextBox;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox showAsHexCheckBox;
        private System.Windows.Forms.Button resetButton;
    }
}