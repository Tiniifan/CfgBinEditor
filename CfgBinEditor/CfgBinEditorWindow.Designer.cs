namespace CfgBinEditor
{
    partial class CfgBinEditorWindow
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle23 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle24 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle25 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle26 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle27 = new System.Windows.Forms.DataGridViewCellStyle();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importTagsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tagsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addEntryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.duplicateStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.filtredListBox = new System.Windows.Forms.ListBox();
            this.exportItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog2 = new System.Windows.Forms.SaveFileDialog();
            this.vsTabControl1 = new CfgBinEditor.UI.VSTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.variablesDataGridView = new System.Windows.Forms.DataGridView();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.stringsDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.importItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog3 = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.vsTabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.variablesDataGridView)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stringsDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.tagsToolStripMenuItem,
            this.searchToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(922, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.importTagsToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.fileToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
            // 
            // importTagsToolStripMenuItem
            // 
            this.importTagsToolStripMenuItem.Name = "importTagsToolStripMenuItem";
            this.importTagsToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.importTagsToolStripMenuItem.Text = "Import Tags";
            this.importTagsToolStripMenuItem.Click += new System.EventHandler(this.ImportTagsToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Enabled = false;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            // 
            // tagsToolStripMenuItem
            // 
            this.tagsToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.tagsToolStripMenuItem.Name = "tagsToolStripMenuItem";
            this.tagsToolStripMenuItem.Size = new System.Drawing.Size(42, 20);
            this.tagsToolStripMenuItem.Text = "Tags";
            // 
            // searchToolStripMenuItem
            // 
            this.searchToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            this.searchToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.searchToolStripMenuItem.Text = "Search";
            this.searchToolStripMenuItem.Click += new System.EventHandler(this.SearchToolStripMenuItem_Click);
            // 
            // treeView1
            // 
            this.treeView1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView1.ForeColor = System.Drawing.Color.White;
            this.treeView1.HideSelection = false;
            this.treeView1.LineColor = System.Drawing.Color.White;
            this.treeView1.Location = new System.Drawing.Point(3, 6);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(311, 358);
            this.treeView1.TabIndex = 1;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeView1_AfterSelect);
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TreeView1_NodeMouseClick);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.contextMenuStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addEntryToolStripMenuItem,
            this.importItemToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.ShowImageMargin = false;
            this.contextMenuStrip1.Size = new System.Drawing.Size(113, 48);
            // 
            // addEntryToolStripMenuItem
            // 
            this.addEntryToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.addEntryToolStripMenuItem.Name = "addEntryToolStripMenuItem";
            this.addEntryToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.addEntryToolStripMenuItem.Text = "Add entry";
            this.addEntryToolStripMenuItem.Visible = false;
            this.addEntryToolStripMenuItem.Click += new System.EventHandler(this.AddEntryToolStripMenuItem_Click);
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog2";
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.contextMenuStrip2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.duplicateStripMenuItem1,
            this.toolStripMenuItem2,
            this.exportItemToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip1";
            this.contextMenuStrip2.ShowImageMargin = false;
            this.contextMenuStrip2.Size = new System.Drawing.Size(127, 70);
            // 
            // duplicateStripMenuItem1
            // 
            this.duplicateStripMenuItem1.ForeColor = System.Drawing.Color.White;
            this.duplicateStripMenuItem1.Name = "duplicateStripMenuItem1";
            this.duplicateStripMenuItem1.Size = new System.Drawing.Size(126, 22);
            this.duplicateStripMenuItem1.Text = "Duplicate item";
            this.duplicateStripMenuItem1.Click += new System.EventHandler(this.DuplicateStripMenuItem1_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.ForeColor = System.Drawing.Color.White;
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(126, 22);
            this.toolStripMenuItem2.Text = "Remove item";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.ToolStripMenuItem2_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(12, 27);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(328, 396);
            this.tabControl1.TabIndex = 8;
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.tabPage3.Controls.Add(this.treeView1);
            this.tabPage3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(320, 370);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Tree View";
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.tabPage4.Controls.Add(this.filtredListBox);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(320, 370);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Filtred";
            // 
            // filtredListBox
            // 
            this.filtredListBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.filtredListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.filtredListBox.ForeColor = System.Drawing.Color.White;
            this.filtredListBox.FormattingEnabled = true;
            this.filtredListBox.Location = new System.Drawing.Point(3, 6);
            this.filtredListBox.Name = "filtredListBox";
            this.filtredListBox.Size = new System.Drawing.Size(311, 351);
            this.filtredListBox.TabIndex = 0;
            this.filtredListBox.SelectedIndexChanged += new System.EventHandler(this.FiltredListBox_SelectedIndexChanged);
            // 
            // exportItemToolStripMenuItem
            // 
            this.exportItemToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.exportItemToolStripMenuItem.Name = "exportItemToolStripMenuItem";
            this.exportItemToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.exportItemToolStripMenuItem.Text = "Export item";
            this.exportItemToolStripMenuItem.Click += new System.EventHandler(this.ExportItemToolStripMenuItem_Click);
            // 
            // vsTabControl1
            // 
            this.vsTabControl1.ActiveIndicator = System.Drawing.Color.White;
            this.vsTabControl1.ActiveTab = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.vsTabControl1.ActiveText = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.vsTabControl1.Background = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.vsTabControl1.BackgroundTab = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.vsTabControl1.Border = System.Drawing.Color.White;
            this.vsTabControl1.Controls.Add(this.tabPage1);
            this.vsTabControl1.Controls.Add(this.tabPage2);
            this.vsTabControl1.Divider = System.Drawing.Color.White;
            this.vsTabControl1.Font = new System.Drawing.Font("Leelawadee UI", 8.25F);
            this.vsTabControl1.InActiveIndicator = System.Drawing.Color.White;
            this.vsTabControl1.InActiveTab = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.vsTabControl1.InActiveText = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.vsTabControl1.Location = new System.Drawing.Point(352, 27);
            this.vsTabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.vsTabControl1.Name = "vsTabControl1";
            this.vsTabControl1.Padding = new System.Drawing.Point(0, 0);
            this.vsTabControl1.SelectedIndex = 0;
            this.vsTabControl1.Size = new System.Drawing.Size(559, 396);
            this.vsTabControl1.TabIndex = 7;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.tabPage1.Controls.Add(this.variablesDataGridView);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(551, 367);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Variables";
            // 
            // variablesDataGridView
            // 
            this.variablesDataGridView.AllowUserToAddRows = false;
            this.variablesDataGridView.AllowUserToDeleteRows = false;
            this.variablesDataGridView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            dataGridViewCellStyle19.Font = new System.Drawing.Font("Leelawadee UI", 8.25F);
            dataGridViewCellStyle19.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle19.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle19.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle19.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.variablesDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle19;
            this.variablesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.variablesDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column4,
            this.Column1,
            this.Column2,
            this.Column3});
            dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            dataGridViewCellStyle21.Font = new System.Drawing.Font("Leelawadee UI", 8.25F);
            dataGridViewCellStyle21.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle21.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle21.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle21.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.variablesDataGridView.DefaultCellStyle = dataGridViewCellStyle21;
            this.variablesDataGridView.Enabled = false;
            this.variablesDataGridView.EnableHeadersVisualStyles = false;
            this.variablesDataGridView.GridColor = System.Drawing.SystemColors.ControlDarkDark;
            this.variablesDataGridView.Location = new System.Drawing.Point(3, 6);
            this.variablesDataGridView.Name = "variablesDataGridView";
            dataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle22.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            dataGridViewCellStyle22.Font = new System.Drawing.Font("Leelawadee UI", 8.25F);
            dataGridViewCellStyle22.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle22.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle22.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle22.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.variablesDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle22;
            this.variablesDataGridView.RowHeadersVisible = false;
            dataGridViewCellStyle23.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.variablesDataGridView.RowsDefaultCellStyle = dataGridViewCellStyle23;
            this.variablesDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.variablesDataGridView.Size = new System.Drawing.Size(548, 358);
            this.variablesDataGridView.TabIndex = 6;
            this.variablesDataGridView.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.VariablesDataGridView_CellValidated);
            this.variablesDataGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.VariableDataGridView_CellValueChanged);
            this.variablesDataGridView.CurrentCellDirtyStateChanged += new System.EventHandler(this.VariableDataGridView_CurrentCellDirtyStateChanged);
            this.variablesDataGridView.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.VariableDataGridView_EditingControlShowing);
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Name";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // Column1
            // 
            dataGridViewCellStyle20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.Column1.DefaultCellStyle = dataGridViewCellStyle20;
            this.Column1.HeaderText = "Type";
            this.Column1.Items.AddRange(new object[] {
            "String",
            "Int",
            "Float",
            "Unknown"});
            this.Column1.Name = "Column1";
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Value";
            this.Column2.Name = "Column2";
            this.Column2.Width = 250;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Show as Hex";
            this.Column3.Name = "Column3";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.tabPage2.Controls.Add(this.stringsDataGridView);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(551, 367);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Strings";
            // 
            // stringsDataGridView
            // 
            this.stringsDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stringsDataGridView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            dataGridViewCellStyle24.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle24.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            dataGridViewCellStyle24.Font = new System.Drawing.Font("Leelawadee UI", 8.25F);
            dataGridViewCellStyle24.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle24.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle24.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle24.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.stringsDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle24;
            this.stringsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.stringsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn1});
            dataGridViewCellStyle25.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle25.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            dataGridViewCellStyle25.Font = new System.Drawing.Font("Leelawadee UI", 8.25F);
            dataGridViewCellStyle25.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle25.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle25.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle25.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.stringsDataGridView.DefaultCellStyle = dataGridViewCellStyle25;
            this.stringsDataGridView.Enabled = false;
            this.stringsDataGridView.EnableHeadersVisualStyles = false;
            this.stringsDataGridView.GridColor = System.Drawing.SystemColors.ControlDarkDark;
            this.stringsDataGridView.Location = new System.Drawing.Point(3, 6);
            this.stringsDataGridView.MultiSelect = false;
            this.stringsDataGridView.Name = "stringsDataGridView";
            dataGridViewCellStyle26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle26.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            dataGridViewCellStyle26.Font = new System.Drawing.Font("Leelawadee UI", 8.25F);
            dataGridViewCellStyle26.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle26.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle26.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle26.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.stringsDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle26;
            this.stringsDataGridView.RowHeadersVisible = false;
            dataGridViewCellStyle27.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.stringsDataGridView.RowsDefaultCellStyle = dataGridViewCellStyle27;
            this.stringsDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.stringsDataGridView.Size = new System.Drawing.Size(548, 358);
            this.stringsDataGridView.TabIndex = 7;
            this.stringsDataGridView.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.StringsDataGridView_CellValidated);
            this.stringsDataGridView.CurrentCellDirtyStateChanged += new System.EventHandler(this.StringsDataGridView_CurrentCellDirtyStateChanged);
            this.stringsDataGridView.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.StringsDataGridView_RowsAdded);
            this.stringsDataGridView.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.StringsDataGridView_RowValidating);
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.FillWeight = 18F;
            this.dataGridViewTextBoxColumn2.HeaderText = "Offset";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.FillWeight = 80F;
            this.dataGridViewTextBoxColumn1.HeaderText = "Text";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // importItemToolStripMenuItem
            // 
            this.importItemToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.importItemToolStripMenuItem.Name = "importItemToolStripMenuItem";
            this.importItemToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.importItemToolStripMenuItem.Text = "Import item";
            this.importItemToolStripMenuItem.Click += new System.EventHandler(this.ImportItemToolStripMenuItem_Click);
            // 
            // openFileDialog3
            // 
            this.openFileDialog3.FileName = "openFileDialog3";
            // 
            // CfgBinEditorWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.ClientSize = new System.Drawing.Size(922, 433);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.vsTabControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "CfgBinEditorWindow";
            this.Text = "CfgBinEditor";
            this.Load += new System.EventHandler(this.CfgBinEditorWindow_Load);
            this.SizeChanged += new System.EventHandler(this.CfgBinEditorWindow_SizeChanged);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.vsTabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.variablesDataGridView)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.stringsDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem addEntryToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewComboBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column3;
        private System.Windows.Forms.ToolStripMenuItem importTagsToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.ToolStripMenuItem tagsToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem duplicateStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private UI.VSTabControl vsTabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView stringsDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        public System.Windows.Forms.TabControl tabControl1;
        public System.Windows.Forms.ListBox filtredListBox;
        public System.Windows.Forms.DataGridView variablesDataGridView;
        private System.Windows.Forms.ToolStripMenuItem exportItemToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog2;
        private System.Windows.Forms.ToolStripMenuItem importItemToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog3;
    }
}

