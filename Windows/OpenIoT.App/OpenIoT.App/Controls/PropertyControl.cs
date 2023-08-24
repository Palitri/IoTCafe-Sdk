using OpenIoT.App.Controls.EventHandlers;
using OpenIoT.Lib.Board.Models;
using OpenIoT.Lib.Composite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIoT.App.Controls
{
    internal class PropertyControl : Panel
    {
        [Browsable(true)]
        [Category("Action")]
        [Description("Invoked when the user clicks on the property control")]
        public event PropertyEventHandler PropertyClicked;

        private const float fontSize = 12.0f;

        private Label lblName;
        private Label lblValue;
        private CheckBox cbValue;
        private TrackBar tbValue;

        private bool trackBarManualChanging = false;
        private bool valueSliderEnabled = false;

        private void TbValue_ValueChanged(object? sender, EventArgs e)
        {
            if (this.Property.BoardProperty.type == BoardPropertyType.Float)
                this.Property.BoardProperty.SetValue((float)(this.Property.PeripheralProperty.Min + ((float)this.tbValue.Value * this.Property.PeripheralProperty.Step)));
            else if (this.Property.BoardProperty.type == BoardPropertyType.Integer)
                this.Property.BoardProperty.SetValue((int)(this.Property.PeripheralProperty.Min + ((float)this.tbValue.Value * this.Property.PeripheralProperty.Step)));

            this.Property.Board.boardDevice.RequestPropertyUpdate(this.Property.BoardProperty);
        }

        private CompositeProperty _property;
        public CompositeProperty Property 
        { 
            get { return this._property; } 

            set
            {
                if (this._property != value)
                {
                    this._property = value;

                    this.lblName.Text = this._property.PeripheralProperty.Name;

                    this.UpdateValueControl();
                }
            }
        }

        public PropertyControl(CompositeProperty property = null)
            : base()
        {
            this.lblName = new Label();
            this.lblName.Font = new Font(this.Font.FontFamily, 16);
            this.lblName.AutoSize = true;
            this.lblName.Top = 0;
            this.lblName.Left = 8;
            this.lblName.Anchor = AnchorStyles.Left | AnchorStyles.Top;
            this.lblName.Click += Control_Clicked;
            this.lblName.MouseEnter += PropertyControl_MouseEnter;
            this.lblName.MouseLeave += PropertyControl_MouseLeave;
            this.Controls.Add(this.lblName);

            this.Height = (int)(this.lblName.Height * 1.5f);

            if ((property?.BoardProperty.type == BoardPropertyType.Float) || (property?.BoardProperty.type == BoardPropertyType.Integer))
            {
                this.lblValue = new Label();
                this.lblValue.Font = new Font(this.Font.FontFamily, 16);
                this.lblValue.AutoSize = true;
                this.lblValue.Top = 0;
                this.lblValue.Left = this.Width - 8 - this.lblValue.Width;
                this.lblValue.RightToLeft = RightToLeft.Yes;
                this.lblValue.Anchor = AnchorStyles.Right | AnchorStyles.Top;
                this.lblValue.Click += Control_Clicked;
                this.lblValue.MouseEnter += PropertyControl_MouseEnter;
                this.lblValue.MouseLeave += PropertyControl_MouseLeave;
                this.Controls.Add(this.lblValue);

                if (this.valueSliderEnabled)
                {
                    this.tbValue = new TrackBar();
                    this.tbValue.TickStyle = TickStyle.Both;
                    this.tbValue.TickFrequency = 0;
                    this.tbValue.AutoSize = false;
                    this.tbValue.Maximum = (int)((property.PeripheralProperty.Max - property.PeripheralProperty.Min) / property.PeripheralProperty.Step);
                    this.tbValue.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                    this.tbValue.Width = 200;
                    this.tbValue.Height = this.Height - 8;
                    this.tbValue.Left = this.Width - 400;
                    this.tbValue.Top = (this.Height - this.tbValue.Height) / 2;
                    this.tbValue.ValueChanged += TbValue_ValueChanged;
                    this.tbValue.MouseDown +=TbValue_MouseDown;
                    this.tbValue.MouseUp += TbValue_MouseUp;
                    this.tbValue.MouseEnter += PropertyControl_MouseEnter;
                    this.tbValue.MouseLeave += PropertyControl_MouseLeave;
                    this.Controls.Add(this.tbValue);
                }
            }
            else if (property?.BoardProperty.type == BoardPropertyType.Bool)
            {
                this.cbValue = new CheckBox();
                this.cbValue.AutoCheck = false;
                this.cbValue.Text = String.Empty;
                this.cbValue.AutoSize = true;
                this.cbValue.Top = (this.Height - this.cbValue.Height) / 2;
                this.cbValue.Left = this.Width - 24 - this.cbValue.Width;
                this.cbValue.Anchor = AnchorStyles.Right | AnchorStyles.Top;
                this.cbValue.Click += Control_Clicked;
                this.cbValue.MouseEnter += PropertyControl_MouseEnter;
                this.cbValue.MouseLeave += PropertyControl_MouseLeave;
                this.Controls.Add(this.cbValue);

            }

            this.UpdateValueControl();


            this.Click += Control_Clicked;  
            this.MouseEnter +=PropertyControl_MouseEnter;
            this.MouseLeave += PropertyControl_MouseLeave;

            this.Property = property;
        }

        private void TbValue_MouseUp(object? sender, MouseEventArgs e)
        {
            this.trackBarManualChanging = false;
        }

        private void TbValue_MouseDown(object? sender, MouseEventArgs e)
        {
            this.trackBarManualChanging = true;
        }

        private void PropertyControl_MouseEnter(object? sender, EventArgs e)
        {
            this.BackColor = SystemColors.ControlLight;
        }

        private void PropertyControl_MouseLeave(object? sender, EventArgs e)
        {
            this.BackColor = SystemColors.Control;
        }

        private void Control_Clicked(object? sender, EventArgs e)
        {
            this.PropertyClicked?.Invoke(this, new PropertyEventArgs(this.Property));
        }

        public void UpdateValueControl()
        {
            if ((this._property?.BoardProperty.type == BoardPropertyType.Float) || (this._property?.BoardProperty.type == BoardPropertyType.Integer))
            {
                this.lblValue.Text = this._property.BoardProperty.value.ToString();
                this.lblValue.Left = this.Width - 8 - this.lblValue.Width;

                if ((this.tbValue != null) &&  !this.trackBarManualChanging)
                    this.tbValue.Value = Math.Max(Math.Min((int)((-this.Property.PeripheralProperty.Min + (this.Property?.BoardProperty.type == BoardPropertyType.Float ? (float)this.Property.BoardProperty.value : (float)((int)this.Property.BoardProperty.value))) / this.Property.PeripheralProperty.Step), this.tbValue.Maximum), this.tbValue.Minimum);
            }
            else if (this._property?.BoardProperty.type == BoardPropertyType.Bool)
                this.cbValue.Checked = this._property.BoardProperty.GetBool();


        }

    }
}
