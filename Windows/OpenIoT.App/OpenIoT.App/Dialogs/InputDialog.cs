using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenIoT.App.Dialogs
{
    public partial class InputDialog : Form
    {
        public string Input { get { return this.tbInput.Text; } set { this.tbInput.Text = value; } }
        public string Message { get { return this.lblText.Text; } set { this.lblText.Text = value; } }

        public InputDialog()
        {
            InitializeComponent();

            this.Message = String.Empty;
            this.Input = String.Empty;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
