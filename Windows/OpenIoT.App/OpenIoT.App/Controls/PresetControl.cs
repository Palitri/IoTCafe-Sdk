using OpenIoT.App.Controls.EventHandlers;
using OpenIoT.Lib.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIoT.App.Controls
{
    internal class PresetControl : Panel
    {
        [Browsable(true)]
        [Category("Action")]
        [Description("Invoked when the user clicks on the preset control")]
        public event PresetEventHandler PresetClicked;
        public event PresetEventHandler PresetUpdate;
        public event PresetEventHandler PresetRename;
        public event PresetEventHandler PresetDelete;

        private const float fontSize = 12.0f;

        private Label lblName;
        private Button btnUpdate, btnRename, btnDelete;

        private Preset _preset;
        public Preset Preset
        { 
            get { return this._preset; } 

            set
            {
                if (this._preset != value)
                {
                    this._preset = value;
                }

                this.lblName.Text = this._preset.Name;
            }
        }

        public PresetControl(Preset preset = null)
            : base()
        {
            this.lblName = new Label();
            this.lblName.Font = new Font(this.Font.FontFamily, 16);
            this.lblName.AutoSize = true;
            this.lblName.Left = 8;
            this.lblName.Anchor = AnchorStyles.Left | AnchorStyles.Top;
            this.lblName.Click += Control_Clicked;
            this.lblName.MouseEnter += Control_MouseEnter;
            this.lblName.MouseLeave += Control_MouseLeave;
            this.Controls.Add(this.lblName);

            this.Height = (int)(this.lblName.Height * 1.5f);

            this.lblName.Top = (this.Height - this.lblName.Height) / 2;

            int buttonSize = (int)(this.Height * 0.75f);
            const int buttonPadding = 8;
            int buttonX = this.Width -  3 * (buttonSize + buttonPadding);
            int buttonY = (this.Height - buttonSize) / 2;

            this.btnUpdate = new Button();
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Top)));
            this.btnUpdate.Location = new System.Drawing.Point(buttonX, buttonY);
            this.btnUpdate.Font = new Font(this.Font.FontFamily, 16);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(buttonSize, buttonSize);
            this.btnUpdate.Text = String.Empty;
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Image = Resources.icon_update;
            this.btnUpdate.Click += btnUpdate_Clicked;
            this.btnUpdate.MouseEnter += Control_MouseEnter;
            this.btnUpdate.MouseLeave += Control_MouseLeave;
            this.Controls.Add(this.btnUpdate);

            buttonX += buttonSize + buttonPadding;
            this.btnRename = new Button();
            this.btnRename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Top)));
            this.btnRename.Location = new System.Drawing.Point(buttonX, buttonY);
            this.btnRename.Font = new Font(this.Font.FontFamily, 16);
            this.btnRename.Name = "btnUpdate";
            this.btnRename.Size = new System.Drawing.Size(buttonSize, buttonSize);
            this.btnRename.Text = String.Empty;
            this.btnRename.UseVisualStyleBackColor = true;
            this.btnRename.Image = Resources.icon_edit;
            this.btnRename.Click += btnRename_Clicked;
            this.btnRename.MouseEnter += Control_MouseEnter;
            this.btnRename.MouseLeave += Control_MouseLeave;
            this.Controls.Add(this.btnRename);


            buttonX += buttonSize + buttonPadding;
            this.btnDelete = new Button();
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Top)));
            this.btnDelete.Location = new System.Drawing.Point(buttonX, buttonY);
            this.btnDelete.Font = new Font(this.Font.FontFamily, 16);
            this.btnDelete.Name = "btnUpdate";
            this.btnDelete.Size = new System.Drawing.Size(buttonSize, buttonSize);
            this.btnDelete.Text = String.Empty;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Image = Resources.icon_delete;
            this.btnDelete.Click += btnDelete_Clicked;
            this.btnDelete.MouseEnter += Control_MouseEnter;
            this.btnDelete.MouseLeave += Control_MouseLeave;
            this.Controls.Add(this.btnDelete);


            this.Click += Control_Clicked;  
            this.MouseEnter += Control_MouseEnter;
            this.MouseLeave += Control_MouseLeave;

            this.Preset = preset;
        }

        private void Control_MouseEnter(object? sender, EventArgs e)
        {
            this.BackColor = SystemColors.ControlLight;
        }

        private void Control_MouseLeave(object? sender, EventArgs e)
        {
            this.BackColor = SystemColors.Control;
        }

        private void Control_Clicked(object? sender, EventArgs e)
        {
            this.PresetClicked?.Invoke(this, new PresetEventArgs(this.Preset));
        }

        private void btnUpdate_Clicked(object? sender, EventArgs e)
        {
            this.PresetUpdate?.Invoke(this, new PresetEventArgs(this.Preset));
        }

        private void btnRename_Clicked(object? sender, EventArgs e)
        {
            this.PresetRename?.Invoke(this, new PresetEventArgs(this.Preset));
        }

        private void btnDelete_Clicked(object? sender, EventArgs e)
        {
            this.PresetDelete?.Invoke(this, new PresetEventArgs(this.Preset));
        }

    }
}
