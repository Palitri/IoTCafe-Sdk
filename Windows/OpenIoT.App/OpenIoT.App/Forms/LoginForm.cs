using OpenIoT.Lib.Web.Api;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenIoT.App.Forms
{
    public partial class LoginForm : Form
    {
        public String Username { get { return this.tbUsername.Text; } set { this.tbUsername.Text = value; } }
        public String Password { get { return this.tbPassword.Text; } set { this.tbPassword.Text = value; } }

        public LoginForm()
        {
            InitializeComponent();
        }

        private async void btnOk_Click(object sender, EventArgs e)
        {
            string token = (await new OpenIoTService().RequestUserLogin(this.Username, this.Password)).Token;

            if (String.IsNullOrWhiteSpace(token))
            {
                MessageBox.Show(Resources.LoginUnsuccessful, Resources.Login, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Username = String.Empty;
                this.Password = String.Empty;
                this.tbUsername.Focus();
                return;
            }

            AppBase.Instance.Board.persistence.setToken(token);

            MessageBox.Show(Resources.LoginSuccessful, Resources.Login, MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.DialogResult = DialogResult.OK;
        }
    }
}
