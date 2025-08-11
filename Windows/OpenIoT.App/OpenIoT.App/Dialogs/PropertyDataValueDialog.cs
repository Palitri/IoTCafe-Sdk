using Palitri.OpenIoT.Composite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Palitri.OpenIoT.App.Dialogs
{
    public partial class PropertyDataValueDialog : Form
    {
        private CompositeProperty property;

        public byte[] Data {
            get
            {
                return Convert.FromHexString(this.tbInput.Text);
            }

            set
            {
                this.tbInput.Text = Convert.ToHexString(value);
            }
        }

        public string Message { get { return this.lblText.Text; } set { this.lblText.Text = value; } }

        public PropertyDataValueDialog(CompositeProperty property)
        {
            InitializeComponent();

            this.property = property;

            this.Message = String.Empty;
            this.Data = this.property.BoardProperty.value as byte[];
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.property?.BoardProperty?.SetValue(this.Data);
            this.property?.Board?.boardDevice?.RequestPropertyUpdate(this.property?.BoardProperty);
        }
    }
}
