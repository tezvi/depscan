using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DepScan.Properties;

namespace DepScan
{
    public partial class FormMain : Form
    {
        private const int MAboutId = 0x100;
        private Utils.SystemMenu _mSystemMenu;

        public FormMain()
        {
            Icon = Resources.MainFormIcon;
            InitializeComponent();
        }

        protected override void WndProc(ref Message msg)
        {
            if (msg.Msg == (int)Utils.WindowMessages.WmSysCommand)
            {
                switch (msg.WParam.ToInt32())
                {
                    case MAboutId:
                        Program.SafeMsgBox($"{Program.AppTitle}\n\n"
                                           + $"By Andrej Vitez\nVersion {Utils.AssemblyVersion} build {Program.Build}",
                            MessageBoxIcon.Information);
                        break;
                }
            }

            base.WndProc(ref msg);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            chkBoxOptions.Items.Clear();
            Config.OptionList.ToList().ForEach(item => chkBoxOptions.Items.Add(item));
            for (var index = 0; index < chkBoxOptions.Items.Count; index++)
            {
                var configItem = (Config.ConfigItem)chkBoxOptions.Items[index];
                chkBoxOptions.SetItemChecked(index, configItem.DefaultChecked);
            }

            listBoxDirs.Items.Clear();
            Config.GetDefaultDirs().ToList().ForEach(path => listBoxDirs.Items.Add(path));
            tlbBtnRemove.Enabled = listBoxDirs.SelectedIndices.Count > 0;

            txtOutputReportPath.Text = Path
                .Combine(Utils.AssemblyDirectory, $"scan_report_{DateTime.Now:yyyy-MM-dd}.txt")
                .Replace(@"\\", @"\");

            // System menu
            try
            {
                _mSystemMenu = Utils.SystemMenu.FromForm(this);
                _mSystemMenu.AppendSeparator();
                _mSystemMenu.AppendMenu(MAboutId, "About");
            }
            catch (Utils.NoSystemMenuException ex)
            {
                Program.WriteLog(ex.ToString());
            }
        }

        private void TlbBtnAdd_Click(object sender, EventArgs e)
        {
            dirSelectDialog.SelectedPath = null;

            if (dirSelectDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            if (listBoxDirs.Items.Cast<string>().Any(dirPath => dirPath.Equals(dirSelectDialog.SelectedPath)))
            {
                Program.SafeMsgBox("Directory is already selected. Try again.", MessageBoxIcon.Warning);
                return;
            }

            listBoxDirs.Items.Add(dirSelectDialog.SelectedPath);
        }

        private void TlbBtnRemove_Click(object sender, EventArgs e)
        {
            foreach (int selectedIndex in listBoxDirs.SelectedIndices)
            {
                listBoxDirs.Items.RemoveAt(selectedIndex);
            }
        }

        private void ListBoxDirs_SelectedValueChanged(object sender, EventArgs e)
        {
            tlbBtnRemove.Enabled = listBoxDirs.SelectedIndices.Count > 0;
        }

        private void BtnScan_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                return;
            }

            if (File.Exists(txtOutputReportPath.Text) &&
                MessageBox.Show(
                    $"Output report file \"{txtOutputReportPath.Text}\" already exists.\nAre you sure you want to overwrite it?",
                    "Confirm overwrite", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            {
                return;
            }

            var scanConfig = new Config.FormData(
                chkBoxOptions.CheckedItems.OfType<Config.ConfigItem>().Select(item => item.Key).ToList(),
                listBoxDirs.Items.OfType<string>().ToList(),
                txtOutputReportPath.Text);
            var resultManager = new ResultManager();
            var formScan = new FormScan(resultManager);

            new Scanner(scanConfig, formScan, resultManager).Start();
        }

        private bool ValidateForm()
        {
            if (listBoxDirs.Items.Count == 0)
            {
                Program.SafeMsgBox("Please select at least one directory to scan.", MessageBoxIcon.Error);
                return false;
            }

            var faultyDir = listBoxDirs.Items.Cast<string>().FirstOrDefault(dirPath => !Directory.Exists(dirPath));
            if (faultyDir != null)
            {
                Program.SafeMsgBox($"Directory {faultyDir} is not accessible or does not exist. Please fix it.",
                    MessageBoxIcon.Error);
                return false;
            }

            if (chkBoxOptions.CheckedIndices.Count == 0)
            {
                Program.SafeMsgBox("Please select at least one scan option.", MessageBoxIcon.Error);
                return false;
            }

            if (txtOutputReportPath.TextLength == 0 ||
                !(new FileInfo(txtOutputReportPath.Text).Directory?.Exists ?? false))
            {
                Program.SafeMsgBox("Report output directory is not defined or it is not accessible.",
                    MessageBoxIcon.Stop);
                return false;
            }

            return true;
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            var formLoad = new FormLoadRegistry();
            switch (formLoad.ShowDialog(this))
            {
                case DialogResult.OK:
                    Focus();
                    break;
                default:
                    Close();
                    break;
            }
        }

        private void BtnSelectOutputPath_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                txtOutputReportPath.Text = saveFileDialog.FileName;
            }
        }
    }
}
