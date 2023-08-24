using OpenIoT.Lib.Composite;
using OpenIoT.Lib.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIoT.App.Controls.EventHandlers
{
    public delegate void PresetEventHandler(object sender, PresetEventArgs args);

    public class PresetEventArgs : System.EventArgs
    {
        public Preset Preset { get; private set; }

        public PresetEventArgs(Preset preset)
            : base()
        {
            this.Preset = preset;
        }
    }
}
