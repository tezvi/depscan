using System;
using System.Threading;
using System.Windows.Forms;
using DepScan.Properties;

namespace DepScan
{
    public partial class FormWait : Form
    {
        public FormWait()
        {
            Icon = Resources.MainFormIcon;
            InitializeComponent();
        }

        private void FormWait_Load(object sender, EventArgs e)
        {
            Text = Program.AppTitle;
        }

        public void SafeHide()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(Hide));
            }
            else
            {
                Hide();
            }
        }

        public void SafeShowWithJob(MethodInvoker invokerJob, Form parentForm, string message = null,
            MethodInvoker onJobComplete = null)
        {
            var job = new Thread(() =>
            {
                invokerJob(); 
                Program.WaitDialog.SafeHide();
                onJobComplete?.Invoke();
            })
            {
                IsBackground = true
            };

            job.Start();
            SafeShow(parentForm, message);
        }

        public void SafeShow(Form owner, string msg = "")
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => {
                    lblMsg.Text = msg;
                    ShowDialog(owner);
                }));
            }
            else
            {
                lblMsg.Text = msg;
                ShowDialog(owner);
            }
        }

        public void SafeMessage(string msg)
        {
            msg += "...";
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { lblMsg.Text = msg; });
            }
            else
            {
                lblMsg.Text = msg;
            }
        }
    }
}
