using OpenIoT.Lib.Board.Models;
using OpenIoT.Lib.Composite;
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
    public partial class PropertyNumericValueDialog : Form
    {
        private float min;
        private float max;
        private float step;
        private bool instantUpdate;

        private bool changed;

        private float originalValue;


        private CompositeProperty property;

        public float Value 
        { 
            get { return this.min + ((float)this.tbValue.Value * this.step); }

            set { this.tbValue.Value = (int)((-this.min + value) / this.step); }
        }

        public PropertyNumericValueDialog(CompositeProperty property)
        {
            InitializeComponent();

            this.property = property;

            if (property.BoardProperty.type == BoardPropertyType.Float)
                this.Initialize(
                    property.PeripheralProperty.Name,
                    (float)this.property.BoardProperty.value,
                    this.property.PeripheralProperty.Min,
                    this.property.PeripheralProperty.Max,
                    this.property.PeripheralProperty.Step,
                    this.property.PeripheralProperty.InstantUpdate);
            else if (property.BoardProperty.type == BoardPropertyType.Integer)
                this.Initialize(
                    property.PeripheralProperty.Name,
                    (int)this.property.BoardProperty.value,
                    this.property.PeripheralProperty.Min,
                    this.property.PeripheralProperty.Max,
                    this.property.PeripheralProperty.Step,
                    this.property.PeripheralProperty.InstantUpdate);
        }

        public void Initialize(string name, float value, float min, float max, float step, bool instantUpdate)
        {
            this.min = min;
            this.max = max;
            this.step = step;
            this.instantUpdate = instantUpdate;

            this.changed = false;
            this.originalValue = value;

            this.Text = name;

            this.tbValue.Maximum = (int)((this.max - this.min) / this.step);
            this.Value = value;
            this.tbValue.ValueChanged += new EventHandler(this.tbValue_ValueChanged);

            this.lblValue.Text = this.Value.ToString();
        }

        private void tbValue_ValueChanged(object sender, EventArgs e)
        {
            this.lblValue.Text = this.Value.ToString();

            if (this.instantUpdate)
                this.UpdateBoardValue(this.Value);

        }

        private void UpdateBoardValue(float value)
        {
            if (this.property?.BoardProperty.type == BoardPropertyType.Float)
            {
                this.property?.BoardProperty?.SetValue(value);
                this.property?.Board?.boardDevice?.RequestPropertyUpdate(this.property?.BoardProperty);
            }
            else if (this.property?.BoardProperty.type == BoardPropertyType.Integer)
            {
                this.property?.BoardProperty?.SetValue((int)Math.Round(value));
                this.property?.Board?.boardDevice?.RequestPropertyUpdate(this.property?.BoardProperty);
            }
        }

        private void btnMinus_Click(object sender, EventArgs e)
        {
            this.tbValue.Value--;
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            this.tbValue.Value++;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (this.instantUpdate)
                this.UpdateBoardValue(this.originalValue);

            this.DialogResult = DialogResult.Cancel;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!this.instantUpdate)
                this.UpdateBoardValue(this.Value);

            this.DialogResult = DialogResult.OK;
        }
    }
}
