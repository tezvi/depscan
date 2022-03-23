
namespace DepScan
{
    partial class FormMain
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listBoxDirs = new System.Windows.Forms.ListBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tlbBtnRemove = new System.Windows.Forms.ToolStripButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkBoxOptions = new System.Windows.Forms.CheckedListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dirSelectDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.txtOutputReportPath = new System.Windows.Forms.TextBox();
            this.tlbBtnAdd = new System.Windows.Forms.ToolStripButton();
            this.btnScan = new System.Windows.Forms.Button();
            this.btnSelectOuputPath = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnScan, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(5, 5);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 66F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(471, 450);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listBoxDirs);
            this.groupBox1.Controls.Add(this.toolStrip1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 2);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(10);
            this.groupBox1.Size = new System.Drawing.Size(465, 171);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select directories to scan recursively";
            // 
            // listBoxDirs
            // 
            this.listBoxDirs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxDirs.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.listBoxDirs.FormattingEnabled = true;
            this.listBoxDirs.Location = new System.Drawing.Point(10, 49);
            this.listBoxDirs.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listBoxDirs.Name = "listBoxDirs";
            this.listBoxDirs.Size = new System.Drawing.Size(445, 112);
            this.listBoxDirs.TabIndex = 0;
            this.listBoxDirs.SelectedValueChanged += new System.EventHandler(this.ListBoxDirs_SelectedValueChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlbBtnAdd,
            this.tlbBtnRemove});
            this.toolStrip1.Location = new System.Drawing.Point(10, 22);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(445, 27);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tlbBtnRemove
            // 
            this.tlbBtnRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbBtnRemove.Name = "tlbBtnRemove";
            this.tlbBtnRemove.Size = new System.Drawing.Size(50, 24);
            this.tlbBtnRemove.Text = "Remove";
            this.tlbBtnRemove.Click += new System.EventHandler(this.TlbBtnRemove_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkBoxOptions);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 177);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(10);
            this.groupBox2.Size = new System.Drawing.Size(465, 171);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Select scan options";
            // 
            // chkBoxOptions
            // 
            this.chkBoxOptions.CheckOnClick = true;
            this.chkBoxOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkBoxOptions.FormattingEnabled = true;
            this.chkBoxOptions.Items.AddRange(new object[] {
            "Follow compressed files (jar, zip, gzip)",
            "Scan Javascript project metadata files (NPM)",
            "Scan Python project metadata files (requirements)",
            "Scan Java project metadata files (POM, Gradle)",
            "Scan PHP project metada files (Composer)",
            "Scan all textual files with extended regex"});
            this.chkBoxOptions.Location = new System.Drawing.Point(10, 22);
            this.chkBoxOptions.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkBoxOptions.Name = "chkBoxOptions";
            this.chkBoxOptions.Size = new System.Drawing.Size(445, 139);
            this.chkBoxOptions.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtOutputReportPath);
            this.groupBox3.Controls.Add(this.btnSelectOuputPath);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(3, 352);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox3.Size = new System.Drawing.Size(465, 62);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Select output file where you\'d like to automatically store report";
            // 
            // dirSelectDialog
            // 
            this.dirSelectDialog.Description = "Select directory path";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "Report text file|*.txt";
            // 
            // txtOutputReportPath
            // 
            this.txtOutputReportPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.txtOutputReportPath.Location = new System.Drawing.Point(9, 25);
            this.txtOutputReportPath.Name = "txtOutputReportPath";
            this.txtOutputReportPath.Size = new System.Drawing.Size(409, 19);
            this.txtOutputReportPath.TabIndex = 2;
            // 
            // tlbBtnAdd
            // 
            this.tlbBtnAdd.Image = global::DepScan.Properties.Resources.openFile_small;
            this.tlbBtnAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tlbBtnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbBtnAdd.Name = "tlbBtnAdd";
            this.tlbBtnAdd.Size = new System.Drawing.Size(96, 24);
            this.tlbBtnAdd.Text = "Add directory";
            this.tlbBtnAdd.Click += new System.EventHandler(this.TlbBtnAdd_Click);
            // 
            // btnScan
            // 
            this.btnScan.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnScan.Image = global::DepScan.Properties.Resources.search;
            this.btnScan.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnScan.Location = new System.Drawing.Point(343, 418);
            this.btnScan.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnScan.Name = "btnScan";
            this.btnScan.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.btnScan.Size = new System.Drawing.Size(125, 30);
            this.btnScan.TabIndex = 5;
            this.btnScan.Text = "Start scan";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.BtnScan_Click);
            // 
            // btnSelectOuputPath
            // 
            this.btnSelectOuputPath.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnSelectOuputPath.Image = global::DepScan.Properties.Resources.openFile_small;
            this.btnSelectOuputPath.Location = new System.Drawing.Point(424, 25);
            this.btnSelectOuputPath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSelectOuputPath.Name = "btnSelectOuputPath";
            this.btnSelectOuputPath.Size = new System.Drawing.Size(31, 21);
            this.btnSelectOuputPath.TabIndex = 1;
            this.btnSelectOuputPath.UseVisualStyleBackColor = true;
            this.btnSelectOuputPath.Click += new System.EventHandler(this.BtnSelectOutputPath_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(481, 460);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)), true);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(350, 351);
            this.Name = "FormMain";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormMain";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tlbBtnAdd;
        private System.Windows.Forms.ToolStripButton tlbBtnRemove;
        private System.Windows.Forms.ListBox listBoxDirs;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckedListBox chkBoxOptions;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.FolderBrowserDialog dirSelectDialog;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnSelectOuputPath;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.TextBox txtOutputReportPath;
    }
}

