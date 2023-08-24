using OpenIoT.App.Controls.EventHandlers;
using OpenIoT.Lib.Composite;
using OpenIoT.Lib.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIoT.App.Controls
{
    internal class PresetsListControl : Panel
    {
        [Browsable(true)]
        [Category("Action")]
        [Description("Invoked when the user clicks a preset item")]
        public event PresetEventHandler PresetClicked;
        public event PresetEventHandler PresetUpdate;
        public event PresetEventHandler PresetRename;
        public event PresetEventHandler PresetDelete;

        public IEnumerable<Preset> Presets
        {
            set
            {
                this.Controls.Clear();

                if (value == null)
                    return;

                int y = 0;
                foreach (Preset preset in value)
                {
                    PresetControl item = new PresetControl(preset)
                    {
                        Left = 0,
                        Top = y,
                        Width = this.Width,
                        Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                        Font = this.Font,
                    };
                    item.PresetClicked += Item_PresetClicked;
                    item.PresetUpdate += Item_PresetUpdate;
                    item.PresetRename += Item_PresetRename;
                    item.PresetDelete += Item_PresetDelete;
                    y += item.Height;

                    this.Controls.Add(item);
                }
            }
        }

        private void Item_PresetClicked(object? sender, EventArgs e)
        {
            this.PresetClicked?.Invoke(this, new PresetEventArgs(((PresetControl)sender).Preset));
        }

        private void Item_PresetUpdate(object? sender, EventArgs e)
        {
            this.PresetUpdate?.Invoke(this, new PresetEventArgs(((PresetControl)sender).Preset));
        }

        private void Item_PresetRename(object? sender, EventArgs e)
        {
            this.PresetRename?.Invoke(this, new PresetEventArgs(((PresetControl)sender).Preset));
        }

        private void Item_PresetDelete(object? sender, EventArgs e)
        {
            this.PresetDelete?.Invoke(this, new PresetEventArgs(((PresetControl)sender).Preset));
        }
    }
}
