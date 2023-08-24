using OpenIoT.App.Controls.EventHandlers;
using OpenIoT.Lib.Composite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIoT.App.Controls
{
    internal class PropertiesListControl : Panel
    {
        [Browsable(true)]
        [Category("Action")]
        [Description("Invoked when the user clicks a property item")]
        public event PropertyEventHandler PropertyClicked;

        public List<PropertyControl> Items { get; private set; } = new List<PropertyControl>();

        public IEnumerable<CompositeProperty> Properties
        {
            set
            {
                this.Controls.Clear();

                if (value == null)
                    return;

                int y = 0;
                foreach (CompositeProperty property in value)
                {
                    PropertyControl item = new PropertyControl(property)
                    {
                        Left = 0,
                        Top = y,
                        Width = this.Width,
                        Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                        Font = this.Font,
                    };
                    item.PropertyClicked += Item_PropertyClicked;
                    y += item.Height;

                    this.Controls.Add(item);
                }
            }
        }

        private void Item_PropertyClicked(object? sender, EventArgs e)
        {
            this.PropertyClicked?.Invoke(this, new PropertyEventArgs(((PropertyControl)sender).Property));
        }

        public void UpdateProperty(CompositeProperty property)
        {
            foreach (Control child in this.Controls)
                if (child is PropertyControl)
                    if (((PropertyControl)child).Property == property)
                        ((PropertyControl)child).UpdateValueControl();
        }
    }
}
