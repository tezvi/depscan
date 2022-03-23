
namespace DepScan
{
    partial class FormScan
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormScan));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.lblPercentage = new System.Windows.Forms.Label();
            this.lblTimer = new System.Windows.Forms.Label();
            this.txtSubContext = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtContext = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.panel2 = new System.Windows.Forms.Panel();
            this.listViewResults = new DepScan.FormScan.ListViewBuffered();
            this.colHeadFilePath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colHeadRepoName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colHeadOrigin = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colHeadOwner = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colHeadRuleKey = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colHeadMatchedValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colHeadLangTags = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripRules = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMatches = new System.Windows.Forms.ToolStripLabel();
            this.toolStripBtnExport = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripScanned = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripErrors = new System.Windows.Forms.ToolStripLabel();
            this.timerScan = new System.Windows.Forms.Timer(this.components);
            this.exportFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.toolStripBtnAutoScroll = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 96F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(992, 572);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.lblPercentage);
            this.panel1.Controls.Add(this.lblTimer);
            this.panel1.Controls.Add(this.txtSubContext);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.txtContext);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.progressBar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(988, 92);
            this.panel1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 38);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Current file:";
            // 
            // lblPercentage
            // 
            this.lblPercentage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPercentage.Location = new System.Drawing.Point(945, 49);
            this.lblPercentage.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPercentage.Name = "lblPercentage";
            this.lblPercentage.Size = new System.Drawing.Size(35, 14);
            this.lblPercentage.TabIndex = 6;
            this.lblPercentage.Text = "100%";
            this.lblPercentage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTimer
            // 
            this.lblTimer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTimer.Location = new System.Drawing.Point(887, 49);
            this.lblTimer.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTimer.Name = "lblTimer";
            this.lblTimer.Size = new System.Drawing.Size(60, 14);
            this.lblTimer.TabIndex = 5;
            this.lblTimer.Text = "00:00";
            this.lblTimer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtSubContext
            // 
            this.txtSubContext.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSubContext.BackColor = System.Drawing.SystemColors.Info;
            this.txtSubContext.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSubContext.Location = new System.Drawing.Point(106, 39);
            this.txtSubContext.Margin = new System.Windows.Forms.Padding(2);
            this.txtSubContext.Name = "txtSubContext";
            this.txtSubContext.ReadOnly = true;
            this.txtSubContext.Size = new System.Drawing.Size(734, 13);
            this.txtSubContext.TabIndex = 4;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(887, 13);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(94, 26);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // txtContext
            // 
            this.txtContext.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtContext.BackColor = System.Drawing.SystemColors.Info;
            this.txtContext.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtContext.Location = new System.Drawing.Point(106, 14);
            this.txtContext.Margin = new System.Windows.Forms.Padding(2);
            this.txtContext.Name = "txtContext";
            this.txtContext.ReadOnly = true;
            this.txtContext.Size = new System.Drawing.Size(734, 13);
            this.txtContext.TabIndex = 2;
            this.txtContext.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 13);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Current directory:";
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(7, 67);
            this.progressBar.Margin = new System.Windows.Forms.Padding(2);
            this.progressBar.MarqueeAnimationSpeed = 15;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(973, 14);
            this.progressBar.Step = 20;
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.listViewResults);
            this.panel2.Controls.Add(this.toolStrip1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(2, 98);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(988, 472);
            this.panel2.TabIndex = 2;
            // 
            // listViewResults
            // 
            this.listViewResults.AllowColumnReorder = true;
            this.listViewResults.AutoArrange = false;
            this.listViewResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colHeadFilePath,
            this.colHeadRepoName,
            this.colHeadOrigin,
            this.colHeadOwner,
            this.colHeadRuleKey,
            this.colHeadMatchedValue,
            this.colHeadLangTags});
            this.listViewResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewResults.FullRowSelect = true;
            this.listViewResults.GridLines = true;
            this.listViewResults.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewResults.Location = new System.Drawing.Point(0, 27);
            this.listViewResults.Margin = new System.Windows.Forms.Padding(2);
            this.listViewResults.MultiSelect = false;
            this.listViewResults.Name = "listViewResults";
            this.listViewResults.ShowItemToolTips = true;
            this.listViewResults.Size = new System.Drawing.Size(988, 445);
            this.listViewResults.TabIndex = 1;
            this.listViewResults.UseCompatibleStateImageBehavior = false;
            this.listViewResults.View = System.Windows.Forms.View.Details;
            // 
            // colHeadFilePath
            // 
            this.colHeadFilePath.Text = "File path";
            this.colHeadFilePath.Width = 220;
            // 
            // colHeadRepoName
            // 
            this.colHeadRepoName.Text = "Dependency";
            this.colHeadRepoName.Width = 130;
            // 
            // colHeadOrigin
            // 
            this.colHeadOrigin.Text = "Country";
            this.colHeadOrigin.Width = 70;
            // 
            // colHeadOwner
            // 
            this.colHeadOwner.Text = "Owner / Maintainer";
            this.colHeadOwner.Width = 220;
            // 
            // colHeadRuleKey
            // 
            this.colHeadRuleKey.Text = "Matched rule";
            this.colHeadRuleKey.Width = 130;
            // 
            // colHeadMatchedValue
            // 
            this.colHeadMatchedValue.Text = "Matched value";
            this.colHeadMatchedValue.Width = 120;
            // 
            // colHeadLangTags
            // 
            this.colHeadLangTags.Text = "Tech stack";
            this.colHeadLangTags.Width = 200;
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripRules,
            this.toolStripSeparator1,
            this.toolStripSeparator3,
            this.toolStripMatches,
            this.toolStripBtnExport,
            this.toolStripSeparator2,
            this.toolStripScanned,
            this.toolStripSeparator4,
            this.toolStripErrors,
            this.toolStripBtnAutoScroll});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(988, 27);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripRules
            // 
            this.toolStripRules.Name = "toolStripRules";
            this.toolStripRules.Size = new System.Drawing.Size(81, 24);
            this.toolStripRules.Text = "Loaded rules: 0";
            this.toolStripRules.ToolTipText = "Total number of loaded dependency rules";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Overflow = System.Windows.Forms.ToolStripItemOverflow.Always;
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripMatches
            // 
            this.toolStripMatches.Name = "toolStripMatches";
            this.toolStripMatches.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.toolStripMatches.Size = new System.Drawing.Size(75, 24);
            this.toolStripMatches.Text = "Matches: 0";
            this.toolStripMatches.ToolTipText = "Total number of matched positives";
            // 
            // toolStripBtnExport
            // 
            this.toolStripBtnExport.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripBtnExport.Enabled = false;
            this.toolStripBtnExport.Image = global::DepScan.Properties.Resources.Download;
            this.toolStripBtnExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnExport.Name = "toolStripBtnExport";
            this.toolStripBtnExport.Size = new System.Drawing.Size(96, 24);
            this.toolStripBtnExport.Text = "Export report";
            this.toolStripBtnExport.ToolTipText = "Download report as file";
            this.toolStripBtnExport.Click += new System.EventHandler(this.ToolStripBtnExport_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripScanned
            // 
            this.toolStripScanned.Name = "toolStripScanned";
            this.toolStripScanned.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.toolStripScanned.Size = new System.Drawing.Size(98, 24);
            this.toolStripScanned.Text = "Scanned files: 0";
            this.toolStripScanned.ToolTipText = "Number of processed files";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripErrors
            // 
            this.toolStripErrors.Name = "toolStripErrors";
            this.toolStripErrors.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.toolStripErrors.Size = new System.Drawing.Size(64, 24);
            this.toolStripErrors.Text = "Errors: 0";
            this.toolStripErrors.ToolTipText = "Shows number of total errors occurred";
            // 
            // timerScan
            // 
            this.timerScan.Enabled = true;
            this.timerScan.Interval = 1000;
            this.timerScan.Tick += new System.EventHandler(this.TimerScan_Tick);
            // 
            // exportFileDialog
            // 
            this.exportFileDialog.Filter = "Text document|*.txt";
            this.exportFileDialog.Title = "Please select file path where to store report contents";
            // 
            // toolStripBtnAutoScroll
            // 
            this.toolStripBtnAutoScroll.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripBtnAutoScroll.Checked = true;
            this.toolStripBtnAutoScroll.CheckOnClick = true;
            this.toolStripBtnAutoScroll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripBtnAutoScroll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripBtnAutoScroll.Image = ((System.Drawing.Image)(resources.GetObject("toolStripBtnAutoScroll.Image")));
            this.toolStripBtnAutoScroll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnAutoScroll.Name = "toolStripBtnAutoScroll";
            this.toolStripBtnAutoScroll.Size = new System.Drawing.Size(61, 24);
            this.toolStripBtnAutoScroll.Text = "Auto scroll";
            this.toolStripBtnAutoScroll.ToolTipText = "Automatically scroll results to bottom";
            // 
            // FormScan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(992, 572);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormScan";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Scanning...";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormScan_FormClosing);
            this.Load += new System.EventHandler(this.FormScan_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblTimer;
        private System.Windows.Forms.TextBox txtSubContext;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtContext;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripRules;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripMatches;
        private System.Windows.Forms.ToolStripButton toolStripBtnExport;
        private ListViewBuffered listViewResults;
        private System.Windows.Forms.Label lblPercentage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripScanned;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripLabel toolStripErrors;
        private System.Windows.Forms.Timer timerScan;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ColumnHeader colHeadFilePath;
        private System.Windows.Forms.ColumnHeader colHeadRepoName;
        private System.Windows.Forms.ColumnHeader colHeadOrigin;
        private System.Windows.Forms.ColumnHeader colHeadOwner;
        private System.Windows.Forms.ColumnHeader colHeadRuleKey;
        private System.Windows.Forms.ColumnHeader colHeadMatchedValue;
        private System.Windows.Forms.ColumnHeader colHeadLangTags;
        private System.Windows.Forms.SaveFileDialog exportFileDialog;
        private System.Windows.Forms.ToolStripButton toolStripBtnAutoScroll;
    }
}