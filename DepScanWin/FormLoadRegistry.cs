using System;
using System.IO;
using System.Windows.Forms;
using DepScan.Properties;

namespace DepScan
{
    public partial class FormLoadRegistry : Form
    {
        public FormLoadRegistry()
        {
            Icon = Resources.MainFormIcon;
            InitializeComponent();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            if (!File.Exists(txtInputFile.Text))
            {
                Program.SafeMsgBox($"File {txtInputFile.Text} is not accessible or it does not exist.",
                    MessageBoxIcon.Error);
                return;
            }

            Program.WaitDialog.SafeShowWithJob(LoadData, this, "Loading XML data");
        }

        private void LoadData()
        {
            try
            {
                Program.Registry.LoadXmlData(txtInputFile.Text);
                DialogResult = DialogResult.OK;
            }
            catch (Exception exception)
            {
                Program.WriteLog(exception);
                DialogResult = DialogResult.None;
                Program.SafeMsgBox($"An error occurred while loading XML contents\n\nDetails: {exception.Message}",
                    MessageBoxIcon.Stop);
            }
        }

        private void BtnSelectOutputPath_Click(object sender, EventArgs e)
        {
            if (selectFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                txtInputFile.Text = selectFileDialog.FileName;
            }
        }

        private void FormLoadRegistry_Load(object sender, EventArgs e)
        {
            selectFileDialog.InitialDirectory = Utils.AssemblyDirectory;
        }
    }
}