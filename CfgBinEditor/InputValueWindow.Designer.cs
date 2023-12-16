namespace CfgBinEditor
{
    partial class InputValueWindow
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
            this.label1 = new System.Windows.Forms.Label();
            this.stringTextBox = new System.Windows.Forms.TextBox();
            this.hexTextBox = new System.Windows.Forms.TextBox();
            this.integerNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.floatNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.confirmButton = new System.Windows.Forms.Button();
            this.myIDsButton = new System.Windows.Forms.Button();
            this.listComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tagComboBox = new System.Windows.Forms.ComboBox();
            this.hashTextBox = new System.Windows.Forms.TextBox();
            this.tagGroupComboBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.integerNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.floatNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(26, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Type your value here\r\n";
            // 
            // stringTextBox
            // 
            this.stringTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.stringTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.stringTextBox.ForeColor = System.Drawing.Color.White;
            this.stringTextBox.Location = new System.Drawing.Point(139, 22);
            this.stringTextBox.Multiline = true;
            this.stringTextBox.Name = "stringTextBox";
            this.stringTextBox.Size = new System.Drawing.Size(229, 86);
            this.stringTextBox.TabIndex = 1;
            this.stringTextBox.Visible = false;
            // 
            // hexTextBox
            // 
            this.hexTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.hexTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hexTextBox.ForeColor = System.Drawing.Color.White;
            this.hexTextBox.Location = new System.Drawing.Point(139, 58);
            this.hexTextBox.MaxLength = 8;
            this.hexTextBox.Name = "hexTextBox";
            this.hexTextBox.Size = new System.Drawing.Size(228, 20);
            this.hexTextBox.TabIndex = 2;
            this.hexTextBox.Visible = false;
            this.hexTextBox.TextChanged += new System.EventHandler(this.HexTextBox_TextChanged);
            // 
            // integerNumericUpDown
            // 
            this.integerNumericUpDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.integerNumericUpDown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.integerNumericUpDown.ForeColor = System.Drawing.Color.White;
            this.integerNumericUpDown.Location = new System.Drawing.Point(139, 58);
            this.integerNumericUpDown.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.integerNumericUpDown.Minimum = new decimal(new int[] {
            2147483647,
            0,
            0,
            -2147483648});
            this.integerNumericUpDown.Name = "integerNumericUpDown";
            this.integerNumericUpDown.Size = new System.Drawing.Size(228, 20);
            this.integerNumericUpDown.TabIndex = 3;
            this.integerNumericUpDown.Visible = false;
            // 
            // floatNumericUpDown
            // 
            this.floatNumericUpDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.floatNumericUpDown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.floatNumericUpDown.DecimalPlaces = 1;
            this.floatNumericUpDown.ForeColor = System.Drawing.Color.White;
            this.floatNumericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.floatNumericUpDown.Location = new System.Drawing.Point(139, 58);
            this.floatNumericUpDown.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.floatNumericUpDown.Minimum = new decimal(new int[] {
            2147483647,
            0,
            0,
            -2147483648});
            this.floatNumericUpDown.Name = "floatNumericUpDown";
            this.floatNumericUpDown.Size = new System.Drawing.Size(228, 20);
            this.floatNumericUpDown.TabIndex = 4;
            this.floatNumericUpDown.Visible = false;
            // 
            // confirmButton
            // 
            this.confirmButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.confirmButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.confirmButton.ForeColor = System.Drawing.Color.White;
            this.confirmButton.Location = new System.Drawing.Point(260, 114);
            this.confirmButton.Name = "confirmButton";
            this.confirmButton.Size = new System.Drawing.Size(97, 23);
            this.confirmButton.TabIndex = 5;
            this.confirmButton.Text = "Confirm";
            this.confirmButton.UseVisualStyleBackColor = false;
            this.confirmButton.Click += new System.EventHandler(this.ConfirmButton_Click);
            // 
            // myIDsButton
            // 
            this.myIDsButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.myIDsButton.Enabled = false;
            this.myIDsButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.myIDsButton.ForeColor = System.Drawing.Color.White;
            this.myIDsButton.Location = new System.Drawing.Point(148, 114);
            this.myIDsButton.Name = "myIDsButton";
            this.myIDsButton.Size = new System.Drawing.Size(97, 23);
            this.myIDsButton.TabIndex = 6;
            this.myIDsButton.Text = "MyIDs.txt";
            this.myIDsButton.UseVisualStyleBackColor = false;
            this.myIDsButton.Click += new System.EventHandler(this.MyIDsButton_Click);
            // 
            // listComboBox
            // 
            this.listComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.listComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.listComboBox.ForeColor = System.Drawing.Color.White;
            this.listComboBox.FormattingEnabled = true;
            this.listComboBox.Location = new System.Drawing.Point(139, 58);
            this.listComboBox.Name = "listComboBox";
            this.listComboBox.Size = new System.Drawing.Size(229, 21);
            this.listComboBox.TabIndex = 7;
            this.listComboBox.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(26, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Type your hash";
            this.label2.Visible = false;
            // 
            // tagComboBox
            // 
            this.tagComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.tagComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.tagComboBox.ForeColor = System.Drawing.Color.White;
            this.tagComboBox.FormattingEnabled = true;
            this.tagComboBox.Location = new System.Drawing.Point(112, 86);
            this.tagComboBox.Name = "tagComboBox";
            this.tagComboBox.Size = new System.Drawing.Size(78, 21);
            this.tagComboBox.TabIndex = 9;
            this.tagComboBox.Visible = false;
            this.tagComboBox.SelectedIndexChanged += new System.EventHandler(this.HashTypeComboBox_SelectedIndexChanged);
            this.tagComboBox.TextChanged += new System.EventHandler(this.HashTypeComboBox_TextChanged);
            // 
            // hashTextBox
            // 
            this.hashTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.hashTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hashTextBox.ForeColor = System.Drawing.Color.White;
            this.hashTextBox.Location = new System.Drawing.Point(281, 84);
            this.hashTextBox.Multiline = true;
            this.hashTextBox.Name = "hashTextBox";
            this.hashTextBox.Size = new System.Drawing.Size(87, 24);
            this.hashTextBox.TabIndex = 10;
            this.hashTextBox.Visible = false;
            this.hashTextBox.TextChanged += new System.EventHandler(this.HashTextBox_TextChanged);
            // 
            // tagGroupComboBox
            // 
            this.tagGroupComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.tagGroupComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.tagGroupComboBox.ForeColor = System.Drawing.Color.White;
            this.tagGroupComboBox.FormattingEnabled = true;
            this.tagGroupComboBox.Location = new System.Drawing.Point(197, 86);
            this.tagGroupComboBox.Name = "tagGroupComboBox";
            this.tagGroupComboBox.Size = new System.Drawing.Size(78, 21);
            this.tagGroupComboBox.TabIndex = 11;
            this.tagGroupComboBox.Visible = false;
            this.tagGroupComboBox.SelectedIndexChanged += new System.EventHandler(this.TagGroupComboBox_SelectedIndexChanged);
            this.tagGroupComboBox.TextChanged += new System.EventHandler(this.TagGroupComboBox_TextChanged);
            // 
            // InputValueWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.ClientSize = new System.Drawing.Size(379, 147);
            this.Controls.Add(this.tagGroupComboBox);
            this.Controls.Add(this.hashTextBox);
            this.Controls.Add(this.tagComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listComboBox);
            this.Controls.Add(this.myIDsButton);
            this.Controls.Add(this.confirmButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.floatNumericUpDown);
            this.Controls.Add(this.integerNumericUpDown);
            this.Controls.Add(this.hexTextBox);
            this.Controls.Add(this.stringTextBox);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(395, 186);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(395, 186);
            this.Name = "InputValueWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "InputValueWindow";
            this.Load += new System.EventHandler(this.InputValueWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.integerNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.floatNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox stringTextBox;
        private System.Windows.Forms.TextBox hexTextBox;
        private System.Windows.Forms.NumericUpDown integerNumericUpDown;
        private System.Windows.Forms.NumericUpDown floatNumericUpDown;
        private System.Windows.Forms.Button confirmButton;
        private System.Windows.Forms.Button myIDsButton;
        private System.Windows.Forms.ComboBox listComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox tagComboBox;
        private System.Windows.Forms.TextBox hashTextBox;
        private System.Windows.Forms.ComboBox tagGroupComboBox;
    }
}