using OpenIoT.Lib.Composite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIoT.App.Controls.EventHandlers
{
    public delegate void PropertyEventHandler(object sender, PropertyEventArgs args);

    public class PropertyEventArgs : System.EventArgs
    {
        public CompositeProperty Property { get; private set; }

        public PropertyEventArgs(CompositeProperty property)
            : base()
        {
            this.Property = property;
        }
    }
}
