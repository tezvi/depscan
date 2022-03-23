using System;
using System.Windows.Forms;
using DepScan.Properties;

namespace DepScan
{
    public partial class FormError : Form
    {
        public FormError()
        {
            Icon = Resources.MainFormIcon;
            InitializeComponent();
        }

        public Exception Exception
        {
            set
            {
                txtError.Text = value.ToString();
                ErrorText = "A fatal error has occurred and application needs to close.\n\nMessage: " 
                                + value.Message;
            }
        }

        public string ErrorText
        {
            set => lblError.Text = value;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormError_Load(object sender, EventArgs e)
        {
            Text = Program.AppTitle;
        }
    }
}